Imports System.ComponentModel
Imports System.Threading
Imports System.IO




Public Class Main_Screen

    Private Sub Error_Handler(ByVal ex As Exception, Optional ByVal identifier_msg As String = "")
        Try
            Dim Display_Message1 As New Display_Message()
            Display_Message1.Message_Textbox.Text = "The Application encountered the following problem: " & vbCrLf & identifier_msg & ": " & ex.Message.ToString
            Display_Message1.Timer1.Interval = 1000
            Display_Message1.ShowDialog()
            Display_Message1.Dispose()
            Display_Message1 = Nothing
            If My.Computer.FileSystem.DirectoryExists((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs") = False Then
                My.Computer.FileSystem.CreateDirectory((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs")
            End If
            Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs\" & Format(Now(), "yyyyMMdd") & "_Error_Log.txt", True)
            filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & identifier_msg & ": " & ex.ToString)
            filewriter.Flush()
            filewriter.Close()
            filewriter = Nothing
            ex = Nothing
            identifier_msg = Nothing
        Catch exc As Exception
            MsgBox("An error occurred in the application's error handling routine. The application will try to recover from this serious error.", MsgBoxStyle.Critical, "Critical Error Encountered")
        End Try
    End Sub

   


    Private Sub cancelAsyncButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.BackgroundWorker1.CancelAsync()
        Catch ex As Exception
            Error_Handler(ex, "cancelAsyncButton_Click")
        End Try
    End Sub

  


    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
            Try
                Process.Start("""" & e.Argument & """")

                e.Result = ""
            Catch ex As Exception
                Error_Handler(ex, "backgroundWorker1_DoWork")
            End Try
            Try
                If Not worker Is Nothing Then
                    worker.Dispose()
                End If
                sender = Nothing
                e = Nothing
            Catch ex As Exception
                Error_Handler(ex, "backgroundWorker1_DoWork")
            End Try
        Catch ex As Exception
            Error_Handler(ex, "backgroundWorker1_DoWork")
        End Try
    End Sub

    Private Sub backgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If Not (e.Error Is Nothing) Then
                Error_Handler(e.Error, "backgroundWorker1_RunWorkerCompleted")
            End If
            Me.Close()
        Catch ex As Exception
            Error_Handler(ex, "backgroundWorker1_RunWorkerCompleted")
        End Try
    End Sub

    
    Private Sub Main_Screen_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try

        Catch ex As Exception
            Error_Handler(ex, "Closed")
        End Try
    End Sub

    Private Sub Main_Screen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)
            Dim urlstring As String = ""
            If My.Computer.FileSystem.FileExists((Application.StartupPath & "\InputURL.txt").Replace("\\", "\")) = True Then
                Dim reader As StreamReader = New StreamReader((Application.StartupPath & "\InputURL.txt").Replace("\\", "\"))
                If reader.Peek <> -1 Then
                    urlstring = reader.ReadLine
                End If
                reader.Close()
                reader.Dispose()
                reader = Nothing
                Label1.Text = "URL: " & urlstring.ToUpper
                BackgroundWorker1.RunWorkerAsync(urlstring)
            Else
                Dim Display_Message1 As New Display_Message()
                Display_Message1.Message_Textbox.Text = "The Application encountered the following problem: " & vbCrLf & "Config File not found" & ": " & "The required config file 'InputURL.txt' could not be located in the application startup path."
                Display_Message1.Timer1.Interval = 1000
                Display_Message1.ShowDialog()
                Display_Message1.Dispose()
                Display_Message1 = Nothing
            End If
        Catch ex As Exception
            Error_Handler(ex, "Load")
        End Try
    End Sub

  
   


End Class
