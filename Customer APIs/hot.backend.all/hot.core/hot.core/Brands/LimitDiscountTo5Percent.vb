Imports Hot.Data

Namespace Brands
    Public Class LimitDiscountTo5Percent
        Public Sub Apply(recharge As xRecharge)
            If recharge.Amount < 1 And recharge.Discount > 5 Then recharge.Discount = 5
        End Sub
    End Class
End NameSpace