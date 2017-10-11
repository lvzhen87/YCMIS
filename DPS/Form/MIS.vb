
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net


Public Class MIS
    Dim userName As String = "admin"

    Private Sub MIS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '线程间调用控件
        Control.CheckForIllegalCrossThreadCalls = False
        '程序集版本
        Dim AssemblyVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        ''文件版本
        'Dim ProductVersion As String = Application.ProductVersion.ToString()
        Me.Text = "试验调度系统" + AssemblyVersion
        'Dim roadStatus As String

        'clsLog.MessageLog("1110000000")
        'roadStatus = clsLog.ReadFileLine("C:\\roadStatus.txt")
        Init()
        BoundInfo()
        AllRoadString()

        DeviceEnableQuery()
        '开启线程自动打印功能
        'If startThreat = "1" Then

        threadCycle = New Thread(AddressOf RoadCarInfoCycle)
        threadCycle.Start()
        'End If

    End Sub



    ''' <summary>
    ''' 查询DeviceEnable 是否启用
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeviceEnableQuery()
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
                    DeviceEnable = ds.Tables(0).Rows(0)(2).ToString()
                    DeviceLock = ds.Tables(0).Rows(0)(3).ToString()
                End If
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "InvalidQuery:" & ex.Message)
            dt = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Init()
        Try

           
            '安装最多11条路做
            ReDim MyRoads.MyRoad(10)
            ReDim RadioStaus(10)
            Dim roadConfigString As String = ""
            Dim roadConfigStringNew As String()
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road0")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(0) = Radio_Road0_1.Checked
            AddRoadInfo(0, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road1")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(1) = Radio_Road1_1.Checked
            AddRoadInfo(1, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road2")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(2) = Radio_Road2_1.Checked
            AddRoadInfo(2, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road3")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(3) = Radio_Road3_1.Checked
            AddRoadInfo(3, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road4")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(4) = Radio_Road4_1.Checked
            AddRoadInfo(4, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road5")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(5) = Radio_Road5_1.Checked
            AddRoadInfo(5, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road6")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(6) = Radio_Road6_1.Checked
            AddRoadInfo(6, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road7")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(7) = Radio_Road7_1.Checked
            AddRoadInfo(7, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road8")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(8) = Radio_Road8_1.Checked
            AddRoadInfo(8, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road9")
            roadConfigStringNew = roadConfigString.Split("_")
            RadioStaus(9) = Radio_Road9_1.Checked
            AddRoadInfo(9, roadConfigStringNew)
            roadConfigString = System.Configuration.ConfigurationManager.AppSettings("road10")
            roadConfigStringNew = roadConfigString.Split("_")
            ''RadioStaus(10) = Radio_Road0_1.Checked
            AddRoadInfo(10, roadConfigStringNew)
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "Init:" & ex.Message)

        End Try
    End Sub

    ''' <summary>
    ''' 将道路信息加载到结构体
    ''' </summary>
    ''' <param name="roadID"></param>
    ''' <param name="roadInfo"></param>
    ''' <remarks></remarks>
    Public Sub AddRoadInfo(ByVal roadID As Integer, ByVal roadInfo As String())
        If roadInfo.Length = 2 Then '未开放道路
            MyRoads.MyRoad(roadID).RoadID = "road" + roadID.ToString()
            MyRoads.MyRoad(roadID).Enable = roadInfo(0)
            MyRoads.MyRoad(roadID).RoadName = roadInfo(1)
        ElseIf roadInfo.Length = 5 Then '开放道路
            MyRoads.MyRoad(roadID).RoadID = "road" + roadID.ToString()
            MyRoads.MyRoad(roadID).Enable = roadInfo(0)
            MyRoads.MyRoad(roadID).RoadNum = roadInfo(1)
            MyRoads.MyRoad(roadID).RoadEntrance = roadInfo(2)
            MyRoads.MyRoad(roadID).RoadExit = roadInfo(3)
            MyRoads.MyRoad(roadID).RoadName = roadInfo(4)
        Else
        End If
    End Sub

    ''' <summary>
    ''' 初始化绑定道路信息
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BoundInfo()

        Try
            GroupBox_Road_0.Text = MyRoads.MyRoad(0).RoadName
            GroupBox_Road_1.Text = MyRoads.MyRoad(1).RoadName
            GroupBox_Road_2.Text = MyRoads.MyRoad(2).RoadName
            GroupBox_Road_3.Text = MyRoads.MyRoad(3).RoadName
            GroupBox_Road_4.Text = MyRoads.MyRoad(4).RoadName
            GroupBox_Road_5.Text = MyRoads.MyRoad(5).RoadName
            GroupBox_Road_6.Text = MyRoads.MyRoad(6).RoadName
            GroupBox_Road_7.Text = MyRoads.MyRoad(7).RoadName
            GroupBox_Road_8.Text = MyRoads.MyRoad(8).RoadName
            GroupBox_Road_9.Text = MyRoads.MyRoad(9).RoadName
            GroupBox_Road_10.Text = MyRoads.MyRoad(10).RoadName

            If MyRoads.MyRoad(0).Enable = False Then
                GroupBox_Road_0_En.Visible = False
                GroupBox_Road_0_Ex.Visible = False
                GroupBox_Road_0_St.Visible = False
                Radio_Road0_1.Visible = False
                Radio_Road0_2.Visible = False
                Button_sub0.Visible = False
                TextBox_updateNum0.Visible = False
            End If
            If MyRoads.MyRoad(1).Enable = False Then
                GroupBox_Road_1_En.Visible = False
                GroupBox_Road_1_Ex.Visible = False
                GroupBox_Road_1_St.Visible = False
                Radio_Road1_1.Visible = False
                Radio_Road1_2.Visible = False
                Button_sub1.Visible = False
                TextBox_updateNum1.Visible = False
            End If
            If MyRoads.MyRoad(2).Enable = False Then
                GroupBox_Road_2_En.Visible = False
                GroupBox_Road_2_Ex.Visible = False
                GroupBox_Road_2_St.Visible = False
                Radio_Road2_1.Visible = False
                Radio_Road2_2.Visible = False
                Button_sub2.Visible = False
                TextBox_updateNum2.Visible = False
            End If
            If MyRoads.MyRoad(3).Enable = False Then
                GroupBox_Road_3_En.Visible = False
                GroupBox_Road_3_Ex.Visible = False
                GroupBox_Road_3_St.Visible = False
                Radio_Road3_1.Visible = False
                Radio_Road3_2.Visible = False
                Button_sub3.Visible = False
                TextBox_updateNum3.Visible = False
            End If
            If MyRoads.MyRoad(4).Enable = False Then
                GroupBox_Road_4_En.Visible = False
                GroupBox_Road_4_Ex.Visible = False
                GroupBox_Road_4_St.Visible = False
                Radio_Road4_1.Visible = False
                Radio_Road4_2.Visible = False
                Button_sub4.Visible = False
                TextBox_updateNum4.Visible = False
            End If
            If MyRoads.MyRoad(5).Enable = False Then
                GroupBox_Road_5_En.Visible = False
                GroupBox_Road_5_Ex.Visible = False
                GroupBox_Road_5_St.Visible = False
                Radio_Road5_1.Visible = False
                Radio_Road5_2.Visible = False
                Button_sub5.Visible = False
                TextBox_updateNum5.Visible = False
            End If
            If MyRoads.MyRoad(6).Enable = False Then
                GroupBox_Road_6_En.Visible = False
                GroupBox_Road_6_Ex.Visible = False
                GroupBox_Road_6_St.Visible = False
                Radio_Road6_1.Visible = False
                Radio_Road6_2.Visible = False
                Button_sub6.Visible = False
                TextBox_updateNum6.Visible = False
            End If
            If MyRoads.MyRoad(7).Enable = False Then
                GroupBox_Road_7_En.Visible = False
                GroupBox_Road_7_Ex.Visible = False
                GroupBox_Road_7_St.Visible = False
                Radio_Road7_1.Visible = False
                Radio_Road7_2.Visible = False
                Button_sub7.Visible = False
                TextBox_updateNum7.Visible = False
            End If
            If MyRoads.MyRoad(8).Enable = False Then
                GroupBox_Road_8_En.Visible = False
                GroupBox_Road_8_Ex.Visible = False
                GroupBox_Road_8_St.Visible = False
                Radio_Road8_1.Visible = False
                Radio_Road8_2.Visible = False
                Button_sub8.Visible = False
                TextBox_updateNum8.Visible = False
            End If
            If MyRoads.MyRoad(9).Enable = False Then
                GroupBox_Road_9_En.Visible = False
                GroupBox_Road_9_Ex.Visible = False
                GroupBox_Road_9_St.Visible = False
                Radio_Road9_1.Visible = False
                Radio_Road9_2.Visible = False
                Button_sub9.Visible = False
                TextBox_updateNum9.Visible = False
            End If
            If MyRoads.MyRoad(10).Enable = False Then
                GroupBox_Road_10_En.Visible = False
                GroupBox_Road_10_Ex.Visible = False
                GroupBox_Road_10_St.Visible = False
                Radio_Road10_1.Visible = False
                Radio_Road10_2.Visible = False
                Button_sub10.Visible = False
                TextBox_updateNum10.Visible = False
            End If

        Catch ex As Exception
            clsLog.MessageLog(LogStr, "BoundInfo:" & ex.Message)

        End Try
    End Sub

    ''' <summary>
    ''' 控制全部道路相关字符串
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AllRoadString()
        For i = 0 To MyRoads.MyRoad.Length - 1
            If MyRoads.MyRoad(i).Enable Then
                sendAllRoadEntranceStr = sendAllRoadEntranceStr + MyRoads.MyRoad(i).RoadEntrance + ","
                sendAllRoadExitStr = sendAllRoadExitStr + MyRoads.MyRoad(i).RoadExit + ","
                sqlSearchWhereStr = sqlSearchWhereStr + " ROADID_S='" + MyRoads.MyRoad(i).RoadNum + "' or"
            End If
        Next
        sendAllRoadEntranceStr = sendAllRoadEntranceStr.Remove(sendAllRoadEntranceStr.Length - 1)
        sendAllRoadExitStr = sendAllRoadExitStr.Remove(sendAllRoadExitStr.Length - 1)
        sqlSearchWhereStr = sqlSearchWhereStr.Remove(sqlSearchWhereStr.Length - 2)

    End Sub

    Public Sub RoadCarInfoCycle()
        While True
            DeviceEnableQuery()
            RoadCarInfo()

            '是否开启判断时间差抬落杆功能
            If DeviceLock = "0" Then
                'clsLog.MessageLog(LogStr, "RoadCarInfoCycle:" & Date.Now.ToString())
                RoadTimeCheckCycle()
            End If

            System.Threading.Thread.Sleep(1000 * cycleTime)
        End While
    End Sub



    ''' <summary>
    ''' 判断道路内车辆数量，当超出最大数时候 入口亮红灯、锁定（可以设置是否启用）；当小于最大数量时候 入口亮绿灯、解锁
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RoadCarInfo()
        Dim mydt As DataTable
        Try
            mydt = RoadCarQuantity()
            If Not mydt Is Nothing Then
                If mydt.Rows.Count > 0 Then
                    For i = 0 To mydt.Rows.Count - 1
                        For j = 0 To MyRoads.MyRoad.Length - 1
                            If MyRoads.MyRoad(j).Enable Then
                                If mydt.Rows(i)(0).ToString() = MyRoads.MyRoad(j).RoadNum Then
                                    '绑定信息

                                    Select Case MyRoads.MyRoad(j).RoadID
                                        Case "road0"
                                            Label_Road0_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road0_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road0_9.BackColor = Color.Red
                                                If road0Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                ' 出口-锁定

                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")

                                                End If



                                            Else

                                                Button_Road0_9.BackColor = Color.Lime
                                                If road0Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                ' 出口-解锁
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                        Case "road1"
                                            Label_Road1_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road1_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road1_9.BackColor = Color.Red
                                                If road1Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")

                                                End If

                                            Else
                                                Button_Road1_9.BackColor = Color.Lime
                                                If road1Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If



                                            End If
                                        Case "road2"
                                            Label_Road2_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road2_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road2_9.BackColor = Color.Red
                                                If road2Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road2_9.BackColor = Color.Lime
                                                If road2Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")


                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If


                                            End If
                                        Case "road3"
                                            Label_Road3_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road3_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road3_9.BackColor = Color.Red
                                                If road3Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If



                                            Else
                                                Button_Road3_9.BackColor = Color.Lime
                                                If road3Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If


                                            End If
                                        Case "road4"
                                            Label_Road4_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road4_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road4_9.BackColor = Color.Red
                                                If road4Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road4_9.BackColor = Color.Lime
                                                If road4Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If

                                        Case "road5"
                                            Label_Road5_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road5_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road5_9.BackColor = Color.Red
                                                If road5Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road5_9.BackColor = Color.Lime
                                                If road5Enable = False Then
                                                    '当前数量小于
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                        Case "road6"
                                            Label_Road6_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road6_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road6_9.BackColor = Color.Red
                                                If road6Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If


                                            Else
                                                Button_Road6_9.BackColor = Color.Lime
                                                If road6Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                        Case "road7"
                                            Label_Road7_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road7_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road7_9.BackColor = Color.Red
                                                If road7Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If



                                            Else
                                                Button_Road7_9.BackColor = Color.Lime
                                                If road7Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                        Case "road8"
                                            Label_Road8_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road8_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road8_9.BackColor = Color.Red
                                                If road8Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road8_9.BackColor = Color.Lime
                                                If road8Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                        Case "road9"
                                            Label_Road9_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road9_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road9_9.BackColor = Color.Red
                                                If road9Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")


                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road9_9.BackColor = Color.Lime
                                                If road9Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If


                                            End If
                                        Case "road10"
                                            Label_Road10_Current.Text = mydt.Rows(i)(1).ToString()
                                            Label_Road10_Max.Text = mydt.Rows(i)(2).ToString()
                                            If CType(mydt.Rows(i)(1), Integer) >= CType(mydt.Rows(i)(2), Integer) Then
                                                Button_Road10_9.BackColor = Color.Red
                                                If road10Enable = False Then
                                                    '当前数量大于等于最大数，亮红灯
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "2"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮红灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "6"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-锁定")


                                                End If

                                            Else
                                                Button_Road10_9.BackColor = Color.Lime
                                                If road10Enable = False Then
                                                    '当前数量小于
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "3"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-亮绿灯")

                                                End If
                                                If DeviceEnable = "0" Then
                                                    'If modMain.RadioStaus(j) = True Then

                                                    '    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁 手动模式,不发送解锁指令")
                                                    '    Continue For
                                                    'End If
                                                    SendMessage(SocketString(MyRoads.MyRoad(j).RoadEntrance, "7"))
                                                    clsLog.MessageLog(LogStr, "道路：" & MyRoads.MyRoad(j).RoadName & "-车辆数：" & CType(mydt.Rows(i)(1), Integer).ToString() & "-最大车辆数：" & CType(mydt.Rows(i)(2), Integer).ToString() & " -执行：入口-解锁")

                                                End If

                                            End If
                                    End Select

                                End If
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarInfo:" & ex.Message)

        End Try
    End Sub

    Public DBserver As clsOracle
    ''' <summary>
    ''' 查询当前场内道路内车辆
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RoadCarQuantity() As DataTable
        Dim strsql As String
        Dim ds As DataSet
        Dim dt As DataTable = Nothing
        DBserver = New clsOracle(connStr)
        Try
            strsql = "select ROADID_S,RESAVEDS3_S,MAXCAPACITY_I from  T_BASIS_ROADINFO where 0=0 and "
            strsql = strsql + sqlSearchWhereStr
            ds = DBserver.GetDataSet(strsql)
            If Not ds Is Nothing Then
                dt = ds.Tables(0)
            End If
            Return dt
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)
            dt = Nothing
            Return dt

        End Try
    End Function

    ''' <summary>
    ''' 查询当前场内道路内车辆
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateRoadNumQuery() As DataTable
        Dim strsql As String
        Dim ds As DataSet
        Dim dt As DataTable = Nothing
        Dim timefrom As String
        Dim timeto As String
        timefrom = DateTimePicker_updateNumFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00"
        timeto = DateTimePicker_updateNumTo.Value.ToString("yyyy-MM-dd") + " 23:59:59"
        DBserver = New clsOracle(connStr)
        Try
            strsql = "select rownum as rownum_s,a.* from ( select * from t_system_updateroadnum  where 1=1 "
            strsql = strsql & " and updatetime_t between to_date('" & timefrom & "','yyyy-mm-dd hh24:mi:ss') and to_date('" & timeto & "','yyyy-mm-dd hh24:mi:ss') order by  updatetime_t desc ) a"
            ds = DBserver.GetDataSet(strsql)
            If Not ds Is Nothing Then
                dt = ds.Tables(0)
            End If
            Return dt
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)
            dt = Nothing
            Return dt

        End Try
    End Function

    ''' <summary>
    ''' 修改当前场内道路内车辆
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateRoadNum(ByVal roadNum As String) As Boolean
        Dim strsql As String
        Dim dt As DataTable = Nothing
        Dim queryList As New List(Of String)
        DBserver = New clsOracle(connStr)
        Try
            If roadNum = Nothing Then
                Return True
            End If
            strsql = "update T_BASIS_ROADINFO set RESAVEDS3_S = RESAVEDS3_S + 1  where 0=0 and " + " roadid_s = '" + roadNum + "'"
            'strsql = strsql + sqlSearchWhereStr
            queryList.Add(strsql)
            DBserver.Execute(queryList)
            Return True

        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)
            dt = Nothing
            Return False

        End Try
    End Function

    ''' <summary>
    ''' 修改当前场内道路内车辆
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SubRoadNum(ByVal roadNum As String) As Boolean
        Dim strsql As String
        Dim dt As DataTable = Nothing
        Dim queryList As New List(Of String)
        DBserver = New clsOracle(connStr)
        Try
            If roadNum = Nothing Then
                Return True
            End If
            strsql = "update T_BASIS_ROADINFO set RESAVEDS3_S = RESAVEDS3_S - 1  where 0=0 and " + " roadid_s = '" + roadNum + "'"
            'strsql = strsql + sqlSearchWhereStr
            queryList.Add(strsql)
            DBserver.Execute(queryList)
            Return True

        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)
            dt = Nothing
            Return False

        End Try
    End Function



    Public DBserverSQL As DataBase

    ''' <summary>
    ''' 判断当前道路落杆状态
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RoadTimeCheck() As DataTable
        Dim strsql As String
        Dim ds As DataSet
        Dim dt As DataTable = Nothing
        DBserverSQL = New DataBase()
        Try
            strsql = "SELECT GETDATE() AS Time,  dbo.DeviceAlarmInfor.[DeviceID] ,dbo.DeviceAlarmInfor.[AlarmID],dbo.DeviceAlarmInfor.[DateTime1], dbo.Device.PassageWayID  FROM  dbo.Device WITH (NOLOCK)  INNER JOIN dbo.DeviceAlarmInfor WITH (NOLOCK)  ON dbo.Device.DeviceID = dbo.DeviceAlarmInfor.DeviceID   where dbo.Device.PassageWayID<>1 and  dbo.Device.PassageWayID<>2  order by dbo.Device.PassageWayID  "

            ds = DBserverSQL.Search(strsql)
            If Not ds Is Nothing Then
                dt = ds.Tables(0)
            End If
            Return dt
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadTimeCheck:" & ex.Message)
            dt = Nothing
            Return dt

        End Try
    End Function

    ''' <summary>
    ''' 判断落杆状态发送落杆指令
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RoadTimeCheckCycle()
        Dim mydt As DataTable
        Try
            mydt = RoadTimeCheck()
            If Not mydt Is Nothing Then
                If mydt.Rows.Count > 0 Then
                    For i = 0 To mydt.Rows.Count - 1
                        'strsql = "SELECT GETDATE() AS Time,  dbo.DeviceAlarmInfor.[DeviceID] ,dbo.DeviceAlarmInfor.[AlarmID],dbo.DeviceAlarmInfor.[DateTime1], dbo.Device.PassageWayID  FROM  dbo.Device WITH (NOLOCK)  INNER JOIN dbo.DeviceAlarmInfor WITH (NOLOCK)  ON dbo.Device.DeviceID = dbo.DeviceAlarmInfor.DeviceID   where dbo.Device.PassageWayID<>1 and  dbo.Device.PassageWayID<>2  order by dbo.Device.PassageWayID  "

                        If mydt.Rows(i)(4).ToString() <> "1" And mydt.Rows(i)(4).ToString() <> "2" Then

                            Dim timeNum As Long
                            '如果 date1 比 date2 来得晚，则 DateDiff 函数的返回值为负数。
                            timeNum = DateDiff(DateInterval.Second, CDate(mydt.Rows(i)(3).ToString()), CDate(mydt.Rows(i)(0).ToString()))
                            If (timeNum > datediffSpan) And (mydt.Rows(i)(2).ToString() = "1") Then
                                SendMessage(SocketString(mydt.Rows(i)(4).ToString(), "5"))

                                clsLog.MessageLog(LogStr, "设备ID：" & mydt.Rows(i)(4).ToString() & "-未落杆时间：" & timeNum.ToString() & "-当前设定时间：" & datediffSpan.ToString() & " -执行：落杆操作")

                            End If


                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadTimeCheckCycle:" & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 发送Socket消息
    ''' </summary>
    ''' <param name="SendMsg"></param>
    ''' <remarks></remarks>
    Public Sub SendMessage(ByVal SendMsg As String)
        Try
            If startThreat = "0" Then
                Return
            End If
            Dim tcpc As New TcpClient(serverIP, serverPort)
            Dim tcpStream As NetworkStream = tcpc.GetStream
            Dim reqStream As New IO.StreamWriter(tcpStream)
            'reqStream.Write("GATEORDER{""barrierGateID"":""7"",""schema"":5}#13#10" + Chr(13) + Chr(10))
            reqStream.Write(SendMsg + Chr(13) + Chr(10))
            reqStream.Flush()
            tcpStream.Close()
            tcpc.Close()
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "SendMessage:" & ex.Message)

        End Try
    End Sub

    ''' <summary>
    ''' 发送Socket消息
    ''' </summary>
    ''' <param name="SendMsg"></param>
    ''' <remarks></remarks>
    Public Sub SendMessageButton(ByVal SendMsg As String)
        Try
            Dim tcpc As New TcpClient(serverIP, serverPort)
            Dim tcpStream As NetworkStream = tcpc.GetStream
            Dim reqStream As New IO.StreamWriter(tcpStream)
            'reqStream.Write("GATEORDER{""barrierGateID"":""7"",""schema"":5}#13#10" + Chr(13) + Chr(10))
            reqStream.Write(SendMsg + Chr(13) + Chr(10))
            reqStream.Flush()
            tcpStream.Close()
            tcpc.Close()
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "SendMessage:" & ex.Message)

        End Try
    End Sub


    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub
    Private Sub MIS_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'System.Environment.Exit(System.Environment.ExitCode)

        Dim returnValue As Integer
        returnValue = MessageBox.Show("确定退出软件吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If returnValue = 1 Then
            System.Environment.Exit(0)
        Else
            e.Cancel = True
        End If

    End Sub

#Region "单个道闸控制"
    Private Function ChangeRoadNumber(ByVal radioNum As Integer, ByRef labelCurrent As Label, ByRef labelMax As Label) As Boolean
        Dim currentNum As Integer
        Dim allowNum As Integer
        Dim current As Label
        Dim max As Label
        Try
            'current = CType(Me.Controls.Find("TabControl1", False)(0).Controls.Find("TabPage1", False)(0).Controls.Find("GroupBox_Road_" & radioNum, False)(0).Controls.Find("GroupBox_Road_" & radioNum & "_St", False)(0).Controls.Find("Label_Road" & radioNum & "_Current", False)(0), Label)

            'max = CType(Me.Controls.Find("TabControl1", False)(0).Controls.Find("TabPage1", False)(0).Controls.Find("GroupBox_Road_" & radioNum, False)(0).Controls.Find("GroupBox_Road_" & radioNum & "_St", False)(0).Controls.Find("Label_Road" & radioNum & "_Max", False)(0), Label)
            current = labelCurrent

            max = labelMax
            currentNum = CType(current.Text, Integer)
            allowNum = CType(max.Text, Integer)
            If allowNum > currentNum Then

            Else
                If modMain.RadioStaus(radioNum) Then
                    UpdateRoadNum(MyRoads.MyRoad(radioNum).RoadNum)
                End If
            End If
        Catch ex As Exception

        End Try
    End Function
    '控制标识：2 红灯 3 绿灯 4 抬杆 5 落杆
#Region "道路0"
    '道路0
    Private Sub Button_Road0_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_1.Click

        If MyRoads.MyRoad(0).Enable = True Then
            ChangeRoadNumber(0, Label_Road0_Current, Label_Road0_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-入口-抬杆")
        End If
    End Sub

    Private Sub Button_Road0_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_2.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road0_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_3.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadEntrance, "3"))
            road0Enable = True
            Button_Road0_3.BackColor = Color.Lime
            Button_Road0_4.BackColor = SystemColors.Control
            Button_Road0_10.Enabled = True

            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road0_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_4.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadEntrance, "2"))
            road0Enable = True
            Button_Road0_3.BackColor = SystemColors.Control
            Button_Road0_4.BackColor = Color.Red
            Button_Road0_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road0_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_5.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road0_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_6.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road0_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_7.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadExit, "3"))
            road0Enable = True
            Button_Road0_7.BackColor = Color.Lime
            Button_Road0_8.BackColor = SystemColors.Control
            Button_Road0_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road0_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_8.Click
        If MyRoads.MyRoad(0).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(0).RoadExit, "2"))
            road0Enable = True
            Button_Road0_7.BackColor = SystemColors.Control
            Button_Road0_8.BackColor = Color.Red
            Button_Road0_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路1"
    '道路1
    Private Sub Button_Road1_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_1.Click
        If MyRoads.MyRoad(1).Enable = True Then
            ChangeRoadNumber(1, Label_Road1_Current, Label_Road1_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road1_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_2.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road1_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_3.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadEntrance, "3"))
            road1Enable = True
            Button_Road1_3.BackColor = Color.Lime
            Button_Road1_4.BackColor = SystemColors.Control
            Button_Road1_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road1_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_4.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadEntrance, "2"))
            road1Enable = True
            Button_Road1_3.BackColor = SystemColors.Control
            Button_Road1_4.BackColor = Color.Red
            Button_Road1_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road1_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_5.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road1_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_6.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road1_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_7.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadExit, "3"))
            road1Enable = True
            Button_Road1_7.BackColor = Color.Lime
            Button_Road1_8.BackColor = SystemColors.Control
            Button_Road1_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road1_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_8.Click
        If MyRoads.MyRoad(1).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(1).RoadExit, "2"))
            road1Enable = True
            Button_Road1_7.BackColor = SystemColors.Control
            Button_Road1_8.BackColor = Color.Red
            Button_Road1_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路2"
    '道路2
    Private Sub Button_Road2_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_1.Click
        If MyRoads.MyRoad(2).Enable = True Then
            ChangeRoadNumber(2, Label_Road2_Current, Label_Road2_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road2_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_2.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road2_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_3.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadEntrance, "3"))
            road2Enable = True
            Button_Road2_3.BackColor = Color.Lime
            Button_Road2_4.BackColor = SystemColors.Control
            Button_Road2_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road2_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_4.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadEntrance, "2"))
            road2Enable = True
            Button_Road2_3.BackColor = SystemColors.Control
            Button_Road2_4.BackColor = Color.Red
            Button_Road2_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road2_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_5.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-出口-抬杆")


        End If
    End Sub

    Private Sub Button_Road2_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_6.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road2_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_7.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadExit, "3"))
            road2Enable = True
            Button_Road2_7.BackColor = Color.Lime
            Button_Road2_8.BackColor = SystemColors.Control
            Button_Road2_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road2_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_8.Click
        If MyRoads.MyRoad(2).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(2).RoadExit, "2"))
            road2Enable = True
            Button_Road2_7.BackColor = SystemColors.Control
            Button_Road2_8.BackColor = Color.Red
            Button_Road2_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路3"
    '道路3
    Private Sub Button_Road3_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_1.Click
        If MyRoads.MyRoad(3).Enable = True Then
            ChangeRoadNumber(3, Label_Road3_Current, Label_Road3_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road3_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_2.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road3_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_3.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadEntrance, "3"))
            road3Enable = True
            Button_Road3_3.BackColor = Color.Lime
            Button_Road3_4.BackColor = SystemColors.Control
            Button_Road3_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road3_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_4.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadEntrance, "2"))
            road3Enable = True
            Button_Road3_3.BackColor = SystemColors.Control
            Button_Road3_4.BackColor = Color.Red
            Button_Road3_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road3_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_5.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road3_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_6.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road3_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_7.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadExit, "3"))
            road3Enable = True
            Button_Road3_7.BackColor = Color.Lime
            Button_Road3_8.BackColor = SystemColors.Control
            Button_Road3_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road3_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_8.Click
        If MyRoads.MyRoad(3).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(3).RoadExit, "2"))
            road3Enable = True
            Button_Road3_7.BackColor = SystemColors.Control
            Button_Road3_8.BackColor = Color.Red
            Button_Road3_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路4"
    '道路4
    Private Sub Button_Road4_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_1.Click
        If MyRoads.MyRoad(4).Enable = True Then
            ChangeRoadNumber(4, Label_Road4_Current, Label_Road4_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road4_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_2.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road4_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_3.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadEntrance, "3"))
            road4Enable = True
            Button_Road4_3.BackColor = Color.Lime
            Button_Road4_4.BackColor = SystemColors.Control
            Button_Road4_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road4_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_4.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadEntrance, "2"))
            road4Enable = True
            Button_Road4_3.BackColor = SystemColors.Control
            Button_Road4_4.BackColor = Color.Red
            Button_Road4_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road4_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_5.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road4_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_6.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road4_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_7.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadExit, "3"))
            road4Enable = True
            Button_Road4_7.BackColor = Color.Lime
            Button_Road4_8.BackColor = SystemColors.Control
            Button_Road4_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road4_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_8.Click
        If MyRoads.MyRoad(4).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(4).RoadExit, "2"))
            road4Enable = True
            Button_Road4_7.BackColor = SystemColors.Control
            Button_Road4_8.BackColor = Color.Red
            Button_Road4_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-出口-亮红灯")

        End If
    End Sub


#End Region
#Region "道路5"
    '道路5
    Private Sub Button_Road5_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_1.Click
        If MyRoads.MyRoad(5).Enable = True Then
            ChangeRoadNumber(5, Label_Road5_Current, Label_Road5_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road5_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_2.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road5_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_3.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadEntrance, "3"))
            road5Enable = True
            Button_Road5_3.BackColor = Color.Lime
            Button_Road5_4.BackColor = SystemColors.Control
            Button_Road5_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road5_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_4.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadEntrance, "2"))
            road5Enable = True
            Button_Road5_3.BackColor = SystemColors.Control
            Button_Road5_4.BackColor = Color.Red
            Button_Road5_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road5_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_5.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road5_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_6.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road5_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_7.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadExit, "3"))
            road5Enable = True
            Button_Road5_7.BackColor = Color.Lime
            Button_Road5_8.BackColor = SystemColors.Control
            Button_Road5_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road5_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_8.Click
        If MyRoads.MyRoad(5).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(5).RoadExit, "2"))
            road5Enable = True
            Button_Road5_7.BackColor = SystemColors.Control
            Button_Road5_8.BackColor = Color.Red
            Button_Road5_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路6"
    '道路6
    Private Sub Button_Road6_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_1.Click
        If MyRoads.MyRoad(6).Enable = True Then
            ChangeRoadNumber(6, Label_Road6_Current, Label_Road6_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road6_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_2.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road6_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_3.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadEntrance, "3"))
            road6Enable = True
            Button_Road6_3.BackColor = Color.Lime
            Button_Road6_4.BackColor = SystemColors.Control
            Button_Road6_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road6_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_4.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadEntrance, "2"))
            road6Enable = True
            Button_Road6_3.BackColor = SystemColors.Control
            Button_Road6_4.BackColor = Color.Red
            Button_Road6_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road6_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_5.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road6_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_6.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road6_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_7.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadExit, "3"))
            road6Enable = True
            Button_Road6_7.BackColor = Color.Lime
            Button_Road6_8.BackColor = SystemColors.Control
            Button_Road6_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road6_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_8.Click
        If MyRoads.MyRoad(6).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(6).RoadExit, "2"))
            road6Enable = True
            Button_Road6_7.BackColor = SystemColors.Control
            Button_Road6_8.BackColor = Color.Red
            Button_Road6_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-出口-亮红灯")

        End If
    End Sub

#End Region
#Region "道路7"
    '道路7
    Private Sub Button_Road7_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_1.Click
        If MyRoads.MyRoad(7).Enable = True Then
            ChangeRoadNumber(7, Label_Road7_Current, Label_Road7_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road7_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_2.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road7_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_3.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadEntrance, "3"))
            road7Enable = True
            Button_Road7_3.BackColor = Color.Lime
            Button_Road7_4.BackColor = SystemColors.Control
            Button_Road7_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road7_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_4.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadEntrance, "2"))
            road7Enable = True
            Button_Road7_3.BackColor = SystemColors.Control
            Button_Road7_4.BackColor = Color.Red
            Button_Road7_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road7_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_5.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road7_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_6.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road7_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_7.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadExit, "3"))
            road7Enable = True
            Button_Road7_7.BackColor = Color.Lime
            Button_Road7_8.BackColor = SystemColors.Control
            Button_Road7_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road7_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_8.Click
        If MyRoads.MyRoad(7).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(7).RoadExit, "2"))
            road7Enable = True
            Button_Road7_7.BackColor = SystemColors.Control
            Button_Road7_8.BackColor = Color.Red
            Button_Road7_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路8"


    '道路8
    Private Sub Button_Road8_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_1.Click
        If MyRoads.MyRoad(8).Enable = True Then
            ChangeRoadNumber(8, Label_Road8_Current, Label_Road8_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road8_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_2.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road8_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_3.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadEntrance, "3"))
            road8Enable = True
            Button_Road8_3.BackColor = Color.Lime
            Button_Road8_4.BackColor = SystemColors.Control
            Button_Road8_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road8_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_4.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadEntrance, "2"))
            road8Enable = True
            Button_Road8_3.BackColor = SystemColors.Control
            Button_Road8_4.BackColor = Color.Red
            Button_Road8_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road8_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_5.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road8_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_6.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road8_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_7.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadExit, "3"))
            road8Enable = True
            Button_Road8_7.BackColor = Color.Lime
            Button_Road8_8.BackColor = SystemColors.Control
            Button_Road8_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road8_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_8.Click
        If MyRoads.MyRoad(8).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(8).RoadExit, "2"))
            road8Enable = True
            Button_Road8_7.BackColor = SystemColors.Control
            Button_Road8_8.BackColor = Color.Red
            Button_Road8_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region
#Region "道路9"
    '道路9
    Private Sub Button_Road9_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_1.Click
        If MyRoads.MyRoad(9).Enable = True Then
            ChangeRoadNumber(9, Label_Road9_Current, Label_Road9_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road9_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_2.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road9_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_3.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadEntrance, "3"))
            road9Enable = True
            Button_Road9_3.BackColor = Color.Lime
            Button_Road9_4.BackColor = SystemColors.Control
            Button_Road9_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road9_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_4.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadEntrance, "2"))
            road9Enable = True
            Button_Road9_3.BackColor = SystemColors.Control
            Button_Road9_4.BackColor = Color.Red
            Button_Road9_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road9_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_5.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road9_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_6.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road9_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_7.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadExit, "3"))
            road9Enable = True
            Button_Road9_7.BackColor = Color.Lime
            Button_Road9_8.BackColor = SystemColors.Control
            Button_Road9_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road9_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_8.Click
        If MyRoads.MyRoad(9).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(9).RoadExit, "2"))
            road9Enable = True
            Button_Road9_7.BackColor = SystemColors.Control
            Button_Road9_8.BackColor = Color.Red
            Button_Road9_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-出口-亮红灯")

        End If
    End Sub

#End Region
#Region "道路10"
    '道路10
    Private Sub Button_Road10_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_1.Click
        If MyRoads.MyRoad(10).Enable = True Then
            ChangeRoadNumber(10, Label_Road10_Current, Label_Road10_Max)
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadEntrance, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-入口-抬杆")

        End If
    End Sub

    Private Sub Button_Road10_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_2.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadEntrance, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-入口-落杆")

        End If
    End Sub

    Private Sub Button_Road10_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_3.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadEntrance, "3"))
            road10Enable = True
            Button_Road10_3.BackColor = Color.Lime
            Button_Road10_4.BackColor = SystemColors.Control
            Button_Road10_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-入口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road10_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_4.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadEntrance, "2"))
            road10Enable = True
            Button_Road10_3.BackColor = SystemColors.Control
            Button_Road10_4.BackColor = Color.Red
            Button_Road10_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-入口-亮红灯")

        End If
    End Sub

    Private Sub Button_Road10_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_5.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadExit, "4"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-出口-抬杆")

        End If
    End Sub

    Private Sub Button_Road10_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_6.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadExit, "5"))
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-出口-落杆")

        End If
    End Sub

    Private Sub Button_Road10_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_7.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadExit, "3"))
            road10Enable = True
            Button_Road10_7.BackColor = Color.Lime
            Button_Road10_8.BackColor = SystemColors.Control
            Button_Road10_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-出口-亮绿灯")

        End If
    End Sub

    Private Sub Button_Road10_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_8.Click
        If MyRoads.MyRoad(10).Enable = True Then
            SendMessageButton(SocketString(MyRoads.MyRoad(10).RoadExit, "2"))
            road10Enable = True
            Button_Road10_7.BackColor = SystemColors.Control
            Button_Road10_8.BackColor = Color.Red
            Button_Road10_10.Enabled = True
            clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-出口-亮红灯")

        End If
    End Sub
#End Region

#End Region

#Region "全部道闸控制"

    '控制标识：2 红灯 3 绿灯 4 抬杆 5 落杆
    Private Sub Button_RoadAll_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_1.Click
        SendMessageButton(SocketString(sendAllRoadEntranceStr, "4"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-入口-抬杆")


    End Sub

    Private Sub Button_RoadAll_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_2.Click

        SendMessageButton(SocketString(sendAllRoadEntranceStr, "5"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-入口-落杆")

    End Sub

    Private Sub Button_RoadAll_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_3.Click

        SendMessageButton(SocketString(sendAllRoadEntranceStr, "3"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-入口-亮绿灯")


        Button_RoadAll_3.BackColor = Color.Lime
        Button_RoadAll_4.BackColor = SystemColors.Control
        Button_RoadAll_10.Enabled = True

        If MyRoads.MyRoad(0).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(0).RoadEntrance, "3"))
            road0Enable = True
            Button_Road0_3.BackColor = Color.Lime
            Button_Road0_4.BackColor = SystemColors.Control
            Button_Road0_10.Enabled = True
        End If

        If MyRoads.MyRoad(1).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(1).RoadEntrance, "3"))
            road1Enable = True
            Button_Road1_3.BackColor = Color.Lime
            Button_Road1_4.BackColor = SystemColors.Control
            Button_Road1_10.Enabled = True
        End If

        If MyRoads.MyRoad(2).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(2).RoadEntrance, "3"))
            road2Enable = True
            Button_Road2_3.BackColor = Color.Lime
            Button_Road2_4.BackColor = SystemColors.Control
            Button_Road2_10.Enabled = True
        End If

        If MyRoads.MyRoad(3).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(3).RoadEntrance, "3"))
            road3Enable = True
            Button_Road3_3.BackColor = Color.Lime
            Button_Road3_4.BackColor = SystemColors.Control
            Button_Road3_10.Enabled = True
        End If

        If MyRoads.MyRoad(4).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(4).RoadEntrance, "3"))
            road4Enable = True
            Button_Road4_3.BackColor = Color.Lime
            Button_Road4_4.BackColor = SystemColors.Control
            Button_Road4_10.Enabled = True
        End If

        If MyRoads.MyRoad(5).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(5).RoadEntrance, "3"))
            road5Enable = True
            Button_Road5_3.BackColor = Color.Lime
            Button_Road5_4.BackColor = SystemColors.Control
            Button_Road5_10.Enabled = True
        End If

        If MyRoads.MyRoad(6).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(6).RoadEntrance, "3"))
            road6Enable = True
            Button_Road6_3.BackColor = Color.Lime
            Button_Road6_4.BackColor = SystemColors.Control
            Button_Road6_10.Enabled = True
        End If

        If MyRoads.MyRoad(7).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(7).RoadEntrance, "3"))
            road7Enable = True
            Button_Road7_3.BackColor = Color.Lime
            Button_Road7_4.BackColor = SystemColors.Control
            Button_Road7_10.Enabled = True
        End If

        If MyRoads.MyRoad(8).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(8).RoadEntrance, "3"))
            road8Enable = True
            Button_Road8_3.BackColor = Color.Lime
            Button_Road8_4.BackColor = SystemColors.Control
            Button_Road8_10.Enabled = True
        End If

        If MyRoads.MyRoad(9).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(9).RoadEntrance, "3"))
            road9Enable = True
            Button_Road9_3.BackColor = Color.Lime
            Button_Road9_4.BackColor = SystemColors.Control
            Button_Road9_10.Enabled = True
        End If


        If MyRoads.MyRoad(10).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(10).RoadEntrance, "3"))
            road10Enable = True
            Button_Road10_3.BackColor = Color.Lime
            Button_Road10_4.BackColor = SystemColors.Control
            Button_Road10_10.Enabled = True
        End If



    End Sub

    Private Sub Button_RoadAll_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_4.Click

        SendMessageButton(SocketString(sendAllRoadEntranceStr, "2"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-入口-亮红灯")

        Button_RoadAll_3.BackColor = SystemColors.Control
        Button_RoadAll_4.BackColor = Color.Red
        Button_RoadAll_10.Enabled = True

        If MyRoads.MyRoad(0).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(0).RoadEntrance, "2"))
            road0Enable = True
            Button_Road0_3.BackColor = SystemColors.Control
            Button_Road0_4.BackColor = Color.Red
            Button_Road0_10.Enabled = True
        End If

        If MyRoads.MyRoad(1).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(1).RoadEntrance, "2"))
            road1Enable = True
            Button_Road1_3.BackColor = SystemColors.Control
            Button_Road1_4.BackColor = Color.Red
            Button_Road1_10.Enabled = True
        End If

        If MyRoads.MyRoad(2).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(2).RoadEntrance, "2"))
            road2Enable = True
            Button_Road2_3.BackColor = SystemColors.Control
            Button_Road2_4.BackColor = Color.Red
            Button_Road2_10.Enabled = True
        End If

        If MyRoads.MyRoad(3).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(3).RoadEntrance, "2"))
            road3Enable = True
            Button_Road3_3.BackColor = SystemColors.Control
            Button_Road3_4.BackColor = Color.Red
            Button_Road3_10.Enabled = True
        End If

        If MyRoads.MyRoad(4).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(4).RoadEntrance, "2"))
            road4Enable = True
            Button_Road4_3.BackColor = SystemColors.Control
            Button_Road4_4.BackColor = Color.Red
            Button_Road4_10.Enabled = True
        End If

        If MyRoads.MyRoad(5).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(5).RoadEntrance, "2"))
            road5Enable = True
            Button_Road5_3.BackColor = SystemColors.Control
            Button_Road5_4.BackColor = Color.Red
            Button_Road5_10.Enabled = True
        End If

        If MyRoads.MyRoad(6).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(6).RoadEntrance, "2"))
            road6Enable = True
            Button_Road6_3.BackColor = SystemColors.Control
            Button_Road6_4.BackColor = Color.Red
            Button_Road6_10.Enabled = True
        End If

        If MyRoads.MyRoad(7).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(7).RoadEntrance, "2"))
            road7Enable = True
            Button_Road7_3.BackColor = SystemColors.Control
            Button_Road7_4.BackColor = Color.Red
            Button_Road7_10.Enabled = True
        End If

        If MyRoads.MyRoad(8).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(8).RoadEntrance, "2"))
            road8Enable = True
            Button_Road8_3.BackColor = SystemColors.Control
            Button_Road8_4.BackColor = Color.Red
            Button_Road8_10.Enabled = True
        End If

        If MyRoads.MyRoad(9).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(9).RoadEntrance, "2"))
            road9Enable = True
            Button_Road9_3.BackColor = SystemColors.Control
            Button_Road9_4.BackColor = Color.Red
            Button_Road9_10.Enabled = True
        End If

        If MyRoads.MyRoad(10).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(10).RoadEntrance, "2"))
            road10Enable = True
            Button_Road10_3.BackColor = SystemColors.Control
            Button_Road10_4.BackColor = Color.Red
            Button_Road10_10.Enabled = True
        End If



    End Sub

    Private Sub Button_RoadAll_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_5.Click
        SendMessageButton(SocketString(sendAllRoadExitStr, "4"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-出口-抬杆")

    End Sub

    Private Sub Button_RoadAll_6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_6.Click
        SendMessageButton(SocketString(sendAllRoadExitStr, "5"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-出口-落杆")

    End Sub

    Private Sub Button_RoadAll_7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_7.Click

        SendMessageButton(SocketString(sendAllRoadExitStr, "3"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-出口-亮绿灯")


        Button_RoadAll_7.BackColor = Color.Lime
        Button_RoadAll_8.BackColor = SystemColors.Control
        Button_RoadAll_10.Enabled = True

        If MyRoads.MyRoad(0).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(0).RoadExit, "3"))
            road0Enable = True
            Button_Road0_7.BackColor = Color.Lime
            Button_Road0_8.BackColor = SystemColors.Control
            Button_Road0_10.Enabled = True
        End If

        If MyRoads.MyRoad(1).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(1).RoadExit, "3"))
            road1Enable = True
            Button_Road1_7.BackColor = Color.Lime
            Button_Road1_8.BackColor = SystemColors.Control
            Button_Road1_10.Enabled = True
        End If

        If MyRoads.MyRoad(2).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(2).RoadExit, "3"))
            road2Enable = True
            Button_Road2_7.BackColor = Color.Lime
            Button_Road2_8.BackColor = SystemColors.Control
            Button_Road2_10.Enabled = True
        End If

        If MyRoads.MyRoad(3).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(3).RoadExit, "3"))
            road3Enable = True
            Button_Road3_7.BackColor = Color.Lime
            Button_Road3_8.BackColor = SystemColors.Control
            Button_Road3_10.Enabled = True
        End If

        If MyRoads.MyRoad(4).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(4).RoadExit, "3"))
            road4Enable = True
            Button_Road4_7.BackColor = Color.Lime
            Button_Road4_8.BackColor = SystemColors.Control
            Button_Road4_10.Enabled = True
        End If

        If MyRoads.MyRoad(5).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(5).RoadExit, "3"))
            road5Enable = True
            Button_Road5_7.BackColor = Color.Lime
            Button_Road5_8.BackColor = SystemColors.Control
            Button_Road5_10.Enabled = True
        End If

        If MyRoads.MyRoad(6).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(6).RoadExit, "3"))
            road6Enable = True
            Button_Road6_7.BackColor = Color.Lime
            Button_Road6_8.BackColor = SystemColors.Control
            Button_Road6_10.Enabled = True
        End If

        If MyRoads.MyRoad(7).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(7).RoadExit, "3"))
            road7Enable = True
            Button_Road7_7.BackColor = Color.Lime
            Button_Road7_8.BackColor = SystemColors.Control
            Button_Road7_10.Enabled = True
        End If

        If MyRoads.MyRoad(8).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(8).RoadExit, "3"))
            road8Enable = True
            Button_Road8_7.BackColor = Color.Lime
            Button_Road8_8.BackColor = SystemColors.Control
            Button_Road8_10.Enabled = True
        End If

        If MyRoads.MyRoad(9).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(9).RoadExit, "3"))
            road9Enable = True
            Button_Road9_7.BackColor = Color.Lime
            Button_Road9_8.BackColor = SystemColors.Control
            Button_Road9_10.Enabled = True
        End If

        If MyRoads.MyRoad(10).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(10).RoadExit, "3"))
            road10Enable = True
            Button_Road10_7.BackColor = Color.Lime
            Button_Road10_8.BackColor = SystemColors.Control
            Button_Road10_10.Enabled = True
        End If



    End Sub

    Private Sub Button_RoadAll_8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_8.Click

        SendMessageButton(SocketString(sendAllRoadExitStr, "2"))
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：全部道闸" & "-出口-亮红灯")

        Button_RoadAll_7.BackColor = SystemColors.Control
        Button_RoadAll_8.BackColor = Color.Red
        Button_RoadAll_10.Enabled = True

        If MyRoads.MyRoad(0).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(0).RoadExit, "2"))
            road0Enable = True
            Button_Road0_7.BackColor = SystemColors.Control
            Button_Road0_8.BackColor = Color.Red
            Button_Road0_10.Enabled = True
        End If

        If MyRoads.MyRoad(1).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(1).RoadExit, "2"))
            road1Enable = True
            Button_Road1_7.BackColor = SystemColors.Control
            Button_Road1_8.BackColor = Color.Red
            Button_Road1_10.Enabled = True
        End If

        If MyRoads.MyRoad(2).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(2).RoadExit, "2"))
            road2Enable = True
            Button_Road2_7.BackColor = SystemColors.Control
            Button_Road2_8.BackColor = Color.Red
            Button_Road2_10.Enabled = True
        End If

        If MyRoads.MyRoad(3).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(3).RoadExit, "2"))
            road3Enable = True
            Button_Road3_7.BackColor = SystemColors.Control
            Button_Road3_8.BackColor = Color.Red
            Button_Road3_10.Enabled = True
        End If

        If MyRoads.MyRoad(4).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(4).RoadExit, "2"))
            road4Enable = True
            Button_Road4_7.BackColor = SystemColors.Control
            Button_Road4_8.BackColor = Color.Red
            Button_Road4_10.Enabled = True
        End If

        If MyRoads.MyRoad(5).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(5).RoadExit, "2"))
            road5Enable = True
            Button_Road5_7.BackColor = SystemColors.Control
            Button_Road5_8.BackColor = Color.Red
            Button_Road5_10.Enabled = True
        End If

        If MyRoads.MyRoad(6).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(6).RoadExit, "2"))
            road6Enable = True
            Button_Road6_7.BackColor = SystemColors.Control
            Button_Road6_8.BackColor = Color.Red
            Button_Road6_10.Enabled = True
        End If

        If MyRoads.MyRoad(7).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(7).RoadExit, "2"))
            road7Enable = True
            Button_Road7_7.BackColor = SystemColors.Control
            Button_Road7_8.BackColor = Color.Red
            Button_Road7_10.Enabled = True
        End If

        If MyRoads.MyRoad(8).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(8).RoadExit, "2"))
            road8Enable = True
            Button_Road8_7.BackColor = SystemColors.Control
            Button_Road8_8.BackColor = Color.Red
            Button_Road8_10.Enabled = True
        End If

        If MyRoads.MyRoad(9).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(9).RoadExit, "2"))
            road9Enable = True
            Button_Road9_7.BackColor = SystemColors.Control
            Button_Road9_8.BackColor = Color.Red
            Button_Road9_10.Enabled = True
        End If

        If MyRoads.MyRoad(10).Enable = True Then
            'SendMessage(SocketString(MyRoads.MyRoad(10).RoadExit, "2"))
            road10Enable = True
            Button_Road10_7.BackColor = SystemColors.Control
            Button_Road10_8.BackColor = Color.Red
            Button_Road10_10.Enabled = True
        End If

    End Sub
#End Region

#Region "恢复按钮"

    Private Sub Button_Road0_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_10.Click
        road0Enable = False
        Button_Road0_3.BackColor = SystemColors.Control
        Button_Road0_4.BackColor = SystemColors.Control
        Button_Road0_7.BackColor = SystemColors.Control
        Button_Road0_8.BackColor = SystemColors.Control
        Button_Road0_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(0).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road1_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_10.Click
        road1Enable = False
        Button_Road1_3.BackColor = SystemColors.Control
        Button_Road1_4.BackColor = SystemColors.Control
        Button_Road1_7.BackColor = SystemColors.Control
        Button_Road1_8.BackColor = SystemColors.Control
        Button_Road1_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(1).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road2_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_10.Click
        road2Enable = False
        Button_Road2_3.BackColor = SystemColors.Control
        Button_Road2_4.BackColor = SystemColors.Control
        Button_Road2_7.BackColor = SystemColors.Control
        Button_Road2_8.BackColor = SystemColors.Control
        Button_Road2_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(2).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road3_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_10.Click
        road3Enable = False
        Button_Road3_3.BackColor = SystemColors.Control
        Button_Road3_4.BackColor = SystemColors.Control
        Button_Road3_7.BackColor = SystemColors.Control
        Button_Road3_8.BackColor = SystemColors.Control
        Button_Road3_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(3).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road4_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_10.Click
        road4Enable = False
        Button_Road4_3.BackColor = SystemColors.Control
        Button_Road4_4.BackColor = SystemColors.Control
        Button_Road4_7.BackColor = SystemColors.Control
        Button_Road4_8.BackColor = SystemColors.Control
        Button_Road4_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(4).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road5_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_10.Click
        road5Enable = False
        Button_Road5_3.BackColor = SystemColors.Control
        Button_Road5_4.BackColor = SystemColors.Control
        Button_Road5_7.BackColor = SystemColors.Control
        Button_Road5_8.BackColor = SystemColors.Control
        Button_Road5_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(5).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road6_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_10.Click
        road6Enable = False
        Button_Road6_3.BackColor = SystemColors.Control
        Button_Road6_4.BackColor = SystemColors.Control
        Button_Road6_7.BackColor = SystemColors.Control
        Button_Road6_8.BackColor = SystemColors.Control
        Button_Road6_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(6).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road7_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_10.Click
        road7Enable = False
        Button_Road7_3.BackColor = SystemColors.Control
        Button_Road7_4.BackColor = SystemColors.Control
        Button_Road7_7.BackColor = SystemColors.Control
        Button_Road7_8.BackColor = SystemColors.Control
        Button_Road7_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(7).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road8_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_10.Click
        road8Enable = False
        Button_Road8_3.BackColor = SystemColors.Control
        Button_Road8_4.BackColor = SystemColors.Control
        Button_Road8_7.BackColor = SystemColors.Control
        Button_Road8_8.BackColor = SystemColors.Control
        Button_Road8_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(8).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road9_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_10.Click
        road9Enable = False
        Button_Road9_3.BackColor = SystemColors.Control
        Button_Road9_4.BackColor = SystemColors.Control
        Button_Road9_7.BackColor = SystemColors.Control
        Button_Road9_8.BackColor = SystemColors.Control
        Button_Road9_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(9).RoadName & "-恢复")

    End Sub

    Private Sub Button_Road10_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_10.Click
        road10Enable = False
        Button_Road10_3.BackColor = SystemColors.Control
        Button_Road10_4.BackColor = SystemColors.Control
        Button_Road10_7.BackColor = SystemColors.Control
        Button_Road10_8.BackColor = SystemColors.Control
        Button_Road10_10.Enabled = False
        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & MyRoads.MyRoad(10).RoadName & "-恢复")

    End Sub

    Private Sub Button_RoadAll_10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RoadAll_10.Click

        Button_RoadAll_3.BackColor = SystemColors.Control
        Button_RoadAll_4.BackColor = SystemColors.Control
        Button_RoadAll_7.BackColor = SystemColors.Control
        Button_RoadAll_8.BackColor = SystemColors.Control
        Button_RoadAll_10.Enabled = False

        road0Enable = False
        Button_Road0_3.BackColor = SystemColors.Control
        Button_Road0_4.BackColor = SystemColors.Control
        Button_Road0_7.BackColor = SystemColors.Control
        Button_Road0_8.BackColor = SystemColors.Control
        Button_Road0_10.Enabled = False

        road1Enable = False
        Button_Road1_3.BackColor = SystemColors.Control
        Button_Road1_4.BackColor = SystemColors.Control
        Button_Road1_7.BackColor = SystemColors.Control
        Button_Road1_8.BackColor = SystemColors.Control
        Button_Road1_10.Enabled = False

        road2Enable = False
        Button_Road2_3.BackColor = SystemColors.Control
        Button_Road2_4.BackColor = SystemColors.Control
        Button_Road2_7.BackColor = SystemColors.Control
        Button_Road2_8.BackColor = SystemColors.Control
        Button_Road2_10.Enabled = False

        road3Enable = False
        Button_Road3_3.BackColor = SystemColors.Control
        Button_Road3_4.BackColor = SystemColors.Control
        Button_Road3_7.BackColor = SystemColors.Control
        Button_Road3_8.BackColor = SystemColors.Control
        Button_Road3_10.Enabled = False

        road4Enable = False
        Button_Road4_3.BackColor = SystemColors.Control
        Button_Road4_4.BackColor = SystemColors.Control
        Button_Road4_7.BackColor = SystemColors.Control
        Button_Road4_8.BackColor = SystemColors.Control
        Button_Road4_10.Enabled = False

        road5Enable = False
        Button_Road5_3.BackColor = SystemColors.Control
        Button_Road5_4.BackColor = SystemColors.Control
        Button_Road5_7.BackColor = SystemColors.Control
        Button_Road5_8.BackColor = SystemColors.Control
        Button_Road5_10.Enabled = False

        road6Enable = False
        Button_Road6_3.BackColor = SystemColors.Control
        Button_Road6_4.BackColor = SystemColors.Control
        Button_Road6_7.BackColor = SystemColors.Control
        Button_Road6_8.BackColor = SystemColors.Control
        Button_Road6_10.Enabled = False

        road7Enable = False
        Button_Road7_3.BackColor = SystemColors.Control
        Button_Road7_4.BackColor = SystemColors.Control
        Button_Road7_7.BackColor = SystemColors.Control
        Button_Road7_8.BackColor = SystemColors.Control
        Button_Road7_10.Enabled = False

        road8Enable = False
        Button_Road8_3.BackColor = SystemColors.Control
        Button_Road8_4.BackColor = SystemColors.Control
        Button_Road8_7.BackColor = SystemColors.Control
        Button_Road8_8.BackColor = SystemColors.Control
        Button_Road8_10.Enabled = False

        road9Enable = False
        Button_Road9_3.BackColor = SystemColors.Control
        Button_Road9_4.BackColor = SystemColors.Control
        Button_Road9_7.BackColor = SystemColors.Control
        Button_Road9_8.BackColor = SystemColors.Control
        Button_Road9_10.Enabled = False

        road10Enable = False
        Button_Road10_3.BackColor = SystemColors.Control
        Button_Road10_4.BackColor = SystemColors.Control
        Button_Road10_7.BackColor = SystemColors.Control
        Button_Road10_8.BackColor = SystemColors.Control
        Button_Road10_10.Enabled = False

        clsLog.MessageLog(LogStr, "用户：" & UserName & " 执行：" & "全部道闸" & "-恢复")

    End Sub

#End Region

#Region "查看道路状态"

    Private Sub Button_Road0_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road0_9.Click

        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(0).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road1_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road1_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(1).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road2_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road2_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(2).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road3_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road3_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(3).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road4_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road4_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(4).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road5_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road5_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(5).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road6_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road6_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(6).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road7_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road7_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(7).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road8_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road8_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(8).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road9_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road9_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(9).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

    Private Sub Button_Road10_9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Road10_9.Click
        Dim roadInfo As String = ""
        roadInfo = MyRoads.MyRoad(10).RoadNum
        Dim MyfrmCarStatus As New frmCarStatus(roadInfo)
        MyfrmCarStatus.ShowDialog()
    End Sub

#End Region

    Private Sub Radio_Road_Manu_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_Road0_1.CheckedChanged, RadioButton20.CheckedChanged, Radio_Road9_1.CheckedChanged, Radio_Road8_1.CheckedChanged, Radio_Road7_1.CheckedChanged, Radio_Road6_1.CheckedChanged, Radio_Road5_1.CheckedChanged, Radio_Road4_1.CheckedChanged, Radio_Road3_1.CheckedChanged, Radio_Road2_1.CheckedChanged, Radio_Road10_1.CheckedChanged, Radio_Road1_1.CheckedChanged
        If CType(sender, RadioButton).Checked Then
            Dim radioName As String = ""
            Dim roadName As String = ""
            Dim radioNum As Integer = 0
            radioName = CType(sender, RadioButton).Name
            Dim nameList(3) As String
            nameList = radioName.Split("_")
            If nameList.Length < 3 Then
                Return
            End If
            roadName = nameList(1)
            radioNum = CInt(roadName.Substring(4))
            modMain.RadioStaus(radioNum) = CType(sender, RadioButton).Checked
            'SendMessage(SocketString(MyRoads.MyRoad(radioNum).RoadEntrance, "2"))
            SendMessage(SocketString(MyRoads.MyRoad(radioNum).RoadEntrance, "6"))
            'SendMessage(SocketString(MyRoads.MyRoad(radioNum).RoadEntrance, "4"))
            'SendMessage(SocketString(MyRoads.MyRoad(radioNum).RoadEntrance, "5"))
        End If

    End Sub

    Private Sub ButtonUpdateRoadNum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            ''numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            current = CType(label_labelCurrent.Text.ToString(), Integer)
            roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            updateNum = CType(text_updateNum.Text.ToString(), Integer)

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If

            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub Radio_Road_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_Road0_2.CheckedChanged, Radio_Road9_2.CheckedChanged, Radio_Road8_2.CheckedChanged, Radio_Road7_2.CheckedChanged, Radio_Road6_2.CheckedChanged, Radio_Road5_2.CheckedChanged, Radio_Road4_2.CheckedChanged, Radio_Road3_2.CheckedChanged, Radio_Road2_2.CheckedChanged, Radio_Road10_2.CheckedChanged, Radio_Road1_2.CheckedChanged
        If CType(sender, RadioButton).Checked = True Then
            Dim radioName As String = ""
            Dim roadName As String = ""
            Dim radioNum As Integer = 0
            radioName = CType(sender, RadioButton).Name
            Dim nameList(3) As String
            nameList = radioName.Split("_")
            If nameList.Length < 3 Then
                Return
            End If
            roadName = nameList(1)
            radioNum = CInt(roadName.Substring(4))
            modMain.RadioStaus(radioNum) = CType(sender, RadioButton).Checked
            SendMessage(SocketString(MyRoads.MyRoad(radioNum).RoadEntrance, "7"))
        End If
    End Sub

    ''' <summary>
    ''' 修改当前场内道路内车辆
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function updateRoadNum(ByVal roadNum As String, ByVal num As Integer, ByVal current As Integer, ByVal updateNum As Integer, ByVal roadName As String, ByVal ipAddr As String) As Boolean
        Dim strsql As String
        Dim strsqllog As String
        Dim dt As DataTable = Nothing
        Dim queryList As New List(Of String)
        DBserver = New clsOracle(connStr)
        Try
            If roadNum = Nothing Then
                Return True
            End If
            strsql = "update T_BASIS_ROADINFO set RESAVEDS3_S = " & updateNum.ToString() & "  where 0=0 and " + " roadid_s = '" + roadNum + "'"
            strsqllog = "insert into t_system_updateroadnum(user_s,currentnum_s,updatenum_s,roadnum_s,updatetime_t,roadname_s,ipaddr_s) values('" & userName & "','" & current.ToString() & "','" & updateNum.ToString() & "','" & num.ToString() & "',to_date('" & Now.ToString() & "','YYYY-MM-DD HH24:MI:SS'),'" & roadName.ToString() & "','" & ipAddr & "')"
            'strsql = strsql + sqlSearchWhereStr
            queryList.Add(strsql)
            queryList.Add(strsqllog)
            DBserver.Execute(queryList)
            Return True

        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)
            dt = Nothing
            Return False

        End Try
    End Function

    Private Sub Button_selectUpdateRoadNum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_selectUpdateRoadNum.Click
        Dim dt_query As DataTable
        dt_query = UpdateRoadNumQuery()
        If dt_query.Rows.Count > 0 Then
            DataGridView_UpdateRoadNum.DataSource = dt_query
        End If
    End Sub
#Region "处理修改车辆数量"

    Private Sub ButtonUpdateRoadNum4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub4.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road4_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road4_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum4.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub5.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            ''numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road5_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road5_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum5.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub



    Private Sub ButtonUpdateRoadNum8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub8.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road8_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road8_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum8.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub1.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road1_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road1_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum1.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If

            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub2.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            current = CType(Label_Road2_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road2_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum2.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub3.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road3_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road3_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum3.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub6.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road6_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road6_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum6.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub7.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            current = CType(Label_Road7_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road7_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum7.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub9.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            current = CType(Label_Road9_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road9_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum9.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If

            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub10.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            current = CType(Label_Road10_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road10_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum10.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)

            current = CType(label_labelCurrent.Text.ToString(), Integer)
            roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If

            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            'numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)

            current = CType(label_labelCurrent.Text.ToString(), Integer)
            roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            ' updateNum = 'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If

            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub

    Private Sub ButtonUpdateRoadNum0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_sub0.Click
        Dim buttonName As String = ""
        Dim control_labelCurrent As String = ""
        Dim control_labelUpdateNum As String = ""
        Dim control_labelRoadNum As String = ""
        Dim control_groupBox As String = ""
        Dim control_textUpdateNum As String = ""
        Dim label_labelCurrent As Label
        'Dim 'numericUpDown_labelUpdateNum As NumericUpDown
        Dim label_labelRoadNum As Label
        Dim label_groupBox As Label
        Dim text_updateNum As TextBox
        Dim buttonNum As Integer = 0
        Dim current As Integer = 0
        Dim roadNum As Integer = 0
        Dim updateNum As Integer = 0
        Try
            buttonName = CType(sender, Button).Name
            buttonNum = CType(buttonName.Substring(10), Integer)
            control_labelCurrent = "Label_Road" & CType(buttonNum, String) & "_Current"
            control_labelUpdateNum = "NumericUpDown_updateNum" & CType(buttonNum, String) & ""
            control_labelRoadNum = "Label_Road" & CType(buttonNum, String) & "_Max"
            control_groupBox = "GroupBox_Road_" & CType(buttonNum, String) & ""
            control_textUpdateNum = "TextBox_updateNum" & CType(buttonNum, String) & ""

            label_labelCurrent = CType(Me.Controls.Item(0).Controls.Find(control_labelCurrent, True)(0), Object)
            ''numericUpDown_labelUpdateNum = CType(Me.Controls.Item(0).Controls.Find(control_labelUpdateNum, True)(0), Object)
            label_labelRoadNum = CType(Me.Controls.Item(0).Controls.Find(control_labelRoadNum, True)(0), Object)
            text_updateNum = CType(Me.Controls.Item(0).Controls.Find(control_textUpdateNum, True)(0), Object)

            'current = CType(label_labelCurrent.Text.ToString(), Integer)
            'roadNum = CType(label_labelRoadNum.Text.ToString(), Integer)
            'updateNum = 'numericUpDown_labelUpdateNum.Value
            'updateNum =  CType(text_updateNum.Text.ToString(), Integer)

            If text_updateNum.Text.ToString() = "" Then
                Return
            End If

            current = CType(Label_Road0_Current.Text.ToString(), Integer)
            roadNum = CType(Label_Road0_Max.Text.ToString(), Integer)
            updateNum = CType(TextBox_updateNum0.Text.ToString(), Integer) ' NumericUpDown_updateNum0.Value '  'numericUpDown_labelUpdateNum.Value

            If openCheck = 1 Then
                Dim fu = New frmUser()
                fu.ShowDialog()
                If fu.returnValue = 1 Then
                    If fu.returnUserName <> "" Then
                        userName = fu.returnUserName
                    End If
                Else
                    Return
                End If
            End If


            If MessageBox.Show("是否确认修正数据,当前数量" & current.ToString() & ",将修改为" & updateNum.ToString(), "控制台", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                UpdateRoadNum(MyRoads.MyRoad(buttonNum).RoadNum, roadNum, current, updateNum, MyRoads.MyRoad(buttonNum).RoadName, strLocalIP)
            End If
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "RoadCarQuantity:" & ex.Message)

        End Try
    End Sub
#End Region

#Region "限制按钮只能输入数字 和退格"
    Private Sub TextBox_updateNum0_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum0.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum1.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum2.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum3_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum3.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum4_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum4.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum5_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum5.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum6_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum6.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum7_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum7.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum8_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum8.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum9_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum9.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox_updateNum10_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_updateNum10.KeyPress
        If IsNumeric(e.KeyChar) Then

        Else
            If Asc(e.KeyChar) = "8" Then

            Else
                e.Handled = True
            End If
        End If
    End Sub

#End Region

End Class