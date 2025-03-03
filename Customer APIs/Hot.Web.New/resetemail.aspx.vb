Imports Common
Imports System.Data.SqlClient
Imports HOT5.Common

Partial Class resetemail
    Inherits System.Web.UI.Page

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click
        Try
            Dim UID As String = txtEmail.Value
            If (Not String.IsNullOrEmpty(UID) And EmailAddressCheck(UID)) Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iAccess As xAccess = xAccessAdapter.SelectCode(UID, sqlConn)
                    If iAccess Is Nothing Then GoTo pageerror

                    Dim iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(iAccess.AccessID, sqlConn)
                    If iAccessWeb Is Nothing Then GoTo pageerror

                    Dim sendmail As New SqlCommand("z_spSendResetEmail", sqlConn)
                    sendmail.CommandType = System.Data.CommandType.StoredProcedure
                    sendmail.Parameters.AddWithValue("@accessid", iAccess.AccessID)
                    sendmail.ExecuteNonQuery()

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showreset", " addLoadEvent( function() {$('.grpReset').hide(); $('#grpSuccess').show();});", True)
                    sqlConn.Close()
                    Exit Sub
                End Using

            End If

pageerror:
            showGeneralError()

        Catch ex As Exception
            showGeneralError()
        End Try
    End Sub
    Sub showGeneralError()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showerror", " addLoadEvent( function() {$('.grpReset').hide();$('#grpError').show();});", True)
    End Sub
End Class
