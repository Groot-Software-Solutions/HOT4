Imports System.Data.SqlClient
Imports EcoCashAPI
Imports Hot.Data

Public Class ctlBank

    Sub New()
        InitializeComponent()
        Bind()
    End Sub

    Private Sub tvw_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvw.AfterSelect
        Try
            tvw.SelectedNode = e.Node
            If TypeOf tvw.SelectedNode.Tag Is xBankTrxBatch Then
                BindTrx(tvw.SelectedNode)
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub Bind()
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                For Each iBank In xBankAdapter.List(sqlConn)
                    Dim nodeBank As New TreeNode(iBank.Bank)
                    nodeBank.Tag = iBank
                    nodeBank.Name = iBank.BankID
                    If DBRoleAllowsFunction(xHotUIFunction.Load_Statement) Then nodeBank.ContextMenuStrip = mnuBank
                    nodeBank.ImageKey = "Book_angleHS.png"
                    nodeBank.SelectedImageKey = "Book_openHS.png"
                    BindBatch(nodeBank, sqlConn)
                    tvw.Nodes.Add(nodeBank)
                Next
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub BindBatch(ByVal nodeBank As TreeNode, ByVal sqlConn As SqlConnection)
        nodeBank.Nodes.Clear()
        For Each iBatch In xBankTrxBatchAdapter.List(nodeBank.Name, sqlConn)
            Dim nodeBatch As New TreeNode(Format(iBatch.BatchDate, "dd MMM yyyy HH:mm") & " -> " & iBatch.BatchReference)
            nodeBatch.Tag = iBatch
            nodeBatch.Name = iBatch.BankTrxBatchID
            nodeBatch.ImageKey = "DocumentHS.png"
            nodeBatch.SelectedImageKey = "PageUpHS.png"
            If nodeBank.Name = "6" Then nodeBatch.ContextMenuStrip = mnuEcoCash
            nodeBank.Nodes.Add(nodeBatch)
        Next
    End Sub
    Private Sub BindTrx(ByVal nodeBatch As TreeNode)
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource
                bs.DataSource = xBankTrxAdapter.List(nodeBatch.Name, sqlConn)
                dg.DataSource = bs
                sqlConn.Close()
                dg.Columns("Amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dg.Columns("Amount").DefaultCellStyle.Format = "#,##0.00"
                dg.Columns("RefName").Width = 150
                dg.AutoResizeColumn(8)

            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub mnuBatchLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBatchLoad.Click
        Try
            Dim f As New frmBankLoader(tvw.SelectedNode.Name)
            If f.ShowDialog = DialogResult.OK Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    BindBatch(tvw.SelectedNode, sqlConn)
                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Try
            If dg.SelectedRows.Count = 1 And DBRoleAllowsFunction(xHotUIFunction.Save_Payment) Then
                Dim iBankTrx As xBankTrx = dg.SelectedRows(0).DataBoundItem
                Dim message As String = "This transaction has already been allocated"
                If iBankTrx.PaymentID > 0 Then
                    Using sqlConn As New SqlConnection(Conn)
                        sqlConn.Open()
                        Dim payment = xPaymentAdapter.SelectPayment(iBankTrx.PaymentID, sqlConn)
                        If Not payment Is Nothing Then
                            Dim account = xAccountAdapter.SelectRow(payment.AccountID, sqlConn)
                            If Not account Is Nothing Then
                                message = $"This transaction has already been allocated to " + vbNewLine + $" {account.AccountName}({account.ReferredBy}) " + vbNewLine + $"on {payment.PaymentDate:dd-MMM-yy HH:mm} by {payment.LastUser}"
                            End If
                        End If
                        sqlConn.Close()
                    End Using
                    MsgBox(message, MsgBoxStyle.Information, "Action Cancelled")
                Else
                    Select Case iBankTrx.BankTrxState.BankTrxStateID
                        Case xBankTrxState.BankTrxStates.Success, xBankTrxState.BankTrxStates.Ignore
                            MsgBox("This transaction has already been allocated", MsgBoxStyle.Information, "Action Cancelled")
                        Case xBankTrxState.BankTrxStates.Suspended, xBankTrxState.BankTrxStates.Pending, xBankTrxState.BankTrxStates.BusyConfirming, xBankTrxState.BankTrxStates.ToBeAllocated
                            Select Case tvw.SelectedNode.Parent.Tag.BankID
                                Case 6 'EcoMerchant
                                    If iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming Or
                                        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending Or
                                        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated Then

                                        Dim iEcoTran As xEcoCashTransaction = xEcoCashAPI.QueryTransaction(iBankTrx.Identifier, iBankTrx.BankTrxID)
                                        ProcessEcoCashTransaction(iBankTrx, iEcoTran)
                                    Else
                                        GoTo ProcessSupense
                                    End If
                                Case Else
ProcessSupense:
                                    Dim f As New frmSuspense(iBankTrx, tvw.SelectedNode.Parent.Tag)
                                    f.Location = New System.Drawing.Point(0, 30)
                                    If f.ShowDialog = DialogResult.OK Then
                                        BindTrx(tvw.SelectedNode)
                                    End If
                            End Select

                    End Select
                End If
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub dg_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles dg.DataBindingComplete
        Try
            For Each iRow As DataGridViewRow In dg.Rows
                Select Case CType(iRow.DataBoundItem, xBankTrx).BankTrxState.BankTrxStateID
                    Case xBankTrxState.BankTrxStates.Pending, xBankTrxState.BankTrxStates.ToBeAllocated
                        iRow.DefaultCellStyle.BackColor = Color.White
                    Case xBankTrxState.BankTrxStates.Success, xBankTrxState.BankTrxStates.ManuallyAllocated
                        iRow.DefaultCellStyle.BackColor = Color.LightSeaGreen
                        iRow.DefaultCellStyle.SelectionBackColor = Color.ForestGreen
                    Case xBankTrxState.BankTrxStates.Suspended
                        iRow.DefaultCellStyle.BackColor = Color.MistyRose
                        iRow.DefaultCellStyle.SelectionBackColor = Color.Crimson
                    Case xBankTrxState.BankTrxStates.Ignore
                        iRow.DefaultCellStyle.BackColor = Color.Gainsboro
                        iRow.DefaultCellStyle.SelectionBackColor = Color.Gray
                End Select
            Next
            dg.ClearSelection()
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Sub ProcessEcoCashTransaction(iBankTrx As xBankTrx, econetResponse As EcoCashAPI.xEcoCashTransaction, Optional AllowOveride As Boolean = True)
        Using sqlConn As New SqlConnection(Conn)
            Try
                If Not sqlConn.State = ConnectionState.Open Then sqlConn.Open()

                Select Case UCase(econetResponse.transactionOperationStatus)
                    Case "COMPLETED"
                        'hot tran done
                        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iBankTrx.Identifier, sqlConn)

                        If Not (iAccess Is Nothing) Then
                            Dim iPayment As New xPayment
                            iPayment.Reference = "EcoCash payment successful. EcoCash Ref: " + econetResponse.ecocashReference
                            iPayment.AccountID = iAccess.AccountID
                            iPayment.Amount = iBankTrx.Amount
                            iPayment.PaymentDate = iBankTrx.TrxDate
                            iPayment.LastUser = gUser.UserName
                            iPayment.PaymentType = New xPaymentType With {.PaymentType = CType(xPaymentType.PaymentTypes.BankAuto, String), .PaymentTypeID = xPaymentType.PaymentTypes.BankAuto}
                            iPayment.PaymentSource = New xPaymentSource With {.PaymentSource = CType(xPaymentSource.PaymentSources.EcoCash, String), .PaymentSourceID = xPaymentSource.PaymentSources.EcoCash}
                            xPaymentAdapter.Save(iPayment, sqlConn)

                            iBankTrx.PaymentID = iPayment.PaymentID
                            If iBankTrx.BankRef <> econetResponse.ecocashReference Then iBankTrx.BankRef = econetResponse.ecocashReference
                            iBankTrx.BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.Success, .BankTrxState = CType(xBankTrxState.BankTrxStates.Success, String)}
                            xBankTrxAdapter.Save(iBankTrx, sqlConn)

                            Dim iSMS As New xSMS
                            iSMS.Direction = False
                            iSMS.Mobile = iBankTrx.Identifier
                            iSMS.Priority.PriorityID = xPriority.Priorities.Normal
                            iSMS.State.StateID = xState.States.Pending
                            iSMS.SMSDate = Now
                            iSMS.SMSText = "Ecocash Payment Received" & vbNewLine &
                                "Amount: " & FormatNumber(iPayment.Amount, 2) & vbNewLine &
                                "Balance: " & FormatNumber(xAccountAdapter.SelectRow(iPayment.AccountID, sqlConn).Balance, 2) & vbNewLine &
                                "Ref: " & iPayment.Reference & vbNewLine &
                                "Source: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                                "HOT Recharge - your favourite service"
                            xSMSAdapter.Save(iSMS, sqlConn)
                            If AllowOveride Then
                                If MsgBox("This transaction has been completed and allocated", MsgBoxStyle.Information, "Transaction Complete") Then
                                    BindTrx(tvw.SelectedNode)
                                End If
                            End If
                        Else
                                GoTo ManuallyAllocation
                        End If

                    Case UCase("FAILED")
                        iBankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Failed
                        xBankTrxAdapter.UpdateState(iBankTrx, sqlConn)
                        If MsgBox("This transaction was marked as failed by Ecocash", MsgBoxStyle.Information, "Transaction Failed") Then
                            BindTrx(tvw.SelectedNode)
                        End If
                    Case Else

ManuallyAllocation:
                        If AllowOveride Then
                            Dim f As New frmSuspense(iBankTrx, tvw.SelectedNode.Parent.Tag)
                            f.Location = New System.Drawing.Point(0, 30)
                            If f.ShowDialog = DialogResult.OK Then
                                BindTrx(tvw.SelectedNode)
                            End If
                        End If

                End Select

            Catch ex As Exception
                ShowEx(ex)
            End Try
        End Using
    End Sub


    Sub CheckEcoCashBatch(ByVal nodeBatch As TreeNode)
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            Dim transactionProcessed As Integer = 0
            Dim iPending As List(Of xBankTrx) = (From iTran As xBankTrx In xBankTrxAdapter.List(nodeBatch.Name, sqlConn)
                                                 Where iTran.TrxDate < Date.Now.AddMinutes(-1) And
                                                     (iTran.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.BusyConfirming Or iTran.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ToBeAllocated)
                                                 Select iTran).ToList()

            For Each iBankTrx As xBankTrx In iPending
                Dim iEcoCashTransaction As xEcoCashTransaction = xEcoCashAPI.QueryTransaction(iBankTrx.Identifier, iBankTrx.BankTrxID)
                If iEcoCashTransaction.ValidResponse Then ProcessEcoCashTransaction(iBankTrx, iEcoCashTransaction, False)
                If iEcoCashTransaction.ValidResponse And iEcoCashTransaction.transactionOperationStatus = "COMPLETED" Then transactionProcessed += 1

            Next
            sqlConn.Close()
            If transactionProcessed > 0 Then
                MsgBox(CStr(transactionProcessed) + " unprocessed transactions have been processed")
            Else
                MsgBox("No outstanding transcations available to process")
            End If
        End Using
    End Sub


    Private Sub mnuEcoCashCheck_Click(sender As Object, e As EventArgs) Handles mnuEcoCashCheck.Click
        Try
            If TypeOf tvw.SelectedNode.Tag Is xBankTrxBatch Then
                CheckEcoCashBatch(tvw.SelectedNode)
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub


End Class
