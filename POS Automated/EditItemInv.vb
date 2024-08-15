Public Class EditItemInv
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox4.Text = "" Then
            MsgBox("PLEASE COMPLETE THE ENTRY", MsgBoxStyle.OkOnly, "ERROR")
            With TextBox1
                .Focus()
                .SelectAll()
            End With


        Else
            MainWindow.TextBox1.Text = TextBox1.Text
            MainWindow.TextBox2.Text = TextBox2.Text
            MainWindow.TextBox4.Text = TextBox4.Text

            Try
                MainWindow.Validate()
                MainWindow.InventoryBindingSource.EndEdit()
                MsgBox("DATA SAVED", MsgBoxStyle.OkOnly, "")

            Catch ex As Exception

                MsgBox("ITEM NAME ALREADY TAKEN", MsgBoxStyle.OkOnly, "")

                Me.Close()
                MainWindow.Enabled = True

            End Try
            MainWindow.TableAdapterManager.UpdateAll(MainWindow.InventoryDataSet)
            MainWindow.TextBox1.Enabled = False
            MainWindow.TextBox2.Enabled = False
            MainWindow.TextBox4.Enabled = False

            Me.Close()
            MainWindow.Enabled = True
            MainWindow.Focus()
        End If
    End Sub
End Class