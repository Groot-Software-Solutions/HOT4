Imports System.Data.SqlClient

Public Class xBank

    Public Enum Banks
        CABS = 1
        AgriBank = 2
        Kingdom = 3
        vPayments = 4
        EcoCash = 5
        EcoMerchant = 6
        CBZ = 7
        OneMoney = 8
        Stanbic = 9
        StanbicZesa = 10
        StewardBank = 11
        StanbicUSD = 12
        CBZUSD = 13
        CABSUSD = 14
        Ecobank = 15
        OneMoneyUSD = 16
        InnBucks = 17
        StewardBankUSD = 18
    End Enum

    Private _BankID As Integer
    Public Property BankID() As Integer
        Get
            Return _BankID
        End Get
        Set(ByVal value As Integer)
            _BankID = value
        End Set
    End Property

    Private _Bank As String
    Public Property Bank() As String
        Get
            Return _Bank
        End Get
        Set(ByVal value As String)
            _Bank = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BankID = sqlRdr("BankID")
        _Bank = sqlRdr("Bank")
    End Sub
    Public Overrides Function ToString() As String
        Return _Bank
    End Function

End Class
Public Class xBankAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xBank)
        Dim iList As New List(Of xBank)
        Using sqlCmd As New SqlCommand("xBank_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xBank(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class