Imports System.Data.SqlClient

Public Class xSubscriber

    Private _SubscriberID As Long
    Public Property SubscriberID() As Long
        Get
            Return _SubscriberID
        End Get
        Set(ByVal value As Long)
            _SubscriberID = value
        End Set
    End Property

    Private _AccountID As Long
    Public Property AccountID() As Long
        Get
            Return _AccountID
        End Get
        Set(ByVal value As Long)
            _AccountID = value
        End Set
    End Property

    Private _SubscriberName As String
    Public Property SubscriberName() As String
        Get
            Return _SubscriberName
        End Get
        Set(ByVal value As String)
            _SubscriberName = value
        End Set
    End Property

    Private _SubscriberMobile As String
    Public Property SubscriberMobile() As String
        Get
            Return _SubscriberMobile
        End Get
        Set(ByVal value As String)
            _SubscriberMobile = value
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

    Private _Active As Boolean
    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Sub New()
        _Brand = New xBrand
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _SubscriberID = sqlRdr("SubscriberID")
        _AccountID = sqlRdr("AccountID")
        _SubscriberName = sqlRdr("SubscriberName")
        _SubscriberMobile = sqlRdr("SubscriberMobile")
        _Brand = New xBrand(sqlRdr)
        _Active = sqlRdr("Active")
    End Sub
End Class
Public Class xSubscriberAdapter
    Public Shared Sub Save(ByRef iSubscriber As xSubscriber, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xSubscriber_Save", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SubscriberID", iSubscriber.SubscriberID)
            sqlCmd.Parameters.AddWithValue("AccountID", iSubscriber.AccountID)
            sqlCmd.Parameters.AddWithValue("SubscriberName", iSubscriber.SubscriberName)
            sqlCmd.Parameters.AddWithValue("SubscriberMobile", iSubscriber.SubscriberMobile)
            sqlCmd.Parameters.AddWithValue("BrandID", iSubscriber.Brand.BrandID)
            sqlCmd.Parameters.AddWithValue("Active", iSubscriber.Active)            
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iSubscriber.SubscriberID = sqlRdr("SubscriberID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub

    Public Shared Sub Delete(ByRef iSubscriber As xSubscriber, ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Using sqlCmd As New SqlCommand("xSubscriber_Delete", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SubscriberID", iSubscriber.SubscriberID)
            sqlCmd.ExecuteNonQuery()
        End Using
    End Sub
    Public Shared Function List(ByVal AccountID As Long, ByVal sqlConn As SqlConnection) As List(Of xSubscriber)
        Dim iList As New List(Of xSubscriber)
        Using sqlCmd As New SqlCommand("xSubscriber_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("AccountID", AccountID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    iList.Add(New xSubscriber(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function SelectRow(ByVal SubscriberID As Long, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xSubscriber
        Dim iRow As xSubscriber = Nothing
        Using sqlCmd As New SqlCommand("xSubscriber_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("SubscriberID", SubscriberID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iRow = New xSubscriber(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iRow
    End Function
End Class