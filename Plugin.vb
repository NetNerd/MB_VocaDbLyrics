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


Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms
Imports MusicBeePlugin.LanguageClass

Public Class Plugin
    Public Structure SettingsCollection
        Dim LangBox1Items() As String
        Dim LangBox2Items() As String
        Dim UILanguage As Language
        Dim BlankCount As Byte

        Function MakeString(Settings() As String) As String
            Dim OutStr As New IO.StringWriter

            For Each Setting In Settings
                If Setting = "LangBox1Items" Then
                    OutStr.Write("LangBox1Items:")
                    For Each Lang In LangBox1Items
                        If Lang = LangBox1Items.Last Then
                            OutStr.Write(Lang)
                        Else
                            OutStr.Write(Lang & ",")
                        End If
                    Next
                    OutStr.WriteLine()

                ElseIf Setting = "LangBox2Items" Then
                    OutStr.Write("LangBox2Items:")
                    For Each Lang In LangBox2Items
                        If Lang = LangBox2Items.Last Then
                            OutStr.Write(Lang)
                        Else
                            OutStr.Write(Lang & ",")
                        End If
                    Next
                    OutStr.WriteLine()

                ElseIf Setting = "UILanguage" Then
                    OutStr.WriteLine("UILanguage:" & UILanguage.Culture)

                ElseIf Setting = "BlankCount" Then
                    OutStr.WriteLine("BlankCount:" & BlankCount)
                End If
            Next
            Return OutStr.ToString
        End Function

        Sub SetFromString(Settings As String)
            Dim SettingsArray() As String = Settings.Split(vbNewLine)

            For Each Setting In SettingsArray
                If Setting.First = vbLf Then Setting = Setting.Remove(0, 1)

                Dim Split() As String = Setting.Split(":")

                If Split(0) = "LangBox1Items" Then
                    LangBox1Items = Split(1).Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

                ElseIf Split(0) = "LangBox2Items" Then
                    LangBox2Items = Split(1).Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

                ElseIf Split(0) = "UILanguage" Then
                    For Each Lang As Language In LangList()
                        If Lang.Culture = Split(1) Then
                            UILanguage = Lang
                        End If
                    Next

                ElseIf Split(0) = "BlankCount" Then
                    BlankCount = Split(1)
                End If
            Next
        End Sub
    End Structure

    Private mbApiInterface As New MusicBeeApiInterface
    Private about As New PluginInfo
    Private SettingsFolder As String = "MB_VocaDbLyrics"
    Private MySettings As New SettingsCollection With {.LangBox1Items = {"Japanese"}, .LangBox2Items = {"Romaji", "English"}, .UILanguage = LangEnUS, .BlankCount = 5}
    Private UseTempSettings As Boolean = False

    Public Function Initialise(ByVal apiInterfacePtr As IntPtr) As PluginInfo
        'I'm not quite sure exactly what this does.
        'It seems like it's just simple initilisation, but done kinda weird.
        'Anyway, this is cleaner than what was in the sample plugin and works fine for me.
        CopyMemory(mbApiInterface, apiInterfacePtr, 4)
        If mbApiInterface.MusicBeeVersion > -1 Then
            CopyMemory(mbApiInterface, apiInterfacePtr, 456)
        End If

        about.PluginInfoVersion = 0.1
        about.Name = "MB_VocaDbLyrics"
        about.Description = "A lyrics provider for VocaDB."
        about.Author = "NetNerd"
        about.TargetApplication = "VocaDB"
        about.Type = PluginType.LyricsRetrieval
        about.VersionMajor = 0
        about.VersionMinor = 1
        about.Revision = 0
        about.MinInterfaceVersion = MinInterfaceVersion
        about.MinApiRevision = 20
        about.ReceiveNotifications = ReceiveNotificationFlags.StartupOnly
        about.ConfigurationPanelHeight = 0
        Return about
    End Function

    Public Function Configure(ByVal panelHandle As IntPtr) As Boolean
        ' save any persistent settings in a sub-folder of this path
        'SettingsPath = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"
        ' panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
        ' keep in mind the panel width is scaled according to the font the user has selected
        ' if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
        If UseTempSettings = False Then TempSettings.Settings = MySettings
        UseTempSettings = True
        Dim ConfigForm As New ConfigForm()
        ConfigForm.Show()
        Return True
    End Function

    ' called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
    ' its up to you to figure out whether anything has changed and needs updating
    Public Sub SaveSettings()
        If UseTempSettings = True Then MySettings = TempSettings.Settings
        UseTempSettings = False

        ' save any persistent settings in a sub-folder of this path
        Dim SettingsPath As String = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"

        If Not FileIO.FileSystem.DirectoryExists(SettingsPath) Then
            Try
                FileIO.FileSystem.CreateDirectory(SettingsPath)
            Catch ex As Exception
                Dim Msg As String = FallbackHelper(MySettings.UILanguage.FolderCreateErrorMsg, LangEnUS.FolderCreateErrorMsg)
                MsgBox(SettingsPath.TrimEnd("\".ToCharArray) & ":" & vbNewLine & Msg)
            End Try
        End If


        Dim LangOrder As String = ""

        For Each Lang In MySettings.LangBox1Items
            LangOrder = LangOrder & Lang & ","
        Next
        LangOrder = LangOrder & ";"
        For Each Lang In MySettings.LangBox2Items
            LangOrder = LangOrder & Lang & ","
        Next


        Try
            FileIO.FileSystem.WriteAllText(SettingsPath & "Settings.conf", MySettings.MakeString({"LangBox1Items", "LangBox2Items", "UILanguage", "BlankCount"}), False)
        Catch ex As Exception
            Dim Msg As String = FallbackHelper(MySettings.UILanguage.SaveErrorMsg, LangEnUS.SaveErrorMsg)
            MsgBox("Settings.conf" & ":" & vbNewLine & Msg)
        End Try
    End Sub

    ' MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
    Public Sub Close(ByVal reason As PluginCloseReason)
    End Sub

    ' uninstall this plugin - clean up any persisted files
    Public Sub Uninstall()
        UseTempSettings = False
        Dim SettingsPath As String = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"
        Try
            FileIO.FileSystem.DeleteFile(SettingsPath & "Settings.conf")
            FileIO.FileSystem.DeleteDirectory(SettingsPath, FileIO.DeleteDirectoryOption.ThrowIfDirectoryNonEmpty)
        Catch ex As Exception
            Dim Msg1 As String = FallbackHelper(MySettings.UILanguage.UninstallErrorMsg1, LangEnUS.UninstallErrorMsg1)
            Dim Msg2 As String = FallbackHelper(MySettings.UILanguage.UninstallErrorMsg2, LangEnUS.UninstallErrorMsg2)
            MsgBox(Msg1 & vbNewLine & vbNewLine & Msg2 & vbNewLine & SettingsPath.TrimEnd("\".ToCharArray))
        End Try
    End Sub

    ' receive event notifications from MusicBee
    ' you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
    Public Sub ReceiveNotification(ByVal sourceFileUrl As String, ByVal type As NotificationType)
        ' perform some action depending on the notification type
        Select Case type
            Case NotificationType.PluginStartup
                ' perform startup initialisation
                Dim SettingsPath As String = mbApiInterface.Setting_GetPersistentStoragePath().TrimEnd("\/".ToCharArray) & "\" & SettingsFolder & "\"

                If FileIO.FileSystem.FileExists(SettingsPath & "Settings.conf") Then
                    MySettings.SetFromString(FileIO.FileSystem.ReadAllText(SettingsPath & "Settings.conf"))
                End If
        End Select
    End Sub

    ' return an array of lyric or artwork provider names this plugin supports
    ' the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
    Public Function GetProviders() As String()
        Return New String() {"VocaDB"}
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

        Dim LyricsLib As New VocaDbLyricsLib With {.UserAgent = "MB_VocaDbLyrics", .AppendDefaultUserAgent = True, .Proxy = WebProxy}
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
