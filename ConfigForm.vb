Imports System.Windows.Forms
Imports System.Drawing
Imports MusicBeePlugin.LanguageClass

Public Class ConfigForm
    Private LangList() As Language = LanguageClass.LangList()
    Private UILanguage As Language
    Private SaveSettings As Boolean = False

    Private Sub LangBox_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles LangBox1.DragDrop, LangBox2.DragDrop
        If sender.PointToClient(New Point(e.X, e.Y)).Y < 0 Then
            sender.Items.Insert(0, e.Data.GetData(DataFormats.Text))
        ElseIf sender.PointToClient(New Point(e.X, e.Y)).Y > sender.ItemHeight * sender.Items.Count Then
            sender.Items.Add(e.Data.GetData(DataFormats.Text))
        Else
            sender.Items.Insert(sender.PointToClient(New Point(e.X, e.Y)).Y / sender.ItemHeight, e.Data.GetData(DataFormats.Text))
        End If

        If LangBox1.SelectedIndex > -1 AndAlso e.Data.GetData(DataFormats.Text) Is LangBox1.Items(LangBox1.SelectedIndex) Then
            LangBox1.Items.RemoveAt(LangBox1.SelectedIndex)
        End If

        If LangBox2.SelectedIndex > -1 AndAlso e.Data.GetData(DataFormats.Text) Is LangBox2.Items(LangBox2.SelectedIndex) Then
            LangBox2.Items.RemoveAt(LangBox2.SelectedIndex)
        End If
    End Sub

    Private Sub LangBox_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles LangBox1.DragOver, LangBox2.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub LangBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LangBox1.MouseDown, LangBox2.MouseDown
        Threading.Thread.Sleep(200)
        sender.DoDragDrop(sender.Text, DragDropEffects.All)
    End Sub

    Private Sub BtnL_Click(sender As Object, e As EventArgs) Handles BtnL.Click
        If LangBox2.SelectedIndex > -1 Then
            LangBox1.Items.Add(LangBox2.Items(LangBox2.SelectedIndex))
            LangBox2.Items.RemoveAt(LangBox2.SelectedIndex)
        End If
    End Sub

    Private Sub BtnR_Click(sender As Object, e As EventArgs) Handles BtnR.Click
        If LangBox1.SelectedIndex > -1 Then
            LangBox2.Items.Add(LangBox1.Items(LangBox1.SelectedIndex))
            LangBox1.Items.RemoveAt(LangBox1.SelectedIndex)
        End If
    End Sub

    Public Sub New()
        InitializeComponent()

        For Each Lang As Language In LangList
            UILangCB.Items.Add(Lang.Name)
        Next

        UILanguage = TempSettings.Settings.UILanguage

        UILangCB.SelectedItem = UILanguage.Name

        For Each Lang As String In TempSettings.Settings.LangBox1Items
            LangBox1.Items.Add(TempSettings.Settings.UILanguage.LocalizeFromString(Lang))
        Next
        For Each Lang As String In TempSettings.Settings.LangBox2Items
            LangBox2.Items.Add(TempSettings.Settings.UILanguage.LocalizeFromString(Lang))
        Next

        TranslateLblsBtns()
    End Sub


    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        SaveSettings = True
        Me.Close()
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnReset_Click(sender As Object, e As EventArgs) Handles BtnReset.Click
        UILanguage = LangEnUS
        UILangCB.SelectedItem = UILanguage.Name

        LangBox1.Items.Clear()
        LangBox2.Items.Clear()
        LangBox1.Items.Add(UILanguage.Japanese)
        LangBox2.Items.AddRange({UILanguage.Romaji, UILanguage.English})

        TranslateLblsBtns()
    End Sub

    Private Sub ConfigForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If SaveSettings = True Then
            Dim LangOrder As String = ""

            ReDim TempSettings.Settings.LangBox1Items(LangBox1.Items.Count - 1)
            ReDim TempSettings.Settings.LangBox2Items(LangBox2.Items.Count - 1)
            For i = 0 To LangBox1.Items.Count - 1
                TempSettings.Settings.LangBox1Items(i) = UILanguage.UnlocalizeFromString(LangBox1.Items(i))
            Next
            For i = 0 To LangBox2.Items.Count - 1
                TempSettings.Settings.LangBox2Items(i) = UILanguage.UnlocalizeFromString(LangBox2.Items(i))
            Next

            TempSettings.Settings.UILanguage = UILanguage
        End If
        Me.Dispose()
    End Sub

    Private Sub UILangCB_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles UILangCB.SelectionChangeCommitted
        Dim NewUILang As Language = LangEnUS
        For Each Lang In LangList
            If UILangCB.SelectedItem Is Lang.Name Then
                NewUILang = Lang
            End If
        Next

        For i = 0 To LangBox1.Items.Count - 1
            LangBox1.Items(i) = NewUILang.LocalizeFromString(UILanguage.UnlocalizeFromString(LangBox1.Items(i)))
        Next
        For i = 0 To LangBox2.Items.Count - 1
            LangBox2.Items(i) = NewUILang.LocalizeFromString(UILanguage.UnlocalizeFromString(LangBox2.Items(i)))
        Next

        UILanguage = NewUILang
        UILangCB.SelectedItem = UILanguage.Name

        TranslateLblsBtns()
    End Sub

    Private Sub TranslateLblsBtns()
        If UILanguage.LblLang1 IsNot Nothing Then LblLang1.Text = UILanguage.LblLang1 Else LblLang1.Text = LangEnUS.LblLang1
        If UILanguage.LblLang2 IsNot Nothing Then LblLang2.Text = UILanguage.LblLang2 Else LblLang2.Text = LangEnUS.LblLang2
        If UILanguage.LblUI IsNot Nothing Then LblUI.Text = UILanguage.LblUI Else LblUI.Text = LangEnUS.LblUI
        If UILanguage.LblApplySave IsNot Nothing Then LblApplySave.Text = UILanguage.LblApplySave Else LblApplySave.Text = LangEnUS.LblApplySave
        If UILanguage.BtnReset IsNot Nothing Then BtnReset.Text = UILanguage.BtnReset Else BtnReset.Text = LangEnUS.BtnReset
        If UILanguage.BtnOk IsNot Nothing Then BtnOk.Text = UILanguage.BtnOk Else BtnOk.Text = LangEnUS.BtnOk
        If UILanguage.BtnCancel IsNot Nothing Then BtnCancel.Text = UILanguage.BtnCancel Else BtnCancel.Text = LangEnUS.BtnCancel
    End Sub
End Class

Public Class TempSettings
    Public Shared Settings As Plugin.SettingsCollection
End Class