Imports System.Data.SqlClient
Imports Hot.Data

Namespace Brands
    Public Class Pin

        Protected ApplicationName As String
        Protected SqlConn As SqlConnection
        Protected ServiceEndpoint As String

        Public Sub New(sqlConn As SqlConnection, applicationName As String)
            Me.ApplicationName = applicationName
            Me.SqlConn = sqlConn
        End Sub

        Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String)
            Me.ApplicationName = applicationName
            Me.SqlConn = sqlConn
            Me.ServiceEndpoint = serviceEndpoint
        End Sub

        Public Function RechargePin(recharge As xRecharge, Optional numberOfDays As Integer = 90) _
            As PinRechargeResponse
            'If this recharge is a failed prepaid transaction then set pin brand to Buddie
            ' If recharge.Brand.BrandID = xBrand.Brands.Prepaid Then recharge.Brand.BrandID = xBrand.Brands.EconetUSD

            Dim limit = New LimitDiscountTo5Percent()
            limit.Apply(recharge)


            Dim pins As List(Of xPin) = xPinAdapter.Recharge(recharge.Amount, recharge.Brand.BrandID, recharge.RechargeID, SqlConn)

            Dim dealerReplies As New List(Of xTemplate)
            Dim iSms As xSMS = xRechargeAdapter.SelectSMS(recharge.RechargeID, SqlConn)
            Dim access As xAccess = xAccessAdapter.SelectRow(recharge.AccessID, SqlConn)
            'No Pins or Pin Count > 5 'RJA: THIS DOESN'T MAKE A LOT OF SENSE, THE IF CLAUSE IS ONLY FILTERING FOR COUNT = 0??
            If pins.Count = 0 Or pins.Count > 5 Then
                recharge.State.StateID = xState.States.Failure
                recharge.RechargeDate = Now
                xRechargeAdapter.Save(recharge, SqlConn)
                dealerReplies.Add(xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPinDenomination, SqlConn))
                If access.Channel.ChannelID = xChannel.Channels.SMS Then
                    dealerReplies = ListPinStock()
                    Return New PinRechargeResponse(iSms, dealerReplies, False)
                End If
                Return New PinRechargeResponse(iSms, dealerReplies, False)
            End If

            recharge.State.StateID = xState.States.Success
            recharge.RechargeDate = Now
            xRechargeAdapter.Save(recharge, SqlConn)

            'Refresh Account Balance / Sale Value

            Dim account As xAccount = xAccountAdapter.SelectRow(access.AccountID, SqlConn)
            'If access.Channel.ChannelID = xChannel.Channels.SMS Then
            Dim header As xTemplate = GetDealerSmsHeader(recharge, account)
            dealerReplies.Add(header)
            'End If
            dealerReplies.AddRange(GetDealerPinReplies(recharge, pins))
            Dim response = New PinRechargeResponse(iSms, dealerReplies, True, GetCustomerReplies(recharge, pins)) With {
                .Pins = pins,
                .Account = account,
                .AccessChannel = access.Channel.ChannelID
            }
            Return response
        End Function

        Public Function GetDealerPinReplies(recharge As xRecharge, pins As List(Of xPin)) As List(Of xTemplate)

            Dim dealerPinReplies = New List(Of xTemplate)()
            For Each iPin As xPin In pins
                Dim dealerPinReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargePin_Dealer, SqlConn)
                dealerPinReply.TemplateText = dealerPinReply.TemplateText.Replace("%PIN%", iPin.Pin)
                dealerPinReply.TemplateText = dealerPinReply.TemplateText.Replace("%REF%", iPin.PinRef)
                dealerPinReply.TemplateText = dealerPinReply.TemplateText.Replace("%PINVALUE%", Formatting.FormatAmount(iPin.PinValue))
                dealerPinReply.TemplateText = dealerPinReply.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                dealerPinReply.TemplateText = dealerPinReply.TemplateText.Replace("%BRAND%", recharge.Brand.BrandName)
                dealerPinReplies.Add(dealerPinReply)
            Next
            Return dealerPinReplies
        End Function

        Public Function GetCustomerReplies(recharge As xRecharge, pins As List(Of xPin)) As List(Of xTemplate)

            Dim customerReplies As New List(Of xTemplate)
            For Each iPin As xPin In pins
                Dim iReplyCustomerPin As New xTemplate
                If recharge.Brand.BrandID = xBrand.Brands.Juice Or recharge.Brand.BrandID = xBrand.Brands.Africom Then
                    iReplyCustomerPin.TemplateText = "*123*" & iPin.Pin & "#"
                ElseIf recharge.Brand.BrandID = xBrand.Brands.NetoneUSD Or recharge.Brand.BrandID = xBrand.Brands.EasyCallEVD Or recharge.Brand.BrandID = xBrand.Brands.NetoneOneFi Or recharge.Brand.BrandID = xBrand.Brands.NetoneOneFusion Then
                    iReplyCustomerPin.TemplateText = "*133*" & iPin.Pin & "#"
                ElseIf recharge.Brand.BrandID = xBrand.Brands.EconetUSD Then
                    iReplyCustomerPin.TemplateText = "*121*" & iPin.Pin & "#"
                Else
                    iReplyCustomerPin.TemplateText = "**" & iPin.Pin & "#"
                End If
                customerReplies.Add(iReplyCustomerPin)
            Next

            'Customer Reply Header
            Dim iReplyCustomerHeader As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargePin_Customer_Header, SqlConn)
            iReplyCustomerHeader.TemplateText = iReplyCustomerHeader.TemplateText.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount, decimalPlaces:=If(recharge.Amount Mod 1 = 0, 0, 2)))
            If recharge.Brand.BrandID = xBrand.Brands.EasyCallEVD _
                Or recharge.Brand.BrandID = xBrand.Brands.NetoneOneFi _
                Or recharge.Brand.BrandID = xBrand.Brands.NetoneOneFusion _
                 Or recharge.Brand.BrandID = xBrand.Brands.NetoneUSD _
                 Or recharge.Brand.BrandID = xBrand.Brands.EconetUSD _
                Then
                iReplyCustomerHeader.TemplateText = iReplyCustomerHeader.TemplateText.Replace("Airtime", recharge.Brand.BrandName)
            End If
            If recharge.Brand.BrandID = xBrand.Brands.NetoneUSD _
                 Or recharge.Brand.BrandID = xBrand.Brands.EconetUSD _
                Then
                iReplyCustomerHeader.TemplateText = iReplyCustomerHeader.TemplateText.Replace("$", "USD$")
            End If

            customerReplies.Add(iReplyCustomerHeader)
            Return customerReplies
        End Function

        Public Function GetDealerSmsHeader(recharge As xRecharge, iAccount As xAccount) As xTemplate
            Dim header As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRecharge_Dealer_Header, SqlConn)
            Dim text As String = header.TemplateText
            header.TemplateText = header.TemplateText.Replace("%MOBILE%", recharge.Mobile)
            header.TemplateText = header.TemplateText.Replace("%AMOUNT%", Formatting.FormatAmount(recharge.Amount))
            header.TemplateText = header.TemplateText.Replace("%DISCOUNT%", Formatting.FormatAmount(recharge.Discount))
            header.TemplateText = header.TemplateText.Replace("%COST%", Formatting.FormatAmount(recharge.Amount * (1 - recharge.Discount / 100), formatForDealerCost:=True))
            Dim Balance As Decimal = iAccount.Balance
            If recharge.Brand.BrandID = xBrand.Brands.EconetUSD Or recharge.Brand.BrandID = xBrand.Brands.NetoneUSD Then
                Balance = iAccount.USDBalance
            End If
            Dim Salevalue As Decimal = Balance + (Balance * (recharge.Discount / 100))

            header.TemplateText = header.TemplateText.Replace("%BALANCE%", Formatting.FormatAmount(Balance))
            header.TemplateText = header.TemplateText.Replace("%SALEVALUE%", Formatting.FormatAmount(Salevalue))
            Return header
        End Function


        Public Function ListPinStock() As List(Of xTemplate)
            Dim iList As New List(Of xTemplate)
            Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpStock, SqlConn)
            iTemplate.TemplateText = "Stocks:"
            Dim currentBrand = ""
            For Each iStock As xPinStock In xPinStockAdapter.Stock(SqlConn)
                If currentBrand <> iStock.BrandName Then
                    currentBrand = iStock.BrandName
                    iTemplate.TemplateText &= vbNewLine & currentBrand & " $"
                End If
                If iStock.PinValue Mod 1 = 0 Then
                    iTemplate.TemplateText &= ", " & Formatting.FormatAmount(iStock.PinValue, decimalPlaces:=0)
                Else
                    iTemplate.TemplateText &= ", " & Formatting.FormatAmount(iStock.PinValue, decimalPlaces:=1)
                End If
            Next

            Try ' Query Netone API for data pins
                Dim netone = New NetonePins(SqlConn, ApplicationName, ServiceEndpoint, False, "", True)
                Dim Query = netone.QueryEVDStock()
                For Each iStock As NetoneAPI.PinStockModel In Query
                    If currentBrand <> iStock.BrandName Then
                        currentBrand = iStock.BrandName
                        iTemplate.TemplateText &= vbNewLine & currentBrand & " $"
                    End If
                    If iStock.PinValue Mod 1 = 0 Then
                        iTemplate.TemplateText &= ", " & Formatting.FormatAmount(iStock.PinValue, decimalPlaces:=0)
                    Else
                        iTemplate.TemplateText &= ", " & Formatting.FormatAmount(iStock.PinValue, decimalPlaces:=1)
                    End If
                Next
            Catch ex As Exception

            End Try

            iList.Add(iTemplate)
            Return iList
        End Function

        Public Shared Function GetPinsFromServiceResponse(irecharge As xRecharge, response As ServiceRechargeResponse) As List(Of xPin)
            Dim result As New List(Of xPin)
            Dim pins = response.RechargePrepaid.Narrative.Split(New String() {"E-Top Up"}, StringSplitOptions.None)(1).Split(";").ToList()
            For Each pin In pins
                Dim pinarray = pin.Split(",")
                result.Add(New xPin With {.Brand = irecharge.Brand,
                       .Pin = pinarray(0),
                       .PinExpiry = CDate(pinarray(3)),
                       .PinRef = pinarray(1),
                       .PinValue = CDec(pinarray(2))
                       })
            Next
            Return result
        End Function


    End Class
End Namespace