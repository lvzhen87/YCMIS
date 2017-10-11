Imports System.Net.Sockets
Public Class clsSocket

    Private _ServerIP As String
    Private _ServerPort As Integer

    'Const protNo As Integer = 500
    'Dim client As TcpClient
    'Dim data() As Byte

    Sub New(ByVal _ServerIP As String, ByVal _ServerPort As Integer)
        Me.ServerIP = _ServerIP
        Me.ServerPort = _ServerPort
    End Sub

    ''' <summary>
    ''' 服务器IP
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ServerIP() As String
        Get
            Return _ServerIP
        End Get
        Set(ByVal value As String)
            _ServerIP = value
        End Set
    End Property

    ''' <summary>
    ''' 服务器端口
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ServerPort() As Integer
        Get
            Return _ServerPort
        End Get
        Set(ByVal value As Integer)
            _ServerPort = value
        End Set
    End Property


    'Public Sub SendMessage(ByVal message As String)
    '    Try
    '        '---send a message to the server---
    '        Dim ns As NetworkStream = client.GetStream
    '        Dim data As Byte() = _
    '            System.Text.Encoding.ASCII.GetBytes(message)
    '        '---send the text---
    '        ns.Write(data, 0, data.Length)
    '        ns.Flush()
    '    Catch ex As Exception
    '        MsgBox(ex.ToString) 'test

    '    End Try
    'End Sub

    'Public Sub ReceiveMessage(ByVal ar As IAsyncResult)
    '    Try
    '        Dim bytesRead As Integer

    '        '---read the data from the server---
    '        bytesRead = client.GetStream.EndRead(ar)
    '        If bytesRead < 1 Then
    '            Exit Sub
    '        Else
    '            '---invoke the delegate to display the received data---
    '            Dim para() As Object = _
    '            {System.Text.Encoding.ASCII.GetString(data, 0, bytesRead)}
    '            Me.Invoke(New delUpdateHistory(AddressOf Me.UpdateHistory), para)
    '        End If
    '        '---continue reading...---
    '        client.GetStream.BeginRead( _
    '            data, 0, CInt(client.ReceiveBufferSize), _
    '            AddressOf ReceiveMessage, Nothing)
    '    Catch ex As Exception
    '        '---ignore the error;fired when a user off---
    '    End Try
    'End Sub

    ''---delegate and subroutine to update the textbox control
    'Public Delegate Sub delUpdateHistory(ByVal str As String)
    'Public Sub UpdateHistory(ByVal str As String)
    '    'txtMessageHistory.AppendText(str)
    'End Sub

    'Public Sub Disconnect()
    '    Try
    '        '---Disconnect from server---
    '        client.GetStream.Close()
    '        client.Close()
    '    Catch ex As Exception
    '        'MsgBox(ex.ToString)
    '        MsgBox("服务器通讯连接失败，请查看网络是否已连接")
    '    End Try
    'End Sub


#Region "Socket"

    'Dim client As TcpClient
    'Dim data() As Byte
    'Dim isSignIn As Boolean
    'Public isDownLoad As Boolean = False

    ' ''' <summary>
    ' ''' 登录
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Function SignIn() As Boolean
    '    Try
    '        client = New TcpClient
    '        client.Connect(MySocket.ServerIP, MySocket.ServerPort)
    '        ReDim data(client.ReceiveBufferSize)
    '        'SendMessage("K000|END")
    '        client.GetStream.BeginRead(data, 0, CInt(client.ReceiveBufferSize), AddressOf ReceiveMessage, Nothing)
    '        isSignIn = True
    '        isDownLoad = True
    '        Return True
    '    Catch ex As Exception
    '        isDownLoad = False
    '        MessageBox.Show("注册Socket失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        Return False
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' 发送消息
    ' ''' </summary>
    ' ''' <param name="message"></param>
    ' ''' <remarks></remarks>
    'Public Sub SendMessage(ByVal message As String)
    '    Try

    '        '---send a message to the server---
    '        Dim ns As NetworkStream = client.GetStream
    '        'Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
    '        Dim data As Byte() = System.Text.Encoding.UTF8.GetBytes(message + Chr(13) + Chr(10))
    '        '---send the text---
    '        ns.Write(data, 0, data.Length)

    '        ns.Flush()
    '    Catch ex As Exception
    '        isDownLoad = False
    '        MessageBox.Show("发送Socket消息失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try

    'End Sub

    ' ''' <summary>
    ' ''' 接收消息
    ' ''' </summary>
    ' ''' <param name="ar"></param>
    ' ''' <remarks></remarks>
    'Public Sub ReceiveMessage(ByVal ar As IAsyncResult)
    '    Try
    '        Dim bytesRead As Integer
    '        '---read the data from the server---
    '        bytesRead = client.GetStream.EndRead(ar)
    '        If bytesRead < 1 Then
    '            Exit Sub
    '        Else
    '            '---invoke the delegate to display the received data---
    '            Dim para() As Object = {System.Text.Encoding.UTF8.GetString(data, 0, bytesRead)}
    '            Me.Invoke(New delUpdateHistory(AddressOf Me.UpdateHistory), para)
    '        End If
    '        '---continue reading...---
    '        client.GetStream.BeginRead(data, 0, CInt(client.ReceiveBufferSize), AddressOf ReceiveMessage, Nothing)
    '    Catch ex As Exception
    '        '---ignore the error;fired when a user off---
    '    End Try
    'End Sub

    ''---delegate and subroutine to update the textbox control
    'Public Delegate Sub delUpdateHistory(ByVal str As String)

    ' ''' <summary>
    ' ''' 接收消息验证逻辑
    ' ''' </summary>
    ' ''' <param name="str"></param>
    ' ''' <remarks></remarks>
    'Public Sub UpdateHistory(ByVal str As String)
    '    If str = sendString Then
    '        If isSignIn = True Then
    '            SignOut()
    '        End If
    '    End If
    'End Sub

    'Public Sub SignOut()
    '    Try
    '        '服务器收到断开连接的信号发送离开的消息
    '        '---Disconnect from server---
    '        isSignIn = False
    '        isDownLoad = False
    '        client.GetStream.Close()
    '        client.Close()
    '    Catch ex As Exception
    '        isDownLoad = False
    '        MessageBox.Show("注销Socket失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    End Try
    'End Sub

#End Region


End Class
