The VocaDB Lyrics Plugin (MB_VocaDbLyrics) is a MusicBee plugin written in
VB.NET to retrieve lyrics for songs from VocaDB and UtaiteDB.

The plugin operates as a standard lyrics provider, which means it will
automatically act just like one of the included lyrics sources. I believe it
should work on MusicBee 2.0 and up, however testing has only been performed
with MusicBee 2.5.
I don't think there's any major issues with it, but I've been wrong about many
things before. If something doesn't work or could be done better, feel free to
create an issue or put in a pull request.

VocaDbLyricsLib is used to retrieve the lyrics. This is another open source
project created by me that makes it very easy to get the lyrics.



--Usage notes:--
Usage is simple, just place any DLLs in the MusicBee plugins directory and set
up your lyrics provider priorities from the 'Tags (2)' settings page.
A visual interface is provided by the plugin to change some options. This
allows you to set the interface language, what languages to display in the
returned lyrics (among other things).
Codes for additional languages can be found in the '639-1' column of this
wikipedia article: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes

Multilingual support is very basic and ideally should be rewritten, but for now
this isn't a major issue.
User-defined languages are currently not supported.



--Legal info:--
NetNerd is not affiliated in any way with VocaDB.

MB_VocaDbLyrics comes with no warranty or guarantees. It probably won't break
anything, but I won't promise you anything. Use is at your own risk.



--Licensing:--
MB_VocaDbLyrics is licensed under the GPL v3.

Source code can be found at https://github.com/NetNerd/MB_VocaDbLyrics.
