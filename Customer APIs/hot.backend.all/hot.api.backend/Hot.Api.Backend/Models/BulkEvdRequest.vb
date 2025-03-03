
Imports System.ComponentModel.DataAnnotations
Namespace Models

    Public Class BulkEvdRequest

        <Required>
        Public BrandId As Byte
        <Required>
        Public Denomination As Decimal
        <Required>
        Public Quantity As Integer

    End Class
End Namespace
