Imports Hot.Data

Namespace Brands
    Public Class NetworkExceptionResolver
        Public Shared Function GetErrorTemplateByStackTrace(narrative As String) As Integer
            If narrative Is Nothing Then
                 Return XTemplate.Templates.NetworkGeneralError
            Else If narrative.Contains("The operation has timed out") Then
                Return xTemplate.Templates.NetworkTimeout
            Else If narrative.Contains("The underlying connection was closed") Then
                Return xTemplate.Templates.NetworkConnectionIssue
            Else If narrative.Contains("Unable to connect to the remote server") Then
                Return xTemplate.Templates.NetworkWebserviceUnavailable
            Else If narrative.Contains("Server was unable to process request") Then
                Return xTemplate.Templates.NetworkWebserviceError
            Else
                Return -1
            End If
        End Function
    End Class
End NameSpace