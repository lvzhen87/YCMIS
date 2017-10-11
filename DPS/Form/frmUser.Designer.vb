<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUser
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button_ok = New System.Windows.Forms.Button()
        Me.Button_esc = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_user = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_psw = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Button_ok
        '
        Me.Button_ok.Location = New System.Drawing.Point(70, 75)
        Me.Button_ok.Name = "Button_ok"
        Me.Button_ok.Size = New System.Drawing.Size(75, 23)
        Me.Button_ok.TabIndex = 0
        Me.Button_ok.Text = "确认"
        Me.Button_ok.UseVisualStyleBackColor = True
        '
        'Button_esc
        '
        Me.Button_esc.Location = New System.Drawing.Point(189, 75)
        Me.Button_esc.Name = "Button_esc"
        Me.Button_esc.Size = New System.Drawing.Size(75, 23)
        Me.Button_esc.TabIndex = 0
        Me.Button_esc.Text = "取消"
        Me.Button_esc.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(50, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "操作人:"
        '
        'TextBox_user
        '
        Me.TextBox_user.Location = New System.Drawing.Point(119, 41)
        Me.TextBox_user.Name = "TextBox_user"
        Me.TextBox_user.Size = New System.Drawing.Size(181, 21)
        Me.TextBox_user.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(52, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "密码:"
        '
        'TextBox_psw
        '
        Me.TextBox_psw.Location = New System.Drawing.Point(119, 14)
        Me.TextBox_psw.Name = "TextBox_psw"
        Me.TextBox_psw.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBox_psw.Size = New System.Drawing.Size(181, 21)
        Me.TextBox_psw.TabIndex = 2
        '
        'frmUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 116)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox_psw)
        Me.Controls.Add(Me.TextBox_user)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button_esc)
        Me.Controls.Add(Me.Button_ok)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(354, 154)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(354, 154)
        Me.Name = "frmUser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "操作人"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_ok As System.Windows.Forms.Button
    Friend WithEvents Button_esc As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox_user As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox_psw As System.Windows.Forms.TextBox
End Class
