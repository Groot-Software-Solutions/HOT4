Imports System.Data.SqlClient
Imports Hot.Data
Imports Microsoft.Office.Interop
Imports System.Text.RegularExpressions
Imports System.IO
Imports ExcelDataReader
Imports System.Globalization
Imports System.Collections.ObjectModel

Public Class frmBankLoader

#Region " Members "
    Private _BankTransactionList As List(Of BankTransaction)
    Private _BankTransactionUpdateList As List(Of xBankTrx)
    Dim MerchantNumber As String = "771998574"
    Dim APIMerchantNumber As String = "783347527"
    Dim ZESAMerchantNumber As String = "783347530"

#End Region

    Private Sub cmdBrowse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBrowse.Click
        _BankTransactionList = New List(Of BankTransaction)
        BindTransactions(_BankTransactionList)
        Dim f As New OpenFileDialog
        If f.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtFileName.Text = f.FileName
        End If
    End Sub

    Sub New(ByVal BankID As xBank.Banks)
        InitializeComponent()
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            cboBank.DisplayMember = "Bank"
            cboBank.ValueMember = "BankID"
            cboBank.DataSource = xBankAdapter.List(sqlConn)
            cboBank.SelectedValue = CInt(BankID)
            sqlConn.Close()
        End Using
        _BankTransactionList = New List(Of BankTransaction)
    End Sub

    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
        If cboBank.SelectedIndex = -1 Then
            Err.SetError(cboBank, "Bank must be selected")
            cboBank.Focus()
            Exit Sub
        End If
        If Not File.Exists(txtFileName.Text) Then
            Err.SetError(txtFileName, "File does not exist")
            txtFileName.Focus()
            Exit Sub
        End If
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    Select Case CType(cboBank.SelectedValue, xBank.Banks)
                        Case xBank.Banks.CABS
                            LoadCABS(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.CABSUSD
                            LoadCABS(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.Stanbic
                            LoadStanbic(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.AgriBank
                            LoadAgriBank(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.EcoMerchant
                            LoadEcocash(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.StanbicZesa
                            LoadStanbic(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.StanbicUSD
                            LoadStanbic(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.StewardBank
                            LoadStewardBank(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.StewardBankUSD
                            LoadStewardBankUSD(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.CBZ
                            LoadCBZBank(txtFileName.Text, sqlConn, sqlTrans)
                        Case xBank.Banks.CBZUSD
                            LoadCBZBank(txtFileName.Text, sqlConn, sqlTrans)
                    End Select
                    sqlTrans.Commit()
                    'MsgBox("Success", MsgBoxStyle.Information)
                    'Me.DialogResult = Windows.Forms.DialogResult.OK
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            'Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub

    Private Sub LoadCBZBank(FilePath As String, sqlConn As SqlConnection, sqlTrans As SqlTransaction)
        Dim iTransactionList As New List(Of BankTransaction)
        Try
            If (FilePath.EndsWith(".csv")) Then iTransactionList = GetTransactionsCBZCSV(FilePath)
            If (FilePath.EndsWith(".xls") Or FilePath.EndsWith(".xlsx")) Then iTransactionList = GetTransactionsCBZExcel(FilePath)

            _BankTransactionList = iTransactionList

            BindTransactions(_BankTransactionList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function GetTransactionsCBZCSV(filePath As String) As List(Of BankTransaction)
        Dim skipingheader As Boolean = True
        Dim iTransactionList As New List(Of BankTransaction)
        Using MyReader As New FileIO.TextFieldParser(filePath)
#Region "CBZ Processing Loop"
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim Regex As New Regex("[0][7]\d\d\d\d\d\d\d\d")
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    If Not IsDate(currentRow(0)) Then GoTo skip
                    Dim iTransaction As New BankTransaction
                    If IsDate(currentRow(0)) Then iTransaction.TransactionDate = CDate(currentRow(0))
                    If currentRow(3) = "USD" Then
                        If Not String.IsNullOrEmpty(currentRow(1)) Then iTransaction.TransactionDescription = currentRow(1)

                        currentRow(4) = GetAmount(currentRow(4))
                        currentRow(7) = GetAmount(currentRow(7))
                        iTransaction.Amount = CDec(currentRow(4))
                        If currentRow(5).ToUpper() = "DR" Then iTransaction.Amount = -CDec(currentRow(4))

                        If IsNumeric(currentRow(7)) Then iTransaction.Balance = CDec(currentRow(7))
                        iTransaction.Branch = currentRow(2)
                        iTransaction.BankReference = currentRow(2)
                    Else
                        If Not String.IsNullOrEmpty(currentRow(2)) Then iTransaction.TransactionDescription = currentRow(2)
                        currentRow(6) = GetAmount(currentRow(6))
                        currentRow(8) = GetAmount(currentRow(8))
                        currentRow(10) = GetAmount(currentRow(10))
                        If IsNumeric(currentRow(6)) Then
                            If Not CDec(currentRow(6)) = 0 Then iTransaction.Amount = -CDec(currentRow(6))
                        End If
                        If IsNumeric(currentRow(8)) Then
                            If Not CDec(currentRow(8)) = 0 Then iTransaction.Amount = CDec(currentRow(8))
                        End If
                        If IsNumeric(currentRow(10)) Then iTransaction.Balance = CDec(currentRow(10))
                    End If
                    iTransaction.Identifier = ""
                    Dim match = Regex.Match(iTransaction.TransactionDescription)
                    If match.Success Then iTransaction.Identifier = match.Value
                    If iTransaction.Amount < 0 Then iTransaction.TransactionType = "WITHDRAWAL"
                    If iTransaction.Amount > 0 Then iTransaction.TransactionType = "DEPOSIT"
                    If iTransaction.TransactionDescription.Contains("BILL ACCOUNT") And iTransaction.Amount > 0 Then iTransaction.TransactionType = "PAYMENT"

                    iTransactionList.Add(iTransaction)

skip:

                Catch ex As FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
#End Region
        End Using
        Return iTransactionList
    End Function

    Function GetTransactionsCBZExcel(FilePath As String) As List(Of BankTransaction)
        Dim FileFormat As Integer = 2
        Dim iTransactionList As New List(Of BankTransaction)
        Using stream = File.Open(FilePath, FileMode.Open, FileAccess.Read)
            Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)
                Try

                    Dim result As DataSet = reader.AsDataSet()
                    Dim iSheet As DataTable = result.Tables(0)
                    pb.Maximum = iSheet.Rows.Count : pb.Step = 1
                    Dim StatmentMerchantNumber As String = ""
                    For Each row As DataRow In iSheet.Rows
                        pb.PerformStep()
                        'Check if Transaction                                
                        If IsDate(row.Item(1)) Then
                            If FileFormat = 1 Then
                                Dim iTransaction As New BankTransaction With {
                                .Amount = CDec(row.Item(7).ToString()),
                                .Balance = If(IsNumeric(row.Item(5).ToString()), CDec(row.Item(5).ToString()), 0),
                                .TransactionDescription = row.Item(2).ToString().ToUpper + " : " + row.Item(3).ToString(),
                                .Identifier = row.Item(8).ToString(),
                                .TransactionType = row.Item(2).ToString().ToUpper,
                                .Branch = row.Item(4).ToString(),
                                .TransactionDate = CDate(row.Item(1).ToString()),
                                .BankReference = row.Item(6).ToString()
                            }
                                iTransactionList.Add(iTransaction)
                            ElseIf FileFormat = 2 Then
                                Dim iTransaction As New BankTransaction With {
                                .Amount = CDec(row.Item(3).ToString()),
                                .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                                .TransactionDescription = row.Item(2).ToString().ToUpper + " : " + row.Item(5).ToString(),
                                .Identifier = row.Item(4).ToString(),
                                .TransactionType = row.Item(2).ToString().ToUpper,
                                .Branch = row.Item(6).ToString(),
                                .TransactionDate = CDate(row.Item(1).ToString()),
                                .BankReference = row.Item(8).ToString()
                            }
                                iTransactionList.Add(iTransaction)
                            Else

                                Dim iTransaction As New BankTransaction With {
                                .Amount = CDec(row.Item(3).ToString()),
                                .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                                .TransactionDescription = row.Item(2).ToString().ToUpper + " : " + row.Item(5).ToString(),
                                .Identifier = row.Item(4).ToString(),
                                .TransactionType = row.Item(2).ToString().ToUpper,
                                .Branch = row.Item(6).ToString(),
                                .TransactionDate = CDate(row.Item(1).ToString()),
                                .BankReference = row.Item(8).ToString()
                            }
                                iTransactionList.Add(iTransaction)
                            End If

                        End If
                        If IsDate(row.Item(0)) Then
                            Dim iTransaction As New BankTransaction With {
                                .Amount = CDec(row.Item(6).ToString()),
                                .Balance = If(IsNumeric(row.Item(4).ToString()), CDec(row.Item(4).ToString()), 0),
                                .TransactionDescription = row.Item(1).ToString().ToUpper + " : " + row.Item(2).ToString(),
                                .Identifier = row.Item(7).ToString(),
                                .TransactionType = row.Item(1).ToString().ToUpper,
                                .Branch = row.Item(3).ToString(),
                                .TransactionDate = CDate(row.Item(0).ToString()),
                                .BankReference = row.Item(5).ToString()
                            }
                            iTransactionList.Add(iTransaction)
                        End If
                    Next
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

        End Using
        Return iTransactionList
    End Function

    Private Sub LoadStewardBank(FileName As String, sqlConn As SqlConnection, sqlTrans As SqlTransaction)
        Dim iTransactionList As New List(Of BankTransaction)
        If FileName.Substring(FileName.LastIndexOf(".")) = ".xlsx" Then
            iTransactionList = GetTransactionsStewardBankExcel(FileName)

        End If

        _BankTransactionList = iTransactionList

        BindTransactions(_BankTransactionList)

    End Sub
    Private Sub LoadStewardBankUSD(FileName As String, sqlConn As SqlConnection, sqlTrans As SqlTransaction)
        Dim iTransactionList As New List(Of BankTransaction)
        If FileName.Substring(FileName.LastIndexOf(".")) = ".xlsx" Then
            iTransactionList = GetTransactionsStewardBankExcel(FileName)

        End If

        _BankTransactionList = iTransactionList

        BindTransactions(_BankTransactionList)

    End Sub

    Private Function GetTransactionsStewardBankExcel(FilePath As String) As List(Of BankTransaction)
        Dim iTransactionList As New List(Of BankTransaction)
        Using stream = File.Open(FilePath, FileMode.Open, FileAccess.Read)
            Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)
                Try
                    Dim result As DataSet = reader.AsDataSet()
                    Dim iSheet As DataTable = result.Tables(0)
                    pb.Maximum = iSheet.Rows.Count : pb.Step = 1

                    For Each row As DataRow In iSheet.Rows
                        pb.PerformStep()
                        'Check if Transaction
                        Dim dateValue As Date
                        If Date.TryParseExact(row.Item(0).ToString(), "dd MMM yy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dateValue) Then

                            Dim iTransaction As New BankTransaction With {
                                .Amount = If(CDec(row.Item(7).ToString()) > 0, CDec(row.Item(7).ToString()), -CDec(row.Item(6).ToString())),
                                .Balance = If(IsNumeric(row.Item(8).ToString()), CDec(row.Item(8).ToString()), 0),
                                .TransactionDescription = $"{row.Item(2)} {row.Item(4)}",
                                .TransactionType = row.Item(3).ToString().ToUpper,
                                .Branch = "N/A",
                                .TransactionDate = dateValue,
                                .BankReference = row.Item(2)
                            }
                            iTransactionList.Add(iTransaction)
                        End If

                    Next
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

        End Using
        Return iTransactionList
    End Function

    'Used for stripping html from CABS bank statement
    Private Function GetRows(ByVal FileName As String) As List(Of String)
        Dim MyRows As New List(Of String)
        Dim sFile As New IO.StreamReader(FileName)
        Dim body As String = sFile.ReadToEnd
        body = body.Substring(body.IndexOf("<hr>")).Replace("<hr>", "")
        body = body.Replace("<table valign=""middle"" border=""0"" width=""100%"">", "|")
        Dim rows() As String = body.Split("|")
        For x As Integer = 0 To rows.GetUpperBound(0) - 1
            Dim sRow As String = rows(x)
            sRow = sRow.Replace("<font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"" color=""black"" >", "")
            sRow = sRow.Replace("</font>", "")
            sRow = sRow.Replace("<tr>", "")
            sRow = sRow.Replace("</tr>", "")
            sRow = sRow.Replace("valign=""TOP"" ", "")
            sRow = sRow.Replace("</td>", "")
            sRow = sRow.Replace("<td", "|")
            sRow = sRow.Replace("<br>", "|width=""16%"">")
            sRow = sRow.Replace(Chr(10), "")
            sRow = sRow.Replace(Chr(13), "")
            'sRow = sRow.Replace(Chr(32), "")
            sRow = sRow.Replace("align=""right""", "")
            sRow = sRow.Replace("</table>", "|")
            sRow = sRow.Replace("<font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"" color=""red"" >", "")
            'sRow = sRow.Replace(">", "")
            Dim cells() As String = sRow.Split("|")
            Dim MyRow As String = ""
            For y As Integer = 0 To cells.GetUpperBound(0) - 1
                Dim i As Integer = cells(y).IndexOf("width")
                If cells(y).Length > 13 Then
                    Dim InsertText As String = cells(y).Substring(i + 12)
                    MyRow &= InsertText.Trim & "|"
                End If
            Next
            If MyRow.Length > 60 Then MyRows.Add(MyRow.Substring(0, MyRow.Length - 1))
        Next
        sFile.Close()
        Return MyRows
    End Function

    'Binds selected transactions for both Agribank and CABS to dgTransactions
    Private Sub BindTransactions(ByVal iTransactionList As List(Of BankTransaction))
        Dim bs As New BindingSource
        bs.DataSource = iTransactionList
        dgTransactions.DataSource = bs
    End Sub

    Private Sub BindTransactions(ByVal iTransactionList As List(Of xBankTrx))
        Dim bs As New BindingSource
        bs.DataSource = iTransactionList
        dgTransactions.DataSource = bs
    End Sub

    'Class used to populate dgTransactions and to view transactions before importing
    Public Class BankTransaction

        Private _TransactionDate As Date
        Public Property TransactionDate() As Date
            Get
                Return _TransactionDate
            End Get
            Set(ByVal value As Date)
                _TransactionDate = value
            End Set
        End Property


        Private _TransactionType As String
        Public Property TransactionType() As String
            Get
                Return _TransactionType
            End Get
            Set(ByVal value As String)
                _TransactionType = value
            End Set
        End Property


        Private _TransactionDescription As String
        Public Property TransactionDescription() As String
            Get
                Return _TransactionDescription
            End Get
            Set(ByVal value As String)
                _TransactionDescription = value
            End Set
        End Property

        Private _Branch As String
        Public Property Branch() As String
            Get
                Return _Branch
            End Get
            Set(ByVal value As String)
                _Branch = value
            End Set
        End Property

        Private _Amount As Decimal
        Public Property Amount() As Decimal
            Get
                Return _Amount
            End Get
            Set(ByVal value As Decimal)
                _Amount = value
            End Set
        End Property


        Private _Balance As Decimal
        Public Property Balance() As Decimal
            Get
                Return _Balance
            End Get
            Set(ByVal value As Decimal)
                _Balance = value
            End Set
        End Property


        Private _Identifier As String
        Public Property Identifier() As String
            Get
                Return _Identifier
            End Get
            Set(ByVal value As String)
                _Identifier = value
            End Set
        End Property

        Private _BankRef As String
        Public Property BankReference() As String
            Get
                Return _BankRef
            End Get
            Set(ByVal value As String)
                _BankRef = value
            End Set
        End Property
    End Class


    Private Function InterpretTrxTypeCABS(ByRef itrx As xBankTrx, ByVal Narrative As String) As xBankTrxType.BankTrxTypes
        Select Case Narrative
            Case "Cash Deposit"
                'itrx.RefName = Replace(Narrative, "CASH DEPOSIT", "").Trim
                Return xBankTrxType.BankTrxTypes.CashDeposit
            Case "Cash Withdrawal"
                Return xBankTrxType.BankTrxTypes.CashWithdrawal
            Case "Internet Debit"
                Return xBankTrxType.BankTrxTypes.eBankingTrfr
            Case "JNL DEBIT"
                Return xBankTrxType.BankTrxTypes.JnlDebit
            Case "TRFR CREDIT", "Paynet Transaction", "POS Deposit", "Inter Acct Transfer"
                Return xBankTrxType.BankTrxTypes.TransferCredit
            Case "Internet Credit"
                Return xBankTrxType.BankTrxTypes.eBankingTrfr
            Case "RTGS FEE"
                Return xBankTrxType.BankTrxTypes.RtgsCharge
            Case "INTERNET RTGS DEBIT PAYMENT"
                Return xBankTrxType.BankTrxTypes.RtgsCharge
            Case "TRFR CREDIT REV"
                Return xBankTrxType.BankTrxTypes.TransferCreditReversal
            Case "JNL DEBIT REV"
                Return xBankTrxType.BankTrxTypes.JnlDebitReversal
            Case "CASH DEP REV"
                Return xBankTrxType.BankTrxTypes.CashdepositReversal
            Case "Charge - Capitalise"
                Return xBankTrxType.BankTrxTypes.RtgsCharge 'NO SERVICE CHARGE TYPE, PROBABLY NEED TO CREATE ONE
            Case Else
                Return xBankTrxType.BankTrxTypes.NotApplicable
        End Select
        'If InStr(Narrative, "CASH DEP") > 0 Then
        '    itrx.RefName = Replace(Narrative, "CASH DEPOSIT", "").Trim
        '    Return xBankTrxType.BankTrxTypes.CashDeposit
        'Else
        '    itrx.RefName = Narrative
        '    Return xBankTrxType.BankTrxTypes.NotApplicable
        'End If
    End Function

    Private Sub LoadCABS(ByVal FilePath As String, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)

        Dim iTransactionList As New List(Of BankTransaction)
        Dim iTransaction As New BankTransaction
        Dim skipingheader As Boolean = True
        Try
            Using MyReader As New FileIO.TextFieldParser(FilePath)
#Region "Cabs Processing Loop"
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")
                Dim currentRow As String()
                While Not MyReader.EndOfData
                    Try
                        currentRow = MyReader.ReadFields()
                        If currentRow.Count < 4 Then ' Skips rows that will cause errors since row count is not always constant
                            ' Solves a Excel 2013 CSV Save adding double Quoutes
                            If currentRow.Count = 1 And (currentRow(0).Replace("""""", "").Length < currentRow(0).Length) Then
                                currentRow = currentRow(0).Replace(",""", ";").Replace(",", "").Replace("""", "").Split(";")
                            Else
                                GoTo skip
                            End If
                        End If
                        If currentRow(0).ToLower().StartsWith("post date") Or currentRow(0).ToLower().StartsWith("book date") Then skipingheader = False : GoTo skip
                        If skipingheader Then GoTo skip
                        If currentRow.ToString = ",,,,,,," Then GoTo skip

                        If currentRow(1).StartsWith("Balance at Period Start") Then GoTo skip
                        If currentRow(1).StartsWith("Balance at Period End") Then
                            If iTransaction.TransactionDescription Is Nothing Then iTransaction.TransactionDescription = iTransaction.Branch
                            iTransactionList.Add(iTransaction)
                            GoTo skip
                        End If

                        If IsDate(currentRow(0)) Then
                            If iTransaction.Balance <> 0 Then
                                If iTransaction.TransactionDescription Is Nothing Then iTransaction.TransactionDescription = iTransaction.Branch
                                iTransactionList.Add(iTransaction)
                                iTransaction = New BankTransaction
                            End If
                            iTransaction.TransactionDate = CDate(currentRow(0))
                        End If
                        If Not String.IsNullOrEmpty(currentRow(1)) Then iTransaction.Branch = currentRow(1) : iTransaction.BankReference = currentRow(1)
                        If Not String.IsNullOrEmpty(currentRow(2)) And Not (String.IsNullOrEmpty(iTransaction.TransactionType)) Then iTransaction.TransactionDescription += IIf(String.IsNullOrEmpty(iTransaction.TransactionDescription), "", ", ") + currentRow(2)
                        If Not String.IsNullOrEmpty(currentRow(2)) And (String.IsNullOrEmpty(iTransaction.TransactionType)) Then iTransaction.TransactionType = currentRow(2)

                        If currentRow.Length < 8 Then
                            currentRow(4) = GetAmount(currentRow(4))
                            currentRow(5) = GetAmount(currentRow(5))
                            currentRow(6) = GetAmount(currentRow(6))

                            If IsNumeric(currentRow(4)) Then
                                If Not CDec(currentRow(4)) = 0 Then iTransaction.Amount = -Math.Abs(CDec(currentRow(4)))
                            End If
                            If IsNumeric(currentRow(5)) Then
                                If Not CDec(currentRow(5)) = 0 Then iTransaction.Amount = CDec(currentRow(5))
                            End If
                            If IsNumeric(currentRow(6)) Then iTransaction.Balance = CDec(currentRow(6))
                        Else
                            currentRow(5) = GetAmount(currentRow(5))
                            currentRow(6) = GetAmount(currentRow(6))
                            currentRow(7) = GetAmount(currentRow(7))

                            If IsNumeric(currentRow(5)) Then
                                If Not CDec(currentRow(5)) = 0 Then iTransaction.Amount = -Math.Abs(CDec(currentRow(5)))
                            End If
                            If IsNumeric(currentRow(6)) Then
                                If Not CDec(currentRow(6)) = 0 Then iTransaction.Amount = CDec(currentRow(6))
                            End If
                            If IsNumeric(currentRow(7)) Then iTransaction.Balance = CDec(currentRow(7))

                        End If


skip:

                    Catch ex As FileIO.MalformedLineException
                        MsgBox("Line " & ex.Message & "is not valid and will be skipped.")

                    End Try
                End While
#End Region
            End Using

            If iTransaction.Amount <> 0 Then
                iTransactionList.Add(iTransaction)
            End If

            _BankTransactionList = iTransactionList

            BindTransactions(_BankTransactionList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetAmount(Amount As String) As String
        Dim stripped As String = ""
        Dim myChars() As Char = Amount.ToCharArray()
        For Each ch As Char In myChars
            If Char.IsDigit(ch) Or ch = "-" Or ch = "." Then stripped += ch

        Next
        Return stripped
    End Function

    Private Sub LoadAgriBank(ByVal FilePath As String, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        _BankTransactionList = New List(Of BankTransaction)
        Dim iTransactionList As New List(Of BankTransaction)

        If FilePath.Substring(FilePath.LastIndexOf(".")) = ".xls" Then

            Dim iExcel As New Excel.Application
            Dim iBook As Excel.Workbook = iExcel.Workbooks.Open(FilePath)
            Try
                Dim iSheet As Excel.Worksheet = iBook.Worksheets(1)

                pb.Maximum = 1000 : pb.Step = 1
                For x As Integer = 1 To 1000
                    pb.PerformStep()
                    Dim iRange As Excel.Range = iSheet.Range("A" & x)
                    'Check if first few lines are not blank                                
                    If IsDate(iRange.Text) Then
                        Dim iTransaction As New BankTransaction

                        'Amount
                        Dim Debit As Decimal = 0
                        If IsNumeric(iSheet.Range("D" & x).Text) Then
                            Debit = CDec(iSheet.Range("D" & x).Text)
                        End If
                        Dim Credit As Decimal = 0
                        If IsNumeric(iSheet.Range("E" & x).Text) Then
                            Credit = CDec(iSheet.Range("E" & x).Text)
                        End If
                        If IsNumeric(iSheet.Range("F" & x).Text) Then
                            iTransaction.Balance = CDec(iSheet.Range("F" & x).Text)
                        Else
                            iTransaction.Balance = 0
                        End If
                        iTransaction.Amount = Credit - Debit

                        'Bank Reference - not applicable
                        iTransaction.TransactionDescription = iSheet.Range("C" & x).Text
                        'Transaction Type                         
                        iTransaction.TransactionType = iSheet.Range("C" & x).Text.ToString.ToUpper

                        iTransaction.Branch = "n/a"
                        iTransaction.TransactionDate = CDate(iSheet.Range("A" & x).Text)


                        iTransactionList.Add(iTransaction)
                    End If
                Next
                _BankTransactionList = iTransactionList
                BindTransactions(_BankTransactionList)
            Catch ex As Exception
                Throw ex
            Finally
                iBook.Close()
                iExcel.Quit()
            End Try
        Else
            Try

                Dim iTransaction As New BankTransaction
                Dim lines As String() = File.ReadAllLines(FilePath)
                Dim readHeader As Boolean = False, currentcolumn As Integer = 0

                For Each line As String In lines

                    If String.IsNullOrWhiteSpace(line) Or String.IsNullOrEmpty(line) Then GoTo Skip
                    If line.StartsWith("Balance at Period Start") Then readHeader = True : GoTo Skip
                    If line.StartsWith("Balance at Period End") Then GoTo Skip

                    If Not readHeader Then GoTo Skip

                    Select Case currentcolumn
                        Case 1
                            iTransaction.TransactionType = line
                        Case 2
                            iTransaction.Branch = line
                        Case 4
                            If IsNumeric(line) Then iTransaction.Amount = line
                        Case 5
                            If IsNumeric(line) Then iTransaction.Balance = line
                        Case 6
                            If line.StartsWith("Transaction Detail") Then GoTo Skip
                            iTransaction.TransactionDescription = line
                            currentcolumn = 0
                            If Not String.IsNullOrEmpty(iTransaction.Branch) Then
                                iTransactionList.Add(iTransaction)
                                iTransaction = New BankTransaction
                            End If
                    End Select
                    If currentcolumn > 0 Then currentcolumn += 1
                    If IsDate(line) And currentcolumn = 0 Then
                        If CDate(line) < Date.Now.AddYears(-2) Then GoTo Skip 'Filter out Invalid Dates i.e. numbers that evaluate to date
                        currentcolumn = 1
                        iTransaction.TransactionDate = CDate(line)
                    End If

Skip:
                Next
                _BankTransactionList = iTransactionList
                BindTransactions(_BankTransactionList)
            Catch ex As Exception
                Throw ex
            End Try
        End If

    End Sub

    Private Function InterpretTrxTypeAgribank(ByRef itrx As xBankTrx, ByVal Narrative As String) As xBankTrxType.BankTrxTypes
        Narrative = (Narrative.ToUpper)
        If InStr(Narrative, "CASH DEPOSIT") > 0 Then
            itrx.RefName = Replace(Narrative, "CASH DEPOSIT", "").Trim
            Return xBankTrxType.BankTrxTypes.CashDeposit
        ElseIf InStr(Narrative, "CASH DEP REV") > 0 Then
            itrx.RefName = Replace(Narrative, "CASH DEP REV", "").Trim
            Return xBankTrxType.BankTrxTypes.CashdepositReversal
        ElseIf InStr(Narrative, "CASH WITHDRAWAL") > 0 Then
            itrx.RefName = Replace(Narrative, "CASH WITHDRAWAL", "").Trim
            Return xBankTrxType.BankTrxTypes.CashdepositReversal
        ElseIf InStr(Narrative, "EBANK TRFR") > 0 Then
            itrx.RefName = Replace(Narrative, "EBANK TRFR", "").Trim
            Return xBankTrxType.BankTrxTypes.eBankingTrfr
        ElseIf InStr(Narrative, "FUNDS TRANSFER") > 0 Then
            itrx.RefName = Replace(Narrative, "FUNDS TRANSFER", "").Trim
            Return xBankTrxType.BankTrxTypes.eBankingTrfr
        ElseIf InStr(Narrative, "DUTY ON CHEQUE BOOK") > 0 Then
            itrx.RefName = Replace(Narrative, "DUTY ON CHEQUE BOOK", "").Trim
            Return xBankTrxType.BankTrxTypes.JnlDebit
        ElseIf InStr(Narrative, "CHEQUE BOOK CHARGES") > 0 Then
            itrx.RefName = Replace(Narrative, "CHEQUE BOOK CHARGES", "").Trim
            Return xBankTrxType.BankTrxTypes.JnlDebit
        ElseIf InStr(Narrative, "RTGS CHARGE") > 0 Then
            itrx.RefName = Replace(Narrative, "RTGS CHARGE", "").Trim
            Return xBankTrxType.BankTrxTypes.RtgsCharge
        ElseIf InStr(Narrative, "ACCOUNT CHARGES") > 0 Then
            itrx.RefName = Replace(Narrative, "ACCOUNT CHARGES", "").Trim
            Return xBankTrxType.BankTrxTypes.RtgsCharge
        ElseIf InStr(Narrative, "SALARY CREDIT") > 0 Then
            itrx.RefName = Replace(Narrative, "SALARY CREDIT", "").Trim
            Return xBankTrxType.BankTrxTypes.SalaryCredit
        ElseIf InStr(Narrative, "TRANSFER") > 0 Then
            itrx.RefName = Replace(Narrative, "TRANSFER", "").Trim
            Return xBankTrxType.BankTrxTypes.SalaryCredit
        ElseIf InStr(Narrative, "TRFR CREDIT") > 0 Then
            itrx.RefName = Replace(Narrative, "TRFR CREDIT", "").Trim
            Return xBankTrxType.BankTrxTypes.TransferCredit
        ElseIf InStr(Narrative, "RECEIPTS-JNL TRANSFER") > 0 Then
            itrx.RefName = Replace(Narrative, "RECEIPTS-JNL TRANSFER", "").Trim
            Return xBankTrxType.BankTrxTypes.TransferCredit
        ElseIf InStr(Narrative, "RTGS RECEIPT") > 0 Then
            itrx.RefName = Replace(Narrative, "RTGS RECEIPT", "").Trim
            Return xBankTrxType.BankTrxTypes.RTGSReceipt
        ElseIf InStr(Narrative, "RTGS PAYMENT") > 0 Then
            itrx.RefName = Replace(Narrative, "RTGS PAYMENT", "").Trim
            Return xBankTrxType.BankTrxTypes.RTGSPayment
        ElseIf InStr(Narrative, "CHEQUE DEBIT") > 0 Then
            itrx.RefName = Replace(Narrative, "CHEQUE DEBIT", "").Trim
            Return xBankTrxType.BankTrxTypes.ChequePayment
        ElseIf InStr(Narrative, "CHQ DEBIT(INWD CLR)") > 0 Then
            itrx.RefName = Replace(Narrative, "CHQ DEBIT(INWD CLR)", "").Trim
            Return xBankTrxType.BankTrxTypes.ChequePayment
        ElseIf InStr(Narrative, "CHEQUE DEPOSIT 4 DAYS CLE") > 0 Then
            itrx.RefName = Replace(Narrative, "CHEQUE DEPOSIT 4 DAYS CLE", "").Trim
            Return xBankTrxType.BankTrxTypes.ChequePayment
        ElseIf InStr(Narrative, "CORPORATE PAYMENT") > 0 Then
            itrx.RefName = Replace(Narrative, "CORPORATE PAYMENT", "").Trim
            Return xBankTrxType.BankTrxTypes.TransferCredit
        ElseIf InStr(Narrative, "SERVICE FEES") > 0 Then
            itrx.RefName = Replace(Narrative, "SERVICE FEES", "").Trim
            Return xBankTrxType.BankTrxTypes.JnlDebit
        ElseIf InStr(Narrative, "LEDGER FEES") > 0 Then
            itrx.RefName = Replace(Narrative, "LEDGER FEES", "").Trim
            Return xBankTrxType.BankTrxTypes.JnlDebit
        ElseIf InStr(Narrative, "MB ZIPIT RECEIVE") > 0 Then
            ' itrx.RefName = Replace(Narrative, "LEDGER FEES", "").Trim
            Return xBankTrxType.BankTrxTypes.TransferCredit
        Else
            itrx.RefName = Narrative
            Return xBankTrxType.BankTrxTypes.NotApplicable
        End If
    End Function


    Private Function InterpretTrxTypeStanbic(ByRef itrx As xBankTrx, ByVal Narrative As String) As xBankTrxType.BankTrxTypes

        If Narrative.StartsWith("FEE -") Or Narrative.StartsWith("EOL IMTT") Then Return xBankTrxType.BankTrxTypes.RtgsCharge
        If Narrative.StartsWith("EOL/IAT") Or Narrative.StartsWith("EOL/RTGS") Then Return xBankTrxType.BankTrxTypes.eBankingTrfr
        If Narrative.Contains("263771998574") And itrx.Amount > 0 Then Return xBankTrxType.BankTrxTypes.TransferCredit
        If Narrative.StartsWith("ZIPIT") And itrx.Amount > 0 Then Return xBankTrxType.BankTrxTypes.RTGSPayment
        If itrx.Amount > 0 Then Return xBankTrxType.BankTrxTypes.MiscCredit
        If itrx.Amount < 0 Then Return xBankTrxType.BankTrxTypes.MiscDebit

        Return xBankTrxType.BankTrxTypes.NotApplicable

    End Function

    Private Sub LoadStanbic(ByVal FilePath As String, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)

        Dim iTransactionList As New List(Of BankTransaction)
        If FilePath.Substring(FilePath.LastIndexOf(".")) = ".csv" Then
            iTransactionList = GetTransactionsStanbicCSV(FilePath)

        End If

        _BankTransactionList = iTransactionList

        BindTransactions(_BankTransactionList)
    End Sub

    Function GetTransactionsStanbicCSV(FilePath As String) As List(Of BankTransaction)
        Dim iTransactionList As New List(Of BankTransaction)
        Using MyReader As New FileIO.TextFieldParser(FilePath)

            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim Row As String()
            While Not MyReader.EndOfData
                Try
                    Row = MyReader.ReadFields()
                    If Row.Count > 5 Then
                        Dim iDate As DateTime
                        Dim isValidDate = DateTime.TryParseExact(Row(1), "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, iDate) Or DateTime.TryParseExact(Row(1), "d/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, iDate)
                        If isValidDate Then
                            Dim iTransaction As New BankTransaction With {
                                .Amount = If(IsNumeric(Row(4)), CDec(Row(4)), (CDec(Row(3)) * -1)),
                                .Balance = If(IsNumeric(Row(5)), CDec(Row(5)), 0),
                                .TransactionDescription = Row(2),
                                .TransactionType = Row(2),
                                .Branch = "na",
                                .TransactionDate = iDate
                            }
                            iTransactionList.Add(iTransaction)

                        End If

                    End If

                Catch ex As Exception
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped." & String.Join(",", If(Row, "")))
                End Try
            End While
        End Using
        Return iTransactionList
    End Function



    Private Function InterpretTrxTypeEcoCash(ByRef itrx As xBankTrx, ByVal Narrative As String) As xBankTrxType.BankTrxTypes
        Select Case Narrative
            Case "CR"
                Return xBankTrxType.BankTrxTypes.EcoCashPending
            Case "DR"
                Return xBankTrxType.BankTrxTypes.JnlDebit
            Case Else
                Return xBankTrxType.BankTrxTypes.NotApplicable
        End Select

    End Function

    Private Sub LoadEcocash(ByVal FilePath As String, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)

        Dim iTransactionList As New List(Of BankTransaction)
        If FilePath.Substring(FilePath.LastIndexOf(".")) = ".xlsx" Or FilePath.Substring(FilePath.LastIndexOf(".")) = ".xls" Then
            Try
                iTransactionList = GetTransactionsEcoCashExcel(FilePath)
            Catch ex As Exception
                If ex.Message.Contains("invalid signature") Then iTransactionList = GetTransactionsEcocashCSV(FilePath)
                Throw ex
            End Try
        ElseIf FilePath.Substring(FilePath.LastIndexOf(".")) = ".csv" Then
            iTransactionList = GetTransactionsEcocashCSV(FilePath)

        End If

        If iTransactionList.Count() > 0 Then

            Dim startdate As Date = (From i As BankTransaction In iTransactionList Order By i.TransactionDate Ascending Select i.TransactionDate).Take(1).Single.AddDays(-1).AddHours(-1)
            Dim enddate As Date = (From i As BankTransaction In iTransactionList Order By i.TransactionDate Descending Select i.TransactionDate).Take(1).Single.AddHours(+1)

            Dim batchids = (From b As xBankTrxBatch In xBankTrxBatchAdapter.List(xBank.Banks.EcoMerchant, sqlConn, sqlTrans)
                            Where (b.BatchDate > startdate And b.BatchDate < enddate) Or (
                                b.BatchDate > Date.Now.AddDays(-1).AddHours(-1) And b.BatchDate < Date.Now.AddHours(+1))
                            Select b.BankTrxBatchID).ToList()

            Dim SystemBankTrx As New List(Of xBankTrx)
            pb.Maximum = batchids.Count() : pb.Step = 1 : pb.Value = 0
            For Each batchid In batchids
                SystemBankTrx.AddRange(xBankTrxAdapter.List(batchid, sqlConn, sqlTrans))
                pb.PerformStep()
            Next


            Dim iTransactionsToAdd As List(Of BankTransaction) =
                (From b In iTransactionList
                 Group Join a In SystemBankTrx On b.TransactionDescription Equals a.BankRef
                    Into bList = Group
                 From a In bList.DefaultIfEmpty()
                 Where (a Is Nothing)
                 Select b).ToList

            ' Ecocash Pending Transactions
            'Dim iTransactionsToUpdate As List(Of xBankTrx) =
            '    (From b In iTransactionList
            '     Group Join a In SystemBankTrx On b.TransactionDescription Equals a.BankRef
            '        Into bList = Group
            '     From a In bList.DefaultIfEmpty()
            '     Where (Not (a Is Nothing)) And
            '         If(a Is Nothing, False,
            '            (IsNumeric(a.RefName) And
            '            (a.PaymentID Is Nothing Or a.PaymentID = 0) And
            '            (If(a Is Nothing, 0, a.BankTrxState.BankTrxStateID) <> xBankTrxState.BankTrxStates.Success)))
            '     Select a).ToList
            ''Where (Not (a Is Nothing)) And (a.BankTrxState.BankTrxStateID <> xBankTrxState.BankTrxStates.Success)
            '_BankTransactionUpdateList = iTransactionsToUpdate
            ' btnEcoCash.Visible = (iTransactionsToUpdate.Count > 0)

            _BankTransactionList = iTransactionsToAdd

        End If

        BindTransactions(_BankTransactionList)
    End Sub


    Function GetTransactionsEcocashCSV(FilePath As String) As List(Of BankTransaction)
        Dim iTransactionList As New Collection(Of BankTransaction)
        Using MyReader As New FileIO.TextFieldParser(FilePath)

            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(vbTab, ",")
            Dim Row As String()
            While Not MyReader.EndOfData
                Try
                    Row = MyReader.ReadFields()
                    If Row.Count > 8 Then
                        If Row(8) = MerchantNumber Or Row(9) = MerchantNumber Or Row(8) = "RTGS$" Or
                             Row(8) = APIMerchantNumber Or Row(9) = APIMerchantNumber Or
                              Row(8) = ZESAMerchantNumber Or Row(9) = ZESAMerchantNumber _
                              Then
                            Dim iTransaction As New BankTransaction With {
                                .Amount = If(IsNumeric(Row(6)), CDec(Row(6)) * (If(Row(4).ToUpper = "DR", -1, 1)), 0),
                                .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                                .TransactionDescription = Row(0),
                                .Identifier = If(Row(5).StartsWith("7"), "0" & Row(5), Row(5)),
                                .TransactionType = Row(4).ToUpper,
                                .Branch = If(Row(8) = APIMerchantNumber Or Row(9) = APIMerchantNumber, "API-User", If(Row(8) = ZESAMerchantNumber Or Row(9) = ZESAMerchantNumber, "ZESA", "na")),
                                .TransactionDate = DateTime.Parse(Row(1))
                            }
                            iTransactionList.Add(iTransaction)
                        ElseIf (Row(1) = MerchantNumber Or Row(1) = APIMerchantNumber Or Row(1) = ZESAMerchantNumber) Then
                            If Row.Count > 10 Then
                                If Not Row(10) = "101" Then
                                    Dim iTransaction As New BankTransaction With {
                                    .Amount = If(IsNumeric(Row(8)), CDec(Row(8)) * (If(Row(6).ToUpper = "DR", -1, 1)), 0),
                                    .Balance = If(IsNumeric(Row(9)), CDec(Row(9)), 0),
                                    .TransactionDescription = Row(0),
                                    .Identifier = If(Row(10).StartsWith("7"), "0" & Row(10), Row(10)),
                                    .TransactionType = Row(6).ToUpper,
                                    .Branch = If(Row(1) = APIMerchantNumber, "API-User", If(Row(1) = ZESAMerchantNumber, "ZESA", "na")),
                                    .TransactionDate = DateTime.Parse(Row(4))
                                }
                                    iTransactionList.Add(iTransaction)
                                ElseIf Row(10) = "101" Then
                                    Dim dateResult As DateTime

                                    If DateTime.TryParseExact(Row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                        Dim iTransaction As New BankTransaction With {
                                   .Amount = If(IsNumeric(Row(5)), CDec(Row(5)) * (If(Row(9).ToUpper = "DR", -1, 1)), 0),
                                   .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                                   .TransactionDescription = Row(2),
                                   .Identifier = If(Row(3).StartsWith("7"), "0" & Row(3), Row(3)),
                                   .TransactionType = Row(9).ToUpper,
                                   .Branch = If(Row(0) = APIMerchantNumber, "API-User", If(Row(0) = ZESAMerchantNumber, "ZESA", "na")),
                                   .TransactionDate = dateResult,
                                   .BankReference = Row(2)
                               }
                                        iTransactionList.Add(iTransaction)
                                    ElseIf DateTime.TryParseExact(Row(2), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                        Dim iTransaction As New BankTransaction With {
                                   .Amount = If(IsNumeric(Row(5)), CDec(Row(5)) * (If(Row(9).ToUpper = "DR", -1, 1)), 0),
                                   .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                                   .TransactionDescription = Row(3),
                                   .Identifier = If(Row(4).StartsWith("7"), "0" & Row(4), Row(4)),
                                   .TransactionType = Row(9).ToUpper,
                                   .Branch = If(Row(1) = APIMerchantNumber, "API-User", If(Row(1) = ZESAMerchantNumber, "ZESA", "na")),
                                   .TransactionDate = dateResult,
                                   .BankReference = Row(3)
                               }
                                        iTransactionList.Add(iTransaction)
                                    End If
                                End If
                            End If
                        ElseIf (Row(0) = APIMerchantNumber Or Row(0) = ZESAMerchantNumber Or Row(0) = MerchantNumber) Then
                            If Row.Count > 11 Then
                                If Row(11) = "101" Then
                                    Dim dateResult As DateTime
                                    If Not DateTime.TryParseExact(Row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                        dateResult = DateTime.ParseExact(Row(1), "dd/M/yyyy HH:mm", New CultureInfo("en-US"))
                                    End If

                                    Dim iTransaction As New BankTransaction With {
                               .Amount = If(IsNumeric(Row(5)), CDec(Row(5)) * (If(Row(12).ToUpper = "DR", -1, 1)), 0),
                               .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                               .TransactionDescription = Row(2),
                               .Identifier = If(Row(3).StartsWith("7"), "0" & Row(3), Row(3)),
                               .TransactionType = Row(8).ToUpper,
                               .Branch = If(Row(0) = APIMerchantNumber, "API-User", If(Row(0) = ZESAMerchantNumber, "ZESA", "na")),
                               .TransactionDate = dateResult
                           }
                                    iTransactionList.Add(iTransaction)

                                ElseIf Row.Length = 13 Then
                                    Dim dateResult As DateTime
                                    If Not DateTime.TryParseExact(Row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                        dateResult = DateTime.ParseExact(Row(1), "dd/M/yyyy HH:mm", New CultureInfo("en-US"))
                                    End If

                                    Dim iTransaction As New BankTransaction With {
                               .Amount = If(IsNumeric(Row(5)), CDec(Row(5)) * (If(Row(11).ToUpper = "DR", -1, 1)), 0),
                               .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                               .TransactionDescription = Row(2),
                               .Identifier = If(Row(3).StartsWith("7"), "0" & Row(3), Row(3)),
                               .TransactionType = Row(10).ToUpper,
                               .Branch = If(Row(0) = APIMerchantNumber, "API-User", If(Row(0) = ZESAMerchantNumber, "ZESA", "na")),
                               .TransactionDate = dateResult
                           }
                                    iTransactionList.Add(iTransaction)


                                End If
                            ElseIf (Row(10) = "101") Then
                                Dim dateResult As DateTime
                                If Not DateTime.TryParseExact(Row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                    dateResult = DateTime.ParseExact(Row(1), "dd/M/yyyy HH:mm", New CultureInfo("en-US"))
                                End If
                                Dim iTransaction As New BankTransaction With {
                                    .Amount = If(IsNumeric(Row(5)), CDec(Row(5)) * (If(Row(9).ToUpper = "DR", -1, 1)), 0),
                                    .Balance = If(IsNumeric(Row(7)), CDec(Row(7)), 0),
                                    .TransactionDescription = Row(2),
                                    .Identifier = If(Row(3).StartsWith("7"), "0" & Row(3), Row(3)),
                                    .TransactionType = Row(8).ToUpper,
                                    .Branch = If(Row(0) = APIMerchantNumber Or Row(0) = APIMerchantNumber, "API-User", If(Row(0) = ZESAMerchantNumber Or Row(0) = ZESAMerchantNumber, "ZESA", "na")),
                                    .TransactionDate = dateResult
                                }
                                iTransactionList.Add(iTransaction)
                            ElseIf (Row(9) = "101") Then
                                Dim dateResult As DateTime
                                If Not DateTime.TryParseExact(Row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                    dateResult = DateTime.ParseExact(Row(1), "dd/M/yyyy HH:mm", New CultureInfo("en-US"))
                                End If
                                Dim iTransaction As New BankTransaction With {
                                    .Amount = If(IsNumeric(Row(4)), CDec(Row(4)) * (If(Row(8).ToUpper = "DR", -1, 1)), 0),
                                    .Balance = If(IsNumeric(Row(6)), CDec(Row(6)), 0),
                                    .TransactionDescription = Row(2),
                                    .Identifier = If(Row(3).StartsWith("7"), "0" & Row(3), Row(3)),
                                    .TransactionType = Row(7).ToUpper,
                                    .Branch = If(Row(0) = APIMerchantNumber Or Row(0) = APIMerchantNumber, "API-User", If(Row(0) = ZESAMerchantNumber Or Row(0) = ZESAMerchantNumber, "ZESA", "na")),
                                    .TransactionDate = dateResult
                                }
                                iTransactionList.Add(iTransaction)
                            End If
                        End If
                    End If

                Catch ex As Exception
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped." & String.Join(",", Row))
                End Try
            End While
        End Using
        Return iTransactionList.ToList()
    End Function

    Function GetTransactionsEcoCashExcel(FilePath) As List(Of BankTransaction)

        Dim iTransactionList As New List(Of BankTransaction)
        Using stream = File.Open(FilePath, FileMode.Open, FileAccess.Read)
            Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)
                Try
                    Dim result As DataSet = reader.AsDataSet()
                    Dim iSheet As DataTable = result.Tables(0)
                    pb.Maximum = iSheet.Rows.Count : pb.Step = 1
                    Dim StatmentMerchantNumber As String = ""
                    For Each row As DataRow In iSheet.Rows
                        pb.PerformStep()
                        'Check if Transaction                                
                        If row.Item(0).ToString() = MerchantNumber Or
                            row.Item(0).ToString() = APIMerchantNumber Or
                            row.Item(0).ToString() = ZESAMerchantNumber Then

                            If row.ItemArray().Count() > 10 Then
                                If row(10) = "101" Then
                                    Dim iTransaction As New BankTransaction With {
                                       .Amount = If(IsNumeric(row.Item(5).ToString()), CDec(row.Item(5).ToString()) * (If(row.Item(9).ToString().ToUpper = "DR", -1, 1)), 0),
                                       .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                                       .TransactionDescription = row.Item(2).ToString(),
                                       .Identifier = If(row.Item(3).ToString().StartsWith("7"), "0" & row.Item(3).ToString(), row.Item(3).ToString()),
                                       .TransactionType = row.Item(8).ToString().ToUpper,
                                       .Branch = If(row.Item(0).ToString() = APIMerchantNumber, "API-User", If(row.Item(0).ToString() = ZESAMerchantNumber, "ZESA", "na")),
                                       .TransactionDate = row.Item(1).ToString()
                                   }

                                    iTransactionList.Add(iTransaction)
                                End If
                                If row(10) = "ZiG" Then
                                    Dim iTransaction As New BankTransaction With {
                                       .Amount = If(IsNumeric(row.Item(6).ToString()), CDec(row.Item(6).ToString()) * (If(row.Item(9).ToString().ToUpper = "DR", -1, 1)), 0),
                                       .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                                       .TransactionDescription = row.Item(3).ToString(),
                                       .Identifier = If(row.Item(4).ToString().StartsWith("7"), "0" & row.Item(4).ToString(), row.Item(4).ToString()),
                                       .TransactionType = row.Item(8).ToString().ToUpper,
                                       .Branch = If(row.Item(0).ToString() = APIMerchantNumber, "API-User", If(row.Item(0).ToString() = ZESAMerchantNumber, "ZESA", "na")),
                                       .TransactionDate = row.Item(2).ToString()
                                   }

                                    iTransactionList.Add(iTransaction)
                                End If
                                If row(10) = "MR" Then
                                    Dim dateResult As DateTime
                                    If Not DateTime.TryParseExact(row(1), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), DateTimeStyles.None, dateResult) Then
                                        dateResult = DateTime.ParseExact(row(1), "dd/M/yyyy HH:mm", New CultureInfo("en-US"))
                                    End If
                                    Dim iTransaction As New BankTransaction With {
                                       .Amount = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()) * (If(row.Item(11).ToString().ToUpper = "DR", -1, 1)), 0),
                                       .Balance = If(IsNumeric(row.Item(9).ToString()), CDec(row.Item(9).ToString()), 0),
                                       .TransactionDescription = row.Item(2).ToString(),
                                       .Identifier = If(row.Item(3).ToString().StartsWith("7"), "0" & row.Item(3).ToString(), row.Item(3).ToString()),
                                       .TransactionType = row.Item(10).ToString().ToUpper,
                                       .Branch = If(row.Item(0).ToString() = APIMerchantNumber, "API-User", If(row.Item(0).ToString() = ZESAMerchantNumber, "ZESA", "na")),
                                       .TransactionDate = dateResult
                                   }

                                    iTransactionList.Add(iTransaction)
                                End If
                            Else
                                Dim iTransaction As New BankTransaction With {
                               .Amount = If(IsNumeric(row.Item(6).ToString()), CDec(row.Item(6).ToString()) * (If(row.Item(9).ToString().ToUpper = "DR", -1, 1)), 0),
                               .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                               .TransactionDescription = row.Item(3).ToString(),
                               .Identifier = If(row.Item(4).ToString().StartsWith("7"), "0" & row.Item(4).ToString(), row.Item(4).ToString()),
                               .TransactionType = row.Item(9).ToString().ToUpper,
                               .Branch = If(row.Item(0).ToString() = APIMerchantNumber, "API-User", If(row.Item(0).ToString() = ZESAMerchantNumber, "ZESA", "na")),
                               .TransactionDate = row.Item(2).ToString()
                           }

                                iTransactionList.Add(iTransaction)
                            End If

                        End If


                        If row.ItemArray.Count() > 9 Then
                            If row.Item(9).ToString() = MerchantNumber Or
                                                        row.Item(9).ToString() = APIMerchantNumber Or
                                                        row.Item(9).ToString() = ZESAMerchantNumber Then

                                Dim iTransaction As New BankTransaction With {
                                    .Amount = If(IsNumeric(row.Item(6).ToString()), CDec(row.Item(6).ToString()) * (If(row.Item(4).ToString().ToUpper = "DR", -1, 1)), 0),
                                    .Balance = If(IsNumeric(row.Item(7).ToString()), CDec(row.Item(7).ToString()), 0),
                                    .TransactionDescription = row.Item(0).ToString(),
                                    .Identifier = If(row.Item(5).ToString().StartsWith("7"), $"0{row.Item(5)}", row.Item(5).ToString()),
                                    .TransactionType = row.Item(3).ToString().ToUpper,
                                    .Branch = If(row.Item(9).ToString() = APIMerchantNumber, "API-User", If(row.Item(9).ToString() = ZESAMerchantNumber, "ZESA", "na")),
                                    .TransactionDate = row.Item(1).ToString()
                                }
                                iTransactionList.Add(iTransaction)

                            End If
                        End If

                        If row.Item(1).ToString() = MerchantNumber Or
                           row.Item(1).ToString() = APIMerchantNumber Or
                           row.Item(1).ToString() = ZESAMerchantNumber Then
                            StatmentMerchantNumber = row.Item(1).ToString()
                        End If

                        If Not String.IsNullOrEmpty(StatmentMerchantNumber) And row.ItemArray(7).ToString().Contains("RTGS") Then

                            Dim iTransaction As New BankTransaction With {
                                .Amount = If(IsNumeric(row.Item(5).ToString()), CDec(row.Item(5).ToString()) * (If(row.Item(3).ToString().ToUpper = "MP", -1, 1)), 0),
                                .Balance = If(IsNumeric(row.Item(6).ToString()), CDec(row.Item(6).ToString()), 0),
                                .TransactionDescription = row.Item(0).ToString(),
                                .Identifier = If(row.Item(4).ToString().StartsWith("7"), $"0{row.Item(4)}", row.Item(4).ToString()),
                                .TransactionType = row.Item(2).ToString().ToUpper,
                                .Branch = If(StatmentMerchantNumber = APIMerchantNumber, "API-User", If(StatmentMerchantNumber = ZESAMerchantNumber, "ZESA", "na")),
                                .TransactionDate = row.Item(1).ToString()
                            }
                            iTransactionList.Add(iTransaction)
                        End If


                    Next
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

        End Using
        Return iTransactionList
    End Function

    'Import transactions, then call InsertPayments method
    Private Sub cmdImportTransactions_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImportTransactions.Click
        Dim iPercentage As Decimal
        Try
            iPercentage = CType(txtDiscount.Text, Decimal)
        Catch ex As Exception
            MsgBox("Discount % must be a decimal number!")
            txtDiscount.SelectAll()
            txtDiscount.Focus()
            Exit Sub
        End Try
        If dgTransactions.RowCount > 0 Then
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Dim iBatch As xBankTrxBatch = MakeBankTrxBatch(sqlConn, sqlTrans)
                Dim iTransactionList As List(Of BankTransaction) = _BankTransactionList

                For Each i In iTransactionList

                    Dim iTrx As New xBankTrx
                    iTrx.BankTrxBatchID = iBatch.BankTrxBatchID
                    iTrx.Amount = i.Amount
                    iTrx.Balance = i.Balance
                    iTrx.BankRef = i.Branch
                    iTrx.RefName = i.TransactionDescription
                    iTrx.BankTrxType = New xBankTrxType
                    Select Case cboBank.SelectedValue
                        Case xBank.Banks.AgriBank
                            ProcessTranTypeAgribank(iTrx, i)

                        Case xBank.Banks.CABS
                            ProcessTranTypeCABS(iTrx, i)
                        Case xBank.Banks.CABSUSD
                            ProcessTranTypeCABS(iTrx, i)

                        Case xBank.Banks.EcoMerchant
                            ProcessTranTypeEcoCash(iTrx, i)

                        Case xBank.Banks.Stanbic
                            ProcessTranTypeStanbic(iTrx, i)

                        Case xBank.Banks.StanbicZesa
                            ProcessTranTypeStanbic(iTrx, i)

                        Case xBank.Banks.StanbicUSD
                            ProcessTranTypeStanbic(iTrx, i)

                        Case xBank.Banks.StewardBank
                            ProcessTranTypeSteward(iTrx, i)
                        Case xBank.Banks.StewardBankUSD
                            ProcessTranTypeSteward(iTrx, i)
                        Case xBank.Banks.CBZ
                            ProcessTranTypeCBZ(iTrx, i)
                        Case xBank.Banks.CBZUSD
                            ProcessTranTypeCBZ(iTrx, i)
                    End Select

                    iTrx.Branch = i.Branch
                    iTrx.TrxDate = i.TransactionDate
                    iTrx.BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.Suspended}

                    If Not String.IsNullOrEmpty(iTrx.Identifier) Then
                        Dim iAccess As xAccess = xAccessAdapter.SelectCode(iTrx.Identifier, sqlConn, sqlTrans)
                        If iAccess IsNot Nothing Then
                            iTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success
                        End If
                    End If

                    xBankTrxAdapter.Insert(iTrx, sqlConn, sqlTrans)
                Next

                Try
                    sqlTrans.Commit()
                    'MsgBox("Transactions imported successfully")

                Catch ex As Exception
                    sqlTrans.Rollback()
                    ShowEx(ex)
                End Try
                'Insert Payments for the batch
                InsertPayments(iBatch)
                _BankTransactionList = New List(Of BankTransaction)
                BindTransactions(_BankTransactionList)

                sqlConn.Close()
            End Using

        End If
    End Sub

    Private Sub ProcessTranTypeCBZ(iTrx As xBankTrx, i As BankTransaction)
        iTrx.BankRef = i.BankReference
        iTrx.Identifier = i.Identifier
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeCBZ(iTrx, i.TransactionType)

    End Sub

    Private Function InterpretTrxTypeCBZ(iBankTrx As xBankTrx, transactionType As String) As Integer
        Select Case UCase(transactionType)
            Case "CASH DEPOSIT", "CASH DEPOSIT M"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashDeposit
            Case "REVERSAL"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashdepositReversal
            Case "CASH WITHDRAWAL", "ATM WDL"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.CashWithdrawal
            Case "CHEQUE DEPOSIT"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequeDeposit
            Case "CHEQUE WITHDRAWAL"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.ChequePayment
            Case "INTERNET BANKING INTER A/C TRANSFER"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.eBankingTrfr
            Case "OTT COMMISSION CHARGE", "CASH WITHDRAWAL CHARGE"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.EmailCharge
            Case "DEBIT", "SUNDRY DEBIT",
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebit
            Case "DEBIT REVERSAL"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.JnlDebitReversal
            Case "TRANSFER CHARGE", "RTGS TRF CHARGE", "INTERMEDIATED MONEY TRANSFER TAX", "TRANSFER HANDLING FEE"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RtgsCharge
            Case "INTERNET BANKING THIRD PARTY PMT", "TELEGRAPHIC TRANSFER", "RTGS TRF",
                "ATM FUNDS TRANSFER", "PAYMENT AS PER YOUR INSTRUCTIONS"
                If iBankTrx.Amount < 0 Then
                    iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSPayment
                Else
                    iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.RTGSReceipt
                End If
            Case "INCOMING BANK TRANFER", "INCOMING INTERBANK FUNDS TRANSFER", "SUNDRY CREDIT"
                iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.SalaryCredit
            Case Else
                If iBankTrx.Amount < 0 Then
                    iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscDebit

                Else
                    iBankTrx.BankTrxType.BankTrxTypeID = xBankTrxType.BankTrxTypes.MiscCredit
                End If
        End Select
        Return iBankTrx.BankTrxType.BankTrxTypeID
    End Function

    Sub ProcessTranTypeEcoCash(ByRef iTrx As xBankTrx, i As BankTransaction)
        iTrx.BankRef = iTrx.RefName
        iTrx.Identifier = i.Identifier
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeEcoCash(iTrx, i.TransactionType)

    End Sub

    Sub ProcessTranTypeStanbic(ByRef iTrx As xBankTrx, i As BankTransaction)
        iTrx.Identifier = ""
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeCABS(iTrx, i.TransactionType)

        CheckBankTransactionForMobileOrEmail(iTrx, i)
    End Sub
    Sub ProcessTranTypeSteward(ByRef iTrx As xBankTrx, i As BankTransaction)
        iTrx.Identifier = ""
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeSteward(iTrx, i.TransactionType)

        CheckBankTransactionForMobileOrEmail(iTrx, i)
    End Sub

    Private Function InterpretTrxTypeSteward(iTrx As xBankTrx, transactionType As String) As Integer

        If transactionType.Contains("Internal Transfer") Then Return xBankTrxType.BankTrxTypes.TransferCredit
        If iTrx.Amount < 0 And transactionType.Contains("debit") Then Return xBankTrxType.BankTrxTypes.JnlDebit
        If transactionType.Contains("Zip") Then Return xBankTrxType.BankTrxTypes.eBankingTrfr
        If transactionType.Contains("cash") Then Return xBankTrxType.BankTrxTypes.eBankingTrfr

        Return xBankTrxType.BankTrxTypes.NotApplicable
    End Function

    Sub ProcessTranTypeCABS(ByRef iTrx As xBankTrx, i As BankTransaction)
        iTrx.Identifier = ""
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeCABS(iTrx, i.TransactionType)

        CheckBankTransactionForMobileOrEmail(iTrx, i)
    End Sub

    Sub CheckBankTransactionForMobileOrEmail(ByRef iTrx As xBankTrx, i As BankTransaction)
        CheckBankTransactionForMobile(iTrx, i)
        CheckBankTransactionForEmail(iTrx, i)
    End Sub
    Sub CheckBankTransactionForMobile(ByRef iTrx As xBankTrx, i As BankTransaction)
        For Each iChar As Char In i.TransactionDescription.ToCharArray
            If IsNumeric(iChar) Then iTrx.Identifier &= iChar
        Next
        If iTrx.Identifier.StartsWith("7") And 8 < iTrx.Identifier.Length < 9 Then iTrx.Identifier = "0" + iTrx.Identifier
        If iTrx.Identifier.StartsWith("+236") Then iTrx.Identifier.Replace("+263", "0")
        If iTrx.Identifier.StartsWith("263") Then iTrx.Identifier = "0" + iTrx.Identifier.Substring(3)
        If (iTrx.Identifier.Length < 10 Or Not iTrx.Identifier.StartsWith("07")) Then iTrx.Identifier = ""
    End Sub
    Sub CheckBankTransactionForEmail(ByRef iTrx As xBankTrx, i As BankTransaction)
        If (i.TransactionDescription.Contains("@") And i.TransactionDescription.Contains(".")) Then

            For Each emailID As String In i.TransactionDescription.Trim.Split(" ")
                Try
                    iTrx.Identifier = New System.Net.Mail.MailAddress(emailID).ToString()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Sub ProcessTranTypeAgribank(ByRef iTrx As xBankTrx, i As BankTransaction)
        iTrx.BankTrxType.BankTrxTypeID = InterpretTrxTypeAgribank(iTrx, i.TransactionType)
        iTrx.Identifier = ""
        If InStr(i.TransactionDescription, "RTGS RECEIPT") > 0 Then
            iTrx.Identifier = ""
        Else
            CheckBankTransactionForMobileOrEmail(iTrx, i)
        End If
    End Sub

    Function MakeBankTrxBatch(ByRef sqlConn As SqlConnection, ByRef sqlTrans As SqlTransaction) As xBankTrxBatch
        Dim FilePath As String = txtFileName.Text
        Dim iBatch As New xBankTrxBatch With {
            .BankID = cboBank.SelectedValue, 'since bankID is the value
            .BatchDate = Now,
            .BatchReference = FilePath.Split("\")(FilePath.Split("\").Length - 1),
            .LastUser = gUser.UserName
        }
        Try
            xBankTrxBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)
        Catch ex As Exception
            ShowEx(ex)
            Throw ex
        End Try
        Return iBatch
    End Function

    'Used for all banks
    Private Sub InsertPayments(ByVal iBatch As xBankTrxBatch)

        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            Dim iBankTrxList = xBankTrxAdapter.List(iBatch.BankTrxBatchID, sqlConn)
            Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction

            InsertPaymentsProcess(iBankTrxList, sqlConn, sqlTrans)

            Try
                sqlTrans.Commit()
                MsgBox("Success", MsgBoxStyle.Information)
            Catch ex As Exception
                sqlTrans.Rollback()
                ShowEx(ex)
            Finally
                sqlConn.Close()
            End Try

            _BankTransactionList.Clear()
            If Not _BankTransactionUpdateList Is Nothing Then
                If Not (_BankTransactionUpdateList.Count > 0) Then Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If


        End Using
    End Sub

    Private Sub InsertPaymentsProcess(ByRef iBankTrxList As List(Of xBankTrx), ByRef sqlConn As SqlConnection, ByRef sqlTrans As SqlTransaction)
        For Each iTrx As xBankTrx In iBankTrxList
            Try

                'Check if transaction identifier is an access code
                Dim iAccess As xAccess = xAccessAdapter.SelectCode(iTrx.Identifier, sqlConn, sqlTrans)
                'If it is a valid access code
                If iAccess IsNot Nothing Then
                    'Insert Payment
                    Dim iPayment As New xPayment With {
                        .PaymentID = 0,
                        .AccountID = iAccess.AccountID,
                        .Amount = iTrx.Amount,
                        .LastUser = gUser.UserName,
                        .PaymentDate = Now,
                        .PaymentSource = New xPaymentSource,
                        .PaymentType = New xPaymentType,
                        .Reference = iTrx.RefName
                    }
                    iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.BankAuto
                    Select Case CType(cboBank.SelectedValue, xBank.Banks)
                        Case xBank.Banks.AgriBank
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Agribank
                        Case xBank.Banks.CABS
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.CABS
                        Case xBank.Banks.Kingdom
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Kingdom
                        Case xBank.Banks.CBZ
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.CBZ

                        Case xBank.Banks.EcoMerchant
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.EcoCash
                            iPayment.Reference = "Ecocash Payment Successful. EcoCash Ref: " & iTrx.BankRef

                        Case xBank.Banks.Stanbic
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.Stanbic
                        Case xBank.Banks.StanbicZesa
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.ZesaStanbic
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.ZESA
                        Case xBank.Banks.StanbicUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDStanbic
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                        Case xBank.Banks.CABSUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDCabs
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                        Case xBank.Banks.CBZUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDCBZ
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD

                        Case xBank.Banks.StewardBank
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.StewardBank
                        Case xBank.Banks.StewardBankUSD
                            iPayment.PaymentSource.PaymentSourceID = xPaymentSource.PaymentSources.USDStewardBank
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD
                    End Select

                    If iTrx.Branch.StartsWith("ZESA") Then
                        If cboBank.SelectedValue = xBank.Banks.EcoMerchant _
                                Or cboBank.SelectedValue = xBank.Banks.OneMoney _
                                Or cboBank.SelectedValue = xBank.Banks.OneMoneyUSD Then
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.ZESA

                        End If
                    End If
                    If iTrx.Branch.StartsWith("UUSD") Then
                        If cboBank.SelectedValue = xBank.Banks.EcoMerchant _
                                Or cboBank.SelectedValue = xBank.Banks.OneMoney _
                                Or cboBank.SelectedValue = xBank.Banks.OneMoneyUSD Then
                            iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USDUtility
                        End If
                    End If


                    xPaymentAdapter.Save(iPayment, sqlConn, sqlTrans)
                    iTrx.PaymentID = iPayment.PaymentID
                    xBankTrxAdapter.UpdatePaymentID(iTrx, sqlConn, sqlTrans)
                    If CType(cboBank.SelectedValue, xBank.Banks) Then iPayment.Reference = iTrx.BankRef

                    'SMS
                    Dim iSMS As New xSMS With {
                    .Direction = False,
                    .Mobile = iAccess.AccessCode,
                    .SMSDate = Now,
                    .SMSText = "Payment Received: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                        "Amount: " & FormatNumber(iPayment.Amount, 2) & vbNewLine &
                        "Balance: " & FormatNumber(GetBalance(iPayment, sqlConn, sqlTrans), 2) & vbNewLine &
                        "Ref: " & iPayment.Reference & vbNewLine &
                        "Source: " & iPayment.PaymentSource.PaymentSource & vbNewLine &
                        "HOT Recharge - your favourite service"
                    }
                    iSMS.Priority.PriorityID = xPriority.Priorities.Normal
                    iSMS.State.StateID = xState.States.Pending
                    xSMSAdapter.Save(iSMS, sqlConn, sqlTrans)

                End If
            Catch ex As Exception
                If Not ex.Message.ToLower().Contains("key") Then
                    ShowEx(ex)
                End If
            End Try
        Next
    End Sub

    Private Function GetBalance(iPayment As xPayment, sqlConn As SqlConnection, sqlTrans As SqlTransaction) As Decimal
        Dim iAccount = xAccountAdapter.SelectRow(iPayment.AccountID, sqlConn, sqlTrans)
        If iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.ZESA Then Return iAccount.ZESABalance
        If iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USD Then Return iAccount.USDBalance
        If iPayment.PaymentType.PaymentTypeID = xPaymentType.PaymentTypes.USDUtility Then Return iAccount.USDUtilityBalance
        Return iAccount.Balance
    End Function

    Private Sub UpdateBankTrx(ByRef iBankTrxList As List(Of xBankTrx), ByRef sqlConn As SqlConnection, ByRef sqlTrans As SqlTransaction)
        For Each iTrx In iBankTrxList
            iTrx.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Success
            xBankTrxAdapter.UpdateState(iTrx, sqlConn, sqlTrans)

        Next
    End Sub

    'Clear transactions grid if bank selection changed
    Private Sub cboBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBank.SelectedIndexChanged
        _BankTransactionList = New List(Of BankTransaction)
        BindTransactions(_BankTransactionList)
    End Sub

    'Validates the value in txtDiscount is a decimal number
    Private Sub txtDiscount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscount.LostFocus
        Dim iDecimal As Decimal
        Try
            iDecimal = CType(txtDiscount.Text, Decimal)
        Catch ex As Exception
            MsgBox("Discount % must be a decimal number!")
            txtDiscount.SelectAll()
            txtDiscount.Focus()
        End Try

    End Sub

    Private Sub btnEcoCash_Click(sender As Object, e As EventArgs) Handles btnEcoCash.Click
        Select Case btnEcoCash.Text
            Case "&View Ecocash Recon"
                btnEcoCash.Text = "&View Ecocash Payments"
                BindTransactions(_BankTransactionUpdateList)
            Case "&View Ecocash Payments"
                btnEcoCash.Text = "&View Ecocash Recon"
                BindTransactions(_BankTransactionList)
        End Select
        cmdImportTransactions.Visible = (btnEcoCash.Text = "&View Ecocash Recon")
        btnUpdateTrx.Visible = Not (btnEcoCash.Text = "&View Ecocash Recon")
    End Sub

    Private Sub btnUpdateTrx_Click(sender As Object, e As EventArgs) Handles btnUpdateTrx.Click
        Dim iBankTrxList As List(Of xBankTrx) = _BankTransactionUpdateList
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction

            UpdateBankTrx(iBankTrxList, sqlConn, sqlTrans)
            InsertPaymentsProcess(iBankTrxList, sqlConn, sqlTrans)

            Try
                sqlTrans.Commit()
                _BankTransactionUpdateList = New List(Of xBankTrx)
                MsgBox("Success", MsgBoxStyle.Information)
                If Not (_BankTransactionList.Count > 0) Then Me.DialogResult = Windows.Forms.DialogResult.OK
            Catch ex As Exception
                sqlTrans.Rollback()
                ShowEx(ex)
            Finally
                sqlConn.Close()
            End Try
            sqlConn.Close()
        End Using
    End Sub

End Class