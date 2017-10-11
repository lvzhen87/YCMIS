<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCarStatus
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCarStatus))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DataGridView_Main = New System.Windows.Forms.DataGridView()
        Me.Button_Exit = New System.Windows.Forms.Button()
        Me.Button_Search = New System.Windows.Forms.Button()
        Me.订单名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.公司名称 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPG牌照号 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.进入时间 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView_Main, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.DataGridView_Main)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1264, 602)
        Me.Panel1.TabIndex = 0
        '
        'DataGridView_Main
        '
        Me.DataGridView_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_Main.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.订单名称, Me.公司名称, Me.CPG牌照号, Me.进入时间})
        Me.DataGridView_Main.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView_Main.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView_Main.Name = "DataGridView_Main"
        Me.DataGridView_Main.RowTemplate.Height = 23
        Me.DataGridView_Main.Size = New System.Drawing.Size(1264, 602)
        Me.DataGridView_Main.TabIndex = 0
        '
        'Button_Exit
        '
        Me.Button_Exit.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button_Exit.Location = New System.Drawing.Point(1073, 630)
        Me.Button_Exit.Name = "Button_Exit"
        Me.Button_Exit.Size = New System.Drawing.Size(123, 43)
        Me.Button_Exit.TabIndex = 1
        Me.Button_Exit.Text = "返回"
        Me.Button_Exit.UseVisualStyleBackColor = True
        '
        'Button_Search
        '
        Me.Button_Search.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button_Search.Location = New System.Drawing.Point(897, 630)
        Me.Button_Search.Name = "Button_Search"
        Me.Button_Search.Size = New System.Drawing.Size(123, 43)
        Me.Button_Search.TabIndex = 2
        Me.Button_Search.Text = "刷新"
        Me.Button_Search.UseVisualStyleBackColor = True
        '
        '订单名称
        '
        Me.订单名称.DataPropertyName = "ordername_s"
        Me.订单名称.HeaderText = "订单名称"
        Me.订单名称.Name = "订单名称"
        Me.订单名称.Width = 400
        '
        '公司名称
        '
        Me.公司名称.DataPropertyName = "customername_s"
        Me.公司名称.HeaderText = "公司名称"
        Me.公司名称.Name = "公司名称"
        Me.公司名称.Width = 400
        '
        'CPG牌照号
        '
        Me.CPG牌照号.DataPropertyName = "resaveds3_s"
        Me.CPG牌照号.HeaderText = "CPG牌照号"
        Me.CPG牌照号.Name = "CPG牌照号"
        Me.CPG牌照号.Width = 200
        '
        '进入时间
        '
        Me.进入时间.DataPropertyName = "editflagtime_s"
        Me.进入时间.HeaderText = "进入时间"
        Me.进入时间.Name = "进入时间"
        Me.进入时间.Width = 230
        '
        'frmCarStatus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 702)
        Me.Controls.Add(Me.Button_Search)
        Me.Controls.Add(Me.Button_Exit)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmCarStatus"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "车辆状态查询"
        Me.Panel1.ResumeLayout(False)
        CType(Me.DataGridView_Main, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DataGridView_Main As System.Windows.Forms.DataGridView
    Friend WithEvents Button_Exit As System.Windows.Forms.Button
    Friend WithEvents Button_Search As System.Windows.Forms.Button
    Friend WithEvents 订单名称 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 公司名称 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CPG牌照号 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents 进入时间 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
