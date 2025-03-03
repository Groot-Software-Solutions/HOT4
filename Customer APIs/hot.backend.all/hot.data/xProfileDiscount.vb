Imports System.Data.SqlClient

Public Class xProfileDiscount

    Private _ProfileDiscountID As Integer
    Public Property ProfileDiscountID() As Integer
        Get
            Return _ProfileDiscountID
        End Get
        Set(ByVal value As Integer)
            _ProfileDiscountID = value
        End Set
    End Property

    Private _ProfileID As Integer
    Public Property ProfileID() As Integer
        Get
            Return _ProfileID
        End Get
        Set(ByVal value As Integer)
            _ProfileID = value
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

    Private _Discount As Decimal
    Public Property Discount() As Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As Decimal)
            _Discount = value
        End Set
    End Property

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _ProfileDiscountID = sqlRdr("ProfileDiscountID")
        _ProfileID = sqlRdr("ProfileID")
        _Brand = New xBrand(sqlRdr)
        _Discount = sqlRdr("Discount")
    End Sub
End Class

Public Class xProfileDiscountAdapter
    Public Shared Function Discount(ByVal ProfileID As Integer, ByVal BrandID As Integer, ByVal sqlConn As SqlConnection) As xProfileDiscount
        Dim iRow As xProfileDiscount = Nothing
        Using sqlCmd As New SqlCommand("xProfileDiscount_Discount", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("ProfileID", ProfileID)
            sqlCmd.Parameters.AddWithValue("BrandID", BrandID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRow = New xProfileDiscount(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
End Class