# SFXPlayer
Windows Sound Effects Launcher
Adrian Wadey


Experimental at the moment but planning to have it useable fairly quickly. I 
played with a few libraries and ended up using NuGet CSCore v1.2.1.2 published 
by Florian R. It allows multiple sounds to play at the same time and multiple 
output devices.

2018-12-15
Now useable.

Known issue:
File Dirty flag isn't set so you won't be prompted to save.
Only supports WAV files (maybe more but only tested WAVs)

To do:
File Dirty flag
The RTF box (bottom left) will be for notes/script but isn't supported yet.
Preview isn't supported yet.
Add a facility to save the cue file and the sound samples in one file for transfer between machines (ZIP?)
Scrolling in whole control units is not easy. I had to catch the mousewheel and trigger a timer, 
allow the system to scroll the panel and then in the timer reset the scroll to whole control units. Would 
like a better way to do this.
