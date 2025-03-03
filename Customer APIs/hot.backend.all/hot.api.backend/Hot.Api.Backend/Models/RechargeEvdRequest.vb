Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class RechargeEvdRequest
        Inherits BulkEvdRequest
        <Required>
        Property TargetNumber As String

    End Class
End Namespace
