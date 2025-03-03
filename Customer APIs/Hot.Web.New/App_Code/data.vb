Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Common
Imports HotRetail
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports HOT5.Common
Imports System.Web.Script.Services
Imports System.Collections
Imports System.Collections.Generic

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://web.hot.co.zw/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class data
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnRecharge(ByVal AccessCode As String, ByVal AccessPassword As String, ByVal TargetMobile As String, ByVal Amount As Decimal) As String
        Dim ws As New HotRetail.ServiceSoapClient
        Dim result As HotRetail.ReturnObject
        Try
            result = ws.RetailRecharge(AccessCode, AccessPassword, TargetMobile, Amount)
            result.ReturnMsg = result.ReturnMsg.Replace(vbLf, ";")
            Return New JavaScriptSerializer().Serialize(result.ReturnMsg)
        Catch
            Return "Recharge Failed."
        End Try
    End Function

    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnGetSoldPins(ByVal AccessCode As String) As String

        Try
            Using sqlConn As New SqlConnection(Conn)
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(AccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")

                sqlConn.Open()

                Dim pinssold As New List(Of xPinSold)
                Dim sqlcmd As New SqlCommand("xWeb_SoldPins", sqlConn)
                sqlcmd.Parameters.AddWithValue("@AccessCode", Sanitize(CStr(AccessCode)))
                Using sqlRdr As SqlDataReader = sqlcmd.ExecuteReader
                    While sqlRdr.Read
                        Dim irow As New xPinSold(sqlRdr)
                        pinssold.Add(irow)
                    End While
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(pinssold)
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)

        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function fnGetSoldPinsAccount(accesscode As String, accesspassword As String) As String

        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectLogin(accesscode, accesspassword, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")



                Dim pinssold As New List(Of xPinSold)
                Dim sqlcmd As New SqlCommand("xWeb_SoldPins", sqlConn)
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure
                sqlcmd.Parameters.AddWithValue("AccessCode", cAccess.AccessCode)
                Using sqlRdr As SqlDataReader = sqlcmd.ExecuteReader
                    While sqlRdr.Read
                        Dim irow As New xPinSold(sqlRdr)
                        pinssold.Add(irow)
                    End While
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(pinssold)
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)

        End Try
    End Function

    <WebMethod()> _
       <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnOnlineCheck() As String
        Try
            Dim xStatus As New xSystemStatus
            ' Stage one Network Connectivity 
            Dim pingsender As New System.Net.NetworkInformation.Ping
            Dim options As New System.Net.NetworkInformation.PingOptions
            Dim data As String = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" '32 bytes of dataData
            Dim buffer = Encoding.ASCII.GetBytes(data)
            Dim timeout As Integer = 120
            Dim reply As System.Net.NetworkInformation.PingReply
            For x = 1 To 5
                reply = pingsender.Send(EconetIPAdd, timeout, buffer, options)
                If (reply.Status = System.Net.NetworkInformation.IPStatus.Success) Then Exit For
                If Not (reply.Status = System.Net.NetworkInformation.IPStatus.Success) And x = 5 Then
                    xStatus.Status = "System Offline"
                    xStatus.Message = "Connection to Recharge Service is Down"
                    Return New JavaScriptSerializer().Serialize(xStatus)
                Else
                    Threading.Thread.Sleep(250)
                End If

            Next



            ' Stage two Database Connectivity
            'Using sqlConn As New SqlConnection(Conn)
            '    Try
            '        sqlConn.Open()
            '        Dim test = xProfileAdapter.Discounts(11, sqlConn)
            '        sqlConn.Close()
            '    Catch ex As Exception
            '        xStatus.Status = "System Offline"
            '        xStatus.Message = "Connection to Database is Down"
            '        Return New JavaScriptSerializer().Serialize(xStatus)
            '    End Try                
            'End Using

            ' Success
            xStatus.Status = "Currently Online"
            xStatus.Message = "System is operating as normal."
            Return New JavaScriptSerializer().Serialize(xStatus)
        Catch
            Return "Error Loading Access User List."
        End Try
    End Function
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnOnlineCheckDetailed() As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim statuses As New List(Of xNetworkStatus)
                Dim sqlcmd As New SqlCommand("xErrorLog_WebOnlineDetail", sqlConn)
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure

                Using sqlRdr As SqlDataReader = sqlcmd.ExecuteReader
                    While sqlRdr.Read
                        Dim irow As New xNetworkStatus(sqlRdr)
                        statuses.Add(irow)
                    End While
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(statuses)
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnIsRegistered(ByVal AccessCode As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(AccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("unregistered")
                sqlConn.Close()
            End Using
            Throw New Exception("This email is already registered")
        Catch ex As Exception
            If ex.Message = "unregistered" Then Return New JavaScriptSerializer().Serialize("True")
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function

#Region "   POS Functions   "
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnGetTotalSales(ByVal AccessID As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectRow(AccessID, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")

                
                Dim sqlcmd As New SqlCommand("xWeb_GetTotalSales", sqlConn)
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure
                sqlcmd.Parameters.AddWithValue("@AccessID", Sanitize(CStr(AccessID)))
                Dim result As Decimal = sqlcmd.ExecuteScalar()
                Return New JavaScriptSerializer().Serialize(result)
                sqlConn.Close()
            End Using
            'Return "here"
        Catch ex As Exception
            Return "Unavailable"
        End Try
    End Function
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnGetBalance(ByVal AccessID As String, ByVal AccountID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Return New JavaScriptSerializer().Serialize(iAccount.Balance)
                sqlConn.Close()
            End Using

        Catch ex As Exception
            Return "Unavailable"
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function fnGetTotalSales_Access(ByVal AccessCode As String, AccessPassword As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectLogin(AccessCode, AccessPassword, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")

                Dim AccessID As Integer = cAccess.AccessID

                Dim sqlcmd As New SqlCommand("xWeb_GetTotalSales", sqlConn)
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure
                sqlcmd.Parameters.AddWithValue("@AccessID", AccessID)
                Dim result As Decimal = sqlcmd.ExecuteScalar()
                Return New JavaScriptSerializer().Serialize(result)
                sqlConn.Close()
            End Using
            'Return "here"
        Catch ex As Exception
            Return "Unavailable"
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function fnGetRecentTransactions(ByVal AccessCode As String, AccessPassword As String) As String
        Try
            Dim rows As New List(Of xReportItem_Retail)
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectLogin(AccessCode, AccessPassword, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")

                Dim AccessID As Integer = cAccess.AccessID

                Dim sqlcmd As New SqlCommand("xTransactions_Recent_Access", sqlConn)
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure
                sqlcmd.Parameters.AddWithValue("@AccessID", AccessID)
                sqlcmd.Parameters.AddWithValue("@Range", 5)

                Using sqlRdr As SqlDataReader = sqlcmd.ExecuteReader

                    While sqlRdr.Read

                        Dim irow As New xReportItem_Retail
                        irow.RechargeDate = sqlRdr("RechargeDate")
                        irow.Mobile = sqlRdr("Mobile")
                        irow.Amount = FormatNumber(sqlRdr("Amount"), 2)
                        irow.StateID = sqlRdr("StateID")
                        irow.RechargeID = sqlRdr("RechargeID")
                        irow.Discount = sqlRdr("Discount")
                        irow.BrandID = sqlRdr("BrandID")
                        irow.AccessID = sqlRdr("AccessID")
                        rows.Add(irow)
                    End While
                End Using
                sqlConn.Close()
            End Using

            Return New JavaScriptSerializer().Serialize(rows)

        Catch ex As Exception
            Return "Unavailable"
        End Try
    End Function



#End Region

#Region "   Subscriber Functions   "
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnAddSubscribers(ByVal AccountID As Long, ByVal AccessCode As String, ByVal Password As String, ByVal Name As String, ByVal Mobile As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(Common.Sanitize(AccessCode), sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")

                Dim iSubscriber As New xSubscriber
                Dim iNetwork As xNetwork = xNetwork_Data.Identify(Common.Sanitize(Mobile), sqlConn)
                iSubscriber.AccountID = AccountID
                iSubscriber.SubscriberMobile = Common.Sanitize(Mobile.Replace(" ", ""))
                iSubscriber.SubscriberName = Common.Sanitize(Name)
                iSubscriber.Brand = xBrandAdapter.Identify(iNetwork, "", sqlConn)
                iSubscriber.Active = True
                xSubscriberAdapter.Save(iSubscriber, sqlConn)
                sqlConn.Close()

            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnDeleteSubscribers(ByVal AccountID As Long, ByVal AccessCode As String, ByVal Password As String, ByVal SubscriberID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(AccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")

                Dim iSubscriber As xSubscriber = xSubscriberAdapter.SelectRow(SubscriberID, sqlConn)
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    xSubscriberAdapter.Delete(iSubscriber, sqlConn, sqlTrans)
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw New Exception(ex.Message)
                End Try
                sqlConn.Close()
            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveSubscribers(ByVal AccountID As Long, ByVal AccessCode As String, ByVal Password As String, ByVal Name As String, ByVal Mobile As String, ByVal SubscriberID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(AccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")

                Dim iSubscriber As xSubscriber = xSubscriberAdapter.SelectRow(SubscriberID, sqlConn)
                If iSubscriber Is Nothing Then Throw New Exception("Subscriber does not exist!")
                If Not (iSubscriber.AccountID = AccountID) Then Throw New Exception("User account is restricted!")

                Dim iNetwork As xNetwork = xNetwork_Data.Identify(Mobile, sqlConn)
                iSubscriber.SubscriberMobile = Mobile.Replace(" ", "")
                iSubscriber.SubscriberName = Name
                iSubscriber.Brand = xBrandAdapter.Identify(iNetwork, "", sqlConn)

                xSubscriberAdapter.Save(iSubscriber, sqlConn)
                sqlConn.Close()

            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnListSubscribers(ByVal AccountID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iSubscribers As List(Of xSubscriber) = xSubscriberAdapter.List(AccountID, sqlConn)
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iSubscribers)
            End Using
        Catch ex As Exception
            Return "Error Loading Subscriber List."
        End Try
    End Function

#End Region

#Region "   Access Users Functions   "
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnAddAccessUser(ByVal AccountID As Long, ByVal MainAccessCode As String, ByVal Password As String, ByVal Username As String, _
                                     ByVal ChannelID As Integer, ByVal AccessCode As String, ByVal AccessPassword As String, ByVal SalesPassword As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(MainAccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessID = xAccessAdapter.AdminSelect(AccountID, sqlConn)) Then Throw New Exception("User account is restricted!")

                Dim iAccess As New xAccess
                Dim iAccessWeb As New xAccessWeb
                iAccess.AccessID = 0
                iAccess.AccountID = AccountID
                iAccess.Channel.ChannelID = ChannelID

                iAccess.AccessCode = AccessCode
                iAccess.AccessPassword = AccessPassword
                xAccessAdapter.Save(iAccess, sqlConn)
                iAccess = xAccessAdapter.SelectLogin(iAccess.AccessCode, iAccess.AccessPassword, sqlConn)

                iAccessWeb.AccessID = iAccess.AccessID
                iAccessWeb.AccessName = Username
                iAccessWeb.WebBackground = ""
                iAccessWeb.SalesPassword = CBool(SalesPassword)
                xAccessWebAdapter.Save(iAccessWeb, sqlConn)

                sqlConn.Close()
            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            If InStr(ex.Message, "Cannot insert duplicate key row") > 0 Then
                Return New JavaScriptSerializer().Serialize("Account user already exists")
            Else
                Return New JavaScriptSerializer().Serialize(ex.Message)
            End If
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveAccessUser(ByVal AccountID As Long, ByVal MainAccessCode As String, ByVal Password As String, ByVal Username As String, _
                                     ByVal ChannelID As Integer, ByVal AccessCode As String, ByVal AccessPassword As String, ByVal AccessID As Long, ByVal SalesPassword As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(MainAccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessID = xAccessAdapter.AdminSelect(AccountID, sqlConn)) Then Throw New Exception("User account is restricted!")

                Dim iAccess As xAccess = xAccessAdapter.SelectRow(AccessID, sqlConn)
                If iAccess Is Nothing Then Throw New Exception("User Account does not exist!")
                If Not (iAccess.AccountID = AccountID) Then Throw New Exception("User account is restricted!")
                Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(AccessID, sqlConn)
                iAccess.Channel.ChannelID = ChannelID
                iAccess.AccessCode = AccessCode
                If AccessPassword <> "" Then iAccess.AccessPassword = AccessPassword
                xAccessAdapter.Save(iAccess, sqlConn)

                iAccessWeb.AccessID = iAccess.AccessID
                iAccessWeb.AccessName = Username
                iAccessWeb.SalesPassword = CBool(SalesPassword)

                xAccessWebAdapter.Save(iAccessWeb, sqlConn)

                sqlConn.Close()
            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveBackground(ByVal AccessCode As String, ByVal AccessPassword As String, ByVal Background As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()

                Dim iAccess As xAccess = xAccessAdapter.SelectCode(AccessCode, sqlConn)
                If iAccess Is Nothing Then Throw New Exception("User Account does not exist!")
                If Not (iAccess.AccessPassword = AccessPassword) Then Throw New Exception("User account is restricted!")

                Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAccess.AccessID, sqlConn)
                iAccessWeb.WebBackground = Background

                xAccessWebAdapter.Save(iAccessWeb, sqlConn)
                sqlConn.Close()
            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnChangeAccessUserStatus(ByVal AccountID As Long, ByVal MainAccessCode As String, ByVal Password As String, ByVal AccessID As Long, ByVal Status As Boolean) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccess As xAccess = xAccessAdapter.SelectCode(MainAccessCode, sqlConn)
                If cAccess Is Nothing Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessPassword = Password) Then Throw New Exception("Logon Failed!")
                If Not (cAccess.AccessID = xAccessAdapter.AdminSelect(AccountID, sqlConn)) Then Throw New Exception("User account is restricted!")

                Dim iAccess As xAccess = xAccessAdapter.SelectRow(AccessID, sqlConn)
                If iAccess Is Nothing Then Throw New Exception("User Account does not exist!")
                If Not (iAccess.AccountID = AccountID) Then Throw New Exception("User account is restricted!")
                If Status Then
                    Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                    Try
                        xAccessAdapter.Delete(iAccess, sqlConn, sqlTrans)
                        sqlTrans.Commit()
                    Catch ex As Exception
                        sqlTrans.Rollback()
                        Throw New Exception(ex.Message)
                    End Try
                Else
                    Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                    Try
                        xAccessAdapter.UnDelete(iAccess, sqlConn, sqlTrans)
                        sqlTrans.Commit()
                    Catch ex As Exception
                        sqlTrans.Rollback()
                        Throw New Exception(ex.Message)
                    End Try
                End If
                sqlConn.Close()
            End Using
            Return New JavaScriptSerializer().Serialize("Success")
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function

    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnPasswordRequired(ByVal AccessID As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim cAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(Sanitize(AccessID), sqlConn)
                If cAccessWeb Is Nothing Then Throw New Exception("Logon Failed!")
                Return New JavaScriptSerializer().Serialize(cAccessWeb.SalesPassword)
                sqlConn.Close()
            End Using

        Catch ex As Exception
            Return "Unavailable"
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnListAccessUsers(ByVal AccountID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccessWebAccounts As New List(Of xAccessWebService)
                Dim iAccessAccounts As List(Of xAccess) = xAccessAdapter.List(AccountID, sqlConn)
                For Each iAcc As xAccess In iAccessAccounts
                    Dim iAccessWebService As New xAccessWebService

                    Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAcc.AccessID, sqlConn)
                    If iAccessWeb Is Nothing Then
                        iAccessWeb = New xAccessWeb
                        iAccessWeb.AccessID = iAcc.AccessID
                        iAccessWeb.AccessName = iAcc.AccessCode
                        iAccessWeb.WebBackground = ""
                    End If
                    iAccessWebService.Access = iAcc
                    iAccessWebService.AccessWeb = iAccessWeb



                    iAccessWebAccounts.Add(iAccessWebService)
                Next
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iAccessWebAccounts)
            End Using
        Catch ex As Exception
            Return "Error Loading Access User List."
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnListAccessUsers_Deleted(ByVal AccountID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccessWebAccounts As New List(Of xAccessWebService)
                Dim iAccessAccounts As List(Of xAccess) = xAccessAdapter.ListDeleted(AccountID, sqlConn)
                For Each iAcc As xAccess In iAccessAccounts
                    Dim iAccessWebService As New xAccessWebService
                    Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAcc.AccessID, sqlConn)

                    iAccessWebService.Access = iAcc
                    iAccessWebService.AccessWeb = iAccessWeb

                    iAccessWebAccounts.Add(iAccessWebService)
                Next
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iAccessWebAccounts)
            End Using
        Catch
            Return "Error Loading Access User List."
        End Try
    End Function
#End Region

#Region "   Account Functions   "
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSelectAccount(ByVal AccountID As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As New List(Of xAccountWeb)
                Dim iAccountWeb As New xAccountWeb
                iAccountWeb.Account = xAccountAdapter.SelectRow(AccountID, sqlConn)
                iAccountWeb.Address = xAddressAdapter.SelectRow(AccountID, sqlConn)
                iAccount.Add(iAccountWeb)
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iAccount)
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveAccount(ByVal AccountID As Long, ByVal AccountName As String, ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal NationalID As String, ByVal ContactName As String, ByVal ContactNumber As String, ByVal VatNumber As String, ByVal Email As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Dim iAddress As xAddress
                Try
                    iAddress = xAddressAdapter.SelectRow(AccountID, sqlConn)
                Catch ex As Exception
                    iAddress = New xAddress
                End Try

                iAccount.AccountName = AccountName
                iAccount.NationalID = NationalID
                iAccount.Email = Email
                iAddress.AccountID = AccountID
                iAddress.Address1 = Address1
                iAddress.Address2 = Address2
                iAddress.City = City
                iAddress.ContactName = ContactName
                iAddress.ContactNumber = ContactNumber
                iAddress.VatNumber = VatNumber

                xAddressAdapter.Save(iAddress, sqlConn)
                xAccountAdapter.Save(iAccount, sqlConn)

                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize("Success")
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveStatus(ByVal AccountID As Long, ByVal status As Long) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Dim iAddress As xAddress
                Try
                    iAddress = xAddressAdapter.SelectRow(AccountID, sqlConn)
                Catch ex As Exception
                    iAddress = New xAddress
                End Try

                Select Case status
                    Case 1
                        If iAddress.Latitude = 0 Then iAddress.Latitude = 1
                    Case 2
                        If iAddress.Latitude = 1 Then iAddress.Latitude = 2
                    Case 3
                        If iAddress.Latitude = 2 Then iAddress.Latitude = 3
                    Case Else

                End Select

                iAddress.AccountID = AccountID
                xAddressAdapter.Save(iAddress, sqlConn)
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize("Success")
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)

        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSendVerificationEmail(ByVal AccountID As Long, ByVal Email As String) As String
        Try
            If Not EmailAddressCheck(Email) Then Return New JavaScriptSerializer().Serialize("Invalid Email")
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim checkpreviouslyverified As New SqlCommand("select count(1)  from tblaccount where email = '" + Email + "' and accountid <> " + CStr(AccountID), sqlConn)
                If CInt(checkpreviouslyverified.ExecuteScalar()) > 0 Then Return New JavaScriptSerializer().Serialize("Duplicate Email")

                Dim sendmail As New SqlCommand("exec z_spSendMailVerification " + CStr(AccountID) + ", '" + Email + "'", sqlConn)
                sendmail.ExecuteScalar()
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize("Email Success Sent")
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)

        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSendResetEmail(ByVal Email As String) As String
        Try
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            Dim iAccess As xAccess = xAccessAdapter.SelectCode(Email, sqlConn)
            If iAccess Is Nothing Then Return New JavaScriptSerializer().Serialize("Invalid Login email")
            Dim sendmail As New SqlCommand("exec z_spSendResetEmail " + CStr(iAccess.AccessID), sqlConn)
            sendmail.ExecuteScalar()
            sqlConn.Close()
            Return New JavaScriptSerializer().Serialize("Email Success Sent")
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnSaveEmail(ByVal AccountID As Long, ByVal Email As String) As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Dim iAddress As xAddress
                Try
                    iAddress = xAddressAdapter.SelectRow(AccountID, sqlConn)
                Catch ex As Exception
                    iAddress = New xAddress
                End Try
                If EmailAddressCheck(Email) Then
                    iAccount.Email = Email
                    iAddress.Latitude = 2
                End If
                xAddressAdapter.Save(iAddress, sqlConn)

                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize("Success")
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try
    End Function
    <WebMethod()> _
     <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnCompletePromo(ByVal AccountID As Long, ByVal Target As String) As String
        Try
            Target = Target.Replace("+263", "0").Replace(" ", "")
            If Not (Target.StartsWith("077") Or Target.StartsWith("071")) Then Return New JavaScriptSerializer().Serialize("Econet and Netone Number Only")
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim checksamenumber As New SqlCommand("select count(1) from tbladdress where replace(replace(replace(contactnumber,'+263','0') ,' ','') ,'-','') = '" + Target + "' and accountid <> " + CStr(AccountID), sqlConn)
                If CInt(checksamenumber.ExecuteScalar()) > 0 Then Return New JavaScriptSerializer().Serialize("Number is also used in another Account")

                Dim checkprevious As New SqlCommand("select count(1) from tblrecharge where accessid  = 10547451 and replace(mobile,'+263','0')  = '" + Target + "'", sqlConn)
                If CInt(checkprevious.ExecuteScalar()) > 0 Then Return New JavaScriptSerializer().Serialize("Number has been Used in this Promotion before")


                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Dim iAddress As xAddress
                Try
                    iAddress = xAddressAdapter.SelectRow(AccountID, sqlConn)
                Catch ex As Exception
                    iAddress = New xAddress
                End Try
                iAddress.Latitude = 3
                iAddress.ContactNumber = Target
                xAddressAdapter.Save(iAddress, sqlConn)

                sqlConn.Close()
                Dim ws As New HotRetail.ServiceSoapClient
                Dim result As HotRetail.ReturnObject
                Try
                    result = ws.RetailRecharge("fb@hot.co.zw", "12HOT789^^!!*)//=ZXvf", Target, 0.1)
                    If Not LCase(result.ReturnMsg).Contains("success") Then GoTo Senderror
                Catch
                    GoTo senderror
                End Try
                Return New JavaScriptSerializer().Serialize("Success")
                Exit Function
Senderror:
                Using sqlConn2 As New SqlConnection(Conn)
                    sqlConn2.Open()

                    Dim emailsql As String = ""
                    emailsql = "Declare	@Sub VarChar(80), @html Varchar(max), @AccountID Bigint, @email varchar(500)" + vbNewLine + _
                        "select @AccountID = " + CStr(AccountID) + " @Sub = 'Facebook Page Promotion Recharge Failure'" + vbNewLine + _
                        "set @html = 'This User finished the promotion process but airtime was not sent to them. Please check if it hasnt been sent before and then credit them manually using the fb@hot.co.zw account <br/><br/>' + (select AccountName from tblAccount where accountid = @AccountID) + <br/>" + Target + _
                         "EXEC msdb.dbo.sp_send_dbmail " + _
                        "@recipients = 'register@hot.co.zw',@subject = @Sub,@body=@html,@body_format='HTML',@profile_name ='Frampol'"

                    Dim sendmail As New SqlCommand(emailsql, sqlConn2)
                    sendmail.ExecuteScalar()
                    sqlConn2.Close()
                    Return New JavaScriptSerializer().Serialize("Recharge Failed but will be sent within the hour")
                End Using
            End Using
        Catch ex As Exception
            Return New JavaScriptSerializer().Serialize(ex.Message)
        End Try

    End Function

#End Region

#Region "   Reports Functions   "
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnAccountTransactions(ByVal AccountID As Long, ByVal AccessCode As String, ByVal AccessPassword As String, ByVal StartDate As String, ByVal EndDate As String, ByVal Filter As String) As String
        Try

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim ds As New List(Of xReportItem)

                Dim iSubscribers As List(Of xSubscriber) = xSubscriberAdapter.List(AccountID, sqlConn)
                Using sqlCmd As New SqlCommand("xTransactions_Date_Access", sqlConn)
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure
                    sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
                    sqlCmd.Parameters.AddWithValue("StartDate", Format(CDate(StartDate), "dd MMM yyyy"))
                    sqlCmd.Parameters.AddWithValue("EndDate", Format(CDate(EndDate), "dd MMM yyyy"))
                    sqlCmd.Parameters.AddWithValue("filter", Filter)
                    Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                        While sqlRdr.Read
                            If IsNumeric(sqlRdr("Reference")) Then
                                Dim irow As New xReportItem
                                irow.ItemType = 1
                                irow.TransactionDate = Format(sqlRdr("TransactionDate"), "dd MMM yyyy")
                                irow.Mobile = sqlRdr("Reference")
                                irow.Amount = FormatNumber(sqlRdr("Amount"), 2)
                                irow.State = sqlRdr("State")
                                irow.AccessCode = sqlRdr("AccessCode")
                                irow.AccessName = sqlRdr("AccessName")

                                For Each iSub As xSubscriber In iSubscribers
                                    If iSub.SubscriberMobile = sqlRdr("Reference") Then irow.Mobile = iSub.SubscriberName : Exit For
                                Next
                                ds.Add(irow)
                            Else
                                Dim irow As New xReportItem
                                irow.ItemType = 2
                                irow.TransactionDate = Format(sqlRdr("TransactionDate"), "dd MMM yyyy")
                                irow.Reference = sqlRdr("Reference")
                                irow.Amount = FormatNumber(sqlRdr("Amount"), 2)
                                irow.State = sqlRdr("State")
                                irow.TranType = sqlRdr("AccessCode")
                                irow.Source = sqlRdr("AccessName")
                                ds.Add(irow)
                            End If

                        End While
                    End Using
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(ds)
            End Using
        Catch ex As Exception
            Return "Error Loading Report" + ex.Message
        End Try
    End Function
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnAccountTransactions_Retail(ByVal AccountID As Long, ByVal AccessCode As String, ByVal AccessPassword As String, ByVal StartDate As String, ByVal EndDate As String) As String
        Try

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                ' Dim ds As New List(Of xReportItem_Retail)
                Dim lines As String = ""
                Dim sdate = CDate(StartDate)
                Dim edate = CDate(EndDate)
                Using sqlCmd As New SqlCommand("xRecharge_AggregatorSelect", sqlConn)
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure
                    sqlCmd.Parameters.AddWithValue("@AccountID", AccountID)
                    sqlCmd.Parameters.AddWithValue("@StartDate", sdate)
                    sqlCmd.Parameters.AddWithValue("@EndDate", edate)


                    Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader

                        While sqlRdr.Read
                            lines +=
                                CStr(Format("dd-MMM-yy hh:mm:ss", sqlRdr("RechargeDate"))) _
                                + "," + sqlRdr("Mobile") _
                                + "," + CStr(FormatNumber(sqlRdr("Amount"), 2, TriState.True,, TriState.False)) _
                                + "," + CStr(sqlRdr("StateID")) _
                                + "," + CStr(sqlRdr("RechargeID")) _
                                + "," + CStr(sqlRdr("Discount")) _
                                + "," + CStr(sqlRdr("BrandID")) _
                                + "," + CStr(sqlRdr("AccessID")) + vbCr



                        End While
                    End Using
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(lines)
            End Using
        Catch ex As Exception
            Return "Error Loading Report" + ex.Message
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function fnAccountStatement_Retail(ByVal AccountID As Long) As String
        Try

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                ' Dim ds As New List(Of xReportItem_Retail)
                Dim lines As New List(Of xReportStatmentItem)
                Using sqlCmd As New SqlCommand("zStatment", sqlConn)
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure
                    sqlCmd.Parameters.AddWithValue("AccountID", AccountID)

                    Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader

                        While sqlRdr.Read
                            lines.Add(New xReportStatmentItem With {
                                .tdate = sqlRdr("tdate"),
                                .trantype = sqlRdr("trantype"),
                                .id = sqlRdr("id"), .reference = sqlRdr("reference"),
                                .amount = sqlRdr("Amount"), .cost = sqlRdr("cost"), .balance = sqlRdr("balance")
                                })

                        End While
                    End Using
                End Using
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(lines)
            End Using
        Catch ex As Exception
            Return "Error Loading Report" + ex.Message
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function fnListBrands() As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iBrands As List(Of xBrand) = xBrandAdapter.List(sqlConn)
               
                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iBrands)
            End Using
        Catch
            Return "Error Loading BrandsList."
        End Try
    End Function
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function fnListStatus() As String
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iStates As List(Of xState) = xState_Data.List(sqlConn)

                sqlConn.Close()
                Return New JavaScriptSerializer().Serialize(iStates)
            End Using
        Catch
            Return "Error Loading BrandsList."
        End Try
    End Function

    Structure xReportStatmentItem

        Public tdate As Date
        Public trantype As String
        Public id As Integer
        Public reference As String
        Public amount As Decimal
        Public cost As Decimal
        Public balance As Decimal

    End Structure
    Structure xReportItem_Retail
        Public RechargeID As Long
        Public RechargeDate As DateTime
        Public AccessID As Long
        Public Mobile As String
        Public BrandID As Integer
        Public Amount As Decimal
        Public Discount As Decimal
        Public StateID As Integer
    End Structure
#End Region





End Class

Public Class xAccessWebService
    Public Access As xAccess
    Public AccessWeb As xAccessWeb
End Class

Public Class xAccountWeb
    Public Account As xAccount
    Public Address As xAddress
End Class

Public Class xPinSold
    Public PinExpiry As String
    Public RechargeDate As String
    Public RechargeID As Long
    Public Mobile As String
    Public BrandName As String
    Public Pin As String
    Public PinRef As String
    Public PinValue As String

   

    Sub New(ByVal sqlrdr As SqlDataReader)
        Me.RechargeID = sqlrdr("RechargeID")
        Me.Mobile = sqlrdr("Mobile")
        Me.RechargeDate = Format(sqlrdr("RechargeDate"), "dd-MMM hh:mm")
        Me.BrandName = sqlrdr("Brandname")
        Me.Pin = sqlrdr("Pin")
        Me.PinRef = sqlrdr("PinRef")
        Me.PinExpiry = Format(sqlrdr("PinExpiry"), "dd-MMM-yy")
        Me.PinValue = Format(sqlrdr("PinValue"), "0.00")
      
       
    End Sub

End Class

Public Class xNetworkStatus
    Public Name As String
    Public StatusID As Integer

    Sub New(ByVal sqlrdr As SqlDataReader)
        Me.Name = sqlrdr("Network")
        Me.Statusid = sqlrdr("HasErrors")
    End Sub

End Class