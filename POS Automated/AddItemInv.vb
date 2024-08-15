Public Class AddItemInv
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ItemIDTextBox.Text = "" Or TextBox2.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Please Enter Correct Information", MsgBoxStyle.Critical)
            ItemIDTextBox.Text = ""
            TextBox2.Clear()
            TextBox4.Clear()
        Else
            MainWindow.InventoryBindingSource.AddNew()
            MainWindow.TextBox1.Text = ItemIDTextBox.Text
            MainWindow.TextBox2.Text = TextBox2.Text
            MainWindow.TextBox4.Text = TextBox4.Text

            Try
                MainWindow.Validate()
                MainWindow.InventoryBindingSource.EndEdit()
                status.Text = "One Item(s) Added"


            Catch ex As Exception
                MsgBox("ItemID already Exist !", MsgBoxStyle.Exclamation)
                MainWindow.InventoryBindingSource.CancelEdit()
                MainWindow.Show()
                Me.Hide()

            End Try
            Me.Close()
            MainWindow.Show()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
    End Sub

    Private Sub InventoryBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs)
        Me.Validate()
        Me.InventoryBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.InventoryDataSet)

    End Sub

    Private Sub AddItemInv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'InventoryDataSet.Inventory' table. You can move, or remove it, as needed.
        Me.InventoryTableAdapter.Fill(Me.InventoryDataSet.Inventory)

    End Sub
End Class