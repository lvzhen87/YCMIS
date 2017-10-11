Public Class frmUser
    Public returnValue As Integer = 0
    Public returnUserName As String

    Private Sub Button_esc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_esc.Click
        returnValue = 0
        Me.Close()
    End Sub

    Private Sub Button_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ok.Click

        If updatePwdStr <> TextBox_psw.Text.ToString() Then
            MessageBox.Show("授权密码错误,请重新输入")
            Return
        End If

        If TextBox_user.Text.ToString() = "" Then
            returnUserName = ""
        End If
        returnUserName = TextBox_user.Text.ToString()
        returnValue = 1
        Me.Close()
    End Sub
End Class