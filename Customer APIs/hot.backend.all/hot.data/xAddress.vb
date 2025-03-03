Imports System.Data.SqlClient

Public Class xAddress
    Private _AccountID As Long
    Public Property AccountID() As Long
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Long)
            _AccountID = value
        End Set
    End Property
    Private _Address1 As String
    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property
    Private _Address2 As String
    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property
    Private _City As String
    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = value
        End Set
    End Property
    Private _ContactName As String
    Public Property ContactName() As String
        Get
            Return _ContactName
        End Get
        Set(ByVal value As String)
            _ContactName = value
        End Set
    End Property
    Private _ContactNumber As String
    Public Property ContactNumber() As String
        Get
            Return _ContactNumber
        End Get
        Set(ByVal value As String)
            _ContactNumber = value
        End Set
    End Property
    Private _VatNumber As String
    Public Property VatNumber() As String
        Get
            Return _VatNumber
        End Get
        Set(ByVal value As String)
            _VatNumber = value
        End Set
    End Property
    Private _Latitude As Decimal
    Public Property Latitude() As Decimal
        Get
            Return _Latitude
        End Get
        Set(ByVal value As Decimal)
            _Latitude = value
        End Set
    End Property
    Private _Longitude As Decimal
    Public Property Longitude() As Decimal
        Get
            Return _Longitude
        End Get
        Set(ByVal value As Decimal)
            _Longitude = value
        End Set
    End Property
    Private _SageID As Long
    Public Property SageID() As Long
        Get
            Return _SageID
        End Get
        Set(ByVal value As Long)
            _SageID = value
        End Set
    End Property
    Private _InvoiceFreq As Integer
    Public Property InvoiceFreq() As Integer
        Get
            Return _InvoiceFreq
        End Get
        Set(ByVal value As Integer)
            _InvoiceFreq = value
        End Set
    End Property
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _AccountID = sqlRdr("AccountID")
        _Address1 = sqlRdr("Address1")
        _Address2 = sqlRdr("Address2")
        _City = sqlRdr("City")
        _ContactName = sqlRdr("ContactName")
        _ContactNumber = sqlRdr("ContactNumber")
        _VatNumber = sqlRdr("VatNumber")
        _Latitude = sqlRdr("Latitude")
        _Longitude = sqlRdr("Longitude")
        _SageID = sqlRdr("SageID")
        _InvoiceFreq = sqlRdr("InvoiceFreq")
        _
    End Sub
End Class

Public Class xAddressAdapter
    Public Shared Sub Save(ByRef iAddress As xAddress, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xAddress_Save", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", iAddress.AccountID)
            sqlCmd.Parameters.AddWithValue("Address1", iAddress.Address1)
            sqlCmd.Parameters.AddWithValue("Address2", iAddress.Address2)
            sqlCmd.Parameters.AddWithValue("City", iAddress.City)
            sqlCmd.Parameters.AddWithValue("ContactName", iAddress.ContactName)
            sqlCmd.Parameters.AddWithValue("ContactNumber", iAddress.ContactNumber)
            sqlCmd.Parameters.AddWithValue("VatNumber", iAddress.VatNumber)
            sqlCmd.Parameters.AddWithValue("Latitude", iAddress.Latitude)
            sqlCmd.Parameters.AddWithValue("Longitude", iAddress.Longitude)
            sqlCmd.Parameters.AddWithValue("SageID", iAddress.SageID)
            sqlCmd.Parameters.AddWithValue("InvoiceFreq", iAddress.InvoiceFreq)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function SelectRow(ByVal AccountID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xAddress
        Dim iRow As xAddress = Nothing
        Using sqlCmd As New SqlCommand("xAddress_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRow = New xAddress(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
End Class
