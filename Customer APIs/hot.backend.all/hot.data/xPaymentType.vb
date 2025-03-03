Imports System.Data.SqlClient

Public Class xPaymentType

    Public Enum PaymentTypes As Integer
        Cash = 1
        Reversal = 2
        CreditAdvanced = 3
        CreditRepayment = 4
        Freebie = 5
        HOTTransfer = 6
        BankManual = 7
        BankAuto = 8
        CommissionPaid = 9
        Depositfees = 10
        zServiceFees = 11
        zBalanceBF = 12
        ShareholdersAllowance = 13
        Writeoff = 14
        Direct = 15
        ZESA = 16
        USD = 17
    End Enum

    Private _PaymentTypeID As Integer
    Public Property PaymentTypeID() As Integer
        Get
            Return _PaymentTypeID
        End Get
        Set(ByVal value As Integer)
            _PaymentTypeID = value
        End Set
    End Property

    Private _PaymentType As String
    Public Property PaymentType() As String
        Get
            Return _PaymentType
        End Get
        Set(ByVal value As String)
            _PaymentType = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PaymentTypeID = sqlRdr("PaymentTypeID")
        _PaymentType = sqlRdr("PaymentType")
    End Sub
    Public Overrides Function ToString() As String
        Return _PaymentType
    End Function
End Class
Public Class xPaymentTypeAdapter
    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xPaymentType)
        Dim iList As New List(Of xPaymentType)
        Using sqlCmd As New SqlCommand("xPaymentType_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xPaymentType(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class