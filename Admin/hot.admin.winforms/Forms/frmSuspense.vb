Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmSuspense

    Private _BankTrx As xBankTrx
    Private ReadOnly _Bank As xBank

    Sub New(ByVal iBankTrx As xBankTrx, ByVal iBank As xBank)
        InitializeComponent()
        _BankTrx = iBankTrx
        _Bank = iBank

        txtTransactionID.Text = iBankTrx.BankTrxID
        'Using sqlConn As New SqlConnection(Conn)
        '    sqlConn.Open()
        '    cboBank.DisplayMember = "Bank"
        '    cboBank.ValueMember = "BankID"
        '    cboBank.DataSource = xBankAdapter.List(sqlConn)
        '    sqlConn.Close()

        'End Using
        txtBank.text = iBank.Bank
        txtAmount.Text = iBankTrx.Amount
        txtReference.Text = iBankTrx.RefName
    End Sub
    Private Sub cmdIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdIgnore.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
        Try
            If String.IsNullOrEmpty(txtIgnore.Text) Then Throw New Exception("Please enter reason for ignoring transaction")
            _BankTrx.Identifier = $"{txtIgnore.Text}"
            _BankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Ignore
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                xBankTrxAdapter.UpdateState(_BankTrx, sqlConn)
                xBankTrxAdapter.UpdateIndentifier(_BankTrx, sqlConn)
                sqlConn.Close()
            End Using
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub cmdFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFind.Click
        Dim f As New frmAccountSearch With {
            .Location = New System.Drawing.Point(0, 340)
        }
        f.txtFilter.Text = _BankTrx.Identifier

        If f.ShowDialog = vbOK Then

            txtAccount.Text = f.Account.AccountName
            txtAccount.Tag = f.Account
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
        Try
            If TypeOf txtAccount.Tag Is xAccount Then
                Dim iAccount As xAccount = txtAccount.Tag

                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()

                    Dim iPayment As New xPayment With {
                        .PaymentID = 0,
                        .AccountID = iAccount.AccountID,
                        .Amount = _BankTrx.Amount,
                        .LastUser = gUser.UserName,
                        .PaymentDate = Now,
                        .PaymentSource = New xPaymentSource,
                        .PaymentType = New xPaymentType With {
                            .PaymentTypeID = xPaymentType.PaymentTypes.BankAuto
                        }
                    }

                    'Select Case CType(cboBank.SelectedValue, xBank.Banks)
                    Select Case _Bank.BankID
                        Case xBank.Banks.AgriBank
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Agribank
                        Case xBank.Banks.CABS
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.CABS
                        Case xBank.Banks.Kingdom
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Kingdom
                        Case xBank.Banks.vPayments
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.vPayment
                        Case xBank.Banks.CBZ
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.CBZ
                        Case xBank.Banks.OneMoney
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.OneMoney
                        Case xBank.Banks.EcoMerchant
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.EcoCash
                        Case xBank.Banks.Stanbic
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Stanbic
                        Case xBank.Banks.StanbicZesa
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.ZesaStanbic
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.ZESA
                        Case xBank.Banks.StanbicUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDStanbic
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                        Case xBank.Banks.StewardBank
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.StewardBank
                        Case xBank.Banks.CABSUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDCabs
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                        Case xBank.Banks.CBZUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDCBZ
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                    End Select
                    'iPayment.PaymentSource.PaymentSourceID = _Bank.BankID

                    If _Bank.BankID = xBank.Banks.EcoMerchant And _BankTrx.Branch.StartsWith("ZESA") Then
                        iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.ZESA

                    End If


                    iPayment.Reference = _BankTrx.RefName
                    xPaymentAdapter.Save(iPayment, sqlConn)
                    _BankTrx.PaymentID = iPayment.PaymentID
                    _BankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success
                    xBankTrxAdapter.Save(_BankTrx, sqlConn)

                    'SMS

                    Dim iPaymentAccess As xAccess = Nothing
                    For Each iAccess In xAccessAdapter.List(iAccount.AccountID, sqlConn)
                        If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then
                            iPaymentAccess = iAccess
                            Exit For
                        End If
                    Next
                    If iPaymentAccess IsNot Nothing Then
                        Dim iSMS As New xSMS With {
                            .Direction = False,
                            .Mobile = iPaymentAccess.AccessCode,
                            .SMSDate = Now
                        }
                        Dim _Account = xAccountAdapter.SelectRow(iPayment.AccountID, sqlConn)
                        Dim _Balance = IIf(iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD, _Account.USDBalance, _Account.Balance)
                        Dim _Currency = IIf(iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD, "USD", "ZiG")

                        iSMS.Priority.PriorityID = xPriority.Priorities.Normal
                        iSMS.State.StateID = xState.States.Pending
                        iSMS.SMSText = "Payment Received: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                                "Amount: " & _Currency & FormatNumber(iPayment.Amount, 2) & vbNewLine &
                                "Balance: " & _Currency & FormatNumber(_Balance, 2) & vbNewLine &
                                "Ref: " & iPayment.Reference & vbNewLine &
                                "Source: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                                "HOT Recharge - your favourite service"
                        xSMSAdapter.Save(iSMS, sqlConn)

                        _BankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success
                        xBankTrxAdapter.UpdateState(_BankTrx, sqlConn)

                    End If

                    'KMR Put in a Deposit fee if necessary
                    If CType(txtDiscount.Text, Decimal) > 0 Then
                        Dim iPaymentCharge As New xPayment With {
                            .PaymentID = 0,
                            .AccountID = iAccount.AccountID,
                            .Amount = 0 - (_BankTrx.Amount * CType(txtDiscount.Text, Decimal) / 100),
                            .LastUser = gUser.UserName,
                            .PaymentDate = Now,
                            .Reference = _BankTrx.RefName & _BankTrx.BankTrxID
                        }
                        iPaymentCharge.PaymentSource.PaymentSourceID = iPayment.PaymentSource.PaymentSourceID
                        iPaymentCharge.PaymentType = New xPaymentType With {
                            .PaymentTypeID = xPaymentType.PaymentTypes.Depositfees
                        }
                        xPaymentAdapter.Save(iPaymentCharge, sqlConn)

                        Dim iSMSCharge As New xSMS With {
                            .Direction = False,
                            .Mobile = iPaymentAccess.AccessCode,
                            .SMSDate = Now
                        }
                        iSMSCharge.Priority.PriorityID = xPriority.Priorities.Normal
                        iSMSCharge.State.StateID = xState.States.Pending
                        iSMSCharge.SMSText = "Deposit Fees: " & vbNewLine &
                                "Amount: " & FormatNumber(iPaymentCharge.Amount, 2) & vbNewLine &
                                "Balance: " & FormatNumber(xAccountAdapter.SelectRow(iPaymentCharge.AccountID, sqlConn).Balance, 2) & vbNewLine &
                                "Ref: " & iPaymentCharge.Reference & vbNewLine &
                                "Source: " & iPaymentCharge.PaymentSource.PaymentSource & vbNewLine &
                                "HOT Recharge - your favourite service"
                        xSMSAdapter.Save(iSMSCharge, sqlConn)

                    End If
                    'KMR
                    sqlConn.Close()
                End Using
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                MsgBox("Please select an account before continuing", MsgBoxStyle.Information, "Action Cancelled")
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
        Try
            _BankTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.ManuallyAllocated
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                xBankTrxAdapter.UpdateState(_BankTrx, sqlConn)
                sqlConn.Close()
            End Using
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click

    End Sub
End Class