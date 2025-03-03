Imports System.Data.SqlClient
Imports Hot.Data

Namespace Recharging
    Public Class Pin

        Protected ApplicationName As String
        Protected SqlConn As SqlConnection
        Protected ServiceEndpoint As String

        Public Sub New(sqlConn As SqlConnection, applicationName as string)
            Me.ApplicationName = applicationName    
            Me.SqlConn = sqlConn
        End Sub

        Public Function RechargePin(recharge As xRecharge, Optional numberOfDays As Integer = 90) _
            As PinRechargeResponseX ' To delete
            'If this recharge is a failed prepaid transaction then set pin brand to Buddie
            'If recharge.Brand.BrandID = xBrand.Brands.EconetPlatform Then recharge.Brand.BrandID = xBrand.BrandsByKey.Buddie
            'If recharge.Brand.BrandID = xBrand.Brands.TelecelPyros Then recharge.Brand.BrandID = xBrand.BrandsByKey.Juice

            If recharge.Amount < 1 And recharge.Discount > 5 Then recharge.Discount = 5

            'Get Access
            Dim iAccess As xAccess = xAccessAdapter.SelectRow(recharge.AccessID, sqlConn)

            'Get Pins
            Dim iPins As List(Of xPin) = xPinAdapter.Recharge(recharge.Amount, recharge.Brand.BrandID,
                                                              recharge.RechargeID, sqlConn)

            'Replies
            Dim iList As New List(Of xTemplate)
            Dim iSms As xSMS = xRechargeAdapter.SelectSMS(recharge.RechargeID, sqlConn)

            'No Pins or Pin Count > 5 
            If iPins.Count = 0 Then
                recharge.State.StateID = xState.States.Failure
                recharge.RechargeDate = Now
                xRechargeAdapter.Save(recharge, sqlConn)
                If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                    'Dim iSMS As xSMS = xRechargeAdapter.SelectSMS(recharge.RechargeID, sqlConn)
                    iList = ListPinStock()
                    iList.Insert(0, xTemplateAdapter.SelectRow(xTemplate.Templates.FailedPinDenomination, sqlConn))
                    Return New PinRechargeResponse(iSms, iList, False)
                End If            
                Return New PinRechargeResponse(Nothing, Nothing, False)
            End If

            'Update Recharge State
            recharge.State.StateID = xState.States.Success
            recharge.RechargeDate = Now
            xRechargeAdapter.Save(recharge, sqlConn)

            'Refresh Account Balance / Sale Value        
            Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

            If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                'Dealer Reply Header
                Dim iReplyDealerHeader As xTemplate =
                        xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRecharge_Dealer_Header, sqlConn)
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%AMOUNT%",
                                                                                          Formatting.FormatAmount(
                                                                                              recharge.Amount))
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%DISCOUNT%",
                                                                                          FormatNumber(recharge.Discount,
                                                                                                       2))
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%COST%",
                                                                                          FormatNumber(
                                                                                              recharge.Amount*
                                                                                              (1 - recharge.Discount/100),
                                                                                              2))
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%BALANCE%",
                                                                                          FormatNumber(iAccount.Balance,
                                                                                                       2))
                iReplyDealerHeader.TemplateText = iReplyDealerHeader.TemplateText.Replace("%SALEVALUE%",
                                                                                          FormatNumber(
                                                                                              iAccount.SaleValue, 2))
                iList.Add(iReplyDealerHeader)
                'Dim iSMS As xSMS = xRechargeAdapter.SelectSMS(recharge.RechargeID, sqlConn)
                'Reply(iSMS, iList, sqlConn)
            End If

            'Create Customer Reply List
            Dim iListCustomer As New List(Of xTemplate)

            'Dealer / Customer Reply Pins
            For Each iPin As xPin In iPins
                Dim iReplyDealerPin As xTemplate =
                        xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargePin_Dealer, sqlConn)
                'Dealer Reply Pin
                iReplyDealerPin.TemplateText = iReplyDealerPin.TemplateText.Replace("%PIN%", iPin.Pin)
                iReplyDealerPin.TemplateText = iReplyDealerPin.TemplateText.Replace("%REF%", iPin.PinRef)
                iReplyDealerPin.TemplateText = iReplyDealerPin.TemplateText.Replace("%PINVALUE%",
                                                                                    FormatNumber(iPin.PinValue, 2))
                iReplyDealerPin.TemplateText = iReplyDealerPin.TemplateText.Replace("%MOBILE%", recharge.Mobile)
                iReplyDealerPin.TemplateText = iReplyDealerPin.TemplateText.Replace("%BRAND%", recharge.Brand.BrandName)
                iList.Add(iReplyDealerPin)
                'iListCustomer.Add(iReplyDealerPin)
            Next

            For Each iPin As xPin In iPins
                'Customer Reply Pin
                Dim iReplyCustomerPin As New xTemplate
                If recharge.Brand.BrandID = xBrand.Brands.Juice Or recharge.Brand.BrandID = xBrand.Brands.Africom Then
                    iReplyCustomerPin.TemplateText = "*123*" & iPin.Pin & "#"
                Else
                    iReplyCustomerPin.TemplateText = "**" & iPin.Pin & "#"
                End If
                iListCustomer.Add(iReplyCustomerPin)
            Next

            'Customer Reply Header
            Dim iReplyCustomerHeader As xTemplate =
                    xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRechargePin_Customer_Header, sqlConn)
            iReplyCustomerHeader.TemplateText = iReplyCustomerHeader.TemplateText.Replace("%AMOUNT%",
                                                                                          Formatting.FormatAmount(
                                                                                              recharge.Amount, 0))
            iListCustomer.Add(iReplyCustomerHeader)

            Dim response = new PinRechargeResponse(iSms, iList, true, iListCustomer)
            response.AccessChannel = iAccess.Channel.ChannelID
            return response
        End Function


        Private Function ListPinStock() As List(Of xTemplate)
            Dim iList As New List(Of xTemplate)
            Dim iTemplate As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.HelpStock, sqlConn)
            iTemplate.TemplateText = "Stocks:"
            Dim currentBrand = ""
            For Each iStock As xPinStock In xPinStockAdapter.Stock(sqlConn)
                If currentBrand <> iStock.BrandName Then
                    currentBrand = iStock.BrandName
                    iTemplate.TemplateText &= vbNewLine & currentBrand & " $"
                End If
                If iStock.PinValue Mod 1 = 0 Then
                    iTemplate.TemplateText &= ", " & FormatNumber(iStock.PinValue, 0)
                Else
                    iTemplate.TemplateText &= ", " & FormatNumber(iStock.PinValue, 1)
                End If
            Next
            iList.Add(iTemplate)
            Return iList
        End Function
    End Class
End NameSpace