Imports System.Data.SqlClient
Imports Hot.Data

Public Class ctlAccount

    Private _Account As xAccount
    Private _AccessList As List(Of xAccess)
    Private _Limit As xLimit

#Region " Init "
    Public Sub Init(ByVal iAccount As xAccount)

        _Account = iAccount

        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()

            cboProfile.ValueMember = "ProfileID"
            cboProfile.DisplayMember = "ProfileName"
            cboProfile.DataSource = xProfileAdapter.List(sqlConn)

            If _Account.AccountID = 0 Then
                txtAcountID.Text = "Pending..."
                txtBalance.Text = "0.00"
                txtUSDUtilityBalance.Text = "0.00"
                TabInvoicing.Visible = False
                _AccessList = New List(Of xAccess)
            Else
                txtAcountID.Text = _Account.AccountID
                txtAccountName.Text = _Account.AccountName
                cboProfile.SelectedValue = _Account.Profile.ProfileID
                txtNationalID.Text = _Account.NationalID
                txtEmail.Text = _Account.Email
                txtReferredBy.Text = _Account.ReferredBy
                txtBalance.Text = FormatNumber(_Account.Balance, 2)
                txtUSDUtilityBalance.Text = FormatNumber(_Account.USDUtilityBalance, 2)
                txtZESABalance.Text = FormatNumber(_Account.ZESABalance, 2)
                txtUSDBalance.Text = FormatNumber(_Account.USDBalance, 2)
                _AccessList = xAccessAdapter.List(_Account.AccountID, sqlConn)
                _Limit = xLimitAdapter.GetLimit(1, _Account.AccountID, sqlConn)
                txtRemainingLimit.Text = FormatNumber(_Limit.LimitRemaining, 2)
                txtMonthLimit.Text = FormatNumber(_Limit.MonthlyLimit, 2)

                LoadNamesForAccessWeb(sqlConn)
                Dim iList As New List(Of xAccess)
                iList = xAccessAdapter.ListDeleted(_Account.AccountID, sqlConn)
                If iList.Count > 0 Then
                    _AccessList.AddRange(iList)
                End If


                BindAccess(sqlConn)
                BindPayment(sqlConn)
                Try
                    dtSMSDate.Value = Format(Now, "dd/MM/yyyy")
                Catch ex As exception
                End Try


                TabInvoicing.Visible = True
            End If

            sqlConn.Close()
        End Using
        ' UI Setup
        cmdPaymentAdd.Visible = DBRoleAllowsFunction(xHotUIFunction.Save_Payment) ' Add Payment
        ' Access User Tab 
        cmdReset.Visible = DBRoleAllowsFunction(xHotUIFunction.Reset_Password)
        cmdNewAccess.Visible = DBRoleAllowsFunction(xHotUIFunction.Add_Access)
        cmdAccessDelete.Visible = DBRoleAllowsFunction(xHotUIFunction.Delete_Access)
        cmdEditAccess.Visible = DBRoleAllowsFunction(xHotUIFunction.Edit_Access)
        'Debit Mobile Account Section
        Label11.Visible = DBRoleAllowsFunction(xHotUIFunction.Debit_Mobile_Account)
        txtDebit.Visible = DBRoleAllowsFunction(xHotUIFunction.Debit_Mobile_Account)
        btnDebit.Visible = DBRoleAllowsFunction(xHotUIFunction.Debit_Mobile_Account)
        ' Transaction Reversal
        cmdReversal.Visible = DBRoleAllowsFunction(xHotUIFunction.Debit_Mobile_Account)



    End Sub

    Private Sub LoadNamesForAccessWeb(sqlConn As SqlConnection)
        For i = 0 To _AccessList.Count - 1
            Dim _iAccessWeb As xAccessWeb = xAccessWebAdapter.SelectRow(_AccessList(i).AccessID, sqlConn)
            If Not (_iAccessWeb Is Nothing) Then
                _AccessList(i).AccessPassword = _iAccessWeb.AccessName
            Else
                _AccessList(i).AccessPassword = ""
            End If
        Next
    End Sub

#End Region

#Region " Account "
    Private Function ValidateForm() As Boolean
        Dim Valid As Boolean = True
        If txtAccountName.Text = String.Empty Then
            Err.SetError(txtAccountName, "Account name may not be blank")
            Valid = False
        End If
        If cboProfile.SelectedIndex = -1 Then
            Err.SetError(cboProfile, "Profile must be selected")
            Valid = False
        End If
        Return Valid
    End Function
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            If ValidateForm() Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    _Account.AccountName = txtAccountName.Text
                    _Account.Email = txtEmail.Text
                    _Account.NationalID = txtNationalID.Text
                    _Account.Profile = cboProfile.SelectedItem
                    _Account.ReferredBy = txtReferredBy.Text
                    xAccountAdapter.Save(_Account, sqlConn)

                    'By Ross Save edited & new access
                    'For Each iAccess As xAccess In _AccessList
                    '    xAccessAdapter.Save(iAccess, sqlConn)
                    'Next

                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub dgPayment_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgPayment.CellDoubleClick
        Try
            If Not dgPayment.SelectedRows.Count = 1 Then Return

            Dim iPayment As xPayment = dgPayment.SelectedRows(0).DataBoundItem
            If Not iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.HOTDealer Then Return
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iTransfer = xTransferAdapter.SelectRow(iPayment.PaymentID, sqlConn)
                If iTransfer Is Nothing Then Return
                Dim otherPaymentId = If(iTransfer.PaymentID_From = iPayment.PaymentID, iTransfer.PaymentID_To, iTransfer.PaymentID_From)
                Dim iPaymentOnOtherAccount = xPaymentAdapter.SelectPayment(otherPaymentId, sqlConn)
                Dim iOtherAccount = xAccountAdapter.SelectRow(iPaymentOnOtherAccount.AccountID, sqlConn)
                Dim tb As TabPage = frmConsole.tcMain.TabPages(TABKEY_ACCOUNT & iOtherAccount.AccountID)
                If tb Is Nothing Then
                    tb = AddTab(TABKEY_ACCOUNT & iOtherAccount.AccountID, iOtherAccount.AccountName)
                    tb.ImageKey = "user.ico"
                    Dim iCtl As New ctlAccount
                    iCtl.Init(iOtherAccount)
                    iCtl.Dock = DockStyle.Fill
                    tb.Controls.Add(iCtl)
                End If
                frmConsole.tcMain.SelectedTab = tb
                tb.Controls(0).Focus()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
#End Region

#Region " Access "
    Private Sub BindAccess(ByVal sqlConn As SqlConnection)
        ' The following code found in Other Functions implies that this function will 
        ' reload AccessList data but Sub doesn't actually do this need to be looked into.
        ' Using sqlConn As New SqlConnection(Conn)    
        '    sqlConn.Open()                          
        '    BindAccess(sqlConn)                     
        '    sqlConn.Close()                            
        ' End Using

        Dim bs As New BindingSource
        bs.DataSource = _AccessList
        dgAccess.DataSource = bs
        dgAccess.Columns(4).HeaderText = "Access Name" ' Access Name Workaround
        dgAccess.Columns(dgAccess.ColumnCount - 1).Visible = False ' Hide Password Hash and Salt fields 
        dgAccess.Columns(dgAccess.ColumnCount - 2).Visible = False
    End Sub

    Private Sub OpenAccess(ByVal iAccess As xAccess, ByVal Add As Boolean)
        Try
            Dim f As New frmHOTAccess(iAccess, _Account.AccountID)
            f.StartPosition = FormStartPosition.CenterScreen
            If f.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Add Then
                    _AccessList.Add(f.GetAccess)
                Else
                    iAccess = f.GetAccess
                End If
                Using sqlConn As New SqlConnection(Conn)    ' Relevance of this section need to be re-evaluated
                    sqlConn.Open()                          '
                    BindAccess(sqlConn)                     '
                    sqlConn.Close()                         '   
                End Using                                   '
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Try
            If dgAccess.SelectedRows.Count = 1 Then
                Dim iAccess As xAccess = dgAccess.SelectedRows(0).DataBoundItem

                Dim rnd As New Random
                Dim iPassword As String = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
                'Generate Hash for Web user from Pin
                If iAccess.Channel.ChannelID = xChannel.Channels.Web Then
                    Dim sha256 As Security.Cryptography.SHA256 = Security.Cryptography.SHA256Managed.Create()
                    Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(iPassword)
                    Dim hash As Byte() = sha256.ComputeHash(bytes)
                    Dim stringBuilder As New System.Text.StringBuilder()
                    For i As Integer = 0 To hash.Length - 1
                        stringBuilder.Append(hash(i).ToString("X2"))
                    Next
                    iPassword = stringBuilder.ToString().Substring(0, 8)
                End If

                iAccess.AccessPassword = iPassword
                Using sqlConn As New SqlConnection(Conn)
                        sqlConn.Open()
                        Dim sqltrans As SqlTransaction = sqlConn.BeginTransaction
                        Try
                            xAccessAdapter.PasswordChange(iAccess, iAccess.AccessPassword, sqlConn, sqltrans)

                            ' !!!!!!!!
                            ' Work around for Temporarily not using encrpyted Password to be removed when we swap over

                            Dim sqlCmd As New SqlCommand("xAccess_PasswordChange", sqlConn, sqltrans)
                            sqlCmd.CommandType = CommandType.StoredProcedure
                            sqlCmd.Parameters.AddWithValue("@AccessID", iAccess.AccessID)
                            sqlCmd.Parameters.AddWithValue("@NewPassword", iPassword)
                            sqlCmd.ExecuteNonQuery()

                            '!!!!!!!

                            sqltrans.Commit()
                        Catch ex As Exception
                            sqltrans.Rollback()
                            Throw ex
                        End Try
                        If iAccess.Channel.ChannelID = xChannel.Channels.SMS Then

                            Dim iReply As xTemplate = xTemplateAdapter.SelectRow(xTemplate.Templates.SuccessfulRegistration, sqlConn)
                            iReply.TemplateText = iReply.TemplateText.Replace("%PASSWORD%", iPassword)

                            Dim iSMSReply As New xSMS
                            iSMSReply.Direction = False
                            iSMSReply.Mobile = iAccess.AccessCode
                            iSMSReply.Priority.PriorityID = xPriority.Priorities.Normal
                            iSMSReply.SMSDate = Now
                            iSMSReply.SMSText = iReply.TemplateText
                            iSMSReply.State.StateID = xState.States.Pending
                            xSMSAdapter.Save(iSMSReply, sqlConn)
                        End If
                        BindAccess(sqlConn)
                        sqlConn.Close()
                    End Using
                    MsgBox("Password Reset - New Password: " & iPassword, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Success")
                End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewAccess.Click
        OpenAccess(New xAccess, True)
    End Sub

    Private Sub cmdEditAccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditAccess.Click
        If dgAccess.SelectedRows.Count = 1 Then

            Dim iAccess As New xAccess
            iAccess = (dgAccess.SelectedRows(0).DataBoundItem)
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                iAccess = xAccessAdapter.SelectRow(iAccess.AccessID, sqlConn)
                sqlConn.Close()
            End Using

            OpenAccess(iAccess, False)
        End If
    End Sub

    Private Sub dgAccess_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgAccess.CellDoubleClick
        If dgAccess.SelectedRows.Count = 1 And DBRoleAllowsFunction(xHotUIFunction.Edit_Access) Then

            Dim iAccess As New xAccess
            iAccess = (dgAccess.SelectedRows(0).DataBoundItem)
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                iAccess = xAccessAdapter.SelectRow(iAccess.AccessID, sqlConn)
                sqlConn.Close()
            End Using

            OpenAccess(iAccess, False)
        End If
    End Sub

    Private Sub cmdAccessDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccessDelete.Click
        If dgAccess.SelectedRows.Count = 1 Then
            Try
                If dgAccess.SelectedRows.Count = 1 Then
                    Dim iAccess As xAccess = dgAccess.SelectedRows(0).DataBoundItem
                    iAccess.Deleted = True
                    '_AccessList.Remove(iAccess)
                    'Dim rnd As New Random
                    'iAccess.AccessPassword = rnd.Next(1, 9999).ToString.PadLeft(4, "0")
                    Using sqlConn As New SqlConnection(Conn)
                        sqlConn.Open()
                        Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                        Try
                            xAccessAdapter.Delete(iAccess, sqlConn, sqlTrans)
                            sqlTrans.Commit()
                        Catch ex As Exception
                            sqlTrans.Rollback()
                            Throw ex
                        End Try

                        BindAccess(sqlConn)
                        sqlConn.Close()
                    End Using
                    MsgBox("Access code: " & iAccess.AccessCode, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Deleted")
                End If
            Catch ex As Exception
                ShowEx(ex)
            End Try
        End If
    End Sub



#End Region

#Region " Payment "
    Private Sub BindPayment(ByVal sqlConn As SqlConnection)
        Dim bs As New BindingSource
        bs.DataSource = xPaymentAdapter.ListRecent(_Account.AccountID, sqlConn)
        dgPayment.DataSource = bs
        dgPayment.Columns("Amount").DefaultCellStyle.Format = "#,##0.00"
        dgPayment.Columns("Amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgPayment.Columns("Amount").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Private Sub cmdPaymentAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPaymentAdd.Click
        Try
            Dim f As New frmPayment(New xPayment, _Account)
            If f.ShowDialog = DialogResult.OK Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    BindPayment(sqlConn)
                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
#End Region

#Region " Recharge "
    Private Sub cmdRechargeFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRechargeFind.Click
        FindRecharge()
    End Sub
    Private Sub txtRechargeMobile_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRechargeMobile.KeyUp
        If e.KeyCode = Keys.Enter Then
            FindRecharge()
        End If
    End Sub
    Private Sub FindRecharge()
        Try
            Label12.Visible = True
            Label12.Update()
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource
                xBrandAdapter.List(sqlConn)

                bs.DataSource = xAccountAdapter.RechargeFind(_Account.AccountID, txtRechargeMobile.Text, sqlConn)
                dgRecharge.DataSource = bs

                dgRecharge.Columns("Amount").DefaultCellStyle.Format = "#,##0.00"
                dgRecharge.Columns("Amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgRecharge.Columns("Amount").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

                dgRecharge.Columns("Discount").DefaultCellStyle.Format = "#,##0.00"
                dgRecharge.Columns("Discount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgRecharge.Columns("Discount").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                sqlConn.Close()
            End Using
            Label12.Visible = False
            Label12.Update()
        Catch ex As Exception
            ShowEx(ex)
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

                        Dim iWebservice As New HOTwebservice.ServiceSoapClient
                        Dim iRet As HOTwebservice.ReturnObject
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
            Label12.Visible = True
            Label12.Update()
            If dgRecharge.SelectedRows.Count = 1 Then
                Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iAccess As New xAccess
                    iAccess = xAccessAdapter.SelectRow(iRecharge.AccessID, sqlConn)
                    Dim iWebservice As New HotWebService.ServiceSoapClient
                    Dim iRet As HotWebService.ReturnObject
                    ' Method needs Later by to allow console to query as console user
                    iAccess.AccessCode = "0772929223"
                    iAccess.AccessPassword = "1211"

                    iRet = iWebservice.HOTPhoneBalance(iAccess.AccessCode, iAccess.AccessPassword, iRecharge.Mobile)
                    MsgBox("Balance of " & iRecharge.Mobile & " is $" & iRet.ReturnValue & vbNewLine & iRet.ReturnMsg, , "Phone Balance")
                    sqlConn.Close()
                End Using
            End If
            Label12.Visible = False
            Label12.Update()
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

    Private Sub dgRecharge_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgRecharge.CellContentClick
        'Looks for the detail for the specific recharge and displays it in the second data grid
        Try
            Label12.Visible = True
            Label12.Update()
            If dgRecharge.SelectedRows.Count = 1 Then
                Dim iRecharge As xRecharge = dgRecharge.SelectedRows(0).DataBoundItem
                dgRechargeDetail.Rows.Clear()
                Select Case iRecharge.Brand.BrandID
                    Case xBrand.Brands.EconetPlatform
                        Using sqlConn As New SqlConnection(Conn)
                            sqlConn.Open()
                            Dim bs2 As New BindingSource
                            bs2.DataSource = xRechargeAdapter.SelectRechargePrepaid(iRecharge.RechargeID, sqlConn)
                            dgRechargeDetail.DataSource = bs2
                            sqlConn.Close()
                        End Using
                    Case xBrand.Brands.EasyCall
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
                            bs2.DataSource = xRechargeAdapter.SelectRechargePin(iRecharge.RechargeID, sqlConn)
                            dgRechargeDetail.DataSource = bs2
                            sqlConn.Close()
                        End Using
                End Select
            End If
            Label12.Visible = False
            Label12.Update()
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
#End Region

#Region " SMS "
    Private Sub cmdSMSRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSMSRefresh.Click
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource
                bs.DataSource = xSMSAdapter.SMSListIn(_Account.AccountID, dtSMSDate.Value.Date, sqlConn)

                dgSMSIn.DataSource = bs
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub dgSMSIn_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSMSIn.CellEnter
        Try
            If dgSMSIn.SelectedRows.Count = 1 Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iSMS As xSMS = dgSMSIn.SelectedRows(0).DataBoundItem
                    Dim bs As New BindingSource
                    bs.DataSource = xSMSAdapter.SMSListOut(iSMS.SMSID, sqlConn)
                    dgSMSOut.DataSource = bs
                    sqlConn.Close()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    

    
    'Private Sub dgSMSIn_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSMSIn.CellDoubleClick
    '    Try
    '        If dgSMSIn.SelectedRows.Count = 1 Then
    '            Using sqlConn As New SqlConnection(Conn)
    '                sqlConn.Open()
    '                Dim iSMS As xSMS = dgSMSIn.SelectedRows(0).DataBoundItem
    '                sqlConn.Close()
    '                Dim f As New frmSMS(iSMS)
    '                f.ShowDialog()
    '            End Using
    '        End If
    '    Catch ex As Exception
    '        ShowEx(ex)
    '    End Try
    'End Sub

    Private Sub dgSMSOut_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSMSOut.CellDoubleClick
        Try
            If dgSMSOut.SelectedRows.Count = 1 Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    Dim iSMS As xSMS = dgSMSOut.SelectedRows(0).DataBoundItem
                    sqlConn.Close()
                    Dim f As New frmSMS(iSMS)
                    f.ShowDialog()
                End Using
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub


    'Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
    '    Open(New xSMS)
    'End Sub
    'Private Sub Open(ByVal iSMS As xSMS)
    '    Try
    '        Dim f As New frmSMS(iSMS)
    '        f.StartPosition = FormStartPosition.CenterScreen
    '        If dgSMSOut.SelectedRows.Count = 0 Then
    '            f.txtMobile.Text = dgSMSIn.SelectedRows(0).DataBoundItem(5).ToString
    '        Else
    '            f.txtMobile.Text = dgSMSIn.SelectedRows(0).DataBoundItem(5).ToString
    '        End If

    '        f.ShowDialog()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Exclamation)
    '    End Try
    'End Sub
#End Region

#Region " Invoicing "

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click
        Dim iAddress As New xAddress
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                iAddress = xAddressAdapter.SelectRow(_Account.AccountID, sqlConn)
                sqlConn.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "Error")
        End Try
        txtAddress1.Text = iAddress.Address1
        txtAddress2.Text = iAddress.Address2
        txtCity.Text = iAddress.City
        txtContact.Text = iAddress.ContactName
        txtContactNumber.Text = iAddress.ContactNumber
        txtLatitude.Text = iAddress.Latitude
        txtLongitude.Text = iAddress.Longitude
        txtVAT.Text = iAddress.VatNumber
        txtSageID.Text = iAddress.SageID
        If iAddress.InvoiceFreq = 0 Then
            chkInvoice.CheckState = CheckState.Unchecked
        Else
            chkInvoice.CheckState = CheckState.Checked
        End If
    End Sub
    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        
        Try
            Dim iAddress As New xAddress
            iAddress.AccountID = _Account.AccountID
            iAddress.Address1 = txtAddress1.Text
            iAddress.Address2 = txtAddress2.Text
            iAddress.City = txtCity.Text
            iAddress.ContactName = txtContact.Text
            iAddress.ContactNumber = txtContactNumber.Text
            iAddress.Latitude = txtLatitude.Text
            iAddress.Longitude = txtLongitude.Text
            iAddress.VatNumber = txtVAT.Text
            If chkInvoice.CheckState = CheckState.Checked Then
                iAddress.InvoiceFreq = 1
            Else
                iAddress.InvoiceFreq = 0
            End If
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                xAddressAdapter.Save(iAddress, sqlConn)
                sqlConn.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString, , "Error")
        End Try
    End Sub







#End Region

End Class