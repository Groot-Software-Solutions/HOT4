Imports System.Data.SqlClient

Public Class xRawRequestAdapter
    Private ReadOnly _connString As String

    Public Sub New(connString As String)
        _connString = connString
    End Sub

    Public Sub SaveRequest(agentReference As String, accessCode As String, headers As String, absoluteUri As String,
                           method As String, body As String)
        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Using sqlCmd As New SqlCommand("xRawRequest_Save", sqlConn)
                sqlCmd.CommandType = CommandType.StoredProcedure
                sqlCmd.Parameters.AddWithValue("IsRequest", True)
                sqlCmd.Parameters.AddWithValue("AgentReference", agentReference)
                sqlCmd.Parameters.AddWithValue("AccessCode", accessCode)
                sqlCmd.Parameters.AddWithValue("Headers", headers)
                sqlCmd.Parameters.AddWithValue("AbsoluteUri", absoluteUri)
                sqlCmd.Parameters.AddWithValue("Method", method)
                sqlCmd.Parameters.AddWithValue("Body", body)
                sqlCmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub SaveResponse(agentReference As String, accessCode As String, responseBody As String, statusCode As String)
        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Using sqlCmd As New SqlCommand("xRawRequest_Save", sqlConn)
                sqlCmd.CommandType = CommandType.StoredProcedure
                sqlCmd.Parameters.AddWithValue("AgentReference", agentReference)
                sqlCmd.Parameters.AddWithValue("AccessCode", accessCode)
                sqlCmd.Parameters.AddWithValue("IsRequest", False)
                sqlCmd.Parameters.AddWithValue("ResponseBody", responseBody)
                sqlCmd.Parameters.AddWithValue("StatusCode", statusCode)
                sqlCmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function GetEntry(agentReference As String, accessCode As String) As xRawRequest
        Dim rawRequest As xRawRequest = Nothing
        Using sqlConn As New SqlConnection(_connString)
            sqlConn.Open()
            Using sqlCmd As New SqlCommand("xRawRequest_Select", sqlConn)
                sqlCmd.CommandType = CommandType.StoredProcedure
                sqlCmd.Parameters.AddWithValue("AgentReference", agentReference)
                sqlCmd.Parameters.AddWithValue("AccessCode", accessCode)
                Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                    If sqlRdr.Read() Then
                        rawRequest = New xRawRequest(sqlRdr)
                        sqlRdr.Close()
                    End If
                End Using
            End Using
        End Using
        Return rawRequest
    End Function
End Class