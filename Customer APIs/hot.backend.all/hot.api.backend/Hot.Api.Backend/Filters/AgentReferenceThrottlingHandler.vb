Imports System.Net.Http
Imports WebApiThrottle

Namespace Filters

    Public Class AgentReferenceThrottlingHandler
        Inherits ThrottlingHandler

        Public Sub New ()
            MyBase.New()
            QuotaExceededMessage =  "x-agent-reference header can only be used once. Please provide a new one for each request."
        End Sub

        Protected Overrides Function SetIdentity(request As HttpRequestMessage) As RequestIdentity
            Dim agentReference = "x-agent-reference"
            Dim accessCode = "x-access-code"
            Dim identity = new RequestIdentity

            If request.Headers.Contains(agentReference) Then
                identity.ClientKey = request.Headers.GetValues(agentReference).First()
            Else
                identity.ClientKey = "anon"
            End If

            If request.Headers.Contains(accessCode) Then
                identity.ClientKey = identity.ClientKey + "-" + request.Headers.GetValues(accessCode).First()
            End If

            identity.ClientIp = GetClientIp(request).ToString()
            identity.Endpoint = request.RequestUri.AbsolutePath.ToLowerInvariant()
            Return identity
        End Function
    End Class
End NameSpace