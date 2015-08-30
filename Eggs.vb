'So yeah... some songs played using the Beep function.
Public Class Eggs

    Private Shared BeepThread As New Threading.Thread(New Threading.ThreadStart(AddressOf BeepThreadSub))
    Private Shared CurrSong As BeepSong
    Private Shared A440Frequency As Integer = 440

    Public Structure BeepSong
        Dim Name As String
        Dim QuarterNoteLength As UInteger
        Dim Notes As Single()
    End Structure

    Public Shared Sub PlaySong(Song As BeepSong)
        If BeepThread.IsAlive = False Then
            CurrSong = Song
            BeepThread = New Threading.Thread(New Threading.ThreadStart(AddressOf BeepThreadSub))
            BeepThread.Start()
        End If
    End Sub

    Public Shared Sub StopBeeps()
        BeepThread.Abort()
    End Sub

    Private Shared Sub BeepThreadSub()
        For i As Integer = 0 To CurrSong.Notes.Length - 1
            Dim Freq As Integer = GetNoteFrequency(CurrSong.Notes(i))
            If Freq = 0 Then
                Threading.Thread.Sleep(GetNoteLength(CurrSong.Notes(i + 1)))
            Else
                Console.Beep(Freq, GetNoteLength(CurrSong.Notes(i + 1)))
            End If
            i += 1
        Next
    End Sub

    Private Shared Function GetNoteFrequency(Note As Single) As Integer
        Dim Freq As Integer = A440Frequency * Math.Pow(1.059463, Note - 69)
        If Freq > 36 And Freq < 32768 Then
            Return Freq
        End If
        Return 0
    End Function

    Private Shared Function GetNoteLength(Length As Single) As Integer
        If Length > -1 Then
            Return CurrSong.QuarterNoteLength * Length / 2
        End If
        Return 0
    End Function


    Public Shared KarakuriPierrot As New BeepSong With {.Name = "Karakuri Pierrot",
                                                        .QuarterNoteLength = 588,
                                                        .Notes = {81, 1.33, 0, 0.67, 77, 0.67, 79, 0.33, 0, 0.67, 81, 1.5, 0, 0.83, 77, 0.67, 79, 0.33, 0, 0.67, 81, 1.5, 0, 0.83,
                                                                  77, 0.67, 79, 0.33, 0, 0.67, 81, 1.33, 79, 1, 77, 0.67, 79, 0.33, 77, 0.41, 0, 0.59,
                                                                  81, 1.17, 0, 0.83, 77, 0.67, 79, 0.33, 0, 0.67, 81, 1.5, 0, 0.83, 77, 0.67, 79, 0.33, 0, 0.67, 81, 1.33,
                                                                  79, 1, 77, 0.67, 79, 1, 77, 3.4, 0, 0.93,
                                                                  81, 1.33, 0, 0.67, 77, 0.67, 79, 0.33, 0, 0.67, 81, 1.5, 0, 0.83, 77, 0.67, 79, 0.33, 0, 0.67,
                                                                  81, 1.33, 81, 1, 81, 0.67, 82, 0.33, 0, 0.67, 84, 1.33, 82, 1, 81, 1, 77, 1, 0, 1,
                                                                  77, 1, 84, 0.67, 84, 0.33, 0, 0.67, 84, 1.33, 86, 1, 81, 1, 77, 1, 79, 1, 77, 0.67, 77, 0.33, 76, 0.67, 77, 0.33, 0, 0.67, 77, 3.33}}

    Public Shared WorldsEndDancehall As New BeepSong With {.Name = "World's End Dancehall",
                                                           .QuarterNoteLength = 351,
                                                           .Notes = {89, 2, 82, 1, 82, 1, 89, 2, 82, 1, 82, 1, 87, 1, 89, 1, 87, 1, 85, 1, 87, 2,
                                                                     89, 2, 87, 2, 0, 1, 89, 1, 87, 2, 0, 1, 89, 1, 87, 1, 89, 1, 87, 1, 89, 1, 93, 2, 81, 2,
                                                                     89, 2, 82, 1, 82, 1, 89, 2, 82, 1, 82, 1, 90, 1, 92, 1, 90, 1, 89, 1, 87, 2,
                                                                     89, 2, 87, 2, 0, 1, 89, 1, 87, 2, 0, 1, 89, 1, 87, 1, 89, 1, 87, 1, 89, 1, 93, 2, 96, 2,
                                                                     89, 2, 82, 1, 82, 1, 89, 2, 82, 1, 82, 1, 87, 1, 89, 1, 87, 1, 85, 1, 87, 2,
                                                                     89, 2, 87, 2, 0, 1, 89, 1, 87, 1, 89, 1, 92, 1, 89, 1, 87, 2, 87, 1, 89, 1, 93, 2, 87, 2,
                                                                     89, 2, 82, 2, 89, 2, 82, 2, 90, 1, 92, 1, 90, 1, 89, 1, 87, 2,
                                                                     89, 2, 87, 2, 0, 1, 89, 1, 87, 2, 0, 1, 89, 1, 87, 1, 89, 1, 87, 1, 89, 1}}
End Class
