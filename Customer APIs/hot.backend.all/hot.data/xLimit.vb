Imports System.Data.SqlClient

Public Class xLimit
    Public Property LimitRemaining As Integer
    Public Property RemainingDailyLimit As Decimal
    Public Property RemainingMonthlyLimit As Decimal
    Public Property DailyLimit As Decimal
    Public Property MonthlyLimit As Decimal
    Public Property SalesToday As Decimal
    Public Property SalesMonthly As Decimal
    Public Property LimitTypeId As Integer

    Sub New(ByVal sqlRdr As SqlDataReader)
        LimitRemaining = sqlRdr("LimitRemaining")
        RemainingDailyLimit = sqlRdr("RemainingDailyLimit")
        RemainingMonthlyLimit = sqlRdr("RemainingMonthlyLimit")
        DailyLimit = sqlRdr("DailyLimit")
        MonthlyLimit = sqlRdr("MonthlyLimit")
        SalesMonthly = sqlRdr("SalesMonthly")
        SalesToday = sqlRdr("SalesToday")
        LimitTypeId = sqlRdr("LimitTypeId")
    End Sub

End Class

Public Class xLimitAdapter

    Public Shared Function GetLimit(ByVal NetworkId As Integer, ByVal AccountId As Integer, ByVal sqlConn As SqlConnection, Optional ByVal sqlTrans As SqlTransaction = Nothing) As xLimit
        Dim iLimit As xLimit = Nothing
        Using sqlCmd As New SqlCommand("xLimits_Select", sqlConn, sqlTrans)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("NetworkId", NetworkId)
            sqlCmd.Parameters.AddWithValue("AccountId", AccountId)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                sqlRdr.Read()
                iLimit = New xLimit(sqlRdr)
                sqlRdr.Close()
            End Using
        End Using
        Return iLimit
    End Function
End Class