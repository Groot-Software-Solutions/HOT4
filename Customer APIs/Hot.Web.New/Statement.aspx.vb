
Imports System.Data
Imports System.Data.SqlClient

Partial Class Statement
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            Dim AccountId As String = Request.QueryString("AccountID")
            If IsNumeric(AccountId) Then
                'Populating a DataTable from database.
                Dim dt As DataTable = Me.GetData(CInt(AccountId))

                'Building an HTML string.
                Dim html As New StringBuilder()

                'Table start.
                html.Append("<table id ='trans' class='hovered bordered tablesorter'>")
                html.Append("<table id ='trans' class='hovered bordered tablesorter'>")
                html.Append("<thead><tr><th Class='sort' data-sort='rd'>Date</th> ")
                html.Append("<th Class='sort' data-sort='rtl'>Transaction Type</th>")
                html.Append("<th Class='sort' data-sort='rid'>Transaction ID</th>")
                html.Append("<th Class='sort' data-sort='rmo'>Mobile/Reference</th>")
                html.Append("<th Class='sort' data-sort='ramt'>Amount</th>")
                html.Append("<th>Cost</th><th Class='sort' data-sort='rst'>Balance</th>")
                html.Append("</tr></thead><tbody Class='list'>")
                'Building the Data rows.
                For Each row As DataRow In dt.Rows
                    Dim classname = If(row("reference").ToString().Contains("Balance Credited in Erro"), "berror", "payment")
                    Dim classs = If(row("trantype") = "Payment", "class='" & classname & "'", "")
                    Dim tr = "<tr " & classs & " >"
                    html.Append(tr)
                    For Each column As DataColumn In dt.Columns
                        html.Append("<td>")
                        Select Case (column.ColumnName)
                            Case "amount"
                                html.Append(FormatNumber(row(column.ColumnName), 2))
                            Case "cost"
                                html.Append(FormatNumber(row(column.ColumnName), 2))
                            Case "balance"
                                html.Append(FormatNumber(If(IsDBNull(row(column.ColumnName)), 0, row(column.ColumnName)), 2))
                            Case Else
                                html.Append(row(column.ColumnName))
                        End Select

                        html.Append("</td>")
                    Next
                    html.Append("</tr>")
                Next
                html.Append("</tbody><tfoot></tfoot></table>")

                'Append the HTML string to Placeholder.
                stmt.Controls.Add(New Literal() With {
               .Text = html.ToString()
             })
            Else
                stmt.Controls.Add(New Literal() With {
              .Text = "No Data"
            })
            End If

        End If
    End Sub

    Private Function GetData(AccountId As Integer) As DataTable

        Using sqlConn As New SqlConnection(Common.Conn)
            sqlConn.Open()
            ' Dim ds As New List(Of xReportItem_Retail) 
            Using sqlCmd As New SqlCommand("zStatment", sqlConn)
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure
                sqlCmd.Parameters.AddWithValue("AccountID", AccountId)
                Using sda As New SqlDataAdapter()
                    sqlCmd.Connection = sqlConn
                    sda.SelectCommand = sqlCmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
            sqlConn.Close()
        End Using
    End Function



End Class
