### About The Project

Hi! I've run into the issue where your mic makes a "crackling" sound in most VoIP Applications (TeamSpeak, Discord, Zoom etc.) when using a Focusrite Scarlett Interface.

There are a lot of reports of this bug with various fixes online, however most of them didn't work for me and, according to the comments, didnt work for a majority of the userbase.

The only reliable fix I found was setting the process priority of "audiodg.exe" to "High" and setting its core affinity so that only one core is used.

I assume the issue appears because the interface drivers run into issues when syncing their clock / refreshrate? Just a guess.

As always, fuck focusrite and remember to not buy from Thomann.

Have a nice day.

### What this does

* Adds / Edits a scheduled Task, which just runs itself on every user logon with highest available privileges. 
* Loops through all processes looking for processes called "audiodg.exe"
* If it doesnt find any, it will wait 15 seconds and try again. This loops infinitively.
* Sets the process priority to HIGH and its core affinity to use only the CPU 2 (third CPU core / thread)

* This probably only works on 64 Bit Windows 10 and higher.

### How to run

* Download the latest .exe [here](https://github.com/TwosHusbandS/FocusriteCracklingFix/releases/download/0.1.0.0/FocusriteCracklingFix.exe)
* Put it into a folder (a permanent place, not just your Download Path)
* Run the .exe once.
* Done. It will automatically start itself on every reboot. So dont move the .exe.
* If you do end up having to move your .exe, just run manually once.

### License

Licensed under MIT, do whatever you want with this.
