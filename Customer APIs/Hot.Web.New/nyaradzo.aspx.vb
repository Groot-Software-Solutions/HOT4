
Imports HOT5.Common

Partial Class nyaradzo
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim iAccess As xAccess = Session.Item("HOTAccess")
            If iAccess Is Nothing Then
                Response.Redirect("index.aspx")
            End If
        Catch ex As Exception
            showex(ex.Message)
        End Try
    End Sub
    Sub showex(ByVal message As String)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Subscriber Error','" + message + "');", True)
    End Sub
End Class
