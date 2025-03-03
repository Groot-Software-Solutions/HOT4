
Imports Hot.Data

Public Class frmRechargeEdit
    Private ReadOnly iRecharge As xRecharge
    Sub New(_Recharge As xRecharge)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        iRecharge = _Recharge
    End Sub
    Private Sub frmRechargeEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtAmount.Text = iRecharge.Amount
        txtDiscount.Text = iRecharge.Discount
        txtRechargeID.Text = iRecharge.RechargeID
        txtStatus.DisplayMember = "State"
        txtStatus.ValueMember = "StateID"
        '  txtStatus.DataSource = xState.States.
        'iRecharge.State
    End Sub
End Class