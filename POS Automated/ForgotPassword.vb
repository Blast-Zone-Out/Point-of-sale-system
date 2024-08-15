Public Class ForgotPassword
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Then
            MsgBox("Provide Required Information", MsgBoxStyle.Critical)
            Exit Sub
        End If
        If TextBox4.Text = "Marikina City" Then
            If TextBox5.Text = "Chopper" Then
                If TextBox6.Text = "Teriyaki" Then
                    Try
                        Dim TA As New POS.POSDataSetTableAdapters.LoginTableAdapter
                        Dim TB = TA.GetDataByPass("password")
                        MsgBox("Your Current Password is " + " = " + TB.Rows(0).Item(1))
                        Select Case MsgBox("Do you wish to change your password?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, )
                            Case MsgBoxResult.Yes
                                Me.Hide()
                                ChangePassword.Show()
                                TextBox4.Clear()
                                TextBox5.Clear()
                                TextBox6.Clear()
                            Case MsgBoxResult.No
                                Me.Hide()
                                MainWindow.Show()
                                TextBox4.Clear()
                                TextBox5.Clear()
                                TextBox6.Clear()
                        End Select
                    Catch ex As Exception
                        MsgBox(ex.ToString)

                    End Try
                End If
            Else MsgBox("Provide the Correct Information", MsgBoxStyle.Critical)
                TextBox4.Clear()
                TextBox5.Clear()
                TextBox6.Clear()
            End If
        End If
        Exit Sub
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
        MainWindow.Show()
    End Sub

    Private Sub ForgotPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Hide()
        Me.Label2.Hide()
        Me.Label3.Hide()
        Me.Button4.Hide()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Label1.Show()
        Me.Label2.Show()
        Me.Label3.Show()

        Me.Button3.Hide()
        Me.Button4.Show()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Label1.Hide()
        Me.Label2.Hide()
        Me.Label3.Hide()
        Me.Button4.Hide()
        Me.Button3.Show()
    End Sub
End Class