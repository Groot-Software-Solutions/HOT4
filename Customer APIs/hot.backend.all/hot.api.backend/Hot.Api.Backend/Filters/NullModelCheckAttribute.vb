Imports System.Net
Imports System.Net.Http
Imports System.Web.Http.Controllers

Namespace Filters
    Public Class NullModelCheckAttribute
        Inherits Web.Http.Filters.ActionFilterAttribute

        Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
            If actionContext.ActionArguments.ContainsValue(Nothing) Then
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You must provide parameters for this request.")
            End If
        End Sub
    End Class
End NameSpace