Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Threading
Public Class clsLog


    Public Shared Sub MessageLog(ByRef logStr As String, ByRef info As String)
        SyncLock messagesLock
            Try
                Dim txtwrites1 As StreamWriter = File.AppendText(logStr & "\" & Now.Date.Year.ToString & Now.Date.Month.ToString() & Now.Date.Day.ToString() & ".txt")
                txtwrites1.WriteLine("{0}:{1}", Now, info)
                txtwrites1.Flush()
                txtwrites1.Close()
                'End If
            Catch ex As Exception

            End Try
        End SyncLock
   
    End Sub

    Public Shared Sub MessageLog(ByRef info As String)
        SyncLock messagesLock
            Try
                'Dim txtwrites1 As StreamWriter =
                'txtwrites1.WriteLine("{0}", info)
                'txtwrites1.Flush()
                'txtwrites1.Close()
                File.WriteAllText("C:\\roadStatus.txt", info)
                'End If
            Catch ex As Exception

            End Try
        End SyncLock

    End Sub


    Public Shared Function ReadFileLine(ByVal aFileName As String) As String
        Dim bb As IO.StreamReader = Nothing
        Dim sline As String
        Dim i As Integer = 0
        Dim mContent As String = ""
        bb = New IO.StreamReader(aFileName)
        sline = bb.ReadLine()  '逐行读取
        'While Not bb.EndOfStream
        '    'tTbl = New DataTable

        '    If Not Trim(sline) = "" Then
        '        mContent = sline.Substring(0, 55).ToString  '按长度截取所需要的值
        '    End If
        'End While
        Return sline
    End Function





End Class
