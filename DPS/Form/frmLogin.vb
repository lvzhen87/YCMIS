Imports System.IO

Public Class frmLogin

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitConfig()
        '程序集版本
        Dim AssemblyVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        ''文件版本
        'Dim ProductVersion As String = Application.ProductVersion.ToString()
        Me.Text = "YCMIS-V" + AssemblyVersion

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        System.Environment.Exit(0)
    End Sub

    ''' <summary>
    ''' 重置按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Reset.Click
        txtUserID.Text = ""
        txtUserPwd.Text = ""
        txtUserID.Focus()
    End Sub


    ''' <summary>
    ''' 确定按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OK.Click

        UserName = txtUserID.Text
        UserPwd = txtUserPwd.Text

        If UserName = "" Or UserPwd = "" Then
            MessageBox.Show("用户名或密码不能为空！", "登陆", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If (UserName = userIDStr) And (UserPwd = userPwdStr) Then

            '查询版本号及密码失效时间
            InvalidQuery()


            If MyfrmMIS Is Nothing Then
                clsLog.MessageLog(LogStr, "用户：" & UserName & " 登录成功")
                MyfrmLogin.Visible = False
                MyfrmMIS = New MIS
                MyfrmMIS.ShowDialog()
            Else
                MessageBox.Show("登陆成功！", "登陆", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("用户名或密码错误！", "登陆", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

    End Sub


    ''' <summary>
    ''' 查询版本号及密码失效日期
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InvalidQuery()
        '程序集版本
        Dim AssemblyVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        ''文件版本
        'Dim ProductVersion As String = Application.ProductVersion.ToString()

        'Me.Text = AssemblyVersion

        Dim oracleINVALIDDATE As String = ""
        Dim oracleAssemblyVersion As String = ""

        Dim timeNum As Long

        Dim MyDBserver As clsOracle
        Dim strsql As String
        Dim ds As DataSet
        Dim dt As DataTable = Nothing
        MyDBserver = New clsOracle(connStr)
        Try
            strsql = "select * from t_invalid where rownum<=1 "
            ds = MyDBserver.GetDataSet(strsql)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    oracleINVALIDDATE = ds.Tables(0).Rows(0)(0).ToString()
                    oracleAssemblyVersion = ds.Tables(0).Rows(0)(1).ToString()
                Else
                    Environment.Exit(0)
                End If
            Else
                Environment.Exit(0)
            End If

            timeNum = DateDiff(DateInterval.Day, CDate(oracleINVALIDDATE), Now)

            If oracleAssemblyVersion <> AssemblyVersion Then
                clsLog.MessageLog(LogStr, "用户：" & UserName & " 登录-软件版本已失效")
                MessageBox.Show("该软件版本已失效，请联系管理员更新软件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Environment.Exit(0)
            End If
            If timeNum > 0 Then
                clsLog.MessageLog(LogStr, "用户：" & UserName & " 登录-密码已过期")
                MessageBox.Show("密码已过期，请联系管理员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Environment.Exit(0)
            End If

        Catch ex As Exception
            clsLog.MessageLog(LogStr, "InvalidQuery:" & ex.Message)
            dt = Nothing
        End Try


    End Sub
   





End Class