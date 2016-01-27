'Copyright © 2015 NetNerd


'This file is part of MB_VocaDbLyrics.

'MB_VocaDbLyrics is free software: you can redistribute it and/or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'MB_VocaDbLyrics Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with MB_VocaDbLyrics.  If Not, see < http: //www.gnu.org/licenses/>.



'This is a pretty messy file. Some of that is just because it's an interface to a program
'I don't really know the API of well And the rest Is because I'm good at creating messes.
'I'm going to try to comment stuff in it now, but that may not help too much.

Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms
Imports MusicBeePlugin.LanguageClass


Public Class Plugin
    Private mbApiInterface As New MusicBeeApiInterface
    Private about As New PluginInfo
    Private SettingsFolder As String = "MB_VocaDbLyrics"
    Private MySettings As New SettingsClass.SettingsCollection With {.LangBox1Items = {"Japanese"}, .LangBox2Items = {"Romaji", "English"}, .UILanguage = LangEnUS, .BlankCount = 5, .ForceArtistMatch = False, .UseOldArtistMatch = False, .UpdateChecking = True}

    Public Function Initialise(ByVal apiInterfacePtr As IntPtr) As PluginInfo
        'I'm not quite sure exactly what this does.
        'It seems like it's just simple initilisation, but done kinda weird.
        'Anyway, this is cleaner than what was in the sample plugin and works fine for me.
        CopyMemory(mbApiInterface, apiInterfacePtr, 4)
        If mbApiInterface.MusicBeeVersion > -1 Then
            CopyMemory(mbApiInterface, apiInterfacePtr, 456)
        End If

        about.Name = "VocaDB Lyrics Plugin"
        about.VersionMajor = 0
        about.VersionMinor = 6
        about.Revision = 0
        about.PluginInfoVersion = about.VersionMinor
        about.Description = "A lyrics provider for VocaDB and UtaiteDB.     (v" & about.VersionMajor & "." & about.VersionMinor & ")"
        about.Author = "NetNerd"
        about.TargetApplication = "VocaDB"
        about.Type = PluginType.LyricsRetrieval
        about.MinInterfaceVersion = MinInterfaceVersion
        about.MinApiRevision = 20
        about.ReceiveNotifications = ReceiveNotificationFlags.StartupOnly
        about.ConfigurationPanelHeight = 171
        Return about
    End Function

    Public Function Configure(ByVal panelHandle As IntPtr) As Boolean
        ' panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
        ' keep in mind the panel width is scaled according to the font the user has selected
        ' if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
        If panelHandle <> IntPtr.Zero Then
            Dim mbPanel As Panel = DirectCast(Panel.FromHandle(panelHandle), Panel)
            ConfigPanel.SetupControls(MySettings)
            mbPanel.Controls.AddRange(ConfigPanel.GetControls)
        End If

        Return True
    End Function

    ' called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
    ' its up to you to figure out whether anything has changed and needs updating
    Public Sub SaveSettings()
        Eggs.StopBeeps()
        If Control.IsKeyLocked(Keys.Scroll) Then
            If Control.ModifierKeys = Keys.Control Then
                Eggs.PlaySong(Eggs.WorldsEndDancehall)
            ElseIf Control.ModifierKeys = Keys.Alt Then
                Eggs.PlaySong(Eggs.KarakuriPierrot)
            End If
        End If

        ' Dim NewSettings As SettingsClass.SettingsCollection = ConfigPanel.GetSettings
        ' If Not NewSettings.BlankCount = Nothing Then MySettings = NewSettings

        MySettings = ConfigPanel.GetSettings

        ' save any persistent settings in a sub-folder of this path
        ' I don't know how MusicBee actually handles this in terms of changes over time, so I'll get the result again every time I need it.
        SettingsClass.SaveFile("Settings.conf", mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\", MySettings.MakeString({"LangBox1Items", "LangBox2Items", "UILanguage", "BlankCount", "ForceArtistMatch", "UseOldArtistMatch", "UpdateChecking"}), MySettings.UILanguage)
    End Sub

    ' MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
    Public Sub Close(ByVal reason As PluginCloseReason)
        Eggs.StopBeeps()
        Updates.StopCheck()
    End Sub

    ' uninstall this plugin - clean up any persisted files
    Public Sub Uninstall()
        Dim StoragePath As String = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"
        Try
            FileIO.FileSystem.DeleteFile(StoragePath & "Settings.conf")
            FileIO.FileSystem.DeleteDirectory(StoragePath, FileIO.DeleteDirectoryOption.ThrowIfDirectoryNonEmpty)
        Catch ex As Exception
            Dim Msg1 As String = FallbackHelper(MySettings.UILanguage.UninstallErrorMsg1, LangEnUS.UninstallErrorMsg1)
            Dim Msg2 As String = FallbackHelper(MySettings.UILanguage.UninstallErrorMsg2, LangEnUS.UninstallErrorMsg2)
            MsgBox(Msg1 & vbNewLine & vbNewLine & Msg2 & vbNewLine & StoragePath.TrimEnd("\".ToCharArray))
        End Try
    End Sub

    ' receive event notifications from MusicBee
    ' you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
    Public Sub ReceiveNotification(ByVal sourceFileUrl As String, ByVal type As NotificationType)
        ' perform some action depending on the notification type
        Select Case type
            Case NotificationType.PluginStartup
                ' perform startup initialisation
                Dim StoragePath As String = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"

                If FileIO.FileSystem.FileExists(StoragePath & "Settings.conf") Then
                    Try
                        MySettings.SetFromString(FileIO.FileSystem.ReadAllText(StoragePath & "Settings.conf"))
                    Catch
                    End Try
                End If

                If FileIO.FileSystem.FileExists(StoragePath & "LastUpdateCheck") Then
                    Try
                        Updates.LastUpdate = DateTime.Parse(FileIO.FileSystem.ReadAllText(StoragePath & "LastUpdateCheck"))
                    Catch
                    End Try
                End If
        End Select
    End Sub

    ' return an array of lyric or artwork provider names this plugin supports
    ' the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
    Public Function GetProviders() As String()
        Return New String() {"VocaDB", "UtaiteDB"}
    End Function

    ' return lyrics for the requested artist/title from the requested provider
    ' only required if PluginType = LyricsRetrieval
    ' return Nothing if no lyrics are found
    Public Function RetrieveLyrics(ByVal sourceFileUrl As String, ByVal artist As String, ByVal trackTitle As String, ByVal album As String, ByVal synchronisedPreferred As Boolean, ByVal provider As String) As String
        Dim WebProxy As Net.WebProxy = Nothing
        Try
            Dim Proxy() As String = mbApiInterface.Setting_GetWebProxy().Split(Convert.ToChar(0))
            If Proxy.Length > 0 Then
                WebProxy = New Net.WebProxy
                WebProxy.Address = New Uri(Proxy(0))
            End If
            If Proxy.Length > 2 Then
                WebProxy.Credentials = New Net.NetworkCredential(Proxy(1), Proxy(2))
            End If
        Catch
        End Try

        If MySettings.UpdateChecking Then
            If Updates.LastUpdate < DateTime.Now.AddDays(-1) Then
                Updates.UpdateCheck(WebProxy, about.VersionMajor, about.VersionMinor, mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\", MySettings.UILanguage)
            End If
        End If

        Dim LyricsLib As New VocaDbLyricsLib With {.UserAgent = "MB_VocaDbLyrics", .AppendDefaultUserAgent = True, .Proxy = WebProxy, .ForceArtistMatch = MySettings.ForceArtistMatch, .UseOldForceArtistMatch = MySettings.UseOldArtistMatch}
        If provider = "UtaiteDB" Then LyricsLib.DatabaseUrl = New Uri("http://utaitedb.net")
        Dim LyricsResult As VocaDbLyricsLib.LyricsResult = LyricsLib.GetLyricsFromName(trackTitle, artist)
        Dim LyricsWriter As New IO.StringWriter

        If LyricsResult.ErrorType = VocaDbLyricsLib.VocaDbLyricsError.None Then
            For Each Lang In MySettings.LangBox2Items
                For Each LyricsContainer In LyricsResult.LyricsContainers
                    If LyricsContainer.Language = Lang Then
                        'If LyricsWriter.ToString.Length > 0 Then LyricsWriter.Write(vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine)
                        If LyricsWriter.ToString.Length > 0 Then
                            For i = 0 To MySettings.BlankCount
                                LyricsWriter.Write(vbNewLine)
                            Next
                        End If
                        If MySettings.LangBox2Items.Count > 1 Then LyricsWriter.WriteLine(MySettings.UILanguage.LocalizeFromString(Lang) & ":")
                        LyricsWriter.Write(LyricsContainer.Lyrics)
                    End If
                Next
            Next
        Else
            Return Nothing
        End If

        If LyricsWriter.ToString.Length > 0 Then
            Return LyricsWriter.ToString.TrimEnd()
        End If
        Return Nothing
    End Function
End Class
