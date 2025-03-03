
Imports System.Web.Http
Imports Hot.Api.Backend.Filters
Imports WebApiThrottle

Public Module WebApiConfig
    Public Sub Register(config As HttpConfiguration)
        ' Web API configuration and services

        ' Web API routes
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        Dim appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(Function(t) t.MediaType = "application/xml")
        config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType)

        config.Filters.Add(New ValidateRequestAttribute)
        config.Filters.Add(New NullModelCheckAttribute)
        config.Filters.Add(New HotExceptionHandler)
        AddIpThrottle(config)
        AddAgentReferenceThrottle(config)
    End Sub

    Private Sub AddAgentReferenceThrottle(config As HttpConfiguration)
        Dim throttle = New AgentReferenceThrottlingHandler
        throttle.Policy = New ThrottlePolicy(perWeek:=1)
        throttle.Policy.EndpointThrottling = False
        throttle.Policy.IpThrottling = False
        throttle.Policy.ClientThrottling = True
        throttle.Repository = new CacheRepository
        config.MessageHandlers.Add(throttle)
    End Sub

    Private Sub AddIpThrottle(config As HttpConfiguration)
         Dim throttle = New ThrottlingHandler
        throttle.Policy = New ThrottlePolicy(perSecond:=ConfigurationManager.AppSettings("RateLimit"))
        throttle.Policy.EndpointThrottling = False
        throttle.Policy.IpThrottling = True
        throttle.Policy.ClientThrottling = False
        throttle.Repository = new CacheRepository
        config.MessageHandlers.Add(throttle)
    End Sub
End Module