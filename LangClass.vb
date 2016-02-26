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


Public Class LanguageClass
    Public Structure Language
        'Core language info for operation. Must be supplied.
        Dim Japanese As String
        Dim Romaji As String
        Dim English As String
        Dim Culture As String
        Dim Name As String


        'Optional UI strings.
        'The "_Tip" strings are used for tooltips.

        'LblLang1 and LblLang2 Are for the listboxes.
        '1 is for the unused language box and 2 is for the used language box.
        Dim LblLang1 As String
        Dim LblLang1_Tip As String
        Dim LblLang2 As String
        Dim LblLang2_Tip As String

        'LblUI is for the UI language selection box.
        Dim LblUI As String
        Dim LblUI_Tip As String

        'LblBlanks is for the separator lines count.
        Dim LblBlanks As String
        Dim LblBlanks_Tip As String

        'LblForceArtist is for the force artist match checkbox.
        Dim LblForceArtist As String
        Dim LblForceArtist_Tip As String

        'LblUpdateCheck is for the auto update checking checkbox.
        Dim LblUpdateCheck As String
        Dim LblUpdateCheck_Tip As String

        Dim SaveErrorMsg As String
        Dim FolderCreateErrorMsg As String
        Dim UninstallErrorMsg1 As String
        Dim UninstallErrorMsg2 As String
        Dim UpdateMsg As String

        Dim CurVer As String
        Dim NewVer As String

        Function LocalizeFromString(Lang As String)
            Select Case Lang
                Case "Japanese"
                    Return Japanese
                Case "Romaji"
                    Return Romaji
                Case "English"
                    Return English
            End Select
            Return "Error"
        End Function

        Function UnlocalizeFromString(Lang As String)
            Select Case Lang
                Case Japanese
                    Return "Japanese"
                Case Romaji
                    Return "Romaji"
                Case English
                    Return "English"
            End Select
            Return "Error"
        End Function
    End Structure

    'LangEnUS needs to be maintained
    Public Shared ReadOnly LangEnUS As New Language With {.Japanese = "Non-English", .Romaji = "Romanized", .English = "English", .Culture = "en-US", .Name = "English (US)",
                                                          .LblLang1 = "Available Languages:", .LblLang1_Tip = "The languages which can be retrieved from VocaDB, but will not be included in the lyrics.",
                                                          .LblLang2 = "Displayed Languages:", .LblLang2_Tip = "The languages which will be included in the lyrics.",
                                                          .LblUI = "UI Language:", .LblUI_Tip = "The language to use for the plugin's UI.",
                                                          .LblBlanks = "Separator Lines:", .LblBlanks_Tip = "The number of blank lines between different languages in the returned lyrics.",
                                                          .LblForceArtist = "Force Artist Matching", .LblForceArtist_Tip = "Require all artists to be present on VocaDB and match the song's entry." & vbNewLine & "When this is unchecked, only artists that are in the database need to match the song's entry - any others will be ignored.",
                                                          .LblUpdateCheck = "Automatically Check for Updates", .LblUpdateCheck_Tip = "Show a notification window when a newer version of the plugin is avaliable.",
                                                          .SaveErrorMsg = "The file could not be saved.", .FolderCreateErrorMsg = "The folder could not be created.",
                                                          .UninstallErrorMsg1 = "MB_VocaDbLyrics failed to remove its settings files.", .UninstallErrorMsg2 = "The remaining files can be found in:",
                                                          .UpdateMsg = "A new version of the plugin is available." & vbNewLine & "Would you like to visit the release page to download it?", .CurVer = "Current Version: ", .NewVer = "New Version: "}

    Public Shared ReadOnly LangEnGB As New Language With {.Japanese = "Non-English", .Romaji = "Romanised", .English = "English", .Culture = "en-GB", .Name = "English (GB)"}

    Public Shared ReadOnly LangJa As New Language With {.Japanese = "非英語", .Romaji = "ローマ字", .English = "英語", .Culture = "ja-JP", .Name = "日本語",
                                                        .LblLang1 = "利用可能言語： ", .LblLang2 = "表示言語：", .LblUI = "UI言語：", .LblBlanks = "区切り行の数:", .LblForceArtist = "アーチスト合うするの保する", .LblUpdateCheck = "新しいバージョンを自動チェックする", '"新しいバージョンの自動チェック",
                                                        .SaveErrorMsg = "ファイルを保存できませんでした。", .FolderCreateErrorMsg = "新規フォルダを書きませんでした。",
                                                        .UninstallErrorMsg1 = "MB_VocaDbLyricsは設定ファイルを削除できませんでした。", .UninstallErrorMsg2 = "残っているファイルの場所：",
                                                        .UpdateMsg = "プラグインの新しいバージョンが利用可能。" & vbNewLine & "ダウンロードするにはリリースページに行きたいか？", .CurVer = "現行のバージョン：　", .NewVer = "新バージョン：　"}
    'Public Shared ReadOnly LangList() As Language = {LangEnUS, LangEnGB, LangJa}
    Public Shared Function LangList() As Language()
        Return {LangEnUS, LangEnGB, LangJa}
    End Function

    Public Shared Function FallbackHelper(Str As String, Fallback As String)
        If Str IsNot Nothing Then Return Str
        If Fallback IsNot Nothing Then Return Fallback
        Return "[Error loading text resource]"
    End Function
End Class
