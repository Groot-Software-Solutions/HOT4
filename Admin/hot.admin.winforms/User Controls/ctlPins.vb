Imports System.Data.SqlClient
Imports HOT.Data

Public Class ctlPins

    Sub New()
        InitializeComponent()
        Bind()
    End Sub
    Private Sub tvw_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvw.NodeMouseClick
        Try
            tvw.SelectedNode = e.Node
            If TypeOf tvw.SelectedNode.Tag Is xPinBatch Then
                BindCountOfPins(tvw.SelectedNode)
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub Bind()
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                For Each iPinBatchType In xPinBatchTypeAdapter.List(sqlConn)
                    Dim nodeType As New TreeNode(iPinBatchType.PinBatchType)
                    nodeType.Tag = iPinBatchType
                    nodeType.Name = iPinBatchType.PinBatchTypeID
                    nodeType.ContextMenuStrip = mnuType
                    nodeType.ImageKey = "Book_angleHS.png"
                    nodeType.SelectedImageKey = "Book_openHS.png"
                    BindBatch(nodeType, sqlConn)
                    tvw.Nodes.Add(nodeType)
                Next
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub BindBatch(ByVal nodeType As TreeNode, ByVal sqlConn As SqlConnection)
        For Each iPinBatch In xPinBatchAdapter.List(nodeType.Name, sqlConn)
            Dim nodeBatch As New TreeNode(Format(iPinBatch.BatchDate, "dd MMM yyyy HH:mm") & " -> " & iPinBatch.PinBatch)
            nodeBatch.Tag = iPinBatch
            nodeBatch.Name = iPinBatch.PinBatchID
            nodeBatch.ImageKey = "DocumentHS.png"
            nodeBatch.SelectedImageKey = "PageUpHS.png"
            nodeType.Nodes.Add(nodeBatch)
        Next
    End Sub
    Private Sub BindPins(ByVal nodeBatch As TreeNode)
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource
                bs.DataSource = xPinAdapter.List(nodeBatch.Name, sqlConn)
                dg.DataSource = bs
                sqlConn.Close()
            End Using
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub BindCountOfPins(ByVal nodeBatch As TreeNode)
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim bs As New BindingSource

                Dim iList As New List(Of xPinStock)
                Using sqlCmd As New SqlCommand("xPin_Loaded", sqlConn)
                    sqlCmd.CommandType = CommandType.StoredProcedure
                    sqlCmd.Parameters.AddWithValue("PinBatchID", nodeBatch.Name)
                    Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                        While sqlRdr.Read
                            iList.Add(New xPinStock(sqlRdr))
                        End While
                        sqlRdr.Close()
                    End Using
                End Using


                bs.DataSource = iList
                dg.DataSource = bs
                sqlConn.Close()
            End Using

        Catch ex As Exception
            ShowEx(ex)

        End Try
    End Sub


    Private Sub mnuBatchLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBatchLoad.Click
        Try
            Dim f As New frmPinLoader(tvw.SelectedNode.Name)
            If f.ShowDialog = DialogResult.OK Then
                Using sqlConn As New SqlConnection(Conn)
                    sqlConn.Open()
                    BindBatch(tvw.SelectedNode, sqlConn)
                    sqlConn.Close()
                End Using                
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
End Class
