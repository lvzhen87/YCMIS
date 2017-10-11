Imports System.Data
Imports System.Data.OracleClient
Public Class clsOracle

    '数据库操作异常时的错误信息。
    Private strMessage As String
    '数据库连接语句。
    Private ConnectionString As String

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

    Sub New(ByVal Connectionstring As String)
        Me.ConnectionString = Connectionstring
    End Sub

    ''' <summary>
    ''' 返回查找的数据,返回值为DataSet类型,返回值为Nothing时数据库异常。
    ''' </summary>
    ''' <param name="strSql"> 查找语句。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal strSql As String) As DataSet
        Return _GetDataSet(strSql)
    End Function
    Private Function _GetDataSet(ByVal strSql As String) As DataSet

        Dim DataSet As New DataSet
        Dim OracleDataAdapter As OracleDataAdapter
        Dim OracleConnection As New OracleConnection
        strMessage = ""

        Try
            OracleConnection.ConnectionString = Connectionstring
            OracleDataAdapter = New OracleDataAdapter(strSql, OracleConnection)
            If OracleConnection.State = ConnectionState.Closed Then
                OracleConnection.Open()
            End If
            OracleDataAdapter.Fill(DataSet)
        Catch ex As Exception
            DataSet = Nothing
            MessageBox(ex.Message + Environment.NewLine + "连接数据库失败,请联系开发人员！", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "错误提示")
        End Try

        If OracleConnection.State = ConnectionState.Open Then
            OracleConnection.Close()
        End If

        OracleDataAdapter = Nothing
        OracleConnection = Nothing

        Return DataSet

    End Function


    ''' <summary>
    ''' 执行多个数据库语句,具有事务回滚作用,返回值为布尔类型,True:执行成功; False:执行失败。
    ''' </summary>
    ''' <param name="strSql">执行语句。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function Execute(ByVal strSql As List(Of String)) As Boolean
        Return _Execute(strSql)
    End Function
    Private Function _Execute(ByVal strSql As List(Of String)) As Boolean

        Dim OracleCn As New OracleConnection
        Dim OracleCmd As OracleCommand = Nothing
        Dim OracleTran As OracleTransaction = Nothing
        Dim blState As Boolean = True
        strMessage = ""
        Try
            OracleCn.ConnectionString = ConnectionString
            If OracleCn.State = ConnectionState.Closed Then
                OracleCn.Open()
            End If
            OracleCmd = OracleCn.CreateCommand()
            OracleTran = OracleCn.BeginTransaction(IsolationLevel.ReadCommitted)
            OracleCmd.Transaction = OracleTran

            Dim cmdText As String
            For Each cmdText In strSql
                OracleCmd.CommandText = Trim(cmdText)
                OracleCmd.ExecuteNonQuery()
            Next
            OracleTran.Commit()
        Catch ex As Exception
            If Not OracleTran Is Nothing Then
                OracleTran.Rollback()
            End If
            blState = False
            MessageBox(ex.Message + Environment.NewLine + "连接数据库失败,请联系开发人员！", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "错误提示")
        End Try

        If OracleCn.State = ConnectionState.Open Then
            OracleCn.Close()
        End If

        strSql = Nothing
        OracleTran = Nothing
        OracleCmd = Nothing
        OracleCn = Nothing

        Return blState

    End Function

    ''' <summary>
    ''' 信息框。
    ''' </summary>
    ''' <param name="Info">信息。</param>
    ''' <param name="MsgBoxStyle">样式。</param>
    ''' <remarks></remarks>
    Private Function MessageBox(ByVal Info As String, ByVal MsgBoxStyle As MsgBoxStyle, ByVal Title As String) As MsgBoxResult

        strMessage = Info
        If Title.Length < 1 Then
            '        Return MsgBox("提示: " & Info, MsgBoxStyle)
        Else
            '       Return MsgBox("提示: " & Info, MsgBoxStyle, Title)
        End If

    End Function

End Class
