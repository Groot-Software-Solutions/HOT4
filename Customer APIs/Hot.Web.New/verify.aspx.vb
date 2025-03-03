Imports Common
Imports HOT5.Common

Partial Class verify
    Inherits System.Web.UI.Page
    
    Sub Page_Load(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim AccountID As Long = 0, email As String = ""
        Try
            email = Request.QueryString("email")
            If String.IsNullOrEmpty(email) Or EmailAddressCheck(email) Then
                result.Text = "Invalid Email. Please click link you received."
                Exit Sub
            End If
        Catch ex As Exception
            result.Text = "Invalid Email. Please click link you received."
            Exit Sub
        End Try
        Try
            AccountID = (CLng(Request.QueryString("code")))
            If AccountID = 0 Then
                result.Text = "Invalid Code. Please try sending the verification email again."
                Exit Sub
            End If
        Catch ex As Exception
            result.Text = "Invalid Code. Please try sending the verification email again."
            Exit Sub
        End Try
        Try
            Using sqlConn As New System.Data.SqlClient.SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(AccountID, sqlConn)
                Dim iAddress As xAddress
                Try
                    iAddress = xAddressAdapter.SelectRow(AccountID, sqlConn)
                Catch ex As Exception
                    iAddress = New xAddress
                End Try

                iAccount.Email = email.Replace("'", "")
                iAddress.Latitude = 2
                xAccountAdapter.Save(iAccount, sqlConn)
                xAddressAdapter.Save(iAddress, sqlConn)

                sqlConn.Close()
                Response.Redirect("https://ssl.hot.co.zw/fbcom.aspx")
                'result.Text = email + ";" + CStr(AccountID)
            End Using
        Catch ex As Exception
            result.Text = "Error Verifying Email please try again later"
        End Try


    End Sub

End Class
