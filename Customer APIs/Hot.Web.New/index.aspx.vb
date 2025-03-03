Imports System.Data.SqlClient
Imports HOT5.Common
Imports Common
Imports System.Security.Cryptography

Partial Class index
    Inherits System.Web.UI.Page
    Sub cmdSignIn_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSignIn.ServerClick
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()

                Dim username = txtUID.Value
                Dim password = txtPWD.Value
                Dim iAccess As xAccess = xAccessAdapter.SelectLogin(username, password, sqlConn) ' New xAccess With {.AccessCode = username, .AccessPassword = password, .AccessID = 12345, .AccountID = 12356, .Deleted = False}
                If iAccess Is Nothing Then
                    Throw New Exception("Logon Failed!")
                ElseIf iAccess.Deleted = True Then
                    Throw New Exception("Your account has been disabled!")
                End If

                Dim aCookie As New HttpCookie("HOTLogin")
                aCookie.Values("UID") = txtUID.Value
                aCookie.Values("AccountID") = iAccess.AccountID
                aCookie.Values("HOTAccessID") = iAccess.AccessID
                aCookie.Values("Pwd") = txtPWD.Value
                Dim profileID As Integer = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn).Profile.ProfileID
                '  aCookie.Values("Retail") = (profileID = 1)
                If (profileID > 20 And profileID < 41) Then aCookie.Values("Retail") = vbTrue
                If (profileID = 47) Then aCookie.Values("NoRechargeUI") = "true" ' Prepaynation Profile

                aCookie.Expires = Date.Now.AddMinutes(15)
                Response.Cookies.Add(aCookie)

                Try
                    Dim bCookie As New HttpCookie("HOTBack")
                    Dim iAccessWeb As xAccessWeb = New xAccessWeb With {.AccessID = iAccess.AccessID}
                    bCookie.Value = iAccessWeb.WebBackground

                    Response.Cookies.Add(bCookie)
                Catch ex As Exception

                End Try



                Session.Item("HOTAccess") = iAccess
                Response.Redirect("myaccount.aspx")

                sqlConn.Close()
            End Using
        Catch ex As Exception
            showex(ex.Message)
            txtPWD.Focus()
        End Try
    End Sub
    Sub showex(ByVal message As String)

        If message.StartsWith("A network-related or instance-specific error occurred while establishing a connection") Then message = "Connection to Database is down"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('Login failure','" + message + "');", True)

    End Sub

    Shared Function GetHash(theInput As String, theSalt As String) As String

        Using hasher As MD5 = MD5.Create()
            Dim dbytes As Byte() =
                hasher.ComputeHash(Encoding.UTF8.GetBytes(theInput & theSalt))
            Dim sBuilder As New StringBuilder()
            For n As Integer = 0 To dbytes.Length - 1
                sBuilder.Append(dbytes(n).ToString("X2"))
            Next n
            Return sBuilder.ToString()
        End Using

    End Function
End Class
