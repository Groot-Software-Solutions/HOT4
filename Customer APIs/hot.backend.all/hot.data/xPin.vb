Imports System.Data.SqlClient

Public Class xPin

    Private _PinID As Long
    Public Property PinID() As Long
        Get
            Return _PinID
        End Get
        Set(ByVal value As Long)
            _PinID = value
        End Set
    End Property

    Private _PinBatch As xPinBatch
    Public Property PinBatch() As xPinBatch
        Get
            Return _PinBatch
        End Get
        Set(ByVal value As xPinBatch)
            _PinBatch = value
        End Set
    End Property

    Private _PinState As xPinState
    Public Property PinState() As xPinState
        Get
            Return _PinState
        End Get
        Set(ByVal value As xPinState)
            _PinState = value
        End Set
    End Property

    Private _Brand As xBrand
    Public Property Brand() As xBrand
        Get
            Return _Brand
        End Get
        Set(ByVal value As xBrand)
            _Brand = value
        End Set
    End Property

    Private _Pin As String
    Public Property Pin() As String
        Get
            Return _Pin
        End Get
        Set(ByVal value As String)
            _Pin = value
        End Set
    End Property

    Private _PinRef As String
    Public Property PinRef() As String
        Get
            Return _PinRef
        End Get
        Set(ByVal value As String)
            _PinRef = value
        End Set
    End Property

    Private _PinValue As Decimal
    Public Property PinValue() As Decimal
        Get
            Return _PinValue
        End Get
        Set(ByVal value As Decimal)
            _PinValue = value
        End Set
    End Property

    Private _PinExpiry As Date
    Public Property PinExpiry() As Date
        Get
            Return _PinExpiry
        End Get
        Set(ByVal value As Date)
            _PinExpiry = value
        End Set
    End Property

    Sub New()
        _PinBatch = New xPinBatch
        _PinState = New xPinState
        _Brand = New xBrand
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _PinID = sqlRdr("PinID")
        _PinBatch = New xPinBatch(sqlRdr)
        _PinState = New xPinState(sqlRdr)
        _Brand = New xBrand(sqlRdr)
        _Pin = sqlRdr("Pin")
        _PinRef = sqlRdr("PinRef")
        _PinValue = sqlRdr("PinValue")
        _PinExpiry = sqlRdr("PinExpiry")        
    End Sub
End Class
Public Class xPinAdapter
    Public Shared Function Recharge(ByVal Amount As Decimal, ByVal BrandID As Integer, ByVal RechargeID As Long, ByVal sqlConn As SqlConnection) As List(Of xPin)
        Dim iList As New List(Of xPin)
        Using sqlCmd As New SqlCommand("xPin_Recharge", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("Amount", Amount)
            sqlCmd.Parameters.AddWithValue("BrandID", BrandID)
            sqlCmd.Parameters.AddWithValue("RechargeID", RechargeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPin(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function List(ByVal PinBatchID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As List(Of xPin)
        Dim iList As New List(Of xPin)
        Using sqlCmd As New SqlCommand("xPin_List", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("PinBatchID", PinBatchID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPin(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Sub Insert(ByRef iPin As xPin, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xPin_Insert", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("PinBatchID", iPin.PinBatch.PinBatchID)
            sqlCmd.Parameters.AddWithValue("PinStateID", iPin.PinState.PinStateID)
            sqlCmd.Parameters.AddWithValue("BrandID", iPin.Brand.BrandID)
            sqlCmd.Parameters.AddWithValue("Pin", iPin.Pin)
            sqlCmd.Parameters.AddWithValue("PinRef", iPin.PinRef)
            sqlCmd.Parameters.AddWithValue("PinValue", iPin.PinValue)
            sqlCmd.Parameters.AddWithValue("PinExpiry", iPin.PinExpiry)
            iPin.PinID = sqlCmd.ExecuteScalar()
        End Using
    End Sub
End Class



Public Class xPinStock

    Private _BrandId As Integer
    Public Property BrandId() As Integer
        Get
            Return _BrandId
        End Get
        Set(ByVal value As Integer)
            _BrandId = value
        End Set
    End Property

    Private _BrandName As String
    Public Property BrandName() As String
        Get
            Return _BrandName
        End Get
        Set(ByVal value As String)
            _BrandName = value
        End Set
    End Property

    Private _PinValue As Decimal
    Public Property PinValue() As Decimal
        Get
            Return _PinValue
        End Get
        Set(ByVal value As Decimal)
            _PinValue = value
        End Set
    End Property

    Private _Stock As Integer
    Public Property Stock() As Integer
        Get
            Return _Stock
        End Get
        Set(ByVal value As Integer)
            _Stock = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BrandName = sqlRdr("BrandName")
        _PinValue = sqlRdr("PinValue")
        _Stock = sqlRdr("Stock")
        _BrandId = sqlRdr("BrandId")
    End Sub

End Class
Public Class xPinStockAdapter
    Public Shared Function Stock(ByVal sqlConn As SqlConnection) As List(Of xPinStock)
        Dim iList As New List(Of xPinStock)
        Using sqlCmd As New SqlCommand("xPin_Stock", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xPinStock(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

End Class