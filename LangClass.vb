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
        'LblLang1 and LblLang2 Are for the listboxes.
        '1 is for the unused language box and 2 is for the used language box.
        Dim LblLang1 As String
        Dim LblLang2 As String
        'LblUI is for the UI language selection box.
        Dim LblUI As String
        'LblBlanks is for the separator lines count/
        Dim LblBlanks As String

        Dim SaveErrorMsg As String
        Dim FolderCreateErrorMsg As String
        Dim UninstallErrorMsg1 As String
        Dim UninstallErrorMsg2 As String

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

    Public Shared ReadOnly LangEnUS As New Language With {.Japanese = "Non-English", .Romaji = "Romanized", .English = "English", .Culture = "en-US", .Name = "English (US)",
                                                          .LblLang1 = "Available Languages:", .LblLang2 = "Displayed Languages:", .LblUI = "UI Language:", .LblBlanks = "Separator Lines:",
                                                          .SaveErrorMsg = "The file could not be saved.", .FolderCreateErrorMsg = "The folder could not be created.",
                                                          .UninstallErrorMsg1 = "MB_VocaDbLyrics failed to remove its settings files.", .UninstallErrorMsg2 = "The remaining files can be found in:"}

    Public Shared ReadOnly LangEnGB As New Language With {.Japanese = "Non-English", .Romaji = "Romanised", .English = "English", .Culture = "en-GB", .Name = "English (GB)"}

    Public Shared ReadOnly LangJa As New Language With {.Japanese = "非英語", .Romaji = "ローマ字", .English = "英語", .Culture = "ja-JP", .Name = "日本語",
                                                        .LblLang1 = "利用可能言語：", .LblLang2 = "表示言語：", .LblUI = "UI言語：", .LblBlanks = "区切り行の数:",
                                                        .SaveErrorMsg = "ファイルを保存できませんでした。", .FolderCreateErrorMsg = "新規フォルダを書きませんでした。",
                                                        .UninstallErrorMsg1 = "MB_VocaDbLyricsは設定ファイルを削除できませんでした。", .UninstallErrorMsg2 = "残っているファイルの場所："}
    'Public Shared ReadOnly LangList() As Language = {LangEnUS, LangEnGB, LangJa}
    Public Shared Function LangList() As Language()
        Return {LangEnUS, LangEnGB, LangJa}
    End Function

    Public Shared Function FallbackHelper(Str As String, Fallback As String)
        If Str IsNot Nothing Then Return Str Else Return Fallback
    End Function
End Class
