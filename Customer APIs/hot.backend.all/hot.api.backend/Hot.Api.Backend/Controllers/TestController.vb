
Imports System.Web.Http
Imports Hot.Api.Backend.Models
Imports Hot.Core

Namespace Controllers
    <RoutePrefix("api/v1/test")>
    Public Class TestController
        Inherits ApiController

        <Route("")>
        <HttpGet>
        Function Index() As RechargeResponseModel
            Dim response = New RechargeResponseModel()
            response.Amount = 999
            Throw new HotRequestException(123, "This is a test")
            Return response
        End Function

        <Route("error-test")>
        <HttpGet>
        Function ErrorTest() As RechargeResponseModel
            Throw new HotRequestException(235, "This is a test2")
        End Function
    End Class
End NameSpace