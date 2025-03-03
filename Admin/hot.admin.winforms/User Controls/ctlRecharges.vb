Imports System.Data.SqlClient
Imports Hot.Data
Public Class ctlRecharges

    Private Sub txtFilter_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFilter.KeyUp
        If e.KeyCode = Keys.Enter Then
            Search()
        End If
    End Sub
    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Search()
    End Sub

    Private Sub Search()
        Try
            Me.Cursor = Cursors.WaitCursor
            lblSearch.Visible = True
            If txtFilter.Text.Length < 3 Then
                Err.SetError(txtFilter, "Filter must be at least 3 characters")
                txtFilter.Focus()
                Exit Sub
            End If
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                xBrandAdapter.List(sqlConn)
                Dim bs As New BindingSource
                Dim iAccessById As New Dictionary(Of Long, xAccess)
                Dim iRecharges As New List(Of xRechargeAccess)
                Dim iRechargeRaw As List(Of xRecharge) = xAccountAdapter.RechargeFindByMobile(txtFilter.Text, sqlConn)

                For Each iAccessID As Long In (From a As xRecharge In iRechargeRaw Select a.AccessID Distinct).ToList()
                    Dim iAccess As xAccess = xAccessAdapter.SelectRow(iAccessID, sqlConn)
                    iAccessById.Add(iAccessID, iAccess)
                Next

                For Each iRecharge As xRecharge In iRechargeRaw
                    iRecharges.Add(New xRechargeAccess(iRecharge, iAccessById(iRecharge.AccessID)))
                Next

                bs.DataSource = iRecharges
                dgRecharge.DataSource = bs

                dgRecharge.Columns(0).DisplayIndex = 4
                dgRecharge.Columns(1).DisplayIndex = 3
                dgRecharge.Columns(6).DisplayIndex = 5
                dgRecharge.Columns(2).DisplayIndex = dgRecharge.Columns.Count - 1
                dgRecharge.Columns(5).Visible = False
                dgRecharge.Columns(11).Visible = False
                dgRecharge.Columns(12).Visible = False
                dgRecharge.Columns(8).DisplayIndex = 5


                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        Finally
            Me.Cursor = Cursors.Default
            lblSearch.Visible = False
        End Try
    End Sub

    Private Sub cmdReversal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReversal.Click
        Try
            If dgRecharge.SelectedRows.Count = 1 Then
                If MsgBox("Reverse Transaction", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Are you sure") = MsgBoxResult.Yes Then
                    Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                    If iRecharge.Amount < 0 Then
                        Throw New Exception("Cannot reverse a debit transaction")
                    End If
                    Dim iRechargePrepaid As New xRechargePrepaid
                    Using sqlConn As New SqlConnection(Conn)
                        sqlConn.Open()
                        'Get corresponding Econet Reference for reversal
                        iRechargePrepaid = xRechargeAdapter.SelectRechargePrepaid(iRecharge.RechargeID, sqlConn)
                        'set up for the Reversal Trx
                        Dim iAccess As xAccess = xAccessAdapter.SelectRow(iRecharge.AccessID, sqlConn)

                        Dim iWebservice As New HotWebService.ServiceSoapClient
                        Dim iRet As HotWebService.ReturnObject
                        iRet = iWebservice.HOTRechargeReversal(iAccess.AccessCode, iAccess.AccessPassword, iRecharge.RechargeID)
                        MsgBox("Reversal" & iRecharge.Mobile & " is " & iRet.ReturnValue & " Message: " & iRet.ReturnMsg, , "Reversal")
                        sqlConn.Close()
                    End Using
                    'FindRecharge()
                End If
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub btnPhoneBal_Click(sender As System.Object, e As System.EventArgs) Handles btnPhoneBal.Click
        Try
            lblSearch.Visible = True
            lblSearch.Update()
            If dgRecharge.SelectedRows.Count = 1 Then
                Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iWebservice As New Hot.API.Interface.HotAPIClient
                    Dim iRet As Hot.API.Interface.Models.EndUserBalanceResponse
                    ' Method needs Later by to allow console to query as console user
                    iWebservice.baseUrl = "http://hot.co.zw/api/v1/agents/"
                    iWebservice.accessCode = "0772929223"
                    iWebservice.accessPassword = "1211"

                    iRet = iWebservice.GetEndUserBalance(iRecharge.Mobile)
                    MsgBox("Balance of " & iRecharge.Mobile & " is $" & iRet.MobileBalance & vbNewLine & iRet.ReplyMsg, , "Phone Balance")
                    sqlConn.Close()
                End Using
            End If
            lblSearch.Visible = False
            lblSearch.Update()
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub btnDebit_Click(sender As System.Object, e As System.EventArgs) Handles btnDebit.Click
        Try
            If dgRecharge.SelectedRows.Count = 1 Then
                Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                If MsgBox("Debit the " & iRecharge.Mobile & " Selected Customer " & FormatCurrency(txtDebit.Text, 2), MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then

                    If IsNumeric(txtDebit.Text) Then
                        If txtDebit.Text <= 0 Then
                            Throw New Exception("Cannot debit 0 or -ve amount transaction")
                        End If

                        Using sqlConn As New SqlConnection(Conn)
                            sqlConn.Open()
                            'set up for the Reversal Trx
                            iRecharge.Amount = 0 - txtDebit.Text
                            iRecharge.RechargeDate = Now
                            iRecharge.State.StateID = xState.States.Pending
                            iRecharge.RechargeID = 0

                            xRechargeAdapter.Save(iRecharge, sqlConn)
                            sqlConn.Close()
                        End Using
                        'FindRecharge()
                        MsgBox("Debit was inserted, check progress with a Find recharge")
                    End If
                End If
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub dgRecharge_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgRecharge.CellContentClick ', dgRecharge.SelectionChanged
        'Looks for the detail for the specific recharge and displays it in the second data grid
        Try
            lblSearch.Visible = True
            lblSearch.Update()
            If dgRecharge.SelectedRows.Count = 1 Then
                Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                dgRechargeDetail.Rows.Clear()
                Select Case iRecharge.Brand.BrandID
                    Case xBrand.Brands.Juice
                        Try
                            Using sqlConn As New SqlConnection(Conn)
                                sqlConn.Open()
                                Dim bs2 As New BindingSource
                                bs2.DataSource = xRechargeAdapter.SelectRechargePrepaid(iRecharge.RechargeID, sqlConn)
                                dgRechargeDetail.DataSource = bs2
                                sqlConn.Close()
                            End Using
                        Catch ex As Exception
                            Using sqlConn As New SqlConnection(Conn)
                                sqlConn.Open()
                                Dim bs2 As New BindingSource
                                bs2.DataSource = xRechargeAdapter.SelectRechargePin(iRecharge.RechargeID, sqlConn)
                                dgRechargeDetail.DataSource = bs2
                                sqlConn.Close()
                            End Using
                        End Try
                    Case Else
                        Using sqlConn As New SqlConnection(Conn)
                            sqlConn.Open()
                            Dim bs2 As New BindingSource
                            bs2.DataSource = xRechargeAdapter.SelectRechargePrepaid(iRecharge.RechargeID, sqlConn)
                            dgRechargeDetail.DataSource = bs2
                            sqlConn.Close()
                        End Using
                End Select
            End If
            lblSearch.Visible = False
            lblSearch.Update()
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    'Private Sub dgRecharge_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgRecharge.CellContentDoubleClick
    '    If dgRecharge.SelectedRows.Count = 1 Then
    '        Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
    '        Dim _edit As New frmRechargeEdit(iRecharge)

    '    End If
    '    Search()

    'End Sub

End Class
