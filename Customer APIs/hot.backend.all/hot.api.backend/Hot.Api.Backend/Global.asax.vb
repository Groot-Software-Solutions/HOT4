Imports System.Web.Http
Imports Hot.Api.Backend.Filters

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf Register)
        RegisterGlobalFilters(GlobalFilters.Filters)

    End Sub
End Class
