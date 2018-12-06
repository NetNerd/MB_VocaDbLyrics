Imports System.Windows.Forms
Imports System.Drawing
Imports MusicBeePlugin.LanguageClass
Imports MusicBeePlugin.SettingsClass

Public Class ConfigPanel
    Private Shared LblLang As Label
    Private Shared WithEvents LangBox As TextBox
    Private Shared LblLangHelp As Label

    Private Shared LblUI As Label
    Private Shared WithEvents UILangCB As ComboBox

    Private Shared LblBlanks As Label
    Private Shared WithEvents BlanksCB As ComboBox
    
    Private Shared LblForceArtist As Label
    Private Shared WithEvents ForceArtistCB As ComboBox

    Private Shared WithEvents LblForceArtist As Label
    Private Shared WithEvents ForceArtistCB As CheckBox

    Private Shared WithEvents LblArtistWhitelist As Label
    Private Shared WithEvents ArtistWhitelistBox As TextBox

    Private Shared WithEvents LblUpdateCheck As Label
    Private Shared WithEvents UpdateCheckCB As CheckBox

    Private Shared Border1 As Panel
    Private Shared Border2 As Panel

    Private Shared Tooltipper As ToolTip
    Private Shared TooltipperLong As ToolTip


    Private Shared MySettings As New SettingsCollection


    Private Shared Sub DefineControls()
        LblLang = New Label With {.Bounds = New Rectangle(10, 8, 292, 14)}
        LangBox = New TextBox With {.Bounds = New Rectangle(10, 24, 292, 21)}
        LblLangHelp = New Label With {.Bounds = New Rectangle(10, 48, 292, 100)}
        LblLangHelp.Font = New Font(LblLangHelp.Font.FontFamily, 8.5, FontStyle.Regular)

        LblUI = New Label With {.Bounds = New Rectangle(10, 154, 98, 14)}
        UILangCB = New ComboBox With {.Bounds = New Rectangle(110, 149, 100, 21), .DropDownStyle = ComboBoxStyle.DropDownList}

        LblBlanks = New Label With {.Bounds = New Rectangle(10, 179, 98, 14)}
        BlanksCB = New ComboBox With {.Bounds = New Rectangle(110, 174, 100, 21), .DropDownStyle = ComboBoxStyle.DropDownList}
        BlanksCB.Items.AddRange({1, 2, 3, 4, 5, 6, 7, 8, 9})

        LblForceArtist = New Label With {.Bounds = New Rectangle(27, 204, 199, 14)}
        ForceArtistCB = New CheckBox With {.Bounds = New Rectangle(10, 205, 13, 13)}

        LblArtistWhitelist = New Label With {.Bounds = New Rectangle(10, 228, 292, 14)}
        ArtistWhitelistBox = New TextBox With {.Bounds = New Rectangle(10, 244, 292, 21)}

        LblUpdateCheck = New Label With {.Bounds = New Rectangle(336, 1, 140, 42)}
        UpdateCheckCB = New CheckBox With {.Bounds = New Rectangle(319, 2, 13, 13)}

        Border1 = New Panel With {.Bounds = New Rectangle(2, 1, 308, 277), .BackColor = Color.FromArgb(224, 224, 224)}
        Border2 = New Panel With {.Bounds = New Rectangle(3, 2, 306, 275)}


        Tooltipper = New ToolTip With {.AutomaticDelay = 1100, .ReshowDelay = 850}
        TooltipperLong = New ToolTip With {.AutomaticDelay = 2000, .ReshowDelay = 1500}
    End Sub

    Shared Sub SetupControls(ByVal Settings As SettingsCollection)
        MySettings = Settings

        DefineControls()

        For Each Lang As Language In LangList()
            UILangCB.Items.Add(Lang.Name)
        Next

        UILangCB.SelectedItem = MySettings.UILanguage.Name

        If MySettings.BlankCount > 0 And MySettings.BlankCount < 10 Then BlanksCB.SelectedIndex = MySettings.BlankCount - 1

        ForceArtistCB.Checked = MySettings.ForceArtistMatch

        ArtistWhitelistBox.Text = MySettings.ArtistWhitelist

        UpdateCheckCB.Checked = MySettings.UpdateChecking

        LangBox.Text = MySettings.LangBoxText

        TranslateLbls()
    End Sub

    Shared Function GetControls() As Control()
        Return {LblLang, LangBox, LblLangHelp, LblUI, UILangCB, LblBlanks, BlanksCB, LblForceArtist, LblArtistWhitelist, ArtistWhitelistBox, ForceArtistCB, LblUpdateCheck, UpdateCheckCB, Border2, Border1}
    End Function

    Shared Function GetSettings() As SettingsCollection
        Return MySettings
    End Function

    Private Shared Sub TranslateLbls()
        LblLang.Text = FallbackHelper(MySettings.UILanguage.LblLang, LangEnUS.LblLang)
        Tooltipper.SetToolTip(LblLang, FallbackHelper(MySettings.UILanguage.LblLang_Tip, LangEnUS.LblLang_Tip))
        TooltipperLong.SetToolTip(LangBox, FallbackHelper(MySettings.UILanguage.LblLang_Tip, LangEnUS.LblLang_Tip))

        LblLangHelp.Text = FallbackHelper(MySettings.UILanguage.LblLangHelp, LangEnUS.LblLangHelp)

        LblUI.Text = FallbackHelper(MySettings.UILanguage.LblUI, LangEnUS.LblUI)
        Tooltipper.SetToolTip(LblUI, FallbackHelper(MySettings.UILanguage.LblUI_Tip, LangEnUS.LblUI_Tip))
        TooltipperLong.SetToolTip(UILangCB, FallbackHelper(MySettings.UILanguage.LblUI_Tip, LangEnUS.LblUI_Tip))

        LblBlanks.Text = FallbackHelper(MySettings.UILanguage.LblBlanks, LangEnUS.LblBlanks)
        Tooltipper.SetToolTip(LblBlanks, FallbackHelper(MySettings.UILanguage.LblBlanks_Tip, LangEnUS.LblBlanks_Tip))
        TooltipperLong.SetToolTip(BlanksCB, FallbackHelper(MySettings.UILanguage.LblBlanks_Tip, LangEnUS.LblBlanks_Tip))

        LblForceArtist.Text = FallbackHelper(MySettings.UILanguage.LblForceArtist, LangEnUS.LblForceArtist)
        Tooltipper.SetToolTip(LblForceArtist, FallbackHelper(MySettings.UILanguage.LblForceArtist_Tip, LangEnUS.LblForceArtist_Tip))
        TooltipperLong.SetToolTip(ForceArtistCB, FallbackHelper(MySettings.UILanguage.LblForceArtist_Tip, LangEnUS.LblForceArtist_Tip))

        LblArtistWhitelist.Text = FallbackHelper(MySettings.UILanguage.LblArtistWhitelist, LangEnUS.LblArtistWhitelist)
        Tooltipper.SetToolTip(LblArtistWhitelist, FallbackHelper(MySettings.UILanguage.LblArtistWhitelist_Tip, LangEnUS.LblArtistWhitelist_Tip))
        TooltipperLong.SetToolTip(ArtistWhitelistBox, FallbackHelper(MySettings.UILanguage.LblArtistWhitelist_Tip, LangEnUS.LblArtistWhitelist_Tip))

        LblUpdateCheck.Text = FallbackHelper(MySettings.UILanguage.LblUpdateCheck, LangEnUS.LblUpdateCheck)
        Tooltipper.SetToolTip(LblUpdateCheck, FallbackHelper(MySettings.UILanguage.LblUpdateCheck_Tip, LangEnUS.LblUpdateCheck_Tip))
        TooltipperLong.SetToolTip(UpdateCheckCB, FallbackHelper(MySettings.UILanguage.LblUpdateCheck_Tip, LangEnUS.LblUpdateCheck_Tip))
    End Sub

    Private Shared Sub LangBox_TextChanged(sender As Object, e As EventArgs) Handles LangBox.TextChanged
        MySettings.LangBoxText = LangBox.Text
    End Sub

    Private Shared Sub UILangCB_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles UILangCB.SelectionChangeCommitted
        Dim NewUILang As Language = LangEnUS
        For Each Lang In LangList()

            If UILangCB.SelectedItem Is Lang.Name Then
                NewUILang = Lang
            End If
        Next

        MySettings.UILanguage = NewUILang
        UILangCB.SelectedItem = MySettings.UILanguage.Name

        TranslateLbls()
    End Sub

    Private Shared Sub BlanksCB_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles BlanksCB.SelectionChangeCommitted
        MySettings.BlankCount = BlanksCB.SelectedItem
    End Sub

    Private Shared Sub ForceArtistCB_CheckedChanged(sender As Object, e As EventArgs) Handles ForceArtistCB.CheckedChanged
        MySettings.ForceArtistMatch = ForceArtistCB.Checked
    End Sub

    Private Shared Sub LblForceArtist_Click(sender As Object, e As EventArgs) Handles LblForceArtist.Click
        ForceArtistCB.Checked = Not ForceArtistCB.Checked
    End Sub

    Private Shared Sub ArtistWhitelistBox_TextChanged(sender As Object, e As EventArgs) Handles ArtistWhitelistBox.TextChanged
        MySettings.ArtistWhitelist = ArtistWhitelistBox.Text
    End Sub

    Private Shared Sub UpdateCheckCB_CheckedChanged(sender As Object, e As EventArgs) Handles UpdateCheckCB.CheckedChanged
        MySettings.UpdateChecking = UpdateCheckCB.Checked
    End Sub

    Private Shared Sub LblUpdateCheck_Click(sender As Object, e As EventArgs) Handles LblUpdateCheck.Click
        UpdateCheckCB.Checked = Not UpdateCheckCB.Checked
    End Sub
End Class
