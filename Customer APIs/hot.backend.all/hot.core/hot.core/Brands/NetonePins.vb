Imports System.Data.SqlClient
Imports System.Reflection
Imports Hot.Core.Brands
Imports Hot.Core.NetoneAPI
Imports Hot.Core.ServiceWrappers
Imports Hot.Data
Imports NSubstitute

Public Class NetonePins
    Inherits NetworkBase(Of NetoneSoap)


    Private Const HotAPIKey As String = "Hot263180"
    Private Const TimeoutPeriod As Integer = 180000

    Public Sub New(sqlConn As SqlConnection, applicationName As String, serviceEndpoint As String, isTestMode As Boolean, referencePrefix As String, webServiceOrCorporate As Boolean)
        MyBase.New(sqlConn, applicationName, serviceEndpoint, isTestMode, webServiceOrCorporate, referencePrefix)
    End Sub


    Private Sub CreditSubscriber(recharge As xRecharge, rechargePrepaid As xRechargePrepaid, numberOfDays As Integer)
        Dim creditRequest As BulkPinSaleRequest = CreateCreditRequest(recharge, rechargePrepaid, numberOfDays)
        Dim client = GetNetworkClient(TimeoutPeriod)
        Try
            Console.WriteLine("Crediting")
            Dim netoneClient = client.GetNetwork()
            Dim Response = netoneClient.BulkPinSale(creditRequest).Body.BulkPinSaleResult
            xLog_Data.WriteJsonToConsole(Response)

            Console.WriteLine("Response from Netone " & Response.ToString)
            If Not HasTimeoutErrors(Response.ReplyMsg) Then
                SetRechargePrepaid(recharge, rechargePrepaid, numberOfDays, Response)

            Else
                'QueryRecharge(recharge, rechargePrepaid, numberOfDays, client)hriow 

                Throw New Exception(Response.ReplyMsg)
            End If
            If recharge.IsSuccessFul() Then
                SavePins(Response.Pins, recharge, SqlConn.ConnectionString)
            End If
        Catch ex As Exception

            client.Abort()
                Console.WriteLine("Credit Exception")
                xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                                   SqlConn.ConnectionString, "RechargeID", recharge.RechargeID)
                recharge.State.StateID = xState.States.Failure
                rechargePrepaid.Narrative = "NetoneAPI Error: " & ex.Message

        Finally
            client.Close()
        End Try
    End Sub

    Private Sub SavePins(pins As ArrayOfString, recharge As xRecharge, connectionString As String)
        Using sqlConn As New SqlConnection(connectionString)
            sqlConn.Open()
            Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
            Try
                'Batch
                Dim iBatch As New xPinBatch
                iBatch.PinBatch = "NetoneAPI-PinBatch-" + pins(0).Split(",")(1).Split("|")(0)
                iBatch.PinBatchType = New xPinBatchType()
                iBatch.PinBatchType.PinBatchTypeID = xPinBatchType.PinBatchTypes.NetOne
                xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)
                Dim iPins = New List(Of xPin)
                For Each pin In pins
                    Dim iPin As xPin = GetPin(pin, recharge, iBatch)
                    If Not String.IsNullOrEmpty(iPin.Pin) Then
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                        iPins.Add(iPin)
                    End If
                Next
                Dim RP = New DataTable()
                RP.Columns.Add("RechargeID")
                RP.Columns.Add("PinID")
                For Each ipin In iPins
                    Dim dr = RP.NewRow()
                    dr("PinID") = ipin.PinID
                    dr("RechargeID") = recharge.RechargeID
                    RP.Rows.Add(dr)
                Next

                Dim objbulk = New SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTrans)
                objbulk.DestinationTableName = "tblRechargePin"
                objbulk.ColumnMappings.Add("RechargeID", "RechargeID")
                objbulk.ColumnMappings.Add("PinID", "PinID")
                objbulk.WriteToServer(RP)
                sqlTrans.Commit()
            Catch ex As Exception
                sqlTrans.Rollback()
                'Throw ex
            Finally
                sqlConn.Close()
            End Try
        End Using
    End Sub

    Private Function GetPin(body As String, recharge As xRecharge, iBatch As xPinBatch) As xPin
        Dim iPin As New xPin
        Try
            ' Sample format - "0030855480903599,000309176729|58,1.00,3/5/2022"
            iPin.Brand = New xBrand
            iPin.Brand = recharge.Brand

            iPin.Pin = body.Split(",")(0)
            iPin.PinRef = body.Split(",")(1)
            iPin.PinBatch = iBatch
            iPin.PinExpiry = CDate(body.Split(",")(3))
            iPin.PinState = New xPinState
            iPin.PinState.PinStateID = xPinState.PinStates.SoldFileExport
            iPin.PinValue = body.Split(",")(2)
        Catch ex As Exception
            Return New xPin()
        End Try 'Brand

        Return iPin
    End Function

    Private Function CreateCreditRequest(recharge As xRecharge, iRechargePrepaid As xRechargePrepaid,
                                             numberOfDays As Integer) As BulkPinSaleRequest
        Return New BulkPinSaleRequest With {
                .Body = New BulkPinSaleRequestBody With {
                .RechargeID = recharge.RechargeID,
                .Quantity = recharge.Quantity,
                .Denomination = recharge.Denomination,
                .BrandId = GetNetonePinBrand(recharge.Brand.BrandID),
                .AppKey = HotAPIKey
            }}
    End Function

    Private Function GetNetonePinBrand(brandID As Integer) As Integer
        Dim result As Integer = 0
        Select Case brandID
            Case xBrand.Brands.EasyCallEVD
                result = 17
            Case xBrand.Brands.NetoneOneFusion
                result = 18
            Case xBrand.Brands.NetoneOneFi
                result = 19
        End Select
        Return result
    End Function

    Public Overrides Function RechargePrepaid(recharge As xRecharge, Optional numberOfDays As Integer = 90) As ServiceRechargeResponse
        ApplyDiscountRules(recharge)
        Dim iRechargePrepaid As xRechargePrepaid = CreateRechargeObject(recharge)
        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)

        If iRechargePrepaid.DebitCredit Then
            CreditSubscriber(recharge, iRechargePrepaid, numberOfDays)
        Else
            'Debit not possible on Netone 
            iRechargePrepaid.Narrative = "Bundle Debit not possible"
            iRechargePrepaid.ReturnCode = "099"
            recharge.State.StateID = xState.States.Failure
        End If
        Console.WriteLine("Saving prepaid")
        xRechargePrepaidAdapter.Save(iRechargePrepaid, SqlConn)
        Console.WriteLine("Saving Recharge")
        xRechargeAdapter.Save(recharge, SqlConn)
        Return New ServiceRechargeResponse(iRechargePrepaid)
    End Function

    Private Shared Function HasTimeoutErrors(Response As String) As Boolean
        If Response.Contains("request channel timed out") Then Return True
        If Response.Contains("task was canceled") Then Return True
        Return False
    End Function

    Private Shared Sub SetRechargePrepaid(ByRef recharge As xRecharge, ByRef rechargePrepaid As xRechargePrepaid, numberOfDays As Integer, Response As NetoneAPI.BulkEvdResponse)
        rechargePrepaid.Narrative = Response.ReplyMsg + String.Join(";", Response.Pins.ToList())
        rechargePrepaid.ReturnCode = Response.ReplyCode
        'rechargePrepaid.Reference = response.AgentReference
        rechargePrepaid.FinalBalance = 0
        rechargePrepaid.InitialBalance = 0
        rechargePrepaid.FinalWallet = Response.WalletBalance

        rechargePrepaid.Window = Date.Now
        rechargePrepaid.DebitCredit = True
        rechargePrepaid.RechargeID = recharge.RechargeID
        rechargePrepaid.Window = DateAdd(DateInterval.Day, numberOfDays, Date.Now)
        recharge.State.StateID = IIf(Response.ReplyCode = xState.States.Success, xState.States.Success,
                                         xState.States.Failure)
        recharge.RechargeDate = Now
        rechargePrepaid.Data = 0
    End Sub


    Public Function QueryEVDTransaction(NetoneRechargeID As String) As QueryEvdResponse

    End Function

    Public Function QueryEVDStock() As List(Of PinStockModel)
        Dim request As QueryEvdStockRequest = New QueryEvdStockRequest(New QueryEvdStockRequestBody(HotAPIKey))
        Dim client = GetNetworkClient(TimeoutPeriod)
        Try
            Dim netoneClient = client.GetNetwork()
            Dim Response = netoneClient.QueryEvdStock(request).Body.QueryEvdStockResult
            Dim items = New List(Of PinStockModel)

            For Each stock In Response.InStock
                items.Add(New PinStockModel() With {
                    .BrandId = GetHotBrandID(stock.BrandId),
                    .BrandName = stock.BrandName,
                    .PinValue = stock.PinValue,
                    .Stock = stock.Stock
                            })
            Next

            Return items.Where(Function(i)
                                   Return Not (i.BrandId = 0 Or i.BrandId = 25)
                               End Function).ToList()

        Catch ex As Exception

            client.Abort()
            Console.WriteLine("Credit Exception")
            xLog_Data.Save(ApplicationName, [GetType]().Name, MethodBase.GetCurrentMethod().Name, ex,
                               SqlConn.ConnectionString, "RechargeID", 0)
            Return Nothing
        Finally
            client.Close()
        End Try
    End Function

    Private Function GetHotBrandID(brandId As Integer) As Integer
        Dim result As Integer = 0
        Select Case brandId
            Case 17
                result = xBrand.Brands.EasyCallEVD
            Case 18
                result = xBrand.Brands.NetoneOneFusion
            Case 19
                result = xBrand.Brands.NetoneOneFi
        End Select
        Return result
    End Function

    Public Overrides Function GetNetworkClient(Optional timeout As Integer = 30000) As INetworkClient(Of NetoneSoap)
        If IsTestMode Then
            Return GetTestClient()
        End If
        Return NetworkClientFactory.Create(Of NetoneSoap)(ServiceEndpoint, timeout)
    End Function

    Private Function GetTestClient() As INetworkClient(Of NetoneSoap)
        Dim netone As NetoneSoap = Substitute.For(Of NetoneSoap)
        ' Change Response Code to anything else but 1 to test failure
        Dim response = New NetoneAPI.RechargeMobileResponse() _
                    With {
                        .Body = New RechargeMobileResponseBody() With {
                            .RechargeMobileResult = New NetoneAPI.RechargeResponse() With {
                                .ReplyCode = 2,
                                .ReplyMessage = "Narrative test",
                                .InitialBalance = 1,
                                .FinalBalance = 1,
                                .Amount = 1,
                                .RechargeID = 1000000,
                                .ReplyMsg = "Test",
                                .AgentReference = "hjkk"
                            }
                        }
                    }
        netone.RechargeMobile(Arg.Any(Of RechargeMobileRequest)).Returns(response)
        Dim fakeFactory = Substitute.For(Of INetworkClient(Of NetoneSoap))()
        fakeFactory.GetNetwork().Returns(netone)
        Return fakeFactory
    End Function

    Protected Overrides Function Name() As String
        Return "NetOne"
    End Function

End Class
