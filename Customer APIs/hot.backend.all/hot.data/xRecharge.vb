
Imports System.Data.SqlClient
Imports System.Linq

Public Class xRecharge

    Private _RechargeID As Long
    Public Property RechargeID() As Long
        Get
            Return _RechargeID
        End Get
        Set(ByVal value As Long)
            _RechargeID = value
        End Set
    End Property

    Private _State As xState
    Public Property State() As xState
        Get
            Return _State
        End Get
        Set(ByVal value As xState)
            _State = value
        End Set
    End Property

    Private _AccessID As Long
    Public Property AccessID() As Long
        Get
            Return _AccessID
        End Get
        Set(ByVal value As Long)
            _AccessID = value
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

    Private _Discount As Decimal
    Public Property Discount() As Decimal
        Get
            Return _Discount
        End Get
        Set(ByVal value As Decimal)
            _Discount = value
        End Set
    End Property

    Private _Mobile As String
    Public Property Mobile() As String
        Get
            Return _Mobile
        End Get
        Set(ByVal value As String)
            _Mobile = value
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

    Private _RechargeDate As Date
    Public Property RechargeDate() As Date
        Get
            Return _RechargeDate
        End Get
        Set(ByVal value As Date)
            _RechargeDate = value
        End Set
    End Property

    Public ReadOnly Property IsSuccessFul As Boolean
        Get 
            Return State.StateID = xState.States.Success
        End Get
    End Property

    Sub New()
        _State = New xState
        _Brand = New xBrand
    End Sub
    Sub New(sqlRdr As SqlDataReader, sqlConn as SqlConnection)
        _RechargeID = sqlRdr("RechargeID")
        Dim stateId As Integer = sqlRdr("StateID")
        _State = New xState(stateId)
        _AccessID = sqlRdr("AccessID")
        _Amount = sqlRdr("Amount")
        _Discount = sqlRdr("Discount")
        _Mobile = sqlRdr("Mobile")
        _Brand = xBrandAdapter.GetBrand(sqlRdr("BrandID"), sqlConn)
        _RechargeDate = sqlRdr("RechargeDate") 
        InsertDate = sqlRdr("InsertDate")        
    End Sub

    Public Property InsertDate As Date

    Public Property ErrorCode As ErrorCodes

    Public Property ProductCode As String

    Public Property Quantity As Long
    Public Property Denomination As Decimal


End Class

Public Class xRechargeAdapter
    Public Shared Sub Save(ByRef iRecharge As xRecharge, ByVal sqlConn As SqlConnection, Optional ByVal SMSID As Integer = Nothing)
        Using sqlCmd As New SqlCommand("xRecharge_Save", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", iRecharge.RechargeID)
            sqlCmd.Parameters.AddWithValue("StateID", iRecharge.State.StateID)
            sqlCmd.Parameters.AddWithValue("AccessID", iRecharge.AccessID)
            sqlCmd.Parameters.AddWithValue("Amount", iRecharge.Amount)
            sqlCmd.Parameters.AddWithValue("Discount", iRecharge.Discount)
            sqlCmd.Parameters.AddWithValue("Mobile", iRecharge.Mobile)
            sqlCmd.Parameters.AddWithValue("BrandID", iRecharge.Brand.BrandID)
            sqlCmd.Parameters.AddWithValue("SMSID", SMSID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRecharge.RechargeID = sqlRdr("RechargeID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub
    Public Shared Function PendingEconet(ByVal sqlConn As SqlConnection) As List(Of xRecharge)
        Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Pending_Econet2", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader

                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    <Obsolete>
    Public Shared Function PendingNetOne(sqlConn As SqlConnection) As List(Of xRecharge)
        Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Pending_NetOne2", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    <Obsolete()>
    Public Shared Function PendingOther(ByVal sqlConn As SqlConnection) As List(Of xRecharge)
        Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Pending_Other2", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function SelectSMS(ByVal RechargeID As Long, ByVal sqlConn As SqlConnection) As xSMS
        Dim iRow As xSMS = Nothing
        Using sqlCmd As New SqlCommand("xRechargeSMS_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", RechargeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = New xSMS(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function

    Public Shared Function SelectRechargePrepaid(ByVal RechargeID As Long, ByVal sqlConn As SqlConnection) As xRechargePrepaid

        Dim iRow As xRechargePrepaid = Nothing
        Using sqlCmd As New SqlCommand("xRechargePrepaid_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", RechargeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRow = New xRechargePrepaid(sqlRdr)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
    Public Shared Function SelectRechargePin(ByVal RechargeID As Long, ByVal sqlConn As SqlConnection) As List(Of xPin)

        Dim iList As New List(Of xPin)
        Using sqlCmd As New SqlCommand("xRechargePIN_Select", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", RechargeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xPin(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function SelectRecharge(ByVal RechargeID As Long, ByVal sqlConn As SqlConnection) As xRecharge

        Dim iRecharge As New xRecharge
        Using sqlCmd As New SqlCommand("xRecharge_Select2", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("RechargeID", RechargeID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                If sqlRdr.Read() Then
                    iRecharge = New xRecharge(sqlRdr, sqlConn)
                End If
                sqlRdr.Close()
            End Using
        End Using
        Return iRecharge
    End Function

    Public Shared Function Pending(brandIds As IList(Of Integer), sqlConn As SqlConnection) As IList
         Dim iList As New List(Of xRecharge)
        Using sqlCmd As New SqlCommand("xRecharge_Pending", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            
            Dim brandList As New DataTable
            brandList.Columns.Add("BrandID", GetType(Integer))

            For Each brandId As Integer In brandIds
                 Dim row As DataRow = brandList.NewRow()
                row("BrandID") = brandId
                brandList.Rows.Add(row)
            Next  
            sqlCmd.Parameters.Add(New SqlParameter("brandList", brandList))         

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xRecharge(sqlRdr, sqlConn))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
End Class