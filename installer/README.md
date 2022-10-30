# Dependencies
#### Not included in python.org install
* diff_match_patch.py from https://github.com/google/diff-match-patch/tree/master/python3 - place in the same level as python files you wish to run
* imp - `pip install imp`
#### Imports included in python.org install
* tkinter
* shutil
* locale

# About
The purpose of the installer is to install mods to the game without containing any game code. This GUI application was developed by !Tea (Dan771) and has the functionality of an installer: Firstly, It checks for any mods already installed by comparing the hash value of the game code Assembly - If mods need to be uninstalled it searches for related uninstall 'patches' to convert back to the original game. From the original game, Install 'patches' are then applied to generate the new modded assembly and install the game code. Alongside this, the installer also adds extracts ModData into the game files with any maps selected by the user. We build the installer using PyInstaller to generate an executable .exe which contains all the dependecies (even including python) so anyone can run it on any system. The GUI is designed with simplicity, usability and functionality in mind to maximise accessabiltiy to less technical individuals. A demonstration of the installer is shown bellow
