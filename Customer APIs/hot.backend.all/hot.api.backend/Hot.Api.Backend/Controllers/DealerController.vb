
Imports System.Web.Http
Imports Hot.Api.Backend.Models

Namespace Controllers
    <RoutePrefix("api/v1/dealers")>
    Public Class DealerController
        Inherits ApiController

        ReadOnly _service As DealerService = New DealerService(Config.IsTestMode())

        <Route("selftopup")>
        <HttpPost>
        Function SelfTopUp(<FromBody> req As SelfTopUpRequest) As SelfTopUpResponse
            Return _service.SelfTopUp(req, Context.Get(Request.Headers))
        End Function

        <Route("selftopup-data")>
        <HttpPost>
        Function SelfTopUpData(<FromBody> req As SelfTopUpDataRequest) As SelfTopUpResponse
            Return _service.SelfTopUpData(req, Context.Get(Request.Headers))
        End Function

        <Route("resetpin")>
        <HttpPost>
        Function ResetPin(<FromBody> req As PinResetRequest) As PinResetResponse
            Return _service.ResetPassword(req, Context.Get(Request.Headers))
        End Function

        <Route("transfer")>
        <HttpPost>
        Function SelfTopUp(<FromBody> req As TransferRequest) As TransferResponse
            Return _service.HandleTransfer(req, Context.Get(Request.Headers))
        End Function

        <Route("register")>
        <HttpPost>
        Function Register(<FromBody> req As RegistrationRequest) As Response
            Return _service.HandleRegistration(req, Context.Get(Request.Headers))
        End Function

    End Class
End Namespace