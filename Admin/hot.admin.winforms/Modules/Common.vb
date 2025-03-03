Imports System.Data.SqlClient
Imports Hot.Data

Module Common

    Public Const ConnString As String = "Data Source=HOT5;Initial Catalog=HOT4;uid=%UID%;pwd=%PWD%;MultipleActiveResultSets=true"
    Public gUser As xUser
    Public DBRole As xSecurityDBRole

    Public Enum xHotUIFunction As Integer
        Reset_Password = 1
        Edit_Access = 2
        Add_Access = 3
        Debit_Mobile_Account = 4
        Delete_Access = 5
        Save_Payment = 6
        Load_Statement = 7
        Send_Bulk_Messages = 8
        Can_Reverse_HOTTransaction = 9
        Admin_Console = 10
    End Enum


    Public Sub Log(LogModule As String, LogObject As String, LogMethod As String, LogDescription As String, TypeID As String, IDNumber As Integer)
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    Dim iLog As New xLog
                    iLog.LogModule = LogModule
                    iLog.LogObject = LogObject
                    iLog.LogMethod = LogMethod
                    iLog.LogDescription = LogDescription
                    iLog.IDNumber = IDNumber
                    iLog.IdType = TypeID
                    xLog_Data.Save(iLog, sqlConn, sqlTrans)
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
        Catch ex As Exception
            '#Catasrophic Failure
            MsgBox(ex.Message)
        End Try
    End Sub

#Region " Tab Control "
    Public Const TABKEY_ACCOUNTSEARCH = "Account Search"
    Public Const TABKEY_ACCOUNT = "Account"
    Public Const TABKEY_PINS = "Pins"
    Public Const TABKEY_BANK = "Banking"
    Public Const TABKEY_SMPP = "Smpp Activity"
    Public Const TABKEY_SYSTEM = "System"
    Public Const TABKEY_RECHARGES = "Recharges"
    Public Const TABKEY_ADMIN = "Administration"

    Public Function AddTab(ByVal Key As String, ByVal Title As String) As TabPage
        frmConsole.tcMain.TabPages.Add(Key, Title & "             ")
        Dim tb As TabPage = frmConsole.tcMain.TabPages(Key)
        Return tb
    End Function

#End Region

    Public Function DBRoleAllowsFunction(_function As xHotUIFunction) As Boolean
        Select Case (_function)
            Case xHotUIFunction.Reset_Password
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Edit_Access
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Add_Access
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Delete_Access
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Debit_Mobile_Account
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Save_Payment
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Load_Statement
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Send_Bulk_Messages
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Supervisor Then Return True
            Case xHotUIFunction.Can_Reverse_HOTTransaction
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Clerk Then Return True
            Case xHotUIFunction.Admin_Console
                If DBRole.RoleID <= xSecurityDBRole.xSecurityDBRoleTypes.Administrator Then Return True
        End Select
        Return False
    End Function

    Public Function Conn() As String
        Return ConnString.Replace("%UID%", gUser.UserName).Replace("%PWD%", gUser.Password)
    End Function
    Public Sub ShowEx(ByVal ex As Exception)
        MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Exception")
    End Sub
End Module

Public Class xUser

    Private _UserName As String
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Private _Password As String
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public Property Roles As New List(Of xSecurityDBRole)

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        UserName = sqlRdr("name")

    End Sub
End Class

Public Class xUserAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xUser)
        Dim iList As New List(Of xUser)
        Using sqlCmd As New SqlCommand("xDBUsers_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xUser(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    Public Shared Function Save(iUser As xUser, ByVal sqlConn As SqlConnection) As Boolean
        Dim sqltrans As SqlTransaction = sqlConn.BeginTransaction
        Try

            Dim sqlCmd As New SqlCommand("CREATE LOGIN """ + iUser.UserName + """ WITH PASSWORD = '" + iUser.Password + "'", sqlConn, sqltrans)
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.ExecuteNonQuery()
            sqlCmd.CommandText = "CREATE USER """ + iUser.UserName + """ FOR LOGIN """ + iUser.UserName + """"
            sqlCmd.ExecuteNonQuery()
            For Each role As xSecurityDBRole In iUser.Roles
                sqlCmd.CommandText = "EXEC sp_addrolemember '" + role.RoleName + "','" + iUser.UserName + "'"
                sqlCmd.ExecuteNonQuery()
            Next

            sqltrans.Commit()
        Catch ex As Exception
            ShowEx(ex)
            sqltrans.Rollback()
            Return False
        End Try

        Return True
    End Function

End Class

Public Class xSecurityDBRole
    Public Property RoleID As xSecurityDBRoleTypes
    Public Property RoleName As String
    Public ReadOnly Property Name As String
        Get
            Return RoleName.Replace("dbr_", "")
        End Get
    End Property

    Public Property PrincipalID As Integer
    Public Property Permissions As New List(Of xSecurityDBRolePermission)

    Enum xSecurityDBRoleTypes As Integer
        System = 1
        Administrator = 2
        Supervisor = 3
        Clerk = 4
        Limited = 90
        Unknown = 99

    End Enum
    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        RoleName = sqlRdr("name")
        RoleID = GetRoleID(RoleName)
        PrincipalID = sqlRdr("principal_id")
    End Sub
    Function GetRoleID(_name As String) As xSecurityDBRoleTypes
        Select Case LCase(_name)
            Case "sysadmin"
                Return xSecurityDBRoleTypes.System
            Case "dbr_admin"
                Return xSecurityDBRoleTypes.Administrator
            Case "dbr_supervisor"
                Return xSecurityDBRoleTypes.Supervisor
            Case "dbr_clerk"
                Return xSecurityDBRoleTypes.Clerk
            Case "dbr_limited"
                Return xSecurityDBRoleTypes.Limited
        End Select
        Return xSecurityDBRoleTypes.Unknown
    End Function

End Class

Public Class xSecurityDBRoleAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xSecurityDBRole)
        Dim iList As New List(Of xSecurityDBRole)
        Using sqlCmd As New SqlCommand("xDBRoles_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xSecurityDBRole(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function List(User As String, ByVal sqlConn As SqlConnection) As List(Of xSecurityDBRole)
        Dim iList As New List(Of xSecurityDBRole)
        Using sqlCmd As New SqlCommand("xDBRoles_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("@user", User)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xSecurityDBRole(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function List_All(ByVal sqlConn As SqlConnection) As List(Of xSecurityDBRole)
        Dim iList As New List(Of xSecurityDBRole)
        Using sqlCmd As New SqlCommand("xDBRoles_List_All", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xSecurityDBRole(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function
    Public Shared Function GetHighestPriv(ByVal sqlConn As SqlConnection) As xSecurityDBRole
        Dim dbroles As List(Of xSecurityDBRole) = xSecurityDBRoleAdapter.List(sqlConn)
        Dim result As xSecurityDBRole.xSecurityDBRoleTypes = xSecurityDBRole.xSecurityDBRoleTypes.Unknown
        For Each role As xSecurityDBRole In dbroles
            If result > role.RoleID Then result = role.RoleID
        Next
        Return New xSecurityDBRole With {.RoleID = result}
    End Function
    Public Shared Function Save(iDBRole As xSecurityDBRole, ByVal sqlConn As SqlConnection) As Boolean
        Dim sqltrans As SqlTransaction = sqlConn.BeginTransaction
        Try

            Dim sqlCmd As New SqlCommand("CREATE ROLE dbr_" + iDBRole.RoleName + " AUTHORIZATION db_owner", sqlConn, sqltrans)
            sqlCmd.CommandType = CommandType.Text
            sqlCmd.ExecuteNonQuery()
            For Each permission As xSecurityDBRolePermission In iDBRole.Permissions
                sqlCmd.CommandText = "GRANT EXECUTE ON OBJECT::HOT4." + permission.ObjectName + " TO " + iDBRole.RoleName
                sqlCmd.ExecuteNonQuery()
            Next

            sqltrans.Commit()
        Catch ex As Exception
            ShowEx(ex)
            sqltrans.Rollback()
            Return False
        End Try

        Return True
    End Function
End Class

Public Class xSecurityDBRolePermission
    Public Property ObjectName As String
    Public Property ObjectType As String
    Public Property PermissionName As String
    Public Property PermissionType As String
    Public Property StateDescription As String
    Public Property Active As Boolean = True
    Sub New()

    End Sub
    Sub New(dbItem As xSecurityDBItem)
        ObjectName = dbItem.ObjectName
        PermissionName = "EXECUTE"
        Active = False
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        ObjectName = sqlRdr("objectname")
        ObjectType = sqlRdr("objecttype")
        PermissionName = sqlRdr("permission_name")
        PermissionType = sqlRdr("permissionType")
        StateDescription = sqlRdr("state_desc")
    End Sub

End Class

Public Class xSecurityDBRolePermissionAdapter

#Region "  Permsion Sets "

#Region " Base Permissions"
    Public Shared BaseConfiguration As New List(Of xSecurityDBRolePermission) From {
New xSecurityDBRolePermission With {.ObjectName = "xAccess_Admin_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_List"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_ListDeleted"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_SelectCode"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_SelectLogin"},
New xSecurityDBRolePermission With {.ObjectName = "xAccess_SelectLogin2"},
New xSecurityDBRolePermission With {.ObjectName = "xAccessWeb_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xAccount_Search"},
New xSecurityDBRolePermission With {.ObjectName = "xAccount_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xAddress_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xBank_List"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_FindDuplicate"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_GetFromBankRef"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_GetFromTrxID"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_GetFromvPayment"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_Insert"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_List"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_ListPendingEcoCash"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_Pending_EcoCash"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrxBatch_GetCurrentBatch"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrxBatch_List"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrxState_List"},
New xSecurityDBRolePermission With {.ObjectName = "xBankTrxType_List"},
New xSecurityDBRolePermission With {.ObjectName = "xBankvPayment_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xBranch_List_EasyLink"},
New xSecurityDBRolePermission With {.ObjectName = "xBrand_Identify"},
New xSecurityDBRolePermission With {.ObjectName = "xBrand_List"},
New xSecurityDBRolePermission With {.ObjectName = "xChannel_List"},
New xSecurityDBRolePermission With {.ObjectName = "xErrorChecks"},
New xSecurityDBRolePermission With {.ObjectName = "xErrorLog_List"},
New xSecurityDBRolePermission With {.ObjectName = "xHotType_Identify"},
New xSecurityDBRolePermission With {.ObjectName = "xNetwork_Identify"},
New xSecurityDBRolePermission With {.ObjectName = "xPayment_List"},
New xSecurityDBRolePermission With {.ObjectName = "xPaymentSource_List"},
New xSecurityDBRolePermission With {.ObjectName = "xPaymentType_List"},
New xSecurityDBRolePermission With {.ObjectName = "xPin_List"},
New xSecurityDBRolePermission With {.ObjectName = "xPin_Loaded"},
New xSecurityDBRolePermission With {.ObjectName = "xPin_Recharge"},
New xSecurityDBRolePermission With {.ObjectName = "xPin_Stock"},
New xSecurityDBRolePermission With {.ObjectName = "xPinBatch_List"},
New xSecurityDBRolePermission With {.ObjectName = "xPinBatchType_List"},
New xSecurityDBRolePermission With {.ObjectName = "xProcessState_List"},
New xSecurityDBRolePermission With {.ObjectName = "xProfile_List"},
New xSecurityDBRolePermission With {.ObjectName = "xProfileDiscount_Discount"},
New xSecurityDBRolePermission With {.ObjectName = "xProfileDiscount_List"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_AggregatorSelect"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Find"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Find2"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Pending"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Pending_Africom"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Pending_Econet"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Pending_NetOne"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Pending_Other"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Select2"},
New xSecurityDBRolePermission With {.ObjectName = "xRecharge_Web_Duplicate"},
New xSecurityDBRolePermission With {.ObjectName = "xRechargePrepaid_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xRechargeSMS_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xSMS_Inbox"},
New xSecurityDBRolePermission With {.ObjectName = "xSMS_List"},
New xSecurityDBRolePermission With {.ObjectName = "xSMS_ListOut"},
New xSecurityDBRolePermission With {.ObjectName = "xSMS_Outbox"},
New xSecurityDBRolePermission With {.ObjectName = "xSMS_Search"},
New xSecurityDBRolePermission With {.ObjectName = "xState_List"},
New xSecurityDBRolePermission With {.ObjectName = "xSubscriber_List"},
New xSecurityDBRolePermission With {.ObjectName = "xSubscriber_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xTemplate_Select"},
New xSecurityDBRolePermission With {.ObjectName = "xTransactions_Date"},
New xSecurityDBRolePermission With {.ObjectName = "xTransactions_Date_Access"},
New xSecurityDBRolePermission With {.ObjectName = "xWebRequest_Select"}
  }
#End Region

    Public Shared LoadStatmentConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_Save"},
        New xSecurityDBRolePermission With {.ObjectName = "xBankTrxBatch_Insert"}
    }
    Public Shared ProcessStatmentConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_UpdatePaymentID"},
        New xSecurityDBRolePermission With {.ObjectName = "xBankTrx_UpdateState"},
        New xSecurityDBRolePermission With {.ObjectName = "xBankvPayment_Update"},
        New xSecurityDBRolePermission With {.ObjectName = "xPayment_Save"},
        New xSecurityDBRolePermission With {.ObjectName = "xTransfer_Save"}
    }
    Public Shared MakePaymentConfiguration As New List(Of xSecurityDBRolePermission) From {
         New xSecurityDBRolePermission With {.ObjectName = "xPayment_Save"}
    }
    Public Shared BulkSendConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xSMS_BulkSend"},
        New xSecurityDBRolePermission With {.ObjectName = "xSMS_BulkSmsSend"},
        New xSecurityDBRolePermission With {.ObjectName = "xSMS_EmailAggregators"},
        New xSecurityDBRolePermission With {.ObjectName = "xSMS_EmailCorporates"}
    }
    Public Shared AdminConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xConfig_Select"},
        New xSecurityDBRolePermission With {.ObjectName = "xErrorLog_Save"},
        New xSecurityDBRolePermission With {.ObjectName = "xGraph_RechargeState"},
        New xSecurityDBRolePermission With {.ObjectName = "xPWD"}
    }
    Public Shared AddAccessUserConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_Save"},
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_Save2"}
    }
    Public Shared ModifyAccessConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_Delete"},
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_PasswordChange"},
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_PasswordChange2"},
        New xSecurityDBRolePermission With {.ObjectName = "xAccess_UnDelete"},
        New xSecurityDBRolePermission With {.ObjectName = "xAccessWeb_Save"}
    }
    Public Shared UpdateAccountConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xAccount_Save"},
        New xSecurityDBRolePermission With {.ObjectName = "xAddress_Save"}
    }
    Public Shared LoadPinsConfiguration As New List(Of xSecurityDBRolePermission) From {
        New xSecurityDBRolePermission With {.ObjectName = "xPin_Insert"},
        New xSecurityDBRolePermission With {.ObjectName = "xPinBatch_Insert"}
    }

#End Region


    Public Shared Function List(Rolename As String, ByVal sqlConn As SqlConnection) As List(Of xSecurityDBRolePermission)
        Dim iList As New List(Of xSecurityDBRolePermission)
        Using sqlCmd As New SqlCommand("xDBRoles_List_Permissions", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlCmd.Parameters.AddWithValue("@RoleName", Rolename)
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xSecurityDBRolePermission(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

    Public Shared Function List_All(ByVal sqlConn As SqlConnection) As List(Of xSecurityDBRolePermission)
        Dim iList As New List(Of xSecurityDBRolePermission)

        For Each DBItem As xSecurityDBItem In xSecurityDBItemAdapter.List(sqlConn)
            iList.Add(New xSecurityDBRolePermission(DBItem))
        Next
        Return iList
    End Function
End Class

Public Class xSecurityDBItem
    Public Property ObjectID As Long
    Public Property ObjectName As String
    Public Property ObjectType As String

    Sub New()

    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        ObjectID = sqlRdr("id")
        ObjectName = sqlRdr("name")
        ObjectType = sqlRdr("xtype")


    End Sub

End Class

Public Class xSecurityDBItemAdapter
    Public Shared Function List(ByVal sqlConn As SqlConnection) As List(Of xSecurityDBItem)
        Dim iList As New List(Of xSecurityDBItem)
        Using sqlCmd As New SqlCommand("xDBItem_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure

            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read()
                    iList.Add(New xSecurityDBItem(sqlRdr))
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return iList
    End Function

End Class


Public Class xRechargeAccess
    Inherits xRecharge
    Public Property AccessCode As String
    Public Property AccessChannel As String
    Public Overloads Property IsSuccessFul As Boolean
    Sub New()

    End Sub
    Sub New(iRech As xRecharge, iAccess As xAccess)
        AccessID = iRech.AccessID
        Amount = iRech.Amount
        Brand = iRech.Brand
        Discount = iRech.Discount
        ErrorCode = iRech.ErrorCode
        InsertDate = iRech.InsertDate
        IsSuccessFul = iRech.IsSuccessFul
        Mobile = iRech.Mobile
        RechargeDate = iRech.RechargeDate
        RechargeID = iRech.RechargeID
        State = iRech.State
        AccessChannel = iAccess.Channel.Channel
        AccessCode = iAccess.AccessCode
    End Sub
End Class
