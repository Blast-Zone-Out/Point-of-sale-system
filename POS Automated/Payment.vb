Imports System.Data.Sql
Public Class Payment
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridView2.Rows.Count <= 0 Then
                MsgBox("Please add some items to print", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            Dim con As New OleDb.OleDbConnection(My.Settings.POSConnectionString)
            con.Open()
            Dim sql As String = "Insert into TotalPrice (SellPrice) Values(:3)"
            Dim cmd1 As New OleDb.OleDbCommand
            With cmd1
                .Connection = con
            End With
            Dim ReciptID As Long = cmd1.ExecuteScalar
            cmd1.Dispose()
            Dim i As Integer
            For i = 0 To DataGridView2.Rows.Count - 1
                Dim SellPrice As String = DataGridView2.Rows(i).Cells(3).Value
                Dim cmd2 As New OleDb.OleDbCommand
                sql = "Insert into ReciptDetails values(:0,:1,:2,:3,:4)"
                With cmd2
                    .Connection = con
                End With
                cmd2.Parameters.AddWithValue(":4", SellPrice)
                cmd2.ExecuteNonQuery()
                cmd2.Dispose()
            Next
            con.Close()
            con.Dispose()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class