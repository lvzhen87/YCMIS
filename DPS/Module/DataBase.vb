Imports System.Data.SqlClient

Public Class DataBase

    '数据库连接语句
    Private connStr = ""

    '定义数据库连接
    Private sqlConn As SqlClient.SqlConnection

    '数据库操作异常时的错误信息。
    Private strMessage As String

    ''' <summary>
    ''' 数据库操作异常时的错误信息。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Message() As String
        Get
            Return strMessage
        End Get
    End Property

    ''' <summary>
    ''' 连接数据库
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetConnection() As Boolean
        sqlConn = New System.Data.SqlClient.SqlConnection(connStrsql)

        If sqlConn.State = ConnectionState.Closed Then
            sqlConn.Open()              '打开数据库              
        End If

        Return 0
    End Function

    ''' <summary>
    ''' 关闭数据库
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseConnection()

        If sqlConn.State = ConnectionState.Open Then
            sqlConn.Close()         '关闭连接，释放资源  
        End If

        sqlConn = Nothing
    End Sub

    ''' <summary>
    ''' 查询数据，并返回DataSet,返回值为Nothing时数据库异常。
    ''' </summary>
    ''' <param name="strSql">查询语句</param>
    ''' <returns></returns>
    ''' <remarks></remarks>    
    Public Function Search(ByVal strSql As String) As DataSet
        Return _Search(strSql)
    End Function
    Private Function _Search(ByVal strSql As String) As DataSet
        Dim DS As New DataSet
        Dim SqlDataAdapter As SqlDataAdapter
        strMessage = ""

        Try
            GetConnection()         '打开数据库连接

            SqlDataAdapter = New SqlDataAdapter(strSql, sqlConn)

            SqlDataAdapter.Fill(DS)
        Catch ex As Exception
            clsLog.MessageLog(LogStr, "_Search:" & ex.Message)
            'MsgBox(ex.Message.ToString)
            DS = Nothing
        End Try

        CloseConnection()           '关闭数据库连接

        SqlDataAdapter = Nothing

        Return DS

    End Function

    ''' <summary>
    ''' 添加数据,返回值为布尔类型,True:数据添加成功; False:数据添加失败。
    ''' </summary>
    ''' <param name="strSql">插入语句。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Insert(ByVal strSql As String) As Boolean
        Return _Insert(strSql)
    End Function
    Private Function _Insert(ByVal strSql As String) As Boolean
        Dim SqlCommand As New SqlCommand
        Dim blState As Boolean = True
        strMessage = ""

        Try
            GetConnection()         '打开数据库连接

            SqlCommand.Connection = sqlConn

            SqlCommand.CommandText = Trim(strSql)

            SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
            blState = False
        End Try

        CloseConnection()           '关闭数据库连接

        SqlCommand = Nothing

        Return blState

    End Function

    ''' <summary>
    ''' 更新数据,返回值为布尔类型,True:数据更新成功; False:数据更新失败。
    ''' </summary>
    ''' <param name="strSql">更新语句。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Update(ByVal strSql As String) As Boolean
        Return _Update(strSql)
    End Function
    Private Function _Update(ByVal strSql As String) As Boolean
        Dim SqlCommand As New SqlCommand
        Dim blState As Boolean = True
        strMessage = ""

        Try
            GetConnection()         '打开数据库连接

            SqlCommand.Connection = sqlConn

            SqlCommand.CommandText = Trim(strSql)

            SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
            blState = False
        End Try

        CloseConnection()           '关闭数据库连接

        SqlCommand = Nothing

        Return blState

    End Function

    ''' <summary>
    ''' 删除数据,返回值为布尔类型,True:数据删除成功; False:数据删除失败。
    ''' </summary>
    ''' <param name="strSql">删除语句。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete(ByVal strSql As String) As Boolean
        Return _Delete(strSql)
    End Function
    Private Function _Delete(ByVal strSql As String) As Boolean
        Dim SqlCommand As New SqlCommand
        Dim blState As Boolean = True
        strMessage = ""

        Try
            GetConnection()         '打开数据库连接

            SqlCommand.Connection = sqlConn

            SqlCommand.CommandText = Trim(strSql)

            SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
            blState = False
        End Try

        CloseConnection()           '关闭数据库连接

        SqlCommand = Nothing

        Return blState

    End Function

    ''' <summary>
    ''' 执行多个数据库语句,具有事务回滚作用,返回值为布尔类型,True:执行成功; False:执行失败。
    ''' </summary>
    ''' <param name="strSqlList">执行语句集。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal strSqlList As List(Of String)) As Boolean
        Return _Execute(strSqlList)
    End Function
    Private Function _Execute(ByVal strSqlList As List(Of String)) As Boolean

        Dim SqlCmd As SqlCommand = Nothing
        Dim SqlTran As SqlTransaction = Nothing
        Dim blState As Boolean = True
        strMessage = ""
        Try
            GetConnection()         '打开数据库连接
            SqlCmd = sqlConn.CreateCommand()
            SqlTran = sqlConn.BeginTransaction(IsolationLevel.ReadCommitted)
            SqlCmd.Transaction = SqlTran

            Dim cmdText As String
            For Each cmdText In strSqlList
                SqlCmd.CommandText = Trim(cmdText)
                SqlCmd.ExecuteNonQuery()
            Next
            SqlTran.Commit()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
            If Not SqlTran Is Nothing Then
                SqlTran.Rollback()
            End If
            blState = False
        End Try

        CloseConnection()           '关闭数据库连接

        strSqlList = Nothing
        SqlTran = Nothing
        SqlCmd = Nothing

        Return blState

    End Function

End Class