# MB_VocaDbLyrics
This is a MusicBee plugin written in VB.NET to retreive lyrics for songs from VocaDB.

The plugin operates as a standard lyrics provider, which means it will automatically act just like one of the included lyrics sources. I believe it should work on MusicBee 2.0 and up, however testing has only been performed with MusicBee 2.5.
I don't think there's any major issues with it, but I've been wrong about many things before. If something doesn't work or could be done better, feel free to create an issue or put in a pull request.

VocaDbLyricsLib is used to retrieve the lyrics. This is another open source project created by me that makes it very easy to get the lyrics.

&nbsp;

### Usage notes:
Usage is simple, just place any DLLs in the MusicBee plugins directory.
A visual interface is provided via the MusicBee plugin settings page. This allows you to set the interface language and what languages to display in the returned lyrics.

Multilingual support is very basic and ideally should be rewritten, but for now this isn't a major issue.
User-defined interface languages are not currently supported.

&nbsp;

### Building:
To successfully complete a build of MB_VocaDbLyrics, copies of [ILMerge](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx) and [7z](http://www.7-zip.org/) will be needed in the path environment variable. Without these, the output will be suitable for testing but not for a final release.

If you do build MB_VocaDbLyrics yourself, you'll most likely want to modify the post build events to suit your system, primarily the commands which copy the plugin to "D:\Program Files (x86)\MusicBee\Plugins" (in my config).

You may also wish to set the start action (under the debug tab of the project properties) to launch your copy of MusicBee too.

I am currently working in the Visual Studio 2015 Community RC. As far as I know, there should be no compatibility issues when using VS 2013, but I haven't even tried loading the solution in the older version.

&nbsp;

### Legal info:
NetNerd is not affiliated in any way with VocaDB.

MB_VocaDbLyrics comes with no warranty or guarantees. It probably won't break anything, but I won't promise you anything. Use is at your own risk.

&nbsp;

### Licensing:
MB_VocaDbLyrics is licensed under the GPL v3.

Source code can be found at https://github.com/NetNerd/MB_VocaDbLyrics.
