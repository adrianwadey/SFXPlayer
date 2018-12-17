# SFXPlayer
Windows Sound Effects Launcher

by Adrian Wadey

Sound Effects Player for theatrical use.

*    Click + to add a cue.
*    Click the "F" File button and choose the sound file. Click "F" again to remove the file.
*    Click the text box on the cue and change the text as needed. There is an
additional large text box bottom left but that only shows for the currently 
selected cue.
*    Check the Stop All check box on the right if you want this cue to stop all others.
*    Check the Stop All check box without adding a sound to add a "Stop" cue.
*    Each cue also has individual play/stop buttons.
*    Export Show will create a zip file with all the sounds and the sfx file.
*    Import Show will decompress a ZIP file and load the sfx file.




Experimental at the moment but planning to have it useable fairly quickly. I 
played with a few libraries and ended up using NuGet CSCore v1.2.1.2 published 
by Florian R. It allows multiple sounds to play at the same time and multiple 
output devices.

### 2018-12-15
Now useable.

### 2018-12-16
Most other major features now added

CueList position is now saved in show file

Added support for Archive. This creates a zip with all files (audio plus cues). Importing 
a show extracts the zip and opens the contents.

## Known issue:
Only supports WAV files (maybe more but only tested WAVs)

## To do:
Preview isn't supported yet.

Index numbers aren't shown yet (all show 000)

Toolbar

File button Icon
sfx file icon

Improve the cue selection. At the moment you have to scroll up and down using the scroll bar far right.

Scrolling in whole control units is not easy. I had to catch the mousewheel and trigger a timer, 
allow the system to scroll the panel and then in the timer reset the scroll to whole control units. Would 
like a better way to do this.

