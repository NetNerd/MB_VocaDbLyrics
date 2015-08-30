Public Class Updates
    Private Shared UpdateThread As New Threading.Thread(New Threading.ThreadStart(AddressOf CheckerThreadStuff))
    Private Shared WebProx As Net.WebProxy
    Private Shared VerMajor As Integer
    Private Shared VerMinor As Integer
    Private Shared UILang As LanguageClass.Language
    Public Shared LastUpdate As New DateTime(0)

    Public Shared Sub UpdateCheck(Proxy As Net.WebProxy, VersionMajor As Integer, VersionMinor As Integer, StoragePath As String, UILanguage As LanguageClass.Language)
        If UpdateThread.IsAlive = False Then
            WebProx = Proxy
            VerMajor = VersionMajor
            VerMinor = VersionMinor
            UILang = UILanguage

            UpdateThread = New Threading.Thread(New Threading.ThreadStart(AddressOf CheckerThreadStuff)) With {.IsBackground = True}
            UpdateThread.Start()

            LastUpdate = DateTime.Now
            SettingsClass.SaveFile("LastUpdateCheck", StoragePath, LastUpdate.ToUniversalTime.ToString("u"), UILang)
        End If
    End Sub

    Public Shared Sub StopCheck()
        UpdateThread.Abort()
    End Sub

    Private Shared Sub CheckerThreadStuff() 'Pro level name right there
        Dim LatestVer() As Integer = GetLatestVersion()
        If (LatestVer(0) > VerMajor) OrElse (LatestVer(0) = VerMajor And LatestVer(1) > VerMinor) Then
            If MsgBox(
                LanguageClass.FallbackHelper(UILang.UpdateMsg, LanguageClass.LangEnUS.UpdateMsg) & vbNewLine & vbNewLine &
                LanguageClass.FallbackHelper(UILang.CurVer, LanguageClass.LangEnUS.CurVer) & VerMajor & "." & VerMinor & vbNewLine &
                LanguageClass.FallbackHelper(UILang.NewVer, LanguageClass.LangEnUS.NewVer) & LatestVer(0) & "." & LatestVer(1),
                MsgBoxStyle.YesNo) = MsgBoxResult.Yes _
                Then
                Try
                    Process.Start("https://github.com/NetNerd/MB_VocaDbLyrics/releases/latest")
                Catch
                End Try
            End If
        End If
    End Sub

    Private Shared Function GetLatestVersion() As Integer()
        Dim Releases() As String

        Try
            Releases = GetReleases().Split(",")
        Catch
            Return {0, 0}
        End Try

        For Each Line In Releases
            If Line.Trim.StartsWith("""tag_name""") Then
                Dim VerStr() As String = Line.Split("v".ToCharArray, 2)(1).Split(".".ToCharArray, 2)
                VerStr(1) = VerStr(1).Split("""".ToCharArray, 2)(0)

                Try
                    Return {VerStr(0), VerStr(1)}
                Catch
                    Return {0, 0}
                End Try
            End If
        Next
        Return {0, 0}
    End Function

    Private Shared Function GetReleases() As String
        Dim WebClient As New System.Net.WebClient
        Dim RtnStr As String

        WebClient.Headers.Add(Net.HttpRequestHeader.UserAgent, "MB_VocaDbLyrics Updater   (v" & VerMajor & "." & VerMinor & ")")
        WebClient.Headers.Add(Net.HttpRequestHeader.Accept, "application/vnd.github.v3+json")
        WebClient.Proxy = WebProx
        WebClient.Encoding = System.Text.Encoding.UTF8

        RtnStr = WebClient.DownloadString("https://api.github.com/repos/netnerd/MB_VocaDbLyrics/releases/latest")
        WebClient.Dispose()
        Return RtnStr
    End Function
End Class
