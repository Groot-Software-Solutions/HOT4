Imports System.ComponentModel.DataAnnotations
Imports System.Data.SqlClient
Imports System.Net.Http.Headers
Imports Hot.Data
Imports WebGrease.Css.Extensions

Namespace Models
    Public Class Context

        Private Shared ReadOnly ConnString = Config.GetConnectionString()

        Private Shared Sub CheckHeaders(names As IEnumerable(Of String), headers As HttpRequestHeaders, missingHeaders As List(Of String))
            names.ForEach(Sub(name)
                              If Not headers.Contains(name) Then
                                  missingHeaders.Add(name)
                              End If
                          End Sub)
        End Sub

        Public Shared Function [Get](requestHeaders As HttpRequestHeaders) As Context

            Dim missingHeaders = New List(Of String)
            CheckHeaders(_headers, requestHeaders, missingHeaders)
            Dim context = New Context()

            If missingHeaders.Count > 0 Then
                Dim headerString = String.Join(", ", missingHeaders)
                context.Error = "The following headers are required: " & headerString
                Return context
            End If

            context.AccessCode = requestHeaders.GetValues(ACCESS_CODE).FirstOrDefault()
            context.AccessPassword = requestHeaders.GetValues(ACCESS_PASSWORD).FirstOrDefault()
            context.AgentReference = requestHeaders.GetValues(AGENT_REFERENCE).FirstOrDefault()


            Using sqlConn As New SqlConnection(ConnString)
                sqlConn.Open()
                Dim access = xAccessAdapter.SelectLogin(context.AccessCode, context.AccessPassword, sqlConn)
                If access IsNot Nothing
                    context.AccessId = access.AccessID
                    context.AccountId = access.AccountID
                Else
                    context.Error =  "Invalid x-access-code or x-access-password invalid."
                End If
            End Using

            return context
        End Function

        Public Property AccountId As Long?


        Public Property [Error] As String


        private Shared _headers = New String() {ACCESS_PASSWORD, ACCESS_CODE, AGENT_REFERENCE}

        Private Const AGENT_REFERENCE = "x-agent-reference"
        Private Const ACCESS_CODE = "x-access-code"
        Private Const ACCESS_PASSWORD = "x-access-password"

        <Required>
        <StringLength(50)>
        Public Property AgentReference As String
        <Required>
        Public Property AccessCode As String
        <Required>
        Public Property AccessPassword As String
        Public Property AccessId As Integer?

    End Class
End NameSpace