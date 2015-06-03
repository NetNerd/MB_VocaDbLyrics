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


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LblLang2 = New System.Windows.Forms.Label()
        Me.LblLang1 = New System.Windows.Forms.Label()
        Me.BtnR = New System.Windows.Forms.Button()
        Me.BtnL = New System.Windows.Forms.Button()
        Me.LangBox2 = New System.Windows.Forms.ListBox()
        Me.LangBox1 = New System.Windows.Forms.ListBox()
        Me.BtnOk = New System.Windows.Forms.Button()
        Me.BtnReset = New System.Windows.Forms.Button()
        Me.LblUI = New System.Windows.Forms.Label()
        Me.UILangCB = New System.Windows.Forms.ComboBox()
        Me.LblApplySave = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LblLang2
        '
        Me.LblLang2.AutoSize = True
        Me.LblLang2.Location = New System.Drawing.Point(174, 10)
        Me.LblLang2.Name = "LblLang2"
        Me.LblLang2.Size = New System.Drawing.Size(112, 13)
        Me.LblLang2.TabIndex = 4
        Me.LblLang2.Text = "Displayed Languages:"
        '
        'LblLang1
        '
        Me.LblLang1.AutoSize = True
        Me.LblLang1.Location = New System.Drawing.Point(10, 10)
        Me.LblLang1.Name = "LblLang1"
        Me.LblLang1.Size = New System.Drawing.Size(109, 13)
        Me.LblLang1.TabIndex = 0
        Me.LblLang1.Text = "Available Languages:"
        '
        'BtnR
        '
        Me.BtnR.Location = New System.Drawing.Point(140, 56)
        Me.BtnR.Name = "BtnR"
        Me.BtnR.Size = New System.Drawing.Size(28, 25)
        Me.BtnR.TabIndex = 3
        Me.BtnR.Text = ">"
        Me.BtnR.UseVisualStyleBackColor = True
        '
        'BtnL
        '
        Me.BtnL.Location = New System.Drawing.Point(140, 29)
        Me.BtnL.Name = "BtnL"
        Me.BtnL.Size = New System.Drawing.Size(28, 25)
        Me.BtnL.TabIndex = 2
        Me.BtnL.Text = "<"
        Me.BtnL.UseVisualStyleBackColor = True
        '
        'LangBox2
        '
        Me.LangBox2.AllowDrop = True
        Me.LangBox2.FormattingEnabled = True
        Me.LangBox2.Location = New System.Drawing.Point(176, 27)
        Me.LangBox2.Name = "LangBox2"
        Me.LangBox2.Size = New System.Drawing.Size(120, 56)
        Me.LangBox2.TabIndex = 5
        '
        'LangBox1
        '
        Me.LangBox1.AllowDrop = True
        Me.LangBox1.FormattingEnabled = True
        Me.LangBox1.Location = New System.Drawing.Point(12, 27)
        Me.LangBox1.Name = "LangBox1"
        Me.LangBox1.Size = New System.Drawing.Size(120, 56)
        Me.LangBox1.TabIndex = 1
        '
        'BtnOk
        '
        Me.BtnOk.Location = New System.Drawing.Point(221, 150)
        Me.BtnOk.Name = "BtnOk"
        Me.BtnOk.Size = New System.Drawing.Size(75, 23)
        Me.BtnOk.TabIndex = 9
        Me.BtnOk.Text = "OK"
        Me.BtnOk.UseVisualStyleBackColor = True
        '
        'BtnReset
        '
        Me.BtnReset.Location = New System.Drawing.Point(12, 150)
        Me.BtnReset.Name = "BtnReset"
        Me.BtnReset.Size = New System.Drawing.Size(75, 23)
        Me.BtnReset.TabIndex = 8
        Me.BtnReset.Text = "Reset"
        Me.BtnReset.UseVisualStyleBackColor = True
        '
        'LblUI
        '
        Me.LblUI.AutoSize = True
        Me.LblUI.Location = New System.Drawing.Point(10, 95)
        Me.LblUI.Name = "LblUI"
        Me.LblUI.Size = New System.Drawing.Size(103, 13)
        Me.LblUI.TabIndex = 6
        Me.LblUI.Text = "Interface Language:"
        '
        'UILangCB
        '
        Me.UILangCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UILangCB.FormattingEnabled = True
        Me.UILangCB.Location = New System.Drawing.Point(12, 110)
        Me.UILangCB.Name = "UILangCB"
        Me.UILangCB.Size = New System.Drawing.Size(121, 21)
        Me.UILangCB.TabIndex = 7
        '
        'LblApplySave
        '
        Me.LblApplySave.AutoSize = True
        Me.LblApplySave.Location = New System.Drawing.Point(12, 176)
        Me.LblApplySave.MaximumSize = New System.Drawing.Size(288, 0)
        Me.LblApplySave.MinimumSize = New System.Drawing.Size(288, 0)
        Me.LblApplySave.Name = "LblApplySave"
        Me.LblApplySave.Size = New System.Drawing.Size(288, 26)
        Me.LblApplySave.TabIndex = 10
        Me.LblApplySave.Text = "You must also click Apply or Save in the MusicBee settings box to save any change" &
    "s."
        Me.LblApplySave.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'BtnCancel
        '
        Me.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCancel.Location = New System.Drawing.Point(140, 150)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 11
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'ConfigForm
        '
        Me.AcceptButton = Me.BtnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnCancel
        Me.ClientSize = New System.Drawing.Size(308, 211)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.LblApplySave)
        Me.Controls.Add(Me.UILangCB)
        Me.Controls.Add(Me.LblUI)
        Me.Controls.Add(Me.BtnReset)
        Me.Controls.Add(Me.BtnOk)
        Me.Controls.Add(Me.LblLang2)
        Me.Controls.Add(Me.LblLang1)
        Me.Controls.Add(Me.BtnR)
        Me.Controls.Add(Me.BtnL)
        Me.Controls.Add(Me.LangBox2)
        Me.Controls.Add(Me.LangBox1)
        Me.Name = "ConfigForm"
        Me.Text = "MB_VocaDbLyrics Config"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblLang2 As System.Windows.Forms.Label
    Friend WithEvents LblLang1 As System.Windows.Forms.Label
    Friend WithEvents BtnR As System.Windows.Forms.Button
    Friend WithEvents BtnL As System.Windows.Forms.Button
    Friend WithEvents LangBox2 As System.Windows.Forms.ListBox
    Friend WithEvents LangBox1 As System.Windows.Forms.ListBox
    Friend WithEvents BtnOk As System.Windows.Forms.Button
    Friend WithEvents BtnReset As System.Windows.Forms.Button
    Friend WithEvents LblUI As System.Windows.Forms.Label
    Friend WithEvents UILangCB As System.Windows.Forms.ComboBox
    Friend WithEvents LblApplySave As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
End Class
