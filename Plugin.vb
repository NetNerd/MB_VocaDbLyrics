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
    Private MySettings As New SettingsClass.SettingsCollection With {.LangBoxText = "rom/Romanized, ja/Japanese, orig/Original Language, en/English", .UILanguage = LangEnUS, .BlankCount = 5, .ForceArtistMatch = False, .UseOldArtistMatch = False, .ArtistWhitelist = "初音ミク,鏡音リン,鏡音レン,巡音ルカ,KAITO,MEIKO,GUMI,IA,結月ゆかり,重音テト,洛天依,神威がくぽ,猫村いろは,雪歌ユフ,SeeU,シユ,蒼姫ラピス,Lily,リリィ,SF-A2,v flower,さとうささら,波音リツ,MAYU,言和,乐正绫,ONE,音街ウナ,AVANNA,歌愛ユキ,VY1,CUL,カル,まふまふ,DECO,みきと,そらる,ギガ,花たん,YURiCa,ユリカ", .UpdateChecking = True}

    Private SpecialLanguages As Dictionary(Of String, String) =
        New Dictionary(Of String, String) From {{"orig", "Original"}, {"rom", "Romanized"}}

    Public Function Initialise(ByVal apiInterfacePtr As IntPtr) As PluginInfo
        'I'm not quite sure exactly what this does.
        'It seems like it's just simple initialisation, but done kinda weird.
        'Anyway, this is cleaner than what was in the sample plugin and works fine for me.
        CopyMemory(mbApiInterface, apiInterfacePtr, 4)
        If mbApiInterface.MusicBeeVersion > -1 Then
            CopyMemory(mbApiInterface, apiInterfacePtr, 456)
        End If

        about.Name = "VocaDB Lyrics Plugin"
        about.VersionMajor = 0
        about.VersionMinor = 9
        about.Revision = 2
        about.PluginInfoVersion = about.VersionMinor
        about.Description = "A lyrics provider for VocaDB, UtaiteDB, and TouhouDB.     (v" & about.VersionMajor & "." & about.VersionMinor & ")"
        about.Author = "NetNerd"
        about.TargetApplication = "VocaDB"
        about.Type = PluginType.LyricsRetrieval
        about.MinInterfaceVersion = MinInterfaceVersion
        about.MinApiRevision = 20
        about.ReceiveNotifications = ReceiveNotificationFlags.StartupOnly
        about.ConfigurationPanelHeight = 278
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
        SettingsClass.SaveFile("Settings.conf", mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\", MySettings.MakeString({"LangBoxText", "UILanguage", "BlankCount", "ForceArtistMatch", "UseOldArtistMatch", "ArtistWhitelist", "UpdateChecking"}), MySettings.UILanguage)
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
        Return New String() {"VocaDB", "UtaiteDB", "TouhouDB"}
    End Function

    ' return lyrics for the requested artist/title from the requested provider
    ' only required if PluginType = LyricsRetrieval
    ' return Nothing if no lyrics are found
    Public Function RetrieveLyrics(ByVal sourceFileUrl As String, ByVal artist As String, ByVal trackTitle As String, ByVal album As String, ByVal synchronisedPreferred As Boolean, ByVal provider As String) As String
        If Not GetProviders().Contains(provider) Then
            Return Nothing 'Are we being run for other providers??
        End If


        ' by default we only get the first artist. this gives us all artists.
        Dim multiartist As String
        If sourceFileUrl.Length = 0 Then
            multiartist = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artists)
        Else
            multiartist = mbApiInterface.Library_GetFileTag(sourceFileUrl, MetaDataType.Artists)
        End If

        If multiartist.Length > 0 Then
            artist = multiartist
        End If

        If MySettings.ArtistWhitelist.Length > 0 Then
            Dim ArtistWhiteList As String() = MySettings.ArtistWhitelist.ToLower.Split(",")
            Dim artistLower = artist.ToLower()
            Dim FoundArtist = False

            For Each WLArtist In ArtistWhiteList
                If artistLower.Contains(WLArtist.Trim()) Then
                    FoundArtist = True
                    Exit For
                End If
            Next

            If Not FoundArtist Then Return Nothing
        End If


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
                Updates.UpdateCheck(WebProxy, about.VersionMajor, about.VersionMinor, about.Revision, mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\", MySettings.UILanguage)
            End If
        End If

        Dim LyricsLib As New VocaDbLyricsLib With {.UserAgent = "MB_VocaDbLyrics", .AppendDefaultUserAgent = True, .Proxy = WebProxy, .ForceArtistMatch = MySettings.ForceArtistMatch, .UseOldForceArtistMatch = MySettings.UseOldArtistMatch}
        If provider = "UtaiteDB" Then
            LyricsLib.DatabaseUrl = New Uri("https://utaitedb.net")
        ElseIf provider = "TouhouDB" Then
            LyricsLib.DatabaseUrl = New Uri("https://touhoudb.net")
        End If


        Dim LyricsResult As VocaDbLyricsLib.LyricsResult = LyricsLib.GetLyricsFromName(trackTitle, artist)
        Dim LyricsWriter As New IO.StringWriter

        If LyricsResult.ErrorType = VocaDbLyricsLib.VocaDbLyricsError.None Then
            Dim DoneLyrics As VocaDbLyricsLib.LyricsContainer()
            ReDim DoneLyrics(0)

            Dim LangboxtextClean = MySettings.LangBoxText.Trim()
            Dim Languages = LangboxtextClean.Split(",")
            For Each Item In Languages
                Dim ItemFriendly As String
                If Item.Contains("/") Then
                    ItemFriendly = Item.Split("/")(1).Trim()
                    Item = Item.Split("/")(0).ToLower().Trim()
                Else
                    ItemFriendly = Item.Trim()
                    Item = Item.ToLower()
                End If

                For Each LyricsContainer In LyricsResult.LyricsContainers
                    If Not DoneLyrics.Contains(LyricsContainer) Then
                        If LangboxtextClean.Length = 0 Or 'Make optional to specify languages
                           (LyricsContainer.Language.StartsWith(Item) And Not LyricsContainer.TranslationType = "Romanized") Or 'Romanized requires special case
                           (SpecialLanguages.ContainsKey(Item) AndAlso LyricsContainer.TranslationType = SpecialLanguages.Item(Item)) Then

                            ReDim Preserve DoneLyrics(DoneLyrics.Length)
                            DoneLyrics(DoneLyrics.Length - 1) = LyricsContainer

                            'If LyricsWriter.ToString.Length > 0 Then LyricsWriter.Write(vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine & vbNewLine)
                            If LyricsWriter.ToString.Length > 0 Then
                                For i = 0 To MySettings.BlankCount
                                    LyricsWriter.Write(vbNewLine)
                                Next
                            End If
                            If LangboxtextClean.Length = 0 Or Languages.Length > 1 Then
                                If ItemFriendly.Length > 0 Then
                                    LyricsWriter.WriteLine(ItemFriendly & ":")
                                Else
                                    LyricsWriter.WriteLine(LyricsContainer.Language & ":")
                                End If
                            End If
                            LyricsWriter.Write(LyricsContainer.Lyrics)
                        End If
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
