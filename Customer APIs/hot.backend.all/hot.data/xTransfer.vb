Imports System.Data.SqlClient

Public Class xTransfer

    Private _TransferID As Long
    Public Property TransferID() As Long
        Get
            Return _TransferID
        End Get
        Set(ByVal value As Long)
            _TransferID = value
        End Set
    End Property

    Private _Channel As xChannel
    Public Property Channel() As xChannel
        Get
            Return _Channel
        End Get
        Set(ByVal value As xChannel)
            _Channel = value
        End Set
    End Property

    Private _PaymentID_From As Long
    Public Property PaymentID_From() As Long
        Get
            Return _PaymentID_From
        End Get
        Set(ByVal value As Long)
            _PaymentID_From = value
        End Set
    End Property

    Private _PaymentID_To As Long
    Public Property PaymentID_To() As Long
        Get
            Return _PaymentID_To
        End Get
        Set(ByVal value As Long)
            _PaymentID_To = value
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

    Private _TransferDate As Date
    Public Property TransferDate() As Date
        Get
            Return _TransferDate
        End Get
        Set(ByVal value As Date)
            _TransferDate = value
        End Set
    End Property

    Private _SMSID As Long
    Public Property SMSID() As Long
        Get
            Return _SMSID
        End Get
        Set(ByVal value As Long)
            _SMSID = value
        End Set
    End Property

    Sub New()
        _Channel = New xChannel
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _TransferID = sqlRdr("TransferID")
        _Channel = New xChannel(sqlRdr)
        _PaymentID_From = sqlRdr("PaymentID_From")
        _PaymentID_To = sqlRdr("PaymentID_To")
        _Amount = sqlRdr("Amount")
        _TransferDate = sqlRdr("TransferDate")
        _SMSID = sqlRdr("SMSID")
    End Sub
End Class

Public Class xTransferAdapter
    Public Shared Sub Save(ByRef iTransfer As xTransfer, ByVal sqlConn As SqlConnection)
        Using sqlCmd As New SqlCommand("xTransfer_Save", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("TransferID", iTransfer.TransferID)
            sqlCmd.Parameters.AddWithValue("ChannelID", iTransfer.Channel.ChannelID)
            sqlCmd.Parameters.AddWithValue("PaymentID_From", iTransfer.PaymentID_From)
            sqlCmd.Parameters.AddWithValue("PaymentID_To", iTransfer.PaymentID_To)
            sqlCmd.Parameters.AddWithValue("Amount", iTransfer.Amount)
            sqlCmd.Parameters.AddWithValue("TransferDate", iTransfer.TransferDate)
            sqlCmd.Parameters.AddWithValue("SMSID", iTransfer.SMSID)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iTransfer.TransferID = sqlRdr("TransferID")
                sqlRdr.Close()
            End Using
        End Using
    End Sub
End Class