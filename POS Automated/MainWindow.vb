Imports System.Drawing.Printing

Public Class MainWindow
    Dim ReciptImage As Bitmap
    Public Property ItemsTAA As Object

    Private Sub CheckUser()
        Dim PSWWin As New PasswordPicker
        PSWWin.ShowDialog()
        If PSWWin.DialogResult <> Windows.Forms.DialogResult.OK Then
            End
        End If
        Dim Psw As String = PSWWin.TextBox1.Text
        Dim TA As New POS.POSDataSetTableAdapters.LoginTableAdapter
        Dim TB = TA.GetDataByPass("password")
        If Psw <> TB.Rows(0).Item(1).ToString Then
            MsgBox("Invalid Password", MsgBoxStyle.Critical)
            CheckUser()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'InventoryDataSet1.Inventory' table. You can move, or remove it, as needed.
        Me.InventoryTableAdapter.Fill(Me.InventoryDataSet.Inventory)
        Me.DoubleBuffered = True

        Try
            CheckUser()
            ItemsTA.Fill(Me.POSDataSet.Items)
            TextBox6.Text = My.Settings.printer
            DataGridView3.Rows.Add("1029293", "Chips", "", 4, 5, 4 * 5)
            DataGridView3.Rows.Add("1067291", "Chocolate", "", 10, 3, 10 * 3)
            DataGridView3.Rows.Add("1648391", "Cofee", "", 5, 2, 2 * 5)
            DataGridView3.Rows.Add("1529184", "Biscuts", "", 15, 2, 2 * 15)
            con.Open()
            cmd.CommandText = "Select * from Printer;"
            cmd.ExecuteNonQuery()
            con.Close()
            With Adapter
                .SelectCommand = cmd
                .Fill(Table)

            End With
            TextBox5.Text = Table.Rows(0).Item(1).ToString
            TextBox8.Text = Table.Rows(0).Item(2).ToString
            TextBox7.Text = Table.Rows(0).Item(3).ToString
            PictureBox1.Image = DrawRecipt(DataGridView3.Rows, 737, DateString, 123, TextBox5.Text, TextBox8.Text, TextBox7.Text)
            TextBox1.Focus()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyData = Keys.Enter Then
            Button1.PerformClick()
        End If
    End Sub


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim TB = ItemsTA.GetDataByBarcode(TextBox1.Text)
        If TB.Rows.Count = 0 Then
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox1.Focus()
            Button1.Enabled = False
            Exit Sub
        End If
        Button1.Enabled = True
        Dim R As POS.POSDataSet.ItemsRow = TB.Rows(0)
        TextBox2.Text = R.ItemName
        TextBox3.Text = R.SellPrice
        Button1.Tag = R
    End Sub

    Private Sub ChangePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangePasswordToolStripMenuItem.Click
        Dim CP As New ChangePassword
        CP.ShowDialog()
    End Sub

    Private Sub AddItemsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddItemsToolStripMenuItem.Click
        Dim AI As New AddItem
        AI.ShowDialog()
        ItemsTA.Fill(POSDataSet.Items)
    End Sub

    Private Sub EditItemsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditItemsToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Exit Sub
        End If
        Dim Barcode = DataGridView1.SelectedRows(0).Cells(0).Value
        Dim EditWin As New EditItem
        EditWin.FillInfo(Barcode)
        If EditWin.ShowDialog = Windows.Forms.DialogResult.OK Then
            ItemsTA.Fill(Me.POSDataSet.Items)
        End If
    End Sub

    Private Sub RemoveItemsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveItemsToolStripMenuItem.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Please select an item to remove", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim Barcode = DataGridView1.SelectedRows(0).Cells(0).Value
        ItemsTA.DeleteItms(Barcode)
        ItemsTA.Fill(Me.POSDataSet.Items)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim R As POS.POSDataSet.ItemsRow = Button1.Tag
        Dim ItemLoc As Integer = -1
        Dim I As Integer
        For I = 0 To DataGridView2.Rows.Count - 1
            If R.Barcode = DataGridView2.Rows(I).Cells(0).Value Then
                ItemLoc = I
                Exit For
            End If
        Next
        If ItemLoc = -1 Then
            DataGridView2.Rows.Add(R.Barcode, R.ItemName, R.SellPrice, R.SellPrice, 1, R.SellPrice)
        Else
            Dim Count As Long = DataGridView2.Rows(ItemLoc).Cells(4).Value
            Count += 1
            Dim NewPrice As Decimal = R.SellPrice * Count
            DataGridView2.Rows(ItemLoc).Cells(4).Value = Count
            DataGridView2.Rows(ItemLoc).Cells(5).Value = NewPrice
        End If
        TextBox1.Text = ""
        TextBox1.Focus()
        Dim sum As Decimal = 0
        For I = 0 To DataGridView2.Rows.Count - 1
            sum += DataGridView2.Rows(I).Cells(5).Value
        Next
        TextBox4.Text = sum
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If DataGridView2.Rows.Count = 0 AndAlso DataGridView2.SelectedCells.Count = 0 Then
                MsgBox("Please select an item to remove", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            Dim var As Double = DataGridView2.SelectedRows(0).Cells(5).Value
            TextBox4.Text = TextBox4.Text - var
            DataGridView2.Rows.Remove(DataGridView2.SelectedRows(0))
            TextBox1.Focus()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If DataGridView2.Rows.Count <= 0 Then
                MsgBox("Please add some items to print", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            Dim con As New OleDb.OleDbConnection(My.Settings.POSConnectionString)
            con.Open()
            Dim sql As String = "Insert into Recipts (ReciptDate,ReciptTotal) Values(:0,:1)"
            Dim cmd As New OleDb.OleDbCommand
            Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
            cmd.Parameters.AddWithValue(":0", todaysdate)
            cmd.Parameters.AddWithValue(":1", TextBox4.Text)
            With cmd
                .Connection = con
                .CommandText = sql
            End With
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            Dim cmd1 As New OleDb.OleDbCommand
            sql = "Select max(ReciptID) as MAXID from Recipts"
            With cmd1
                .CommandText = sql
                .Connection = con
            End With
            Dim ReciptID As Long = cmd1.ExecuteScalar
            cmd1.Dispose()
            Dim i As Integer
            For i = 0 To DataGridView2.Rows.Count - 1
                Dim Barcode As String = DataGridView2.Rows(i).Cells(0).Value
                Dim BuyPrice As String = DataGridView2.Rows(i).Cells(2).Value
                Dim SellPrice As String = DataGridView2.Rows(i).Cells(3).Value
                Dim ItemCount As Integer = DataGridView2.Rows(i).Cells(4).Value
                Dim cmd2 As New OleDb.OleDbCommand
                sql = "Insert into ReciptDetails values(:0,:1,:2,:3,:4)"
                With cmd2
                    .CommandText = sql
                    .Connection = con
                End With
                cmd2.Parameters.AddWithValue(":0", ReciptID)
                cmd2.Parameters.AddWithValue(":1", Barcode)
                cmd2.Parameters.AddWithValue(":2", ItemCount)
                cmd2.Parameters.AddWithValue(":3", BuyPrice)
                cmd2.Parameters.AddWithValue(":4", SellPrice)
                cmd2.ExecuteNonQuery()
                cmd2.Dispose()
            Next
            con.Close()
            con.Dispose()
            If Not IsNothing(TextBox6.Text) Then
                ReciptImage = DrawRecipt(DataGridView2.Rows, ReciptID, Format(Now.Date, "dd-mm-yyyy"), TextBox4.Text, TextBox5.Text, TextBox8.Text, TextBox7.Text)
                If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    PrintDoc.PrinterSettings = PrintDialog1.PrinterSettings
                    PrintDoc.Print()
                    DataGridView2.Rows.Clear()
                    TextBox4.Clear()
                End If
            ElseIf PictureBox1.Image Is Nothing Then
                MsgBox("Can't Print receipt please check the settings", MsgBoxStyle.Critical)
            Else
                MsgBox("You did not setup the printer", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If (ComboBox1.Text = "Total Profit Of All Time") Then
                Dim con As New OleDb.OleDbConnection(My.Settings.POSConnectionString)
                Dim cmd As New OleDb.OleDbCommand("", con)
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Profit Of All Time"
                head.Visible = True
                profit.Visible = True
            ElseIf (ComboBox1.Text = "Total Profit Today") Then
                cmd.Parameters.AddWithValue(":0", Date.Now.ToString("dddd"))
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Profit Today"
                head.Visible = True
                profit.Visible = True
            ElseIf (ComboBox1.Text = "Total Profit Yesterday") Then
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", Date.Now.AddDays(-1))
                cmd.Parameters.AddWithValue(":0", todaysdate)
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Profit Yesterday"
                head.Visible = True
                profit.Visible = True
            ElseIf (ComboBox1.Text = "Total Weekly Profit") Then
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", Date.Now.AddDays(-7))
                cmd.Parameters.AddWithValue(":0", todaysdate)
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Weekly Profit "
                head.Visible = True
                profit.Visible = True
            ElseIf (ComboBox1.Text = "Total Monthly Profit") Then
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", Date.Now.ToString("MMMM"))
                cmd.Parameters.AddWithValue(":0", todaysdate)
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Monthly Profit "
                head.Visible = True
                profit.Visible = True
            ElseIf (ComboBox1.Text = "Total Annual Profit") Then
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", Date.Now.Year)
                cmd.Parameters.AddWithValue(":0", todaysdate)
                con.Open()
                cmd.CommandText = "Select SUM((ItemSellPrice)*ItemCount) from ReciptDetails"
                profit.Text = cmd.ExecuteScalar & " Pesos"
                con.Close()
                head.Text = "Total Annual Profit"
                head.Visible = True
                profit.Visible = True
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub head_Click(sender As Object, e As EventArgs) Handles head.Click

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs)

    End Sub


    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Panel2.Refresh()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
    Public Function DrawRecipt(ByVal Rows As DataGridViewRowCollection, ReciptNo As String, ReciptDate As String, ReciptTotal As Decimal, UnitWidth As Integer, UnitHeight As Integer, Fontize As Integer) As Bitmap

        Dim ReciptWidth As Integer = 13 * UnitWidth
        Dim ReciptDetailsHeight As Integer = Rows.Count * UnitHeight
        Dim ReciptHeight As Integer = 6 * UnitWidth + ReciptDetailsHeight
        Dim BMP As New Bitmap(ReciptWidth + 1, ReciptHeight)
        Dim GR As Graphics = Graphics.FromImage(BMP)
        ' GR.FillRectangle(Brushes.White, 0, 0, ReciptWidth, ReciptHeight)
        GR.Clear(Color.White)
        Dim LNHeaderYStart = 3 * UnitHeight
        Dim LNDetailsStart = LNHeaderYStart + UnitHeight
        GR.DrawRectangle(Pens.Black, UnitWidth * 0, LNHeaderYStart, UnitWidth, UnitHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 1, LNHeaderYStart, UnitWidth * 5, UnitHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 6, LNHeaderYStart, UnitWidth * 2, UnitHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 8, LNHeaderYStart, UnitWidth * 2, UnitHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 10, LNHeaderYStart, UnitWidth * 3, UnitHeight)

        GR.DrawRectangle(Pens.Black, UnitWidth * 0, LNDetailsStart, UnitWidth * 1, ReciptDetailsHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 1, LNDetailsStart, UnitWidth * 5, ReciptDetailsHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 6, LNDetailsStart, UnitWidth * 2, ReciptDetailsHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 8, LNDetailsStart, UnitWidth * 2, ReciptDetailsHeight)
        GR.DrawRectangle(Pens.Black, UnitWidth * 10, LNDetailsStart, UnitWidth * 3, ReciptDetailsHeight)

        Dim FNT As New Font("Times", Fontize, FontStyle.Bold)

        GR.DrawString("No", FNT, Brushes.Black, UnitWidth * 0, LNHeaderYStart)
        GR.DrawString("Item", FNT, Brushes.Black, UnitWidth * 1, LNHeaderYStart)
        GR.DrawString("Price", FNT, Brushes.Black, UnitWidth * 6, LNHeaderYStart)
        GR.DrawString("Count", FNT, Brushes.Black, UnitWidth * 8, LNHeaderYStart)
        GR.DrawString("Sum", FNT, Brushes.Black, UnitWidth * 10, LNHeaderYStart)

        Dim I As Integer
        For I = 0 To Rows.Count - 1
            Dim YLOC = UnitHeight * I + LNDetailsStart
            GR.DrawString(I, FNT, Brushes.Black, UnitWidth * 0, YLOC)

            GR.DrawString(Rows(I).Cells(1).Value, FNT, Brushes.Black, UnitWidth * 1, YLOC)
            GR.DrawString(Rows(I).Cells(3).Value, FNT, Brushes.Black, UnitWidth * 6, YLOC)
            GR.DrawString(Rows(I).Cells(4).Value, FNT, Brushes.Black, UnitWidth * 8, YLOC)
            GR.DrawString(Rows(I).Cells(5).Value, FNT, Brushes.Black, UnitWidth * 10, YLOC)

        Next
        GR.DrawString("Total:" & ReciptTotal, FNT, Brushes.Black, 0, LNDetailsStart + ReciptDetailsHeight)

        GR.DrawString("Recipt No:" & ReciptNo, FNT, Brushes.Black, 0, 0)
        GR.DrawString("Recipt Date:" & ReciptDate, FNT, Brushes.Black, 0, UnitHeight)

        Return BMP
    End Function


    Private Sub PrintDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDoc.PrintPage
        e.Graphics.DrawImage(ReciptImage, 0, 0, ReciptImage.Width, ReciptImage.Height)
        e.HasMorePages = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
        Try
            TextBox6.Text = PrintDialog1.PrinterSettings.PrinterName
            My.Settings.printer = TextBox6.Text
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        DrawPreview()
        Try
            If PictureBox1.Image IsNot Nothing Then
                con.Open()
                cmd.CommandText = "Update Printer Set UnitWidth='" & TextBox5.Text & "',UnitHeight='" & TextBox8.Text & "' ,FontSize='" & TextBox7.Text & "' Where Sr=1 "
                cmd.ExecuteNonQuery()
                con.Close()
            End If
        Catch ex As Exception
            PictureBox1.Image = Nothing
        End Try
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        DrawPreview()
        Try
            If PictureBox1.Image IsNot Nothing Then
                con.Open()
                cmd.CommandText = "Update Printer Set UnitWidth='" & TextBox5.Text & "',UnitHeight='" & TextBox8.Text & "' ,FontSize='" & TextBox7.Text & "'Where Sr=1 "
                cmd.ExecuteNonQuery()
                con.Close()
            End If
        Catch ex As Exception
            PictureBox1.Image = Nothing
        End Try
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        DrawPreview()
        Try
            If PictureBox1.Image IsNot Nothing Then
                con.Open()
                cmd.CommandText = "Update Printer Set UnitWidth='" & TextBox5.Text & "',UnitHeight='" & TextBox8.Text & "' ,FontSize='" & TextBox7.Text & "' Where Sr=1"
                cmd.ExecuteNonQuery()
                con.Close()
            End If
        Catch ex As Exception
            PictureBox1.Image = Nothing
        End Try
    End Sub

    Private Sub DataGridView3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellContentClick

    End Sub
    Public Sub DrawPreview()
        If Not IsNumeric(TextBox5.Text) Then
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        Dim L As Double = Long.Parse(TextBox5.Text)
        If Math.Truncate(L) <> L Then
            MsgBox("You Should Enter an Integer Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        If L <= 0 Then
            MsgBox("You Should Enter an Positive Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If

        If Not IsNumeric(TextBox8.Text) Then
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        L = Long.Parse(TextBox8.Text)
        If Math.Truncate(L) <> L Then
            MsgBox("You Should Enter an Integer Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        If L <= 0 Then
            MsgBox("You Should Enter an Positive Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If

        If Not IsNumeric(TextBox7.Text) Then
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        L = Long.Parse(TextBox7.Text)
        If Math.Truncate(L) <> L Then
            MsgBox("You Should Enter an Integer Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        If L <= 0 Then
            MsgBox("You Should Enter an Positive Value", MsgBoxStyle.Critical)
            PictureBox1.Image = Nothing
            Exit Sub
        End If
        Try
            PictureBox1.Image = DrawRecipt(DataGridView3.Rows, 737, DateString, 123, TextBox5.Text, TextBox8.Text, TextBox7.Text)
        Catch ex As Exception
            PictureBox1.Image = Nothing
        End Try
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            con.Open()
            cmd.CommandText = "Update Printer Set UnitWidth=16,UnitHeight=14 ,FontSize=8 Where Sr=1"
            cmd.ExecuteNonQuery()
            cmd.CommandText = "Select * from Printer;"
            cmd.ExecuteNonQuery()
            con.Close()
            Table.Clear()
            With Adapter
                .SelectCommand = cmd
                .Fill(Table)
            End With
            TextBox5.Text = Table.Rows(0).Item(1).ToString
            TextBox8.Text = Table.Rows(0).Item(2).ToString
            TextBox7.Text = Table.Rows(0).Item(3).ToString
            PictureBox1.Image = DrawRecipt(DataGridView3.Rows, 737, DateString, 123, TextBox5.Text, TextBox8.Text, TextBox7.Text)
            MsgBox("Restored", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Me.TextBox4.Clear()
        DataGridView2.Rows.Clear()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub profit_TextChanged(sender As Object, e As EventArgs) Handles profit.TextChanged

    End Sub

    Private Sub LogOutToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub LogOutToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles LogOutToolStripMenuItem.Click
        Select Case MsgBox("Are you sure you want to Log Out?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, )
            Case MsgBoxResult.Yes
                Me.Hide()
                End
            Case MsgBoxResult.No
                Me.Show()
        End Select
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub AddItemsToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Dim AII As New AddItemInv
        AII.ShowDialog()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

    End Sub

    Private Sub EditItemsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditItemsToolStripMenuItem1.Click
        EditItemInv.Show()
    End Sub

    Private Sub AddItemsToolStripMenuItem1_Click_1(sender As Object, e As EventArgs) Handles AddItemsToolStripMenuItem1.Click
        AddItemInv.Show()
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub DataGridView4_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub InventoryDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView4_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub RemoveItemsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveItemsToolStripMenuItem1.Click
        Me.InventoryBindingSource.RemoveCurrent()
        Me.InventoryBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.InventoryDataSet)

        MsgBox("ITEM DELETED", MsgBoxStyle.OkOnly, "")

    End Sub
End Class
