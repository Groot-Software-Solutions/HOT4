Imports log4net
Imports Microsoft.VisualBasic
Imports HOT5.Common

Public Class Common
    Public Const Conn As String = "data source=econet;initial catalog=HOT4;uid=hotadmin;pwd=3c0n3753rv3r" 'uid=showzim;pwd=D4nc3r;"
    Public Structure xSystemStatus
        Public Status As String
        Public Message As String

    End Structure
    Public Structure xReportItem
        Public ItemType As Integer
        Public TransactionDate As String
        Public Mobile As String
        Public Amount As Decimal
        Public State As String
        Public AccessCode As String
        Public AccessName As String
        Public Reference As String
        Public TranType As String
        Public Source As String                                
    End Structure
    Public Shared Function EmailAddressCheck(ByVal emailAddress As String) As Boolean

        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            EmailAddressCheck = True
        Else
            EmailAddressCheck = False
        End If

    End Function
End Class
