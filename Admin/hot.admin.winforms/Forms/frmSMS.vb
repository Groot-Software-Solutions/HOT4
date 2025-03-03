Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmSMS

    Sub New(ByVal iSMS As xSMS)

        ' This call is required by the designer.
        InitializeComponent()
        txtMobile.Text = iSMS.Mobile
        txtMessage.Text = iSMS.SMSText

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            If txtMobile.Text.Length < 10 Then
                Err.SetError(txtMobile, "Invalid Mobile")
            End If
            If txtMessage.Text.Length = 0 Then
                Err.SetError(txtMessage, "Invalid Message")
            End If
            Dim iSMS As New xSMS
            iSMS.Direction = False
            iSMS.Mobile = txtMobile.Text
            iSMS.Priority.PriorityID = xPriority.Priorities.Normal
            iSMS.SMSDate = Now
            iSMS.SMSText = txtMessage.Text
            iSMS.State.StateID = xState.States.Pending
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                xSMSAdapter.Save(iSMS, sqlConn)
                sqlConn.Close()
            End Using
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub txtMessage_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMessage.TextChanged
        txtCounter.Text = txtMessage.Text.Length
        txtCounter.Update()
    End Sub
End Class