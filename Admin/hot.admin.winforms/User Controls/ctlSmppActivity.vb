Imports System.Data.SqlClient
'Imports HOT.Data.SMS
Imports HOT.Data

Public Class ctlSmppActivity

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'InitializeComponent()

        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                dtpFrom.Value = Now.AddHours(-2)
                dtpTo.Value = Now
                txtTimeFrom.Text = dtpFrom.Value.TimeOfDay.ToString.Substring(0, 5)
                txtTimeTo.Text = dtpTo.Value.TimeOfDay.ToString.Substring(0, 5)
                
                Dim iListSmpp As List(Of xSmpp) = xSmpp_Data.List(sqlConn)
                Dim iSmpp As New xSmpp
                iSmpp.SmppID = -1
                iSmpp.SmppName = "<All>"
                iListSmpp.Insert(0, iSmpp)

                cboSmpp.DisplayMember = "SmppName"
                cboSmpp.ValueMember = "SmppID"
                cboSmpp.DataSource = iListSmpp
                cboSmpp.SelectedIndex = 0

                Dim iListState As List(Of xState) = xState_Data.List(sqlConn)
                Dim iState As New xState
                iState.StateID = -1
                iState.State = "<All>"
                iListState.Insert(0, iState)

                cboState.DisplayMember = "State"
                cboState.ValueMember = "StateID"
                cboState.DataSource = iListState
                cboState.SelectedIndex = 0

                sqlConn.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Label12.Visible = True
        Label12.Update()
        Try
            'If Not IsDate(txtFrom.Text) Or Not IsDate(txtTo.Text) Then
            '    Throw New Exception("Invalid Period")
            'End If

            Dim iIn As New List(Of xSMS)
            Dim iOut As New List(Of xSMS)
            dtpFrom.Value = dtpFrom.Value.Date + " " & txtTimeFrom.Text
            dtpTo.Value = dtpTo.Value.Date + " " & txtTimeTo.Text

            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                For Each iSMS As xSMS In xSMSAdapter.Search(dtpFrom.Value, dtpTo.Value, txtMobile.Text, _
                                            txtMessage.Text, cboState.SelectedValue, cboSmpp.SelectedValue, sqlConn)
                    If iSMS.Direction = True Then
                        iIn.Add(iSMS)
                    Else
                        iOut.Add(iSMS)
                    End If
                    'If iSMS.SMSID_In IsNot Nothing Then
                    '    iOut.Add(iSMS)
                    'Else
                    '    iIn.Add(iSMS)
                    'End If
                Next
                Dim bsIn As New BindingSource
                bsIn.DataSource = iIn
                dgIncoming.DataSource = iIn

                Dim bsOut As New BindingSource
                bsOut.DataSource = iOut
                dgOutgoing.DataSource = iOut

                sqlConn.Close()
            End Using
            
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
        Label12.Visible = False
        Label12.Update()
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Open(New xSMS)
    End Sub
    'Private Sub dgIncoming_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgIncoming.CellDoubleClick
    '    Open(dgIncoming.SelectedRows(0).DataBoundItem)
    'End Sub
    'Private Sub dgIncoming_CellSingleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgIncoming.CellDoubleClick
    '    Dim iSMS As xSMS = dgIncoming.SelectedRows(0).DataBoundItem
    '    txtMobile.Text = iSMS.Mobile
    'End Sub
    Private Sub dgOutgoing_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgOutgoing.CellDoubleClick
        Open(dgOutgoing.SelectedRows(0).DataBoundItem)
    End Sub
    Private Sub Open(ByVal iSMS As xSMS)
        Try
            Dim f As New frmSMS(iSMS)
            f.StartPosition = FormStartPosition.CenterScreen
            f.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub dgIncoming_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgIncoming.CellEnter
        Label12.Visible = True
        Label12.Update()

        Try

            If dgIncoming.SelectedRows.Count = 1 Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iSMS As xSMS = dgIncoming.SelectedRows(0).DataBoundItem
                    txtMobile.Text = iSMS.Mobile
                    Dim bs As New BindingSource
                    bs.DataSource = xSMSAdapter.SMSListOut(iSMS.SMSID, sqlConn)
                    dgOutgoing.DataSource = bs
                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
        Label12.Visible = False
        Label12.Update()

    End Sub

End Class
''Private Sub Init()

''InitializeComponent()

'    Try
'        Using sqlConn As New SqlConnection(Conn)
'            sqlConn.Open()
'            dtpFrom.Value = Now.AddHours(-1)
'            dtpTo.Value = Now
'Dim iListSmpp As List(Of xSmpp) = xSmpp_Data.List(sqlConn)
'Dim iSmpp As New xSmpp
'            iSmpp.SmppID = -1
'            iSmpp.SmppName = "<All>"
'            iListSmpp.Insert(0, iSmpp)

'            cboSmpp.DisplayMember = "SmppName"
'            cboSmpp.ValueMember = "SmppID"
'            cboSmpp.DataSource = iListSmpp
'            cboSmpp.SelectedIndex = 0

'Dim iListState As List(Of xState) = xState_Data.List(sqlConn)
'Dim iState As New xState
'            iState.StateID = -1
'            iState.State = "<All>"
'            iListState.Insert(0, iState)

'            cboState.DisplayMember = "State"
'            cboState.ValueMember = "StateID"
'            cboState.DataSource = iListState
'            cboState.SelectedIndex = 0

'            sqlConn.Close()
'        End Using
'    Catch ex As Exception
'        MsgBox(ex.Message, MsgBoxStyle.Exclamation)
'    End Try
'End Sub
