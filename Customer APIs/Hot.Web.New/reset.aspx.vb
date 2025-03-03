Imports System.Data.SqlClient
Imports Common
Imports HOT5.Common

Partial Class reset
    Inherits System.Web.UI.Page

    Dim UID As String
    Dim Token As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            UID = Request.QueryString("u")
            Token = Request.QueryString("t")
            If Not IsPostBack Then

                If (Not String.IsNullOrEmpty(UID) And (Not String.IsNullOrEmpty(Token) And Token.Length = 32)) Then
                    Using sqlConn As New SqlConnection(Conn)
                        sqlConn.Open()
                        Dim iAccess As xAccess = xAccessAdapter.SelectCode(UID, sqlConn)
                        If iAccess Is Nothing Then GoTo pageerror

                        Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAccess.AccessID, sqlConn)
                        If iAccessWeb Is Nothing Then GoTo pageerror
                        If Not iAccessWeb.ResetToken = Token Then GoTo pageerror
                        txtUser.Value = UID

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showreset", "addLoadEvent( function() {showreset();});", True)
                        sqlConn.Close()
                        Exit Sub
                    End Using
                End If
            End If
pageerror:
            showGeneralError()

        Catch ex As Exception
            showGeneralError()
        End Try
    End Sub

    Protected Sub cmdGo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click
        Try

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectCode(txtUser.Value, sqlConn)
                If iAccess Is Nothing Then showGeneralError() : Exit Try
                Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAccess.AccessID, sqlConn)
                If iAccessWeb Is Nothing Then showGeneralError() : Exit Try
                If iAccessWeb.ResetToken = Token Then
                    Dim MinimumPasswordLength As Integer = 4

                    If txtConfirm.Value <> txtPassword.Value Then
                        showex("<i class='icon-warning'></i> Passwords do not Match!")
                        Exit Try
                    End If
                    If txtPassword.Value.Length < MinimumPasswordLength Then
                        showex("<i class='icon-warning'></i> Password too short!")
                        Exit Try
                    End If

                    Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                    Try
                        xAccessAdapter.PasswordChange(iAccess, txtConfirm.Value, sqlConn, sqlTrans)
                        sqlTrans.Commit()
                        shownote("<i class='icon-checkmark'></i> Password Changed")
                    Catch ex As Exception
                        sqlTrans.Rollback()
                        showex("<i class='icon-warning'></i> Password Change Failed!")
                    End Try
                    Response.Redirect("/")
                Else
                    showGeneralError()
                End If
                sqlConn.Close()
            End Using
        Catch ex As Exception
            showex("<i class='icon-warning'></i> " + ex.Message + "!")
        End Try

    End Sub
    Sub showex(ByVal message As String)

        If message.StartsWith("A network-related or instance-specific error occurred while establishing a connection") Then message = "Connection to Database is down"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Reset Failure','" + message + "');", True)

    End Sub
    Sub shownote(ByVal message As String)

        If message.StartsWith("A network-related or instance-specific error occurred while establishing a connection") Then message = "Connection to Database is down"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "shownotification('Reset Password','" + message + "');", True)

    End Sub

    Sub showGeneralError()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showerror", " addLoadEvent( function() {showerror();});", True)
    End Sub
End Class
