Imports System.Data.SqlClient

Public Class xPaymentSource

    Public Enum PaymentSources As Integer
        Office = 1
        Agribank = 2
        Kingdom = 3
        Credit = 4
        Freebie = 5
        HOTDealer = 6
        Commission = 7
        MCExecutive = 8
        CABS = 9
        Stanbic = 10
        vPayment = 11
        EcoCash = 12
        CBZ = 13
        OneMoney = 14
        Direct = 15
        ZesaStanbic = 16
        USDStanbic = 17
        USDCash = 18
        OneMoneyMerchant = 19
        StewardBank = 20

    End Enum

    Private _PaymentSourceID As Integer
    Public Property PaymentSourceID() As Integer
        Get
            Return _PaymentSourceID
        End Get
        Set(ByVal value As Integer)
            _PaymentSourceID = value
        End Set
    End Property

    Private _PaymentSource As String
    Public Property PaymentSource() As String
        Get
            Return _PaymentSource
        End Get
        Set(ByVal value As String)
            _PaymentSource = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PaymentSourceID = sqlRdr("PaymentSourceID")
        _PaymentSource = sqlRdr("PaymentSource")
    End Sub
    Public Overrides Function ToString() As String
        Return _PaymentSource
    End Function
End Class

Public Class xPaymentSourceAdapter
    'all under here added by KMR 16 dec 2012 from office web server

    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xPaymentSource)
        Dim iList As New List(Of xPaymentSource)
        Using sqlCmd As New SqlCommand("xPaymentSource_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xPaymentSource(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class
