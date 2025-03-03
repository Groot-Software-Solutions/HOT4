Imports System.Data.SqlClient
Imports HOT.Data

Public Class frmPinLoader
    Sub New(ByVal PinBatchTypeID As xPinBatchType.PinBatchTypes)
        InitializeComponent()
        Using sqlConn As New SqlConnection(Conn)
            sqlConn.Open()
            cboType.DisplayMember = "PinBatchType"
            cboType.ValueMember = "PinBatchTypeID"
            cboType.DataSource = xPinBatchTypeAdapter.List(sqlConn)
            cboType.SelectedValue = CInt(PinBatchTypeID)
            sqlConn.Close()
        End Using
    End Sub
    Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Try
            Dim f As New OpenFileDialog
            If f.ShowDialog = Windows.Forms.DialogResult.OK Then
                txtFileName.Text = f.FileName
            End If
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub cmdLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoad.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.None
            If cboType.SelectedIndex = -1 Then
                Err.SetError(cboType, "Batch Type must be selected")
                cboType.Focus()
                Exit Sub
            End If
            If Not IO.File.Exists(txtFileName.Text) Then
                Err.SetError(txtFileName, "File does not exist")
                txtFileName.Focus()
                Exit Sub
            End If

            Select Case CType(cboType.SelectedValue, xPinBatchType.PinBatchTypes)
                Case xPinBatchType.PinBatchTypes.Econet
                    ImportEconet(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.Telecel
                    ImportTelecel(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.NetOne
                    ImportNetOne(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.eTopUp
                    ImportETopUp(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.TPS
                    ImportTPS(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.Africom
                    ImportAfricom(IO.File.ReadAllLines(txtFileName.Text))
                Case xPinBatchType.PinBatchTypes.Migrated
                    Err.SetError(cboType, "Migrated pins may not be loaded")
                    Exit Sub
            End Select
            'added to rename loaded files so confusion doesn't occur.
            IO.File.Move(txtFileName.Text, txtFileName.Text & ".loaded")
        Catch ex As Exception
            ShowEx(ex)
        End Try
    End Sub
    Private Sub ImportEconet(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch                    
                    iBatch.PinBatch = Body(0).Split("|")(2)
                    iBatch.PinBatchID = 0
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    pb.Maximum = Body.Length()
                    pb.Step = 1
                    pb.Value = 0

                    For x As Integer = 0 To Body.Length() - 1
                        pb.PerformStep()
                        Me.Text = "Pin Loader: " & x + 1 & "/" & Body.Length()
                        Application.DoEvents()
                        Dim iPin As New xPin
                        iPin.PinID = 0
                        'Brand
                        iPin.Brand = New xBrand
                        Select Case Body(x).Split("|")(5)
                            Case "01", "14"
                                iPin.Brand.BrandID = xBrand.Brands.EconetBB
                            Case "55", "56", "57", "42"
                                iPin.Brand.BrandID = xBrand.Brands.Econet_USD
                            Case Else
                                Throw New Exception("Unknown Brand")
                        End Select

                        iPin.Pin = Body(x).Split("|")(0)
                        iPin.PinRef = Body(x).Split("|")(2) & "|" & Body(x).Split("|")(1)
                        iPin.PinBatch = iBatch
                        iPin.PinExpiry = Date.ParseExact((Body(x).Split("|")(4)), "dd/MM/yyyy", Nothing)
                        iPin.PinState = New xPinState
                        iPin.PinState.PinStateID = xPinState.PinStates.Available
                        iPin.PinValue = Body(x).Split("|")(3)
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                    Next
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ImportNetOne(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch
                    iBatch.PinBatch = Body(0).Split(",")(5)
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    pb.Maximum = Body.Length()
                    pb.Step = 1
                    pb.Value = 0

                    For x As Integer = 0 To Body.Length() - 1
                        pb.PerformStep()
                        Me.Text = "Pin Loader: " & x & "/" & Body.Length()
                        Application.DoEvents()
                        Dim iPin As New xPin

                        'Brand
                        iPin.Brand = New xBrand
                        iPin.Brand.BrandID = xBrand.Brands.Netone_USD
                        Dim data = Body(x).Split(",")
                        iPin.Pin = data(4)
                        iPin.PinRef = $"{data(8)}|{data(5)}"
                        iPin.PinBatch = iBatch
                        iPin.PinExpiry = Date.ParseExact(data(7), "dd-MMM-yyyy", Globalization.CultureInfo.InvariantCulture) 'CDate(data(4).Replace("-", "/"))
                        iPin.PinState = New xPinState
                        iPin.PinState.PinStateID = xPinState.PinStates.Available
                        iPin.PinValue = data(3)
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                    Next
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)

            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ImportTelecel(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch                    
                    iBatch.PinBatch = Body(0).Split(",")(0)
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    pb.Maximum = Body.Length()
                    pb.Step = 1
                    pb.Value = 0

                    For x As Integer = 1 To Body.Length() - 1
                        pb.PerformStep()
                        Me.Text = "Pin Loader: " & x + 1 & "/" & Body.Length()
                        Application.DoEvents()
                        Dim iPin As New xPin                        
                        'Brand
                        iPin.Brand = New xBrand
                        iPin.Brand.BrandID = xBrand.Brands.Juice

                        iPin.Pin = Body(x).Split(",")(1)
                        iPin.PinRef = iBatch.PinBatch & "|" & Body(x).Split(",")(0)
                        iPin.PinBatch = iBatch
                        iPin.PinExpiry = Date.ParseExact((Body(0).Split(",")(4).Substring(0, 2) & "/" & Body(0).Split(",")(4).Substring(2, 2) & "/" & Body(0).Split(",")(4).Substring(4, 4)), "dd/MM/yyyy", New Globalization.CultureInfo("en-US"))
                        iPin.PinState = New xPinState
                        iPin.PinState.PinStateID = xPinState.PinStates.Available
                        iPin.PinValue = CDec(Body(0).Split(",")(5).Replace(">", ""))
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                    Next
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ImportETopUp(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch                    
                    iBatch.PinBatch = Body(0).Split(",")(8)
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    pb.Maximum = Body.Length()
                    pb.Step = 1
                    pb.Value = 0

                    For x As Integer = 1 To Body.Length() - 1
                        pb.PerformStep()
                        Me.Text = "Pin Loader: " & x + 1 & "/" & Body.Length()
                        Application.DoEvents()
                        Dim iPin As New xPin                        
                        'Brand
                        iPin.Brand = New xBrand
                        If Body(x).Split(",")(2).Substring(0, 2) = "BP" Then
                            iPin.Brand.BrandID = xBrand.Brands.Econet_USD
                        ElseIf Body(x).Split(",")(2).Substring(0, 1) = "B" Then
                            If Body(x).Split(",")(2).Substring(Body(x).Split(",")(2).Length - 3, 3) = "USD" Then
                                iPin.Brand.BrandID = xBrand.Brands.Econet_USD
                            Else
                                iPin.Brand.BrandID = xBrand.Brands.Text
                            End If
                        ElseIf Body(x).Split(",")(2).Substring(0, 1) = "L" Then
                            If Body(x).Split(",")(2).Substring(Body(x).Split(",")(2).Length - 3, 3) = "USD" Then
                                iPin.Brand.BrandID = xBrand.Brands.Econet_USD
                            Else
                                iPin.Brand.BrandID = xBrand.Brands.Text
                            End If
                        End If
                        If iPin.Brand.BrandID = 0 Then
                            Throw New Exception("Unknown Brand")
                        End If
                        iPin.Pin = Body(x).Split(",")(4)
                        iPin.PinRef = iBatch.PinBatch & "|" & Body(x).Split(",")(5)
                        iPin.PinBatch = iBatch
                        iPin.PinExpiry = CDate(Body(x).Split(",")(7))
                        iPin.PinState = New xPinState
                        iPin.PinState.PinStateID = xPinState.PinStates.Available
                        iPin.PinValue = CDec(Body(x).Split(",")(3))
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                    Next
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ImportTPS(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch
                    iBatch.PinBatch = Body(0).Split("|")(2)
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    pb.Maximum = Body.Length()
                    pb.Step = 1
                    pb.Value = 0

                    For x As Integer = 1 To Body.Length() - 1
                        pb.PerformStep()
                        Me.Text = "Pin Loader: " & x + 1 & "/" & Body.Length()
                        Application.DoEvents()
                        Dim iPin As New xPin
                        'Brand
                        iPin.Brand = New xBrand
                        Select Case Body(x).Split("|")(5)
                            Case "01", "23", "14"
                                iPin.Brand.BrandID = xBrand.Brands.Econet_USD
                            Case "25", "20"
                                iPin.Brand.BrandID = xBrand.Brands.Text
                        End Select
                        If iPin.Brand.BrandID = 0 Then
                            Throw New Exception("Unknown Brand")
                        End If
                        iPin.Pin = Body(x).Split("|")(0)
                        iPin.PinRef = iBatch.PinBatch & "|" & Body(x).Split("|")(1)
                        iPin.PinBatch = iBatch
                        iPin.PinExpiry = CDate(Body(x).Split("|")(4))                        
                        iPin.PinState.PinStateID = xPinState.PinStates.Available
                        iPin.PinValue = CDec(Body(x).Split("|")(3))
                        xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                    Next
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ImportAfricom(ByVal Body() As String)
        Me.Cursor = Cursors.WaitCursor
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim sqlTrans As SqlTransaction = sqlConn.BeginTransaction
                Try
                    'Batch
                    Dim iBatch As New xPinBatch
                    iBatch.PinBatch = Body(0).Split(":")(1)
                    iBatch.PinBatchType = cboType.SelectedItem
                    xPinBatchAdapter.Insert(iBatch, sqlConn, sqlTrans)

                    'Batches have one kind of PIN, so set general PIN qualities here
                    Dim iPinGeneral As New xPin
                    iPinGeneral.Brand.BrandID = xBrand.Brands.Africom
                    iPinGeneral.PinValue = Body(3).Split(":")(1) / 100
                    iPinGeneral.PinExpiry = CDate(Body(5).Split(":")(1).Substring(6, 2) & "/" & Body(5).Split(":")(1).Substring(4, 2) & "/" & Body(5).Split(":")(1).Substring(0, 4))



                    pb.Maximum = Body.Length() - 40
                    pb.Step = 1
                    pb.Value = 0
                    If Body(38) = "[BEGIN]" And Body(Body.Length - 1) = "[END]" Then
                        For x As Integer = 39 To Body.Length() - 2
                            pb.PerformStep()
                            Me.Text = "Pin Loader: " & pb.Value & "/" & pb.Maximum
                            Application.DoEvents()
                            Dim iPin As New xPin
                            'Brand
                            iPin.Brand = New xBrand
                            iPin.Brand.BrandID = xBrand.Brands.Africom
                            iPin.Pin = Body(x).Split(" ")(1)
                            iPin.PinRef = Body(x).Split(" ")(0)
                            iPin.PinBatch = iBatch
                            iPin.PinExpiry = iPinGeneral.PinExpiry
                            iPin.PinState = New xPinState
                            iPin.PinState.PinStateID = xPinState.PinStates.Available
                            iPin.PinValue = iPinGeneral.PinValue
                            xPinAdapter.Insert(iPin, sqlConn, sqlTrans)
                        Next
                    End If
                    sqlTrans.Commit()
                Catch ex As Exception
                    sqlTrans.Rollback()
                    Throw ex
                Finally
                    sqlConn.Close()
                End Try
            End Using
            MsgBox("Pins loaded successfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
End Class