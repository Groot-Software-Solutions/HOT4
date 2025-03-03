Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmBulkSMS
    Private Sub txtMessage_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMessage.TextChanged
        txtCounter.Text = txtMessage.Text.Length
        txtCounter.Update()

        lblSMS.Text = "Editing SMS"
    End Sub
    Private Sub txtBody_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBody.TextChanged
        txtCounter2.Text = txtBody.Text.Length
        txtCounter2.Update()
        lblEmail.Text = "Editing Email"
        lblAgg.Text = "Editing Email"

    End Sub

    Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        If txtMessage.Text <> String.Empty Then

            Dim SMSSent As Integer = Nothing
            lblSMS.Text = "Sending...."
            Try
                Me.DialogResult = Windows.Forms.DialogResult.None
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Using sqlCmd As New SqlCommand("xSMS_BulkSMSSend", sqlConn)
                        sqlCmd.CommandType = CommandType.StoredProcedure
                        sqlCmd.Parameters.AddWithValue("MessageText", txtMessage.Text)
                        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                            sqlRdr.Read()
                            SMSSent = sqlRdr(SMSSent)
                            sqlRdr.Close()
                        End Using
                    End Using
                    lblSMS.Text = SMSSent & " SMSs sent to customers"
                    sqlConn.Close()
                End Using
            Catch ex As Exception
                ShowEx(ex)
            End Try
        Else
            MessageBox.Show("You must enter a message to send!")

        End If

    End Sub

    Private Sub cmdSendToCorporates_Click(sender As System.Object, e As System.EventArgs) Handles cmdSendToCorporates.Click
        If txtMessage.Text = String.Empty Or txtSubject.Text = String.Empty Then
            MessageBox.Show("You must enter a Subject and Message to send!")
            txtSubject.Focus()
            Exit Sub
        End If

        Dim EmailsSent As Integer = Nothing

        lblEmail.Text = "Sending...."

        'Send the emails from DBmail
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Using sqlCmd As New SqlCommand("xSMS_EmailCorporates", sqlConn)
                    sqlCmd.CommandType = CommandType.StoredProcedure
                    sqlCmd.Parameters.AddWithValue("Sub", txtSubject.Text)
                    sqlCmd.Parameters.AddWithValue("MessageText", txtBody.Text)
                    sqlCmd.CommandTimeout = 180
                    Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                        sqlRdr.Read()
                        EmailsSent = sqlRdr(EmailsSent)
                        sqlRdr.Close()
                    End Using
                End Using
                lblEmail.Text = EmailsSent & " Emails Sent"
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try



    End Sub

    Private Sub cmdSendToAggregators_Click(sender As System.Object, e As System.EventArgs) Handles cmdSendToAggregators.Click

        If txtMessage.Text <> String.Empty And txtSubject.Text <> String.Empty Then
            Dim EmailsSent As Integer = Nothing
            lblAgg.Text = "Sending...."
            
            Try
                Me.DialogResult = Windows.Forms.DialogResult.None
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Using sqlCmd As New SqlCommand("xSMS_EmailAggregators", sqlConn)
                        sqlCmd.CommandType = CommandType.StoredProcedure
                        sqlCmd.Parameters.AddWithValue("Sub", txtSubject.Text)
                        sqlCmd.Parameters.AddWithValue("MessageText", txtBody.Text)
                        Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                            sqlRdr.Read()
                            EmailsSent = sqlRdr(EmailsSent)
                            sqlRdr.Close()
                        End Using
                    End Using
                    lblAgg.Text = EmailsSent & " Emails Sent"
                    sqlConn.Close()
                End Using
            Catch ex As Exception
                ShowEx(ex)
            End Try
        Else
            MessageBox.Show("You must enter a Subject and Message to send!", "Error")
        End If
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        txtBody.Text = txtMessage.Text
        txtBody.Update()

    End Sub

    Private Sub btnFormat_Click(sender As Object, e As EventArgs) Handles btnFormat.Click
        Dim EmailBody As String = ""
        'Format email body as HTML
        For x = 0 To txtBody.Lines.Length - 2
            If txtBody.Lines(x).Length > 3 Then
                If txtBody.Lines(x).Substring(txtBody.Lines(x).Length - 4) <> "<br>" Then
                    EmailBody = EmailBody + txtBody.Lines(x) + "<br>" + vbCrLf
                Else
                    EmailBody = EmailBody + txtBody.Lines(x) + vbCrLf
                End If
            Else
                EmailBody = EmailBody + txtBody.Lines(x) + "<br>" + vbCrLf
            End If
        Next
        EmailBody = EmailBody + txtBody.Lines(txtBody.Lines.Length - 1)
        txtBody.Text = EmailBody
        Me.Refresh()
    End Sub

    Private Sub frmBulkSMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub

End Class