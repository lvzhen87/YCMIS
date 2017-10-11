Public Class frmCarStatus

    Dim strRoad As String

    Sub New(ByVal roadStr As String)
        InitializeComponent()
        strRoad = roadStr
    End Sub

    Private Sub frmCarStatus_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CarStatusQuery(strRoad)
    End Sub

    Private Sub Button_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Search.Click
        CarStatusQuery(strRoad)
    End Sub

    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    Private Sub CarStatusQuery(ByVal roadStr As String)

        Dim MyDBserver As clsOracle
        Dim ds As DataSet
        Dim dt As DataTable = Nothing
        Try


            Dim sqlstr As String = "select case when t3.ordername_s is null then '公卡' else t3.ordername_s end ordername_s, t4.customername_s, t2.resaveds3_s, t2.editflagtime_s  from (select cardid_s, max(createtime_s) m from t_barriergate_inoroutinfo_bak t group by cardid_s) t1, t_barriergate_inoroutinfo_bak t2  left join t_subscribe_order t3 on t2.resaveds2_s = t3.orderid_s left join t_subscribe_customer t4 on t3.customerid_s = t4.customerid_s where t1.cardid_s = t2.cardid_s  and t1.m = t2.createtime_s  and t2.motion_s = 1  and t2.createtime_s > to_char(sysdate, 'yyyy-MM-dd')  and t2.barriergateid_s in   (select b.gatenumber_s from t_basis_barriergate b   where b.entranceroadid_s = '" + roadStr + "' and b.resaveds1_s = '1')"
         
            MyDBserver = New clsOracle(connStr)
            ds = MyDBserver.GetDataSet(sqlstr)
            If Not ds Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    DataGridView_Main.DataSource = ds.Tables(0)
                Else
                    clsLog.MessageLog(LogStr, sqlstr)
                    MessageBox.Show("数据为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                clsLog.MessageLog(LogStr, sqlstr)
                MessageBox.Show("数据为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "CarStatusQuery:" & ex.Message)
            dt = Nothing
        End Try
    End Sub


End Class