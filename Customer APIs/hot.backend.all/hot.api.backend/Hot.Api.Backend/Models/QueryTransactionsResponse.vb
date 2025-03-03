

Namespace Models
    Public Class QueryTransactionsResponse
        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property OriginalAgentReference As String = ""

        Public RawReply As String
        Public Property AgentReference As String
    End Class
End NameSpace