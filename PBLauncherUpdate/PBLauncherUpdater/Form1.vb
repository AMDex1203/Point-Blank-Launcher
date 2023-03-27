Imports System.Net
Imports System.IO
Imports Shell32

Public Class Form1
    WithEvents wc As New WebClient
    Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        wc.DownloadFileTaskAsync(New Uri("http://newlauncher.rootpb.com/launcherupdate/updates/launcher_files/update.zip"), Application.StartupPath & "\update.zip")
    End Sub
    Private Sub wc_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wc.DownloadFileCompleted
        If e.Error IsNot Nothing Then
            MsgBox("An error occurred while downloading the update. Terminating the updater. (Error 0x01)")
            Me.Close()
            Return
        End If

        Dim _pasta As Folder = shObj.NameSpace(Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\")))
        Dim _arquivo As Folder = shObj.NameSpace(Application.StartupPath & "\update.zip")
        Label1.Text = "Extracting new launcher ..."
        Try
            _pasta.CopyHere(_arquivo.Items, 16 + 64 + 4)
            MsgBox("Full update. The new launcher will open.")
        Catch ex As Exception
            MsgBox("An error occurred while downloading the update. Terminating the updater.(Error 0x02)")
            Me.Close()
            Return
        End Try
        Try
            Process.Start(Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\")) & "\PBLauncher.exe", "vamos la")
        Catch

        End Try
        Me.Close()
    End Sub
    Private Sub wc_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles wc.DownloadProgressChanged
        ProgressBar1.Maximum = CInt(e.TotalBytesToReceive)
        ProgressBar1.Value = CInt(e.BytesReceived)
    End Sub
End Class