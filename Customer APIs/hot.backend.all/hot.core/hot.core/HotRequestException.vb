Public Class HotRequestException
    Inherits Exception

    Public Property Body As HotRequestExceptionBody = new HotRequestExceptionBody
       
    Sub New (replyCode As String, replyMessage As String)
        Body.ReplyCode = replyCode
        Body.ReplyMessage = replyMessage
    End Sub


End Class

Public Class HotRequestExceptionBody
        Public Property ReplyCode
    Public Property ReplyMessage
End Class