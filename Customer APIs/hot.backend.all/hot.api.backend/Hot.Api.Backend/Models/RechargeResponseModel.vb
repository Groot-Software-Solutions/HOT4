Namespace Models
    Public Class RechargeResponseModel
        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property WalletBalance As Decimal = 0
        Public Property Amount As Decimal = 0
        Public Property Discount As Decimal = 0
        ' New
        Public Property InitialBalance As Decimal = 0
        Public Property FinalBalance As Decimal = 0
        Public Property Window As Date?
        Public Property Data As Decimal?
        Public Property SMS As Integer?
        ' End New
        Public Property AgentReference As String = ""
        Public Property RechargeID As Integer

    End Class
End NameSpace