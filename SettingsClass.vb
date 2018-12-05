Public Class SettingsClass
    Public Shared Sub SaveFile(FileName As String, StoragePath As String, Data As String, ErrLang As LanguageClass.Language, Optional SilentErrors As Boolean = False)
        If Not FileIO.FileSystem.DirectoryExists(StoragePath) Then
            Try
                FileIO.FileSystem.CreateDirectory(StoragePath)
            Catch ex As Exception
                If SilentErrors = False Then
                    Dim Msg As String = LanguageClass.FallbackHelper(ErrLang.FolderCreateErrorMsg, LanguageClass.LangEnUS.FolderCreateErrorMsg)
                    MsgBox(StoragePath.TrimEnd("\".ToCharArray) & ":" & vbNewLine & Msg)
                End If
            End Try
        End If


        Try
            FileIO.FileSystem.WriteAllText(StoragePath & FileName, Data, False)
        Catch
            If SilentErrors = False Then
                Dim Msg As String = LanguageClass.FallbackHelper(ErrLang.SaveErrorMsg, LanguageClass.LangEnUS.SaveErrorMsg)
                MsgBox(FileName & ":" & vbNewLine & Msg)
            End If
        End Try
    End Sub

    Public Structure SettingsCollection
        Dim LangBoxText As String
        Dim UILanguage As LanguageClass.Language
        Dim BlankCount As Byte
        Dim ForceArtistMatch As Boolean
        Dim UseOldArtistMatch As Boolean
        Dim UpdateChecking As Boolean

        Function MakeString(Settings() As String) As String
            Dim OutStr As New IO.StringWriter

            For Each Setting In Settings
                Select Case Setting
                    Case "LangBoxText"
                        OutStr.WriteLine("LangBoxText:" & LangBoxText)

                    Case "UILanguage"
                        OutStr.WriteLine("UILanguage:" & UILanguage.Culture)

                    Case "BlankCount"
                        OutStr.WriteLine("BlankCount:" & BlankCount)

                    Case "ForceArtistMatch"
                        OutStr.WriteLine("ForceArtistMatch:" & ForceArtistMatch.ToString())

                    Case "UseOldArtistMatch"
                        OutStr.WriteLine("UseOldArtistMatch:" & UseOldArtistMatch.ToString())

                    Case "UpdateChecking"
                        OutStr.WriteLine("UpdateChecking:" & UpdateChecking.ToString())

                End Select
            Next
            Return OutStr.ToString
        End Function

        Sub SetFromString(Settings As String)
            Dim SettingsArray() As String = Settings.Split(vbNewLine)

            For Each Setting In SettingsArray
                If Setting.First = vbLf Then Setting = Setting.Remove(0, 1)

                Dim Split() As String = Setting.Split(":")

                If Split.Length > 1 AndAlso String.IsNullOrEmpty(Split(0)) = False Then
                    Select Case Split(0)
                        Case "LangBox2Items"
                            Dim temp = Split(1)
                            temp = temp.Replace("Romaji", "rom/" & UILanguage.Romaji)
                            temp = temp.Replace("English", "en/" & UILanguage.English)
                            temp = temp.Replace("Japanese", "orig/" & UILanguage.OriginalLanguage)
                            LangBoxText = temp

                        Case "LangBoxText"
                            LangBoxText = Split(1)

                        Case "UILanguage"
                            For Each Lang As LanguageClass.Language In LanguageClass.LangList()
                                If Lang.Culture = Split(1) Then
                                    UILanguage = Lang
                                End If
                            Next

                        Case "BlankCount"
                            BlankCount = Split(1)

                        Case "ForceArtistMatch"
                            ForceArtistMatch = Boolean.Parse(Split(1))

                        Case "UseOldArtistMatch"
                            UseOldArtistMatch = Boolean.Parse(Split(1))

                        Case "UpdateChecking"
                            UpdateChecking = Boolean.Parse(Split(1))

                    End Select
                End If
            Next
        End Sub
    End Structure
End Class
