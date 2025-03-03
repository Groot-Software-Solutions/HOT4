Imports System.Data.SqlClient

Public Class xRechargePrepaid

    Private _RechargeID As Long
    Public Property RechargeID() As Long
        Get
            Return _RechargeID
        End Get
        Set(ByVal value As Long)
            _RechargeID = value
        End Set
    End Property

    Private _DebitCredit As Boolean
    Public Property DebitCredit() As Boolean
        Get
            Return _DebitCredit
        End Get
        Set(ByVal value As Boolean)
            _DebitCredit = value
        End Set
    End Property

    Private _ReturnCode As String
    Public Property ReturnCode() As String
        Get
            Return _ReturnCode
        End Get
        Set(ByVal value As String)
            _ReturnCode = value
        End Set
    End Property

    Private _Narrative As String
    Public Property Narrative() As String
        Get
            Return _Narrative
        End Get
        Set(ByVal value As String)
            _Narrative = value
        End Set
    End Property

    Private _InitialBalance As Decimal
    Public Property InitialBalance() As Decimal
        Get
            Return _InitialBalance
        End Get
        Set(ByVal value As Decimal)
            _InitialBalance = value
        End Set
    End Property

    Private _FinalBalance As Decimal
    Public Property FinalBalance() As Decimal
        Get
            Return _FinalBalance
        End Get
        Set(ByVal value As Decimal)
            _FinalBalance = value
        End Set
    End Property

    Private _Reference As String
    Public Property Reference() As String
        Get
            Return _Reference
        End Get
        Set(ByVal value As String)
            _Reference = value
        End Set
    End Property

    Public Property InitialWallet As Decimal

    Public Property FinalWallet As Decimal

    Public Property Window As Date?
    Public Property Data As String

    Public Property SMS As String


    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _RechargeID = sqlRdr("RechargeID")
        _DebitCredit = sqlRdr("DebitCredit")
        _ReturnCode = sqlRdr("ReturnCode")
        _Narrative = sqlRdr("Narrative")
        _InitialBalance = sqlRdr("InitialBalance")
        _FinalBalance = sqlRdr("FinalBalance")
        _Reference = sqlRdr("Reference")  

        InitialWallet = sqlRdr("InitialWallet")
        FinalWallet = sqlRdr("FinalWallet")
        Window = sqlRdr("Window")
        Data = sqlRdr("Data")
        SMS = sqlRdr("SMS")
    End Sub

End Class

Public Class xRechargePrepaidAdapter
    Public Shared Sub Save(ByRef iRechargePrepaid As xRechargePrepaid, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xRechargePrepaid_Save", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", iRechargePrepaid.RechargeID)
            sqlCmd.Parameters.AddWithValue("DebitCredit", iRechargePrepaid.DebitCredit)
            sqlCmd.Parameters.AddWithValue("ReturnCode", iRechargePrepaid.ReturnCode)
            sqlCmd.Parameters.AddWithValue("Narrative", iRechargePrepaid.Narrative)
            sqlCmd.Parameters.AddWithValue("InitialBalance", iRechargePrepaid.InitialBalance)
            sqlCmd.Parameters.AddWithValue("FinalBalance", iRechargePrepaid.FinalBalance)
            sqlCmd.Parameters.AddWithValue("Reference", iRechargePrepaid.Reference)

            sqlCmd.Parameters.AddWithValue("InitialWallet", iRechargePrepaid.InitialWallet)
            sqlCmd.Parameters.AddWithValue("FinalWallet", iRechargePrepaid.FinalWallet)
            If iRechargePrepaid.Window.HasValue And iRechargePrepaid.Window >= Date.Now() Then
                sqlCmd.Parameters.AddWithValue("Window", iRechargePrepaid.Window)
            Else 
                sqlCmd.Parameters.AddWithValue("Window", DBNull.Value)
            End If
            sqlCmd.Parameters.AddWithValue("Data", iRechargePrepaid.Data)
            sqlCmd.Parameters.AddWithValue("SMS", iRechargePrepaid.SMS)
                    
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    
End Class