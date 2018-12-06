# VocaDB Lyrics Plugin for MusicBee (MB_VocaDbLyrics)
This is a MusicBee plugin written in VB.NET to retrieve lyrics for songs from VocaDB, UtaiteDB, and TouhouDB.

The plugin operates as a standard lyrics provider, which means it will automatically act just like one of the default
lyrics sources. I believe it should work on MusicBee 2.0 and up, however testing has only ever been performed with
MusicBee 2.5+ (current dev is with 3.1).
I don't think there's any major issues with it, but I've been wrong about many things before. If something doesn't
work or could be done better, feel free to create an issue or put in a pull request.

VocaDbLyricsLib is used to retrieve the lyrics. This is another open source project created by me that makes it very
easy to get the lyrics.

　

### Usage notes:
Usage is simple, just place any DLLs in the MusicBee plugins directory and set up your lyrics provider priorities from
the 'Tags (2)' settings page.  
A visual interface is provided via the MusicBee plugin settings page. This allows you to set the interface language,
what languages to display in the returned lyrics (among other things).

Multilingual support is very basic and ideally should be rewritten, but for now this isn't a major issue.
User-defined languages are currently not supported.

　

### Options:
Note: all options will also show a tooltip when hovering over them.

* Displayed Lyrics: Sets languages to be included in the result and their order. Input a comma-separated list of
    language codes (additional codes can be found in the '639-1' column of
    [this wikipedia article](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)).  
    Adding "/[anything]" after a language code will show what you specify as the language name in lyrics
    (if omitted the language code will be shown instead).  
    Duplicated languages are automatically hidden, so including 'orig' won't show duplicated lyrics.  
    _Default value: "rom/Romanized, ja/Japanese, orig/Original Language, en/English"_

* UI Language: As the name implies, this sets the plugin interface language. English and Japanese are available.  
    _Default value: English (US)_

* Separator Lines: Sets the number of blank lines to leave between different languages in lyrics
(this makes it easier to quickly find the lyrics you're looking for).  
    _Default value: 5_

* Force Artist Matching: When enabled, all artists in the song tags must be present on VocaDB/UtaiteDB and also match
    the song's entry. If disabled, artists that aren't present in the database will be ignored.
    (all artists that are in the database must still match the song)  
    _Default value: Disabled_

* Artist Whitelist: This helps limit many unnecessary requests to the databases. At least one of these must be found in
    the song artist tag for any attempts at retrieving lyrics to occur.  
    It can be disabled by leaving the whitelist empty if you don't care about sending info about every song you play to
    VocaDB/UtaiteDB.  
    The default setting covers the 40 most used voices on VocaDB and top 10 artists on UtaiteDB in Japanese, so it may
    skip some artists you care about (it certainly will for UtaiteDB and TouhouDB users).  
    _Default value: "初音ミク,鏡音リン,鏡音レン,巡音ルカ,KAITO,MEIKO,GUMI,IA,結月ゆかり,重音テト,洛天依,神威がくぽ,猫村いろは,雪歌ユフ,SeeU,シユ,蒼姫ラピス,Lily,リリィ,SF-A2,v flower,さとうささら,波音リツ,MAYU,言和,乐正绫,ONE,音街ウナ,AVANNA,歌愛ユキ,VY1,CUL,カル,まふまふ,DECO,みきと,そらる,ギガ,花たん,YURiCa,ユリカ"_  
    _Romanized version of default: "Hatsune,Kagamine,Megurine,KAITO,MEIKO,GUMI,IA,Yuzuki,Kasane,Tianyi,Gakupo,Gackpoid,Nekomura,Sekka,SeeU,Aoki,Lily,SF-A2,v flower,Satou,Namine,MAYU,YANHE,Yan He,Yuezheng,ONE,Otomachi,AVANNA,Kaai,VY1,CUL,Mafumafu,Mikito,Soraru,GigaP,Hanatan,YuRiCa,Yurika"_

　

### Building:
To successfully complete a build of MB_VocaDbLyrics, copies of
[ILMerge](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx) and [7z](http://www.7-zip.org/) will be
needed in the path environment variable. Without these, the output will be suitable for testing but not for a final
release.

If you do build MB_VocaDbLyrics yourself, you'll most likely want to modify the post build events to suit your system,
primarily the commands which copy the plugin to "D:\Program Files (x86)\MusicBee\Plugins" (in my config).

You may also wish to set the start action (under the debug tab of the project properties) to launch your copy of
MusicBee too.

I am currently working in the Visual Studio 2015. As far as I know, there should be no compatibility issues with newer
verions, but I haven't even tried loading the solution something newer yet.

　

### Legal info:
NetNerd is not affiliated in any way with VocaDB.

MB_VocaDbLyrics comes with no warranty or guarantees. It probably won't break anything, but I won't promise you
anything. Use is at your own risk.

　

### Licensing:
MB_VocaDbLyrics is licensed under the GPL v3.

Source code can be found at https://github.com/NetNerd/MB_VocaDbLyrics.
