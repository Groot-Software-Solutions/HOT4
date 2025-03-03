Imports HOT5.Common
Imports Common
Imports System.Data.SqlClient

Partial Class register
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Try
            If txtConfirm.Value <> txtPassword.Value Then
                Throw New Exception("Passwords do not match")
            End If
            If txtPassword.Value.Length < 6 Then
                Throw New Exception("Password must be at least 6 characters")
            End If
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim transaction As SqlTransaction = sqlConn.BeginTransaction
                Try

                    Dim iAccount As New xAccount
                    iAccount.AccountName = Sanitize(txtAccName.Value)
                    iAccount.Email = txtEmail.Value
                    iAccount.NationalID = txtID.Value
                    iAccount.ReferredBy = Sanitize(IIf(Sanitize(txtReferredBy.Value) = "Other", txtReferredByOther.Value, txtReferredBy.Value))
                    iAccount.Profile = New xProfile
                    iAccount.Profile.ProfileID = IIf(txtAccType2.Value = 2, 31, 11)
                    iAccount.Profile.ProfileID = IIf(txtAccType2.Value = 3, 12, iAccount.Profile.ProfileID)
                    xAccountAdapter.Save(iAccount, sqlConn, transaction)

                    Dim iAccess As New xAccess
                    iAccess.AccessCode = txtEmail.Value
                    iAccess.AccessPassword = txtPassword.Value
                    iAccess.AccountID = iAccount.AccountID
                    iAccess.Channel.ChannelID = xChannel.Channels.Web
                    xAccessAdapter.Save(iAccess, sqlConn, transaction)

                    Dim iAccessWeb As New xAccessWeb
                    iAccessWeb.AccessID = iAccess.AccessID
                    iAccessWeb.AccessName = iAccount.AccountName
                    iAccessWeb.WebBackground = ""
                    xAccessWebAdapter.Save(iAccessWeb, sqlConn, transaction)

                    transaction.Commit()

                    Dim iAddress As New xAddress
                    iAddress.AccountID = iAccount.AccountID
                    iAddress.ContactName = iAccount.AccountName
                    iAddress.InvoiceFreq = 1
                    Try
                        xAddressAdapter.Save(iAddress, sqlConn)
                    Catch ex As Exception

                    End Try



                    sqlConn.Close()

                    Dim aCookie As New HttpCookie("HOTLogin")
                    aCookie.Values("UID") = txtEmail.Value
                    aCookie.Values("AccountID") = iAccess.AccountID
                    aCookie.Values("HOTAccessID") = iAccess.AccessID
                    aCookie.Values("Pwd") = txtPassword.Value
                    If (iAccount.Profile.ProfileID > 20 And iAccount.Profile.ProfileID < 41) Then aCookie.Values("Retail") = vbTrue
                    Response.Cookies.Add(aCookie)

                    Session.Item("HOTAccess") = iAccess

                    Response.Redirect("myaccount.aspx?info=1")
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Registration','" + ex.Message + "');", True)
                    transaction.Rollback()
                End Try
            End Using
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Registration','" + ex.Message + "');", True)

        End Try
    End Sub
End Class
