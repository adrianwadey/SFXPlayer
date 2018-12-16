# SFXPlayer
Windows Sound Effects Launcher

Adrian Wadey


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

Scrolling in whole control units is not easy. I had to catch the mousewheel and trigger a timer, 
allow the system to scroll the panel and then in the timer reset the scroll to whole control units. Would 
like a better way to do this.
