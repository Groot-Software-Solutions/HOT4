Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmPayment

    Private _Payment As xPayment
    Sub New(ByVal iPayment As xPayment, ByVal iAccount As xAccount)
        InitializeComponent()
        _Payment = iPayment

        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()

            cboType.DisplayMember = "PaymentType"
            cboType.ValueMember = "PaymentTypeID"
            cboType.DataSource = xPaymentTypeAdapter.List(sqlConn)
            cboType.SelectedIndex = 1

            cboSource.DisplayMember = "PaymentSource"
            cboSource.ValueMember = "PaymentSourceID"
            cboSource.DataSource = xPaymentSourceAdapter.List(sqlConn)
            cboSource.SelectedIndex = 1

            sqlConn.Close()
        End Using

        If _Payment.PaymentID = 0 Then
            _Payment.AccountID = iAccount.AccountID
            txtPaymentID.Text = "Pending..."
            txtAccount.Text = iAccount.AccountName
            txtProfileID.Text = iAccount.Profile.ProfileID
            'If iAccount.Profile.ProfileID > 10 And iAccount.Profile.ProfileID < 30 Then
            txtDiscount.Text = "0.0"
            'Else
            '    txtDiscount.Text = "0.5"
            'End If
            Try
                dtDate.Value = Format(Now, "dd/MM/yyyy")
            Catch ex As Exception

            End Try

            txtTime.Text = Format(Now, "HH:mm")
            txtUser.Text = gUser.UserName
        Else

        End If
    End Sub

    Private Function ValidateForm() As Boolean
        Dim Valid As Boolean = True
        If cboType.SelectedIndex = -1 Then
            Err.SetError(cboType, "Payment type must be selected")
            Valid = False
        End If
        If cboSource.SelectedIndex = -1 Then
            Err.SetError(cboSource, "Payment source must be selected")
            Valid = False
        End If
        If cboSource.SelectedItem.ToString = xPaymentSource.PaymentSources.HOTDealer.ToString Then
            Err.SetError(cboSource, "Payment source not valid")
            Valid = False
        End If
        If Not IsNumeric(txtAmount.Text) Then
            Err.SetError(txtAmount, "Amount must be a numeric value")
            Valid = False
        End If


        If txtReference.Text = String.Empty Then
            Err.SetError(txtReference, "Reference must be entered")
            Valid = False
        End If
        If Not IsDate(txtTime.Text) Then
            Err.SetError(txtTime, "Time is invalid")
        End If
        Select Case cboType.SelectedItem.ToString
            Case xPaymentType.PaymentTypes.BankAuto.ToString, xPaymentType.PaymentTypes.zBalanceBF.ToString,
                xPaymentType.PaymentTypes.zServiceFees.ToString ', xPaymentType.PaymentTypes.Writeoff.ToString, xPaymentType.PaymentTypes.HOTTransfer.ToString
                MessageBox.Show("Transaction type is not available", "Error")
                Valid = False
            Case xPaymentType.PaymentTypes.BankManual.ToString
                Select Case cboSource.SelectedItem.ToString
                    Case xPaymentSource.PaymentSources.Commission.ToString, xPaymentSource.PaymentSources.Credit.ToString, xPaymentSource.PaymentSources.Freebie.ToString, _
                        xPaymentSource.PaymentSources.HOTDealer.ToString, xPaymentSource.PaymentSources.MCExecutive.ToString, xPaymentSource.PaymentSources.Office.ToString ', _
                        xPaymentSource.PaymentSources.EcoCash.ToString()
                        MessageBox.Show("Can only load payments with Type Bank Manual to Bank Sources", "Error")
                        Valid = False
                End Select
            Case xPaymentType.PaymentTypes.CreditAdvanced.ToString, xPaymentType.PaymentTypes.CreditRepayment.ToString
                If cboSource.SelectedItem.ToString <> xPaymentSource.PaymentSources.Credit.ToString Then
                    MessageBox.Show("Credit Type transactions can only have a Credit Source", "Error")
                    Valid = False
                End If
            Case xPaymentType.PaymentTypes.Reversal.ToString
                If txtAmount.Text > 0 Then
                    MessageBox.Show("Reversals must be negative values", "Amount Error")
                    Valid = False
                End If


            Case xPaymentType.PaymentTypes.Cash.ToString, xPaymentType.PaymentTypes.CommissionPaid.ToString,
                xPaymentType.PaymentTypes.Freebie.ToString, xPaymentType.PaymentTypes.ShareholdersAllowance.ToString,
                 xPaymentType.PaymentTypes.Direct
                If txtAmount.Text < 0 Then
                    MessageBox.Show("Cash, Commissions & Freebies must be positive values", "Amount Error")
                    Valid = False
                End If
                Select Case cboSource.SelectedItem.ToString
                    Case xPaymentSource.PaymentSources.Commission.ToString, xPaymentSource.PaymentSources.Credit.ToString,
                        xPaymentSource.PaymentSources.Freebie.ToString, xPaymentSource.PaymentSources.MCExecutive.ToString,
                        xPaymentSource.PaymentSources.Office.ToString, xPaymentSource.PaymentSources.EcoCash.ToString,
                        xPaymentSource.PaymentSources.Direct.ToString
                    Case Else
                        MessageBox.Show("Cash Payment Type can only load payments to non-Bank Sources", "Error")
                        Valid = False
                End Select
            Case xPaymentType.PaymentTypes.Depositfees.ToString
                Select Case cboSource.SelectedItem.ToString
                    Case xPaymentSource.PaymentSources.Commission.ToString, xPaymentSource.PaymentSources.Credit.ToString, xPaymentSource.PaymentSources.Freebie.ToString, _
                        xPaymentSource.PaymentSources.MCExecutive.ToString
                        MessageBox.Show("Wrong Source for Deposit Fees", "Wrong Source Error")
                        Valid = False
                    Case Else

                End Select

        End Select
        Return Valid
    End Function
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            If ValidateForm() Then

                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    _Payment.Amount = txtAmount.Text
                    _Payment.LastUser = gUser.UserName
                    _Payment.PaymentDate = dtDate.Value '& " " & txtTime.Text
                    _Payment.PaymentSource = cboSource.SelectedItem
                    _Payment.PaymentType = cboType.SelectedItem
                    _Payment.Reference = txtReference.Text
                    xPaymentAdapter.Save(_Payment, sqlConn)

                    'SMS
                    For Each iAccess As xAccess In xAccessAdapter.List(_Payment.AccountID, sqlConn)
                        If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                            Dim iSMS As New xSMS
                            iSMS.Direction = False
                            iSMS.Mobile = iAccess.AccessCode
                            iSMS.Priority.PriorityID = xPriority.Priorities.Normal
                            iSMS.State.StateID = xState.States.Pending
                            iSMS.SMSDate = Now
                            Dim _Account = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
                            Dim _Balance = GetBalance(_Payment.PaymentType, _Account)
                            Dim _Currency = IIf(_Payment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD Or _Payment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USDUtility, "USD", "ZiG")

                            iSMS.SMSText = "Payment Received: " & vbNewLine &
                                    "Amount: " & _Currency & FormatNumber(_Payment.Amount, 2) & vbNewLine &
                                    "Balance: " & _Currency & FormatNumber(_Balance, 2) & vbNewLine &
                                    "Ref: " & _Payment.Reference & vbNewLine &
                                    "Source: " & _Payment.PaymentSource.PaymentSource & vbNewLine &
                                    "HOT Recharge - your favourite service"
                            xSMSAdapter.Save(iSMS, sqlConn)
                        End If
                        Exit For
                    Next


                    ''KMR Put in a Deposit fee if necessary
                    If CType(txtDiscount.Text, Decimal) > 0 Then
                        '1% Service Charge if correct discount group
                        Dim iPaymentCharge As New xPayment
                            iPaymentCharge.PaymentID = 0
                            iPaymentCharge.AccountID = _Payment.AccountID
                            iPaymentCharge.Amount = 0 - (_Payment.Amount * CType(txtDiscount.Text, Decimal) / 100)
                            iPaymentCharge.LastUser = gUser.UserName
                            iPaymentCharge.PaymentDate = Now
                            iPaymentCharge.PaymentSource.PaymentSourceID = _Payment.PaymentSource.PaymentSourceID
                            iPaymentCharge.PaymentType = New xPaymentType
                            iPaymentCharge.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.Depositfees
                            iPaymentCharge.Reference = "Deposit Fees " & txtReference.Text
                            xPaymentAdapter.Save(iPaymentCharge, sqlConn)
                        'KMR
                        For Each iAccess As xAccess In xAccessAdapter.List(_Payment.AccountID, sqlConn)
                            If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                                Dim iSMSCharge As New xSMS
                                iSMSCharge.Direction = False
                                iSMSCharge.Mobile = iAccess.AccessCode
                                iSMSCharge.Priority.PriorityID = xPriority.Priorities.Normal
                                iSMSCharge.State.StateID = xState.States.Pending
                                iSMSCharge.SMSDate = Now
                                iSMSCharge.SMSText = "Deposit Fees: " & vbNewLine &
                                        "Ref: " & iPaymentCharge.Reference & vbNewLine &
                                        "Amount: " & FormatNumber(iPaymentCharge.Amount, 2) & vbNewLine &
                                        "Balance: " & FormatNumber(xAccountAdapter.SelectRow(iPaymentCharge.AccountID, sqlConn).Balance, 2) & vbNewLine &
                                        "Source: " & _Payment.PaymentSource.ToString & vbNewLine &
                                        "HOT Recharge - your favourite service"
                                xSMSAdapter.Save(iSMSCharge, sqlConn)
                            End If
                            Exit For
                        Next
                    End If
                    sqlConn.Close()
                End Using
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Function GetBalance(paymentType As xPaymentType, account As xAccount) As Object
        If paymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD Then Return account.USDBalance
        If paymentType.PaymentTypeID = xPaymentType.PaymentTypes.USDUtility Then Return account.USDUtilityBalance
        Return account.Balance

    End Function

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub
End Class