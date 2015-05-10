Option Strict On
Imports System.Runtime.InteropServices

<ComVisible(True)>
<Guid("D0A174A7-2C20-40EA-B797-EC3B8D199875")>
<ClassInterface(ClassInterfaceType.AutoDispatch)>
Public Class VocaDbLyricsLib
    'Public variables
    ''' <summary>The user agent to be used in any web requests.</summary>
    Public UserAgent As String = "VocaDbLyricsLib/0.1"
    Private DefaultUserAgent As String = "VocaDbLyricsLib/0.1"

    ''' <summary>Controls whether or not the default user agent of the library is appended to the provided one. Recommended if using a custom user agent. Defaults to true.</summary>
    Public AppendDefaultUserAgent As Boolean = True

    ''' <summary>The web proxy to be used. 'Nothing' (and presumably 'null') use the default system proxy.</summary>
    ''' Nothing is equivalent to not setting anything as far as the WebClient is concerned.
    Public Proxy As Net.WebProxy = Nothing

    ''' <summary>The URL of the site to get results from. This allows the library to be used for other sites based on the VocaDB code. Note that the only other such site, UtaiteDB, uses an older version of the API and is incompatible with this library (as of 2015-04-27).</summary>
    Public DatabaseUrl As Uri = New Uri("http://vocadb.net/")

    ''' <summary>An array of strings used to split the artist field into multiple strings (eg. "feat.", "and").</summary>
    Public ArtistSplitStrings() As String = {"feat.", "ft.", " X ", " x ", " Ｘ ", " ｘ ", "×", "✕", "✖", "⨯", "╳", ",", ";", "+", "(", "&", " and "}

    ''' <summary>Sets how many entries the library will search for lyrics in before giving up. It will stop at the first original song found. Not applicable when getting lyrics by ID.</summary>
    Public SearchTries As UInt16 = 2

    ''' <summary>When this is true, the library will stop searching for new results (see <see cref="SearchTries" />) if it finds one with the instrumental tag. In most cases, this helps to prevent issues from finding versions with lyrics, but it may cause issues.</summary>
    Public DetectInstrumental As Boolean = True

    ''' <summary>VocaDB supports multiple orders to present results in when searching. This selects the one to use for songs. By default, FavoritedTimes is used to favour more popular songs.</summary>
    Public SongSearchSort As String = "FavoritedTimes"

    ''' <summary>VocaDB supports multiple orders to present results in when searching. This selects the one to use for artists. By default, FollowerCount is used to favour more popular artists.</summary>
    Public ArtistSearchSort As String = "FollowerCount"


    'Data Structures
    ''' <summary>Contains lyrics and associated information.</summary>
    Public Structure LyricsContainer
        Public Language As String
        Public Lyrics As String
    End Structure

    ''' <summary>Contains a LyricsContainer, as well as any errors or warnings encountered while getting the lyrics.</summary>
    ''' <remarks>
    ''' Errors mean that no lyrics have been returned; the LyricsContainer is Nothing.
    ''' Warnings mean that lyrics were successfully retrieved, but there may be issues with them.
    ''' </remarks>
    Public Structure LyricsResult
        Public LyricsContainers() As LyricsContainer
        Public ErrorType As VocaDbLyricsError
        Public WarningType As VocaDbLyricsWarning
    End Structure


    'Enums (errors/warnings)
    ''' <summary>Indicates the type of error that was encountered.</summary>
    Public Enum VocaDbLyricsError
        ''' <summary>No error was encountered.</summary>
        None = 0

        ''' <summary>The specified song was not found.</summary>
        NoSong = 1

        ''' <summary>No lyrics were found for the song.</summary>
        NoLyrics = 2

        ''' <summary>There was an error connecting to VocaDB.</summary>
        ConnectionError = 3

        ''' <summary>The song was detected as intrumental. Only used if <see cref="DetectInstrumental" /> is true.</summary>
        IsInstrumental = 4
    End Enum

    ''' <summary>Indicates the type of warning that was encountered.</summary>
    Public Enum VocaDbLyricsWarning
        ''' <summary>No warning was encountered.</summary>
        None = 0

        ''' <summary>The specified artist(s) was not found, and therefore ignored.</summary>
        NoArtist = 1

        ''' <summary>Lyrics were taken from the original version of the song instead of the detected cover/remix.</summary>
        UsedOriginal = 2
    End Enum


    ''' <summary>
    ''' Returns the lyrics of a song on VocaDB from its name.
    ''' </summary>
    ''' <param name="Song">The name of the song.</param>
    ''' <param name="Artist">One or more artists of the song. Only the first provided artist that is found in the database will be used to find the song. (Optional)</param>
    Public Function GetLyricsFromName(Song As String, Optional Artist As String = Nothing) As LyricsResult
        Dim LyricsResult As New LyricsResult
        Dim ArtistId As Integer = -1

        Song = Song.Trim()

        If Artist IsNot Nothing Then
            Dim Artists() As String
            Artists = Artist.Split(ArtistSplitStrings, 2, StringSplitOptions.RemoveEmptyEntries)

            For i = 0 To Artists.Count - 1
                If ArtistId = -1 Then
                    Artists(i) = Artists(i).Trim()
                    ArtistId = GetArtistId(Artists(i))
                Else
                    i = Artists.Count
                End If
            Next
        End If

        For i = 0 To SearchTries - 1
            LyricsResult = GetLyricsResultFromXml(GetSongFromName(Song, ArtistId, i))
            If LyricsResult.ErrorType = VocaDbLyricsError.None Or LyricsResult.ErrorType = VocaDbLyricsError.IsInstrumental Then i = SearchTries
        Next

        If ArtistId = -1 AndAlso Artist IsNot Nothing AndAlso Artist.Length > 0 Then
            LyricsResult.WarningType = VocaDbLyricsWarning.NoArtist
        End If

        Return LyricsResult
    End Function

    ''' <summary>
    ''' Returns the lyrics for a song on VocaDB from its ID.
    ''' </summary>
    ''' <param name="SongId">The ID of the song.</param>
    Public Function GetLyricsFromId(SongId As Integer) As LyricsResult
        Return GetLyricsResultFromXml(GetSongFromId(SongId))
    End Function


    Private Function GetArtistId(Artist As String) As Integer
        Dim Xml As New Xml.XmlDocument

        Try
            Xml.LoadXml(DownloadXml(DatabaseUrl.AbsoluteUri & "api/artists?query=" & Artist & "&sort=" & ArtistSearchSort & "&maxResults=1&nameMatchMode=exact&maxResults=1"))
        Catch
            Return -1
        End Try

        If Xml.GetElementsByTagName("Id").Count > 0 Then
            Return CInt(Xml.GetElementsByTagName("Id")(0).InnerText)
        Else
            Return -1
        End If
    End Function

    Private Function GetSongFromName(Song As String, Optional ArtistId As Integer = Nothing, Optional Start As Integer = 0) As Xml.XmlDocument
        Dim Xml As New Xml.XmlDocument

        Try
            If ArtistId = -1 Then Xml.LoadXml(DownloadXml(DatabaseUrl.AbsoluteUri & "api/songs?query=" & Song & "&sort=" & SongSearchSort & "&fields=lyrics,tags&nameMatchMode=exact&maxResults=1&start=" & Start)) _
                Else Xml.LoadXml(DownloadXml(DatabaseUrl.AbsoluteUri & "api/songs?query=" & Song & "&artistId=" & ArtistId & "&sort=" & SongSearchSort & "&fields=lyrics,tags&nameMatchMode=exact&maxResults=1&start=" & Start))
        Catch
            Return Nothing
        End Try

        Return Xml
    End Function

    Private Function GetSongFromId(SongId As Integer) As Xml.XmlDocument
        Dim Xml As New Xml.XmlDocument

        Try
            Xml.LoadXml(DownloadXml(DatabaseUrl.AbsoluteUri & "api/songs/" & SongId & "?fields=lyrics,tags"))
        Catch
            Return Nothing
        End Try

        Return Xml
    End Function

    Private Function DownloadXml(Url As String) As String
        Dim WebClient As New System.Net.WebClient
        Dim RtnStr As String

        If UserAgent IsNot DefaultUserAgent AndAlso AppendDefaultUserAgent = True Then WebClient.Headers.Add(Net.HttpRequestHeader.UserAgent, UserAgent & " (" & DefaultUserAgent & ")") _
            Else WebClient.Headers.Add(Net.HttpRequestHeader.UserAgent, UserAgent)

        WebClient.Headers.Add(Net.HttpRequestHeader.Accept, "application/xml")
        WebClient.Proxy = Proxy
        WebClient.Encoding = System.Text.Encoding.UTF8

        RtnStr = WebClient.DownloadString(Url)
        WebClient.Dispose()
        Return RtnStr
    End Function

    Private Function GetLyricsResultFromXml(Xml As Xml.XmlDocument) As LyricsResult
        Dim LyricsResult As New LyricsResult
        LyricsResult.LyricsContainers = {}

        'This is the error detection code:
        If Xml Is Nothing Then
            LyricsResult.ErrorType = VocaDbLyricsError.ConnectionError
            Return LyricsResult
        End If

        If Xml.GetElementsByTagName("Id").Count = 0 Then
            LyricsResult.ErrorType = VocaDbLyricsError.NoSong
            Return LyricsResult
        End If

        If Xml.GetElementsByTagName("LyricsForSongContract").Count = 0 Then

            'We only want to return lyrics for non-instrumentals. If we detect an instrumental, we can return an error (if DetectInstrumental = true).
            If DetectInstrumental = True Then
                For i = 0 To Xml.GetElementsByTagName("TagUsageForApiContract").Count - 1
                    If Xml.GetElementsByTagName("TagUsageForApiContract")(i).Item("Name").InnerText = "instrumental" Then
                        LyricsResult.ErrorType = VocaDbLyricsError.IsInstrumental
                        Return LyricsResult
                    End If
                Next
            End If

            If Xml.GetElementsByTagName("OriginalVersionId").Count > 0 Then
                'If there's an id for an original version, we've found a cover/remix and there might be lyrics in the original version's entry.
                LyricsResult = GetLyricsResultFromXml(GetSongFromId(CInt(Xml.GetElementsByTagName("OriginalVersionId")(0).InnerText)))
                LyricsResult.WarningType = VocaDbLyricsWarning.UsedOriginal
                Return LyricsResult
            Else
                'If there's no original version, we have no other options; there's no lyrics available.
                LyricsResult.ErrorType = VocaDbLyricsError.NoLyrics
                Return LyricsResult
            End If
        End If

        Dim LyricsContainers(Xml.GetElementsByTagName("LyricsForSongContract").Count - 1) As LyricsContainer
        For i = 0 To LyricsContainers.Length - 1
            Dim Lyrics = Xml.GetElementsByTagName("LyricsForSongContract")(i)
            LyricsContainers(i).Language = Lyrics.Item("Language").InnerText
            LyricsContainers(i).Lyrics = Lyrics.Item("Value").InnerText
        Next
        LyricsResult.LyricsContainers = LyricsContainers
        Return LyricsResult
    End Function
End Class

