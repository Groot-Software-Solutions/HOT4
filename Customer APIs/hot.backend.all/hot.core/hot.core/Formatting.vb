Public Class Formatting
    Public Shared Function FormatAmount(amount As Decimal, Optional formatForDealerCost As Boolean = False, Optional decimalPlaces As Integer = 2) As String
        If formatForDealerCost AND amount < 1 Then 
            Dim fourDecs = FormatNumber(amount, 4)
            Return fourDecs.TrimEnd(New String({"0", "."}))
        End If
        return FormatNumber(amount, decimalPlaces)
    End Function
End Class