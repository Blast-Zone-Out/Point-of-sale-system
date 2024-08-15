Public Class ChangePassword

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MsgBox("Provide Required Information", MsgBoxStyle.Critical)
            Exit Sub
        End If
        Try
            Dim TA As New POS.POSDataSetTableAdapters.LoginTableAdapter
            Dim TB = TA.GetDataByPass("password")
            If TextBox1.Text <> TB.Rows(0).Item(1) Then
                MsgBox("Incorrect Old Password", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox2.Text = TextBox3.Text Then
                TA.UpdatePass(TextBox2.Text, TextBox1.Text)
                MsgBox("Password Updated", MsgBoxStyle.Information)
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox3.Clear()
            Else
                MsgBox("New Password Does'nt Match", MsgBoxStyle.Critical)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.UseSystemPasswordChar = True
        TextBox2.UseSystemPasswordChar = True
        TextBox3.UseSystemPasswordChar = True
        TextBox1.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class