Imports System.Net
Imports System.Net.Http
Imports System.Web.Http.Controllers
Imports Hot.Api.Backend.Models

Namespace Filters
    Public Class ValidateRequestAttribute
        Inherits Web.Http.Filters.ActionFilterAttribute

        Public Overrides Sub OnActionExecuting(filterContext As HttpActionContext)
            Dim ctx = Context.Get(filterContext.Request.Headers)
            If ctx.AccessId Is Nothing Then
                filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ctx.Error)
                Return
            End If
          
            If filterContext.ModelState.IsValid Then Return
            filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, filterContext.ModelState)
        End Sub
    End Class
End Namespace