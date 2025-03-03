
Imports Hot.Data

Public Class PinRechargeResponse : Inherits RechargeResponse
    Public Property Sms As xSMS
    Public Property CustomerTemplates As List(Of xTemplate)
    Public Property Templates As List(Of xTemplate)
    Public Property Success As Boolean
    Public Property Pins As List(Of xPin)

    Public Property AccessChannel As xChannel.Channels

    Public Property Account As xAccount


    Public Sub New(sms As xSMS, templates As List(Of xTemplate), success As Boolean, Optional customerTemplates As List(Of xTemplate) = Nothing)
        Me.Sms = sms
        Me.CustomerTemplates = customerTemplates
        Me.Templates = templates
        Me.Success = success
    End Sub
End Class