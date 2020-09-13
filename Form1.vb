

Public Class Form1


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        '管理者権限を確認
        Dim IsAdmin As Boolean = IsAdministrator()
        If IsAdmin = False Then
            MessageBox.Show("管理者権限が必要です。一度終了し、このファイルを右クリックし「管理者として実行」をクリックしてください。")
            Exit Sub
        End If



        'Processオブジェクトを作成
        Dim p As New System.Diagnostics.Process()

        'ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
        p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec")
        '出力を読み取れるようにする
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.RedirectStandardInput = False
        'ウィンドウを表示しないようにする
        p.StartInfo.CreateNoWindow = True


        'プロダクトキー入力
        'コマンドラインを指定し実行（"/c"は実行後閉じるために必要）cscript /Bオプションによりメッセージは非表示
        p.StartInfo.Arguments = "/c cscript /B %windir%\System32\slmgr.vbs /ipk xxxxx-xxxxx-xxxxx-xxxxx-xxxxx"
        p.Start()

        'results01に出力を読み取る（デバッグ用のため表示はしない）
        Dim results01 As String = p.StandardOutput.ReadToEnd()

        'プロセス終了まで待機する
        'WaitForExitはReadToEndの後である必要がある
        '(親プロセス、子プロセスでブロック防止のため)
        p.WaitForExit()
        p.Close()


        'ライセンス認証
        'コマンドラインを指定し実行（"/c"は実行後閉じるために必要）
        p.StartInfo.Arguments = "/c cscript %windir%\System32\slmgr.vbs /ato"
        p.Start()

        'results02に出力を読み取る
        Dim results02 As String = p.StandardOutput.ReadToEnd()

        'プロセス終了まで待機する
        'WaitForExitはReadToEndの後である必要がある
        '(親プロセス、子プロセスでブロック防止のため)
        p.WaitForExit()
        p.Close()

        '出力された結果を表示
        Console.WriteLine(results01)
        MessageBox.Show(results02)

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'Dim v As New System.Net.WebClient()
        'v.DownloadFile("https://www.osaka-u.ac.jp/ja/access/files/access_map2019.jpg", "accessmap.jpg")
        'v.Dispose()

        'ダウンロード元のURL
        Dim url As String = "https://www.osaka-u.ac.jp/ja/access/"

        'WebClientを作成
        Dim wc As New System.Net.WebClient()
        '文字コードを指定
        wc.Encoding = System.Text.Encoding.UTF8
        'データを文字列としてダウンロードする
        Dim source As String = wc.DownloadString(url)
        '後始末
        wc.Dispose()

        'ダウンロードしたデータを表示する

        Dim s10 As String = source.Substring(0, 10)

        Console.WriteLine(s10)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        '管理者権限を確認
        Dim IsAdmin As Boolean = IsAdministrator()
        MessageBox.Show(IsAdmin)


    End Sub

    Function IsAdministrator() As Boolean
        '現在のユーザーを表すWindowsIdentityオブジェクトを取得する
        Dim wi As System.Security.Principal.WindowsIdentity =
            System.Security.Principal.WindowsIdentity.GetCurrent()
        'WindowsPrincipalオブジェクトを作成する
        Dim wp As New System.Security.Principal.WindowsPrincipal(wi)
        'Administratorsグループに属しているか調べる
        Return wp.IsInRole(
            System.Security.Principal.WindowsBuiltInRole.Administrator)
    End Function


End Class