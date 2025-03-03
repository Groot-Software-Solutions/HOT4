Imports System.Data.SqlClient
Imports Common
Imports HOT5.Common

Partial Class myaccount
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
                    Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)

                    If iAccess.AccessID = xAccessAdapter.AdminSelect(iAccess.AccountID, sqlConn) Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "isAdmin", "$(""#btnManageUsers"").ready(function () {$(""#btnManageUsers"").show(); $(""#btnLock"").show();});", True)
                    End If

                    sqlConn.Close()
                End Using
            End If

        Catch ex As Exception

        End Try
    End Sub

    Sub btnChangePwd_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangePwd.ServerClick
        Try

        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
                Dim iAccess As xAccess = Session.Item("HOTAccess")
                Dim MinimumPasswordLength As Integer = 4
                If String.IsNullOrEmpty(txtOldPwd.Value) Then
                    ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Please Enter Current Password!</h6>")
                    Exit Try
                End If
                If iAccess.AccessPassword <> txtOldPwd.Value Then
                    ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Old Password Incorrect!</h6>")
                Else
                    If txtCfmPwd.Value <> txtNewPwd.Value Then
                        ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Passwords do not Match!</h6>")
                        Exit Try
                    End If
                    If txtNewPwd.Value.Length < MinimumPasswordLength Then
                        ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Password too short!</h6>")
                        Exit Try
                    End If

                    Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                    Try
                        xAccessAdapter.PasswordChange(iAccess, txtCfmPwd.Value, sqlConn, sqlTrans)
                        sqlTrans.Commit()
                        ShowChangePasswordStatus("<h6><i class='icon-checkmark'></i> Password Changed</h6>")
                    Catch ex As Exception
                        sqlTrans.Rollback()
                        ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Password Change Failed!</h6>")
                    End Try
                End If
            sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowChangePasswordStatus("<h6><i class='icon-warning'></i> " + ex.Message + "!</h6>")
        End Try

    End Sub

    Sub ShowChangePasswordStatus(ByVal message As String)
        If message.StartsWith("A network-related or instance-specific error occurred while establishing a connection") Then message = "Connection to Database is down"
        txtAlert.InnerHtml = message
    End Sub

    Protected Sub btnSaveRequred_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRequred.ServerClick
        Try

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccess As xAccess = Session.Item("HOTAccess")
               If String.IsNullOrEmpty(txtOldPwd.Value) Then
                    ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Please enter Current Password!</h6>")
                    Exit Try
                End If
                If iAccess.AccessPassword <> txtOldPwd.Value Then
                    ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Password Incorrect!</h6>")
                Else


                      Try
                        Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAccess.AccessID, sqlConn)
                        iAccessWeb.SalesPassword = txtPasswordRequired.Checked
                        xAccessWebAdapter.Save(iAccessWeb, sqlConn)

                        ShowChangePasswordStatus("<h6><i class='icon-checkmark'></i> Status Saved</h6>")
                    Catch ex As Exception

                        ShowChangePasswordStatus("<h6><i class='icon-warning'></i> Status Saved Failed!</h6>")
                    End Try
                End If
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowChangePasswordStatus("<h6><i class='icon-warning'></i> " + ex.Message + "!</h6>")
        End Try
    End Sub

End Class
