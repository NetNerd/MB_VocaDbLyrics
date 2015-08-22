Public Class Updates
    Private Shared UpdateThread As New Threading.Thread(New Threading.ThreadStart(AddressOf CheckerThreadStuff))
    Private Shared WebProx As Net.WebProxy
    Private Shared VerMajor As Integer
    Private Shared VerMinor As Integer
    Private Shared UILang As LanguageClass.Language
    Private Shared LastUpdate As New DateTime(0)

    Public Shared Sub UpdateCheck(Proxy As Net.WebProxy, VersionMajor As Integer, VersionMinor As Integer, UILanguage As LanguageClass.Language)
        If UpdateThread.IsAlive = False Then
            WebProx = Proxy
            VerMajor = VersionMajor
            VerMinor = VersionMinor
            UILang = UILanguage

            UpdateThread = New Threading.Thread(New Threading.ThreadStart(AddressOf CheckerThreadStuff)) With {.IsBackground = True}
            UpdateThread.Start()

            LastUpdate = DateTime.Now
        End If
    End Sub

    Private Shared Sub CheckerThreadStuff() 'Pro level name right there
        Dim LatestVer() As Integer = GetLatestVersion()
        If (LatestVer(0) > VerMajor) OrElse (LatestVer(0) = VerMajor And LatestVer(1) > VerMinor) Then
            If MsgBox(LanguageClass.FallbackHelper(UILang.UpdateMsg, LanguageClass.LangEnUS.UpdateMsg), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then ', "MB_VocaDbLyrics") Then
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

        RtnStr = WebClient.DownloadString("https://api.github.com/repos/netnerd/MB_VocaDbLyrics/releases")
        WebClient.Dispose()
        Return RtnStr
    End Function

    Public Shared Function LastCheckTime(StoragePath As String) As DateTime
        'Return New DateTime(0)

        If LastUpdate > New DateTime(0) Then
            Return LastUpdate
        End If

        If FileIO.FileSystem.FileExists(StoragePath & "LastUpdateCheck") Then
            Try
                Return DateTime.Parse(FileIO.FileSystem.ReadAllText(StoragePath & "LastUpdateCheck"))
            Catch
            End Try
        Else
            Return New DateTime(0)
        End If

        Return New DateTime(9999, 12, 31)
    End Function

    Public Shared Sub SaveLastUpdate(StoragePath As String)
        Try
            FileIO.FileSystem.WriteAllText(StoragePath & "LastUpdateCheck", LastUpdate.ToUniversalTime.ToString("u"), False)
        Catch ex As Exception
            Dim Msg As String = LanguageClass.FallbackHelper(UILang.SaveErrorMsg, LanguageClass.LangEnUS.SaveErrorMsg)
            MsgBox("LastUpdateCheck" & ":" & vbNewLine & Msg)
        End Try
    End Sub
End Class
