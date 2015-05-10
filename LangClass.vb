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
        'LblApplySave is the note about saving settings at the bottom.
        Dim LblApplySave As String

        Dim BtnReset As String
        Dim BtnOk As String
        Dim BtnCancel As String

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
                                                          .LblLang1 = "Available Languages:", .LblLang2 = "Displayed Languages:", .LblUI = "Interface Language:", .LblApplySave = "You must also click Apply or Save in the MusicBee settings box to save any changes.",
                                                          .BtnReset = "Reset", .BtnOk = "OK", .BtnCancel = "Cancel",
                                                          .SaveErrorMsg = "The file could not be saved.", .FolderCreateErrorMsg = "The folder could not be created.",
                                                          .UninstallErrorMsg1 = "The VocaDB Lyrics plugin failed to remove its settings files.", .UninstallErrorMsg2 = "The remaining files can be found in:"}

    Public Shared ReadOnly LangEnGB As New Language With {.Japanese = "Non-English", .Romaji = "Romanised", .English = "English", .Culture = "en-GB", .Name = "English (GB)"}

    Public Shared ReadOnly LangJa As New Language With {.Japanese = "非英語", .Romaji = "ローマ字", .English = "英語", .Culture = "ja-JP", .Name = "日本語",
                                                        .LblLang1 = "使用できる言語：", .LblLang2 = "表示される言語：", .LblUI = "インターフェイスの言語：", .LblApplySave = "また、変更を保存するには、MusicBeeの設定で適用又は保存するをクリックなければなりません。",
                                                        .BtnReset = "リセット", .BtnOk = "OK", .BtnCancel = "キャンセル",
                                                        .SaveErrorMsg = "ファイルは保存できませんでした。", .FolderCreateErrorMsg = "フォルダは作成できませんでした。",
                                                        .UninstallErrorMsg1 = "VocaDB Lyricsプラグインの構成ファイルの削除に失敗しました。", .UninstallErrorMsg2 = "残りのファイルが次の場所にあります。"}
    'Public Shared ReadOnly LangList() As Language = {LangEnUS, LangEnGB, LangJa}
    Public Shared Function LangList() As Language()
        Return {LangEnUS, LangEnGB, LangJa}
    End Function
End Class
