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
        Dim OriginalLanguage As String
        Dim Romaji As String
        Dim English As String
        Dim Culture As String
        Dim Name As String


        'Optional UI strings.
        'The "_Tip" strings are used for tooltips.

        Dim LblLang As String
        Dim LblLang_Tip As String
        Dim LblLangHelp As String

        'LblUI is for the UI language selection box.
        Dim LblUI As String
        Dim LblUI_Tip As String

        'LblBlanks is for the separator lines count.
        Dim LblBlanks As String
        Dim LblBlanks_Tip As String

        'LblForceArtist is for the force artist match checkbox.
        Dim LblForceArtist As String
        Dim LblForceArtist_Tip As String

        'LblArtistWhitelist is for the artist whitelist.
        Dim LblArtistWhitelist As String
        Dim LblArtistWhitelist_Tip As String

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
    End Structure

    'LangEnUS needs to be maintained
    Public Shared ReadOnly LangEnUS As New Language With {.OriginalLanguage = "Original Language", .Romaji = "Romanized", .English = "English", .Culture = "en-US", .Name = "English (US)",
                                                          .LblLang = "Displayed Languages:", .LblLang_Tip = "The languages which will be included in the lyrics.",
                                                          .LblLangHelp = "This is a comma-separated list." & vbNewLine & "Special values 'orig' and 'rom' will retrieve original language and romanized lyrics respectively." &
                                                                    vbNewLine & "Put a slash between language code and a friendly name to make nicer output (eg. 'ja/Japanese').",
                                                          .LblUI = "UI Language: ", .LblUI_Tip = "The language to use for the plugin's UI.",
                                                          .LblBlanks = "Separator Lines:", .LblBlanks_Tip = "The number of blank lines between different languages in the returned lyrics.",
                                                          .LblForceArtist = "Force Artist Matching", .LblForceArtist_Tip = "Require all artists to be present on VocaDB and match the song's entry." & vbNewLine & "When this is unchecked, only artists that are in the database need to match the song's entry - any others will be ignored.",
                                                          .LblArtistWhitelist = "Artist Whitelist", .LblArtistWhitelist_Tip = "Songs that don't contain at least one of these artists will not be searched for. (disabled if empty)",
                                                          .LblUpdateCheck = "Automatically Check for Updates", .LblUpdateCheck_Tip = "Show a notification window when a newer version of the plugin is avaliable.",
                                                          .SaveErrorMsg = "The file could not be saved.", .FolderCreateErrorMsg = "The folder could not be created.",
                                                          .UninstallErrorMsg1 = "MB_VocaDbLyrics failed to remove its settings files.", .UninstallErrorMsg2 = "The remaining files can be found in:",
                                                          .UpdateMsg = "A new version of the plugin is available." & vbNewLine & "Would you like to visit the release page to download it?", .CurVer = "Current Version: ", .NewVer = "New Version: "}

    Public Shared ReadOnly LangEnGB As New Language With {.OriginalLanguage = "Original Language", .Romaji = "Romanised", .English = "English", .Culture = "en-GB", .Name = "English (GB)",
                                                          .LblLangHelp = "This is a comma-separated list." & vbNewLine & "Special values 'orig' and 'rom' will retrieve original language and romanised lyrics respectively." &
                                                                    vbNewLine & "Put a slash between language code and a friendly name to make nicer output (eg. 'ja/Japanese')."}

    Public Shared ReadOnly LangJa As New Language With {.OriginalLanguage = "元の言語", .Romaji = "ローマ字", .English = "英語", .Culture = "ja-JP", .Name = "日本語",
                                                        .LblLang = "表示言語：", .LblLang_Tip = "歌詞リザルトに込むの言語",
                                                        .LblLangHelp = "これはコンマ(「,」)で区切るのリストです。" & vbNewLine & "得な言語の「orig」は元の言語となります、「rom」はローマ字となります。" &
                                                                    vbNewLine & "スラッシュ(「/」)で歌詞リザルトに出るの分かりやすい名前が設定できる。(例：「ja/日本語」)" &
                                                                    vbNewLine & "コンマやスラッシュは半角文字のみが使わられる。",
                                                        .LblUI = "UI言語：", .LblUI_Tip = "プラグインのインタフェースで使われる言語",
                                                        .LblBlanks = "区切り行の数:", .LblBlanks_Tip = "歌詞リザルトでそれぞれな言語の間に込めた間隔",
                                                        .LblForceArtist = "アーチスト合うの保する", .LblForceArtist_Tip = "セットした場合では曲のアーチストは全部DBに存在するさらに全アーチストは曲のデータと合うの保する。" & vbNewLine & "セットしない場合ではDBに存在してないアーチストは放っておく。",
                                                        .LblArtistWhitelist = "アーチスト・ホワイトリスト", .LblArtistWhitelist_Tip = "このホワイトリストからアーチスト1つもない曲では歌詞が探されない。(抜けば無効)",
                                                        .LblUpdateCheck = "新しいバージョンを自動にチェックする", .LblUpdateCheck_Tip = "新しいバージョンの解放後に通知を表示",
                                                        .SaveErrorMsg = "ファイルを保存できませんでした。", .FolderCreateErrorMsg = "新規フォルダを書きませんでした。",
                                                        .UninstallErrorMsg1 = "MB_VocaDbLyricsは設定ファイルを削除できませんでした。", .UninstallErrorMsg2 = "残っているファイルの場所：",
                                                        .UpdateMsg = "プラグインの新しいバージョンが解放しました。" & vbNewLine & "ダウンロードするのためにリリースページに行きたいか？", .CurVer = "現行のバージョン：　", .NewVer = "新バージョン：　"}
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
