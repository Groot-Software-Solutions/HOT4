
Imports System.Data.SqlClient

Public Class xRawRequest

    Public Sub New ()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Headers = reader("Headers")
        Method = reader("Method")
        AbsoluteUri = reader("AbsoluteUri")
        Body = reader("Body")
        StatusCode = reader("StatusCode")
        ResponseBody = reader("ResponseBody")
        AgentReference = reader("AgentReference")
    End Sub

    Public Property Headers As String
    Public Property Method As String
    Public Property AbsoluteUri As String
    Public Property Body As String
    Public Property StatusCode As String
    Public Property ResponseBody As String
    Public Property AgentReference As String

End Class