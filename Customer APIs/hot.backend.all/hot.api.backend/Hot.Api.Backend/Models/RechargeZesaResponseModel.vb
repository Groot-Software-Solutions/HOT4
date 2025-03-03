Imports System.ComponentModel.DataAnnotations
Imports Hot.Core.ZesaAPI

Namespace Models
    Public Class RechargeZesaResponseModel
        Public Property ReplyCode As Integer = 0
        Public Property ReplyMsg As String = ""
        Public Property WalletBalance As Decimal = 0
        Public Property Amount As Decimal = 0
        Public Property ProviderFees As Decimal = 0
        Public Property Discount As Decimal = 0
        ' New
        Public Property Meter As String = ""
        Public Property AccountName As String = ""
        Public Property Address As String = ""
        Public Property Tokens As List(Of TokenItem)
        ' End New
        Public Property AgentReference As String = ""
        Public Property RechargeID As Integer
        Public Property CustomerInfo As CustomerInfo
    End Class
End Namespace

