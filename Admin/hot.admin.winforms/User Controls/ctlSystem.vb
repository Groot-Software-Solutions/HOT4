Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Hot.Data
Public Class ctlSystem
#Region " Properties "

    Public Property ErrorLoggerOK As Boolean = False
    Public Property BalanceCheckerOK As Boolean = False
    Public Property RechargeErrorsOK As Boolean = False

#End Region


#Region " Init "
    Public Sub Init()
        lblWorking.Visible = False
        'SystemErrorStatus()
        ChkBxNet1.Checked = True
        Timer1.Start()
    End Sub
#End Region
    Private Sub ChkBxNet1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkBxNet1.CheckedChanged
        If ChkBxNet1.Checked Then
            ChkBxNet1.Refresh()
            Timer1.Interval = txtTimerTick.Text * 60000
            Timer1.Start()
            Timer1_Tick(Nothing, Nothing)
        Else
            Timer1.Stop()
            dgError.Rows.Clear()
            dgError.Refresh()
            HOT4.Console.frmConsole.cmdSystem.BackColor = Color.LightSeaGreen
            Me.BackColor = Color.LightGray
        End If
    End Sub
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        'Dim x As Integer
        'x = x + 1

        'If x = 10 Then
        '    x = 0

        'End If
        'Set the colours of the items alerting the users 
        SystemErrorStatus()
        If BalanceCheckerOK And ErrorLoggerOK Then
            Me.BackColor = DefaultBackColor
            frmConsole.cmdSystem.BackColor = Color.LightGreen

        End If
    End Sub
    Private Sub SystemErrorStatus()
        lblWorking.Visible = True : lblWorking.Refresh()
        Dim bs As New BindingSource
        dgError.AutoGenerateColumns = True
        dgError.DataSource = bs
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                bs.DataSource = xErrorLog.List(sqlConn)
                dgError.Columns.Item(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgError.DataSource = bs
                dgError.AutoResizeColumns()
                'Global colour based on errors


                If dgError.Rows.Item(0).Cells(2).Value = "ALL" Then
                    dgError.Rows.Item(0).DefaultCellStyle.BackColor = Color.LightGreen
                    HOT4.Console.frmConsole.cmdSystem.BackColor = Color.LightGreen
                    Me.BackColor = Color.MintCream
                Else
                    HOT4.Console.frmConsole.cmdSystem.BackColor = Color.Yellow
                    Me.BackColor = Color.LightGoldenrodYellow

                    'Format the datagrid based on each line's error value
                    'dgError.Columns.Item(4).DefaultCellStyle.Format = "####0.##"
                    dgError.BackgroundColor = Color.White
                    For x = 0 To dgError.RowCount - 1
                        dgError.Rows(x).Cells(4).ValueType = GetType(Decimal)
                        dgError.Rows.Item(x).Cells(4).Value = dgError.Rows.Item(x).Cells(4).Value / 60
                        Select Case dgError.Rows.Item(x).Cells(4).Value
                            Case Is > 60
                                dgError.Rows.Item(x).DefaultCellStyle.BackColor = Color.Red
                                HOT4.Console.frmConsole.cmdSystem.BackColor = Color.Red
                            Case Is > 30
                                dgError.Rows.Item(x).DefaultCellStyle.BackColor = Color.LightCoral
                            Case Is > 5
                                dgError.Rows.Item(x).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                            Case Is > 1
                                dgError.Rows.Item(x).DefaultCellStyle.BackColor = Color.White
                        End Select
                    Next

                End If
                dgError.Refresh()
                sqlConn.Close()
                lblWorking.Visible = False
            End Using
        Catch ex As Exception
            HOT4.Console.frmConsole.cmdSystem.BackColor = Color.Red
            Me.BackColor = Color.Red
            lblWorking.Visible = False
            MsgBox("Error reading the Error Log on HOT5 server. Indicates that the Error log service is off or the HOT4 Database is not accessible", MsgBoxStyle.Exclamation, "Error in the ErrorLog")
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click

        txtNetOne.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Busy"
        txtNetOne.Refresh()
        BalanceCheck()
    End Sub
    Private Sub BalanceCheck()
        EconetBundlesBalanceCheck()
        EconetBundlesUSDBalanceCheck()
        NetoneBalanceCheck()
        NetoneUSDBalanceCheck()
        TelecelBalanceCheck()
        TelecelUSDBalanceCheck()
        TeloneBalanceCheck()
        TeloneUSDBalanceCheck()
        ZesaBalanceCheck()
        ZesaUSDBalanceCheck()
        AfricomBalanceCheck()

        BalanceCheckerOK = True

    End Sub

    Private Sub AfricomBalanceCheck()
        'Try 'Get Africom Wallet Balance

        '    Dim z As New Africom.ServiceSoapClient
        '    Dim iEncrypt As String = z.AfriEncrypt("8644003060|cD&7Q3|8644003060-" & Now.ToString & "|8644003060", "629V25eJ")

        '    Dim iAfriRet As Africom.Balance = z.AfriBalanceEnquiry("8644003060|" & iEncrypt)

        '    If iAfriRet.Header.Code = "000" And iAfriRet.AccountTypes.Count > 0 Then
        '        txtAfricom.Text = Nothing
        '        For y = 0 To iAfriRet.AccountTypes.Count - 1
        '            txtAfricom.Text = txtAfricom.Text & _
        '                            "Balance:" & iAfriRet.AccountTypes.Item(y).Balance & _
        '                            " ProductName:" & iAfriRet.AccountTypes.Item(y).ProductName & _
        '                            " Description:" & iAfriRet.AccountTypes.Item(y).Description & _
        '                            " Item String:" & iAfriRet.AccountTypes.Item(y).ToString & vbCrLf
        '        Next
        '        txtAfricom.Text = txtAfricom.Text & _
        '                            "Balance Header Code:" & iAfriRet.Header.Code & _
        '                            " Description:" & iAfriRet.Header.Description & _
        '                            " Ref:" & iAfriRet.Header.DealerReferenceNo & _
        '                            " Count of Balances:" & iAfriRet.AccountTypes.Count.ToString
        '        txtAfricom.BackColor = Color.LightGreen
        '    Else
        '        txtAfricom.Text = "ERROR Balance Header " & _
        '                        " Code" & iAfriRet.Header.Code & _
        '                        " Error Description:" & iAfriRet.Header.Description
        '        txtAfricom.BackColor = Color.LightCoral
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        '    txtAfricom.Text = "Exception: Africom web service failed"
        '    txtAfricom.BackColor = Color.Coral
        '    txtAfricom.Refresh()
        '    ChkBxNet1.BackColor = Color.Aqua
        'End Try
    End Sub

    Private Sub EconetBundlesBalanceCheck()

        Dim bundles As New EconetBundles.EconetBundleSoapClient
        Dim iRet3 As New EconetBundles.AccountBalanceResponse


        Try 'Get Econet Wallet Balance
            iRet3 = bundles.GetBalance("Hot263180", "0")
            If Not iRet3.AccountBalances.Count() = 0 Then
                txtEconet.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Econet Bundle $" & FormatNumber((iRet3.AccountBalances(0).Amount / 100), 2)
                txtEconet.BackColor = Color.LightGreen
            Else
                txtEconet.Text = " Msg:" & iRet3.RawResponseData
                txtEconet.BackColor = Color.LightCoral
            End If
            txtEconet.Refresh()
        Catch ex As Exception
            txtEconet.Text = "Exception: Econet Bunlde Webservice failed"
            txtEconet.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub
    Private Sub EconetBundlesUSDBalanceCheck()

        Dim bundles As New EconetBundles.EconetBundleSoapClient
        Dim iRet3 As New EconetBundles.AccountBalanceResponse


        Try 'Get Econet Wallet Balance
            iRet3 = bundles.GetBalance("Hot263180", "1")
            If Not iRet3.AccountBalances.Count() = 0 Then
                txtEconetUsd.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Econet Bundle USD $" & FormatNumber((iRet3.AccountBalances(0).Amount / 100), 2)
                txtEconetUsd.BackColor = Color.LightGreen
            Else
                txtEconetUsd.Text = " Msg:" & iRet3.RawResponseData
                txtEconetUsd.BackColor = Color.LightCoral
            End If
            txtEconetUsd.Refresh()
        Catch ex As Exception
            txtEconetUsd.Text = "Exception: Econet Bunlde Webservice failed"
            txtEconetUsd.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub
    Private Sub TelecelBalanceCheck()
        Dim iRet2 As New TelecelEJuice.BalanceResponse
        Dim x As New TelecelEJuice.TelecelSoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.JuiceBalance("733357030", "5394C86F977989489AFC65FEFC525CE1")
            If iRet2.resultcode = 0 Then
                txtTelecel.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Telecel $" & CDec(iRet2.amount).ToString("#,##0.00")
                txtTelecel.BackColor = Color.LightGreen
            Else
                txtTelecel.Text = "Telecel Balance $" & iRet2.amount & " Code:" & iRet2.resultcode & " Msg:" & iRet2.resultdescription
                txtTelecel.BackColor = Color.LightCoral
            End If
            txtTelecel.Refresh()
        Catch ex As Exception
            txtTelecel.Text = "Exception: Telecel EJuice Webservice failed"
            txtTelecel.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub

    Private Sub NetoneBalanceCheck()

        Dim iWebService As New NetoneDongle.NetoneSoapClient
        Dim iRet As New NetoneDongle.WalletBalanceResponse

        txtNetOne.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Busy"
        txtNetOne.Refresh()
        Try 'Get NetOne Wallet Balance
            iRet = iWebService.GetBalance("Hot263180")
            If iRet.ReplyCode = 2 Then
                txtNetOne.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " NetOne $ " & iRet.WalletBalance.ToString("#,##0.00")
                txtNetOne.BackColor = Color.LightGreen
            Else
                txtNetOne.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) _
                    & " NetOne Balance $" & iRet.WalletBalance & " Code:" & iRet.ReplyCode & " Msg" & iRet.ReplyMsg
                txtNetOne.BackColor = Color.LightCoral
            End If
            txtNetOne.Refresh()
        Catch ex As Exception
            txtNetOne.Text = "Exception: Dongle web service failed"
            txtNetOne.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try

    End Sub
    Private Sub NetoneUSDBalanceCheck()

        Dim iWebService As New NetoneDongle.NetoneSoapClient
        Dim iRet As New NetoneDongle.WalletBalanceResponse

        Try
            iRet = iWebService.GetUSDBalance("Hot263180")
            If iRet.ReplyCode = 2 Then
                txtNetoneUSD.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " NetOne USD $ " & iRet.WalletBalance.ToString("#,##0.00")
                txtNetoneUSD.BackColor = Color.LightGreen
            Else
                txtNetoneUSD.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) _
                    & " NetOne USD Balance $" & iRet.WalletBalance & " Code:" & iRet.ReplyCode & " Msg" & iRet.ReplyMsg
                txtNetoneUSD.BackColor = Color.LightCoral
            End If
            txtNetoneUSD.Refresh()
        Catch ex As Exception
            txtNetoneUSD.Text = "Exception: Dongle web service failed"
            txtNetoneUSD.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try

    End Sub

    Private Sub ZesaBalanceCheck()
        Dim iRet2 As New ZesaService.VendorBalanceResponse
        Dim x As New ZesaService.ZESASoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.GetBalance("Hot263180", "ZiG")
            If iRet2.ReplyCode = 2 Then
                txtZesa.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " ZESA $ " & CDec(iRet2.VendorBalance.Balance).ToString("#,##0.00")
                txtZesa.BackColor = Color.LightGreen
            Else
                txtZesa.Text = "ZESA Balance $ Unknown Code:" & iRet2.ReplyCode & " Msg:" & iRet2.ReplyMessage
                txtZesa.BackColor = Color.LightCoral
            End If
            txtZesa.Refresh()
        Catch ex As Exception
            txtZesa.Text = "Exception: ZESA Webservice failed"
            txtZesa.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub
    Private Sub ZesaUSDBalanceCheck()
        Dim iRet2 As New ZesaService.VendorBalanceResponse
        Dim x As New ZesaService.ZESASoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.GetBalance("Hot263180", "USD")
            If iRet2.ReplyCode = 2 Then
                txtZESAUSD.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " ZESA USD $ " & CDec(iRet2.VendorBalance.Balance).ToString("#,##0.00")
                txtZESAUSD.BackColor = Color.LightGreen
            Else
                txtZESAUSD.Text = "ZESA Balance $ Unknown Code:" & iRet2.ReplyCode & " Msg:" & iRet2.ReplyMessage
                txtZESAUSD.BackColor = Color.LightCoral
            End If
            txtZesa.Refresh()
        Catch ex As Exception
            txtZESAUSD.Text = "Exception: ZESA Webservice failed"
            txtZESAUSD.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub


    Private Sub TeloneBalanceCheck()
        Dim iRet2 As New TeloneService.AccountBalanceResponse
        Dim x As New TeloneService.TeloneSoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.GetBalance("Hot263180")
            If iRet2.ReplyCode = 2 Then
                txtTelone.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Telone $ " & iRet2.Balance.ToString("#,##0.00")
                txtTelone.BackColor = Color.LightGreen
            Else
                txtTelone.Text = "Telone Balance $ Unknown Code:" & iRet2.ReplyCode & " Msg:" & iRet2.ReplyMessage
                txtTelone.BackColor = Color.LightCoral
            End If
            txtTelone.Refresh()
        Catch ex As Exception
            txtTelone.Text = "Exception: Telone Webservice failed"
            txtTelone.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub
    Private Sub TeloneUSDBalanceCheck()
        Dim iRet2 As New TeloneService.AccountBalanceResponse
        Dim x As New TeloneService.TeloneSoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.GetBalanceUSD("Hot263180")
            If iRet2.ReplyCode = 2 Then
                txtTeloneUSD.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Telone USD $ " & iRet2.Balance.ToString("#,##0.00")
                txtTeloneUSD.BackColor = Color.LightGreen
            Else
                txtTeloneUSD.Text = "Telone Balance $ Unknown Code:" & iRet2.ReplyCode & " Msg:" & iRet2.ReplyMessage
                txtTeloneUSD.BackColor = Color.LightCoral
            End If
            txtTeloneUSD.Refresh()
        Catch ex As Exception
            txtTeloneUSD.Text = "Exception: Telone Webservice failed"
            txtTeloneUSD.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub
    Private Sub TelecelUSDBalanceCheck()
        Dim iRet2 As New TelecelEJuice.BalanceResponse
        Dim x As New TelecelEJuice.TelecelSoapClient
        ChkBxNet1.BackColor = Color.Cornsilk

        Try 'Get Telecel Wallet Balance
            iRet2 = x.JuiceBalanceUSD("733357030", "5394C86F977989489AFC65FEFC525CE1")

            If iRet2.resultcode = 0 Then
                txtTelecelUSDBalance.Text = Date.Now.TimeOfDay.ToString.Substring(0, 5) & " Telecel USD $" & CDec(iRet2.amount).ToString("#,##0.00")
                txtTelecelUSDBalance.BackColor = Color.LightGreen
            Else
                txtTelecelUSDBalance.Text = "Telecel Balance $" & iRet2.amount & " Code:" & iRet2.resultcode & " Msg:" & iRet2.resultdescription
                txtTelecelUSDBalance.BackColor = Color.LightCoral
            End If
            txtTelecelUSDBalance.Refresh()
        Catch ex As Exception
            txtTelecelUSDBalance.Text = "Exception: Telecel EJuice Webservice failed"
            txtTelecelUSDBalance.Refresh()
            ChkBxNet1.BackColor = Color.Aqua
        End Try
    End Sub



End Class

Public Class xErrorLog
    'Public LogDate As DateTime = Date.Now
    'Public Server As String
    'Public TestName As String
    'Public ErrorData As String = 0
    'Public ErrorDuration As Integer = 0
    'Public TestTypeName As String



    Private _LogDate As DateTime
    Public Property LogDate() As DateTime
        Get
            Return _LogDate
        End Get
        Set(ByVal value As DateTime)
            _LogDate = value
        End Set
    End Property

    Private _Server As String
    Public Property Server() As String
        Get
            Return _Server
        End Get
        Set(ByVal value As String)
            _Server = value
        End Set
    End Property

    Private _TestName As String
    Public Property TestName() As String
        Get
            Return _TestName
        End Get
        Set(ByVal value As String)
            _TestName = value
        End Set
    End Property

    Private _ErrorData As String
    Public Property ErrorData() As String
        Get
            Return _ErrorData
        End Get
        Set(ByVal value As String)
            _ErrorData = value
        End Set
    End Property

    Private _ErrorDuration As Integer
    Public Property ErrorDuration() As Integer
        Get
            Return _ErrorDuration
        End Get
        Set(ByVal value As Integer)
            _ErrorDuration = value
        End Set
    End Property

    Private _TestTypeName As String
    Public Property TestTypeName() As String
        Get
            Return _TestTypeName
        End Get
        Set(ByVal value As String)
            _TestTypeName = value
        End Set
    End Property

    Sub New(sqlRdr As SqlDataReader)
        LogDate = sqlRdr("LogDate")
        Server = sqlRdr("Server")
        TestName = sqlRdr("Name")
        ErrorData = sqlRdr("ErrorData")
        ErrorDuration = sqlRdr("ErrorDuration")
        TestTypeName = sqlRdr("TestTypeName")
    End Sub


    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xErrorLog)
        Dim iList As New List(Of xErrorLog)
        Using sqlCmd As New SqlCommand("xErrorLog_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xErrorLog(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

End Class


