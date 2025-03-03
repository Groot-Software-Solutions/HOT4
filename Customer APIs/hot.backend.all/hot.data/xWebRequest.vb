
Imports System.Data.SqlClient
Imports System.Linq

Public Class xWebRequest
    Private _cost1 As Decimal?

    Sub New ()

    End Sub

    Sub New (accessId As Long, agentReference As string)
        Me.AccessID = accessId
        Me.AgentReference = agentReference
    End Sub

    Public Property WebID As Long = 0
    Public Property HotTypeID As Short = 1
    Public Property AccessID As Long? = new Long?
    Public Property StateID As Short = 1
    Public Property InsertDate As Date
    Public Property ReplyDate As Date?
    Public Property AgentReference As String = ""
    Public Property Reply As String = ""
    Public Property ReturnCode As Integer = 0
    Public Property ChannelID As Short = 2
    Public Property RechargeID As Long?
    Public Property WalletBalance As Decimal?

    Public ReadOnly Property Cost As Decimal?
        Get
            If RechargeId Is Nothing Then return new Decimal?
            If Amount Is Nothing Then Return  new Decimal?
            If Discount Is Nothing Then Return Amount
            Return Amount*(1-Discount/100)
        End Get
    End Property

    Public Property Discount As Decimal?
    Public Property Amount As Decimal?

End Class

Public Class WebRequestService
    Private ReadOnly _sqlConn As String

    Public Sub New(sqlConn As String)
        _sqlConn = sqlConn
    End Sub

    Public Function GetRequest(agentReference As String, accessId As Long)  As xWebRequest
        Using sqlCon As New SqlConnection(_sqlConn)
            sqlCon.Open()
            Using command As New SqlCommand("xWebRequest_Select", sqlCon)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddWithValue("@AgentReference", AgentReference)
                command.Parameters.AddWithValue("@AccessID", AccessID)
                Dim reader As SqlDataReader = command.ExecuteReader
                Dim results As New List(Of xWebRequest)
                While reader.Read
                    results.Add(Read(reader))
                End While
                If results.Count = 0 Then
                    Return New xWebRequest() With {
                        .Reply = "Transaction not found."    
                    }
                End If
                Return results.First()
            End Using
        End Using
    End Function

    Public Sub Save(req As xWebRequest, isRequest As Boolean)
        Using sqlCon As New SqlConnection(_sqlConn)
            sqlCon.Open()
            Using command As New SqlCommand("xWebRequest_Save", sqlcon)
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.AddWithValue("@HotTypeID", req.HotTypeID)
                AddNullableParam("@AccessID", req.AccessID, command)
             
                command.Parameters.AddWithValue("@StateID", req.StateID)
                command.Parameters.AddWithValue("@AgentReference", req.AgentReference)
                command.Parameters.AddWithValue("@ReturnCode", req.ReturnCode)
                command.Parameters.AddWithValue("@ChannelID", req.ChannelID)
                command.Parameters.AddWithValue("@RechargeID", req.RechargeID)
                command.Parameters.AddWithValue("@IsRequest", isRequest)
                command.Parameters.AddWithValue("@Reply", req.Reply)

                 AddNullableParam("@WalletBalance", req.WalletBalance, command)
                 AddNullableParam("@Cost", req.Cost, command)
                 AddNullableParam("@Discount", req.Discount, command)
                 AddNullableParam("@Amount", req.Amount, command)

                command.ExecuteScalar()
            End Using
        End Using
    End Sub

    Private Sub AddNullableParam(name As String, value As Object, command As SqlCommand)
           If value IsNot Nothing
                    command.Parameters.AddWithValue(name, value)
                Else 
                    command.Parameters.AddWithValue(name, DBNull.Value)
                 End If
    End Sub

        Private Function Read(reader As SqlDataReader) As xWebRequest
            Dim req As xWebRequest = new xWebRequest
            req.ChannelID = IIf(IsDBNull(Reader.Item("ChannelID")), 0, CInt(Reader.Item("ChannelID")))
            if IsDBNull(Reader.Item("AccessID")) Then
                req.AccessId =  new Long?
            Else 
                req.AccessId = DirectCast(Reader.Item("AccessID"), Long?)
            End If

            req.Amount = ReadNullableDecimal("Amount", Reader)
            req.Discount = ReadNullableDecimal("Discount", Reader)
            req.WalletBalance = ReadNullableDecimal("WalletBalance", Reader)

            req.AgentReference = IIf(Not Equals(Reader.Item("AgentReference"), DBNull.Value),
                                     CStr(Reader.Item("AgentReference")), "")
            req.HotTypeID = IIf(Not Equals(Reader.Item("HotTypeID"), DBNull.Value), CInt(Reader.Item("HotTypeID")), 0)
            req.InsertDate = IIf(Not Equals(Reader.Item("InsertDate"), DBNull.Value), CDate(Reader.Item("InsertDate")),
                                 Date.Now)

            If Not IsDBNull(Reader.Item("RechargeID")) Then
                req.RechargeID =  CLng(Reader.Item("RechargeID"))
            End If

            IF Not IsDBNull(Reader.Item("Reply")) Then 
                req.Reply = CStr(Reader.Item("Reply"))
            End If

            If not IsDBNull(Reader.Item("ReplyDate")) Then
                 req.ReplyDate =  CDate(Reader.Item("ReplyDate"))
            End If

            If not IsDBNull(Reader.Item("ReturnCode")) Then
                 req.ReturnCode =  CInt(Reader.Item("ReturnCode"))
            End If

            If not IsDBNull(Reader.Item("StateID")) Then
                 req.StateID =  CInt(Reader.Item("StateID"))
            End If

            If not IsDBNull(Reader.Item("WebID")) Then
                 req.WebID =  CInt(Reader.Item("WebID"))
            End If

            Return req
    End Function

        Private Shared Function ReadNullableDecimal(name As String, reader As SqlDataReader) As Decimal?
          if IsDBNull(Reader.Item(name)) Then
                return new Decimal?
            Else 
                return DirectCast(Reader.Item(name), Decimal?)
            End If
        End Function


End Class