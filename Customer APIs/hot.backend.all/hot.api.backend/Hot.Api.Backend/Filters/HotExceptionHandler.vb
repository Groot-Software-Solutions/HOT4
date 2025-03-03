Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Formatting
Imports System.Reflection
Imports System.Web.Http.Filters
Imports Hot.Core
Imports Hot.Core.Brands
Imports Hot.Data

Namespace Filters
    Public Class HotExceptionHandler
        Inherits ExceptionFilterAttribute

         Private ReadOnly _applicationName as String = Assembly.GetCallingAssembly().GetName().Name
         Private ReadOnly _connString = Config.GetConnectionString()

        Public Overrides Sub OnException(actionExecutedContext As HttpActionExecutedContext)
            Dim ex = actionExecutedContext.Exception
            Dim req = actionExecutedContext.Request
            Dim response As HttpResponseMessage
            Dim formatter As MediaTypeFormatter = New JsonMediaTypeFormatter()

            If TypeOf ex Is HotRequestException Then 
                Dim hotEx = CType(ex, HotRequestException)
                response = req.CreateResponse(HttpStatusCode.BadRequest, hotEx.Body, formatter)
            Else
                xLog_Data.Save(_applicationName, ex.Source, ex.TargetSite.Name, ex, _connString)
                Dim templateId = NetworkExceptionResolver.GetErrorTemplateByStackTrace(ex.Message)
                If templateId > -1 Then
                    Using sqlConn = New SqlConnection(_connString)
                        sqlConn.Open()
                        Dim template = xTemplateAdapter.SelectRow(templateId, sqlConn)
                        Dim text = template.TemplateText.Replace("%NETWORK%", "The cell network")
                        response = req.CreateResponse(HttpStatusCode.InternalServerError,  New HotRequestException(templateId, text).Body, formatter)
                    End Using
                Else
                    response = req.CreateResponse(HttpStatusCode.InternalServerError, New HotRequestException(-1, "Internal Server Error").Body, formatter)
                End If
            End If
            actionExecutedContext.Response = response
        End Sub
    End Class
End NameSpace