Imports Hot.Data

Public Class ServiceRechargeResponse : Inherits RechargeResponse
    Public Sub New(iRechargePrepaid As xRechargePrepaid)
        RechargePrepaid = iRechargePrepaid
    End Sub

    Public Property RechargePrepaid As xRechargePrepaid

    Public Property CustomCustomerCreditSuccessSMS As String

End Class