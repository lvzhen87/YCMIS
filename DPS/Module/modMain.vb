
Imports System.Threading
Imports Microsoft.Win32
Imports System.Configuration
Module modMain
    Public LogStr As String '日志路径

    Public UserName As String = "" '用户名
    Public UserPwd As String = "" '用户密码
    Public userIDStr As String = "" '用户名配置文件
    Public userPwdStr As String = "" '用户密码配置文件
    Public updatePwdStr As String = "" '用户修改数量密码

    Public DeviceEnable As String = "1"  '0914配置控制自动锁定抬落杆。 1代表未启用0代表启用
    Public DeviceLock As String = "1"  '0917判断时间差落杆功能。 1代表未启用0代表启用
    Public strLocalIP As String = "塔台"
    Public openCheck As String = "0"
    Public startThreat As String = ""
    Public RadioStaus() As Boolean

    Public datediffSpan As Integer = 65

    Public messagesLock As New Object

    Public connStrsql As String = ""

    Public connStr As String
    Public sendString As String = ""
    'Public MyfrmMIS As New MIS
    Public MyfrmMIS As MIS
    Public MyfrmLogin As New frmLogin

    Public serverIP As String
    Public serverPort As Integer
    Public MyRoads As Roads
    Public sendAllRoadEntranceStr As String = ""
    Public sendAllRoadExitStr As String = ""
    Public sqlSearchWhereStr As String = ""
    Public cycleTime As Integer = 10
    Public threadCycle As Threading.Thread

    Public road0Enable As Boolean = False
    Public road1Enable As Boolean = False
    Public road2Enable As Boolean = False
    Public road3Enable As Boolean = False
    Public road4Enable As Boolean = False
    Public road5Enable As Boolean = False
    Public road6Enable As Boolean = False
    Public road7Enable As Boolean = False
    Public road8Enable As Boolean = False
    Public road9Enable As Boolean = False
    Public road10Enable As Boolean = False

    Structure Roads
        Dim MyRoad() As Road
    End Structure

    Structure Road
        'true_DL2014091000_3_4_外部噪声路
        Dim RoadID As String
        Dim Enable As Boolean
        Dim RoadNum As String
        Dim RoadEntrance As String
        Dim RoadExit As String
        Dim RoadName As String
    End Structure

    Sub Main()
        'Dim bExist As Boolean
        'Dim MyMutex As New Mutex(True, "MyfrmMIS", bExist)
        'If (bExist) Then
        '    Application.Run(MyfrmMIS)
        '    MyMutex.ReleaseMutex()
        'Else
        '    MsgBox("提示: 程序已经运行！", MsgBoxStyle.Exclamation)
        'End If

        Dim bExist As Boolean
        Dim MyMutex As New Mutex(True, "MyfrmLogin", bExist)
        If (bExist) Then
            Application.Run(MyfrmLogin)
            MyMutex.ReleaseMutex()
        Else
            MsgBox("提示: 程序已经运行！", MsgBoxStyle.Exclamation)
        End If
    End Sub




    ''' <summary>
    ''' 控制单个道闸
    ''' </summary>
    ''' <param name="barrierGateID">道闸ID</param>
    ''' <param name="schema">控制标识：2 红灯 3 绿灯 4 抬杆 5 落杆</param>
    ''' <returns></returns>
    ''' <remarks>"GATEORDER{""barrierGateID"":""7"",""schema"":5}" + "vbcrlf"</remarks>
    Public Function SocketString(ByVal barrierGateID As String, ByVal schema As String) As String
        Try
            'Dim mystr = "{[""GATEORDER"":""true""],[""barrierGateID"":""" + barrierGateID + """,""schema"":""" + schema + """]}#13#10"
            'Dim mystr = "GATEORDER{""barrierGateID"":""" + barrierGateID + """,""schema"":" + schema + "]}#13#10" + vbCrLf
            Dim mystr = "GATEORDER{""barrierGateID"":""" + barrierGateID + """,""schema"":" + schema + "}#13#10"

            Return mystr
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "SocketString:" & ex.Message)

            Return ""
        End Try
    End Function

    ''' <summary>
    ''' 控制所用道闸
    ''' </summary>
    ''' <param name="barrierGateID">道闸ID数组</param>
    ''' <param name="schema">控制标识：2 红灯 3 绿灯 4 抬杆 5 落杆</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SocketStringAll(ByVal barrierGateID As String(), ByVal schema As String) As String
        Try
            Dim barrierGateIDStr As String = ""
            For i = 0 To barrierGateID.Length - 1
                If i = barrierGateID.Length - 1 Then
                    barrierGateIDStr = barrierGateIDStr + barrierGateID(i)
                Else
                    barrierGateIDStr = barrierGateIDStr + barrierGateID(i) + ","
                End If
            Next
            Dim mystr = "GATEORDER{""barrierGateID"":""" + barrierGateIDStr + """,""schema"":" + schema + "}#13#10"
            Return mystr
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "SocketStringAll:" & ex.Message)

            Return ""
        End Try
    End Function


    Public Sub InitConfig()
        startThreat = System.Configuration.ConfigurationManager.AppSettings("startThreat")
        LogStr = System.Configuration.ConfigurationManager.AppSettings("logStr")
        'oracle数据库连接字符串
        connStr = System.Configuration.ConfigurationManager.AppSettings("connStr")
        connStrsql = System.Configuration.ConfigurationManager.AppSettings("connStrsql")
        datediffSpan = System.Configuration.ConfigurationManager.AppSettings("datediffSpan")

        'socket服务器IP及端口号
        serverIP = System.Configuration.ConfigurationManager.AppSettings("serverIP")
        serverPort = System.Configuration.ConfigurationManager.AppSettings("serverPort")
        cycleTime = System.Configuration.ConfigurationManager.AppSettings("cycleTime")

        userIDStr = System.Configuration.ConfigurationManager.AppSettings("userIDStr")
        userPwdStr = System.Configuration.ConfigurationManager.AppSettings("userPwdStr")
        updatePwdStr = System.Configuration.ConfigurationManager.AppSettings("updatePwdStr")
        strLocalIP = System.Configuration.ConfigurationManager.AppSettings("strLocalIP")
        openCheck = System.Configuration.ConfigurationManager.AppSettings("openCheck")
    End Sub


End Module
