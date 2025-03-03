Imports HOT5.Common
Imports System.Data.SqlClient
Imports Common

Partial Class Vpayments_UpdateStatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        VPayments.UpdateStatus(Request("guid"))
    End Sub
End Class
