Imports HOT5.Common
Imports System.Data.SqlClient
Imports Common

Partial Class users
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim iAccess As xAccess = Session.Item("HOTAccess")
            If iAccess Is Nothing Then
                Response.Redirect("index.aspx")
            End If
            If Not IsPostBack Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                   
                    If Not (iAccess.AccessID = xAccessAdapter.AdminSelect(iAccess.AccountID, sqlConn)) Then Response.Redirect("myaccount.aspx")

                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            showex(ex.Message)
        End Try
    End Sub
    Sub showex(ByVal message As String)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Users Error','" + message + "');", True)
    End Sub
End Class
