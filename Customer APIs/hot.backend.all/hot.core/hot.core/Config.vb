Imports System.Configuration

Public Class Config

    Public Shared Function GetConnectionString() As String
        Return IIf(IsTestMode(), ConfigurationManager.AppSettings("TestConnectionString"), ConfigurationManager.AppSettings("ConnectionString"))
    End Function

    Public Shared Function IsTestMode() As Boolean
        return Convert.ToBoolean(ConfigurationManager.AppSettings("testMode"))
    End Function

End Class