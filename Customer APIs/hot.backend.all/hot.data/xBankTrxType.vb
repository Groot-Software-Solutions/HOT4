Imports System.Data.SqlClient

Public Class xBankTrxType

    Public Enum BankTrxTypes
        NotApplicable = 0
        CashDeposit = 1
        CashdepositReversal = 2
        eBankingTrfr = 3
        JnlDebit = 4
        TransferCredit = 5
        RtgsCharge = 6
        SalaryCredit = 7
        RTGSReceipt = 8
        ChequePayment = 9
        RTGSPayment = 10
        ChequeDeposit = 11
        EmailCharge = 12
        JnlDebitReversal = 13
        TransferCreditReversal = 14
        vPaymentsPending = 15
        vPaymentsConfirmed = 16
        EcoCashPending = 17
        CashWithdrawal = 18
        OneMoney = 19
        ZIPIT = 20
        MobileBillPayment = 21
        InnBucks = 22
        MiscCredit = 98
        MiscDebit = 99
    End Enum

    Private _BankTrxTypeID As Integer
    Public Property BankTrxTypeID() As Integer
        Get
            Return _BankTrxTypeID
        End Get
        Set(ByVal value As Integer)
            _BankTrxTypeID = value
        End Set
    End Property

    Private _BankTrxType As String
    Public Property BankTrxType() As String
        Get
            Return _BankTrxType
        End Get
        Set(ByVal value As String)
            _BankTrxType = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankTrxTypeID = sqlRdr("BankTrxTypeID")
        _BankTrxType = sqlRdr("BankTrxType")
    End Sub
    Public Overrides Function ToString() As String
        Return _BankTrxType
    End Function

End Class
Public Class xBankTrxTypeAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBankTrxType)
        Dim iList As New List(Of xBankTrxType)
        Using sqlCmd As New SqlCommand("xBankTrxType_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBankTrxType(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class
