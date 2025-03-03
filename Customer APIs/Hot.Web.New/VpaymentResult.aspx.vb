Imports Common
Imports HOT5.Common
Imports System.Data.SqlClient

Partial Class Vpayments_Result
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Vpayments.UpdateStatus(Request("guid"))
        Catch ex As Exception
            Vpayments.GetLogger().Error("Transaction Ref: " + Request("guid") + " - " + ex.Message)
        End Try

        Try
            Dim guid As Guid = New Guid(Request("guid"))
            Dim connection As SqlConnection = New SqlConnection(Conn)
            connection.Open()

            Dim transaction As xBankTrx = xBankTrxAdapter.GetFromvPayment(guid, connection)
            Dim payment As xvPayment = xBankvPaymentAdapter.SelectRow(New xvPayment() With {.vPaymentID = guid}, connection)

            lblDate.Text = transaction.TrxDate.ToString("d MMM yyyy")
            lblAmount.Text = transaction.Amount.ToString("#,##0.00")
            lblvRef.Text = payment.vPaymentRef
            lblDetails.Text = payment.ErrorMsg
            lblResult.Text = transaction.BankTrxState.BankTrxState

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub
    Sub showex(ByVal message As String)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('VPayment Error','" + message + "');", True)
    End Sub
End Class
