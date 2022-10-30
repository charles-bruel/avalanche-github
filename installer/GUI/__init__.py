import tkinter as tk
from tkinter import DISABLED, Canvas, Scrollbar, ttk
from tkinter import filedialog
from tkinter import messagebox

import shutil

import webbrowser

import sys
import os
from turtle import right
import zipfile

from distutils import command
from distutils.command import install

import Validator
import Converter_b64ToBinary as B64toB
import AssemblyPatch
import Converter_binaryToB64 as BtoB64

LARGEFONT =("Verdana", 15)
NORMALFONT = ("Arial", 10)
SMALLFONT = ("Arial", 8)

filename = False
BasePath = False
AssemblyPath = False
ManagedPath = False
#current = os.path.dirname(sys.executable)
current = os.getcwd()
ChangeLogs = os.listdir(f'{current}/ChangeLogs/')
CurrentChangeLog = False
SelectedChangeLog = False
ModDataZip = 'ModData.zip'
maps = os.listdir(f'{current}/maps/')
SelectedMaps = []

#custom logging
class Logger(object):
        def __init__(self):
                self.terminal = sys.stdout
                self.log = open("logfile.log", "w", encoding="utf8")

        def write(self, message):
                self.terminal.write(message)
                self.log.write(message)  

        def flush(self):
                pass

        def close(self):
                self.log.close()

sys.stdout = Logger()

class tkinterApp(tk.Tk):
        global filename
        # __init__ function for class tkinterApp
        def __init__(self, *args, **kwargs):

                # __init__ function for class Tk
                tk.Tk.__init__(self, *args, **kwargs)
                
                # creating a container
                container = tk.Frame(self)
                container.pack(side = "top", fill = "both", expand = False)
                
                self.geometry('450x325+700+200')
                self.minsize(450, 325)
                self.title('Snowtopia Mod Installer')
                
                # initializing frames to an empty array
                self.frames = {}

                # iterating through a tuple consisting
                # of the different page layouts
                for F in (StartPage, EnterFile, Configure, Validate, InstallPage, FinalPage):
                        frame = F(container, self)

                        # initializing frame of that object from
                        # pages respectively with
                        # for loop
                        self.frames[F] = frame

                        frame.grid(row = 0, column = 0, sticky ="nsew")

                self.show_frame(StartPage)

        # to display the current frame passed as
        # parameter
        def show_frame(self, cont):
                frame = self.frames[cont]
                frame.tkraise()

class StartPage(tk.Frame):
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)

                #blank space (padding + filling)
                fill1 = ttk.Label(self, text='')
                fill1.grid(row = 0, column = 0, pady=25, padx = 220)

                #heading
                label = ttk.Label(self, text ="Startpage", font = LARGEFONT)
                label.grid(row = 1, column = 0, padx = 15, pady = 15)

                #paragraph
                paragraph = ttk.Label(self, text="Welcome to the Snowtopia Mod installer!", font = NORMALFONT)
                paragraph.grid(row = 2, column = 0, pady = 15, padx = 10)
                
                #blank space (padding)
                fill2 = ttk.Label(self, text='')
                fill2.grid(row = 3, column = 0, pady=40)

                #help link
                helpLink = ttk.Label(self, text="Need Help?",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                helpLink.grid(row = 4, column = 0, padx = 10, sticky = tk.W)

                #make text an actual link
                helpLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://snowtopia-modders.fandom.com/wiki/Manual_installation_guide"))

                #Next button
                button2 = ttk.Button(self, text ="Next Page",
                command = lambda : controller.show_frame(EnterFile))
                
                button2.grid(row = 4, column = 0, padx = 10, pady = 10, sticky = tk.E)

class EnterFile(tk.Frame):
        global filename, ModDataToggle
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)

                #blank space (padding + filling)
                fill1 = ttk.Label(self, text='')
                fill1.grid(row = 0, column = 0, pady=15, padx = 220)

                #heading
                label = ttk.Label(self, text ="Enter Files", font = LARGEFONT)
                label.grid(row = 1, column = 0, padx = 100, pady = 20)

                #Entry Point text
                EPT = ttk.Label(self, text ="Input Snowtopia.exe here: ", font = NORMALFONT)
                EPT.grid(row = 2, column = 0, padx = 5, pady = 5)

                #Entry Point
                Browse = ttk.Button(self, text = "Browse A File",
                command = self.fileDialog)
                Browse.grid(row = 3, column = 0, padx = 5, pady = 5)

                global PathInput
                PathInput = tk.Text(self, font=SMALLFONT, width = 50, height = 1)
                PathInput.grid(row = 4, column = 0)

                #blank space (padding)
                fill2 = ttk.Label(self, text='')
                fill2.grid(row = 5, column = 0, pady=30)
                
                #help link
                helpLink = ttk.Label(self, text="Need Help?",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                helpLink.grid(row = 6, column = 0, padx = 10, sticky = tk.W)

                #make text an actual link
                helpLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://snowtopia-modders.fandom.com/wiki/Manual_installation_guide"))

                #Next button
                global Page1Next
                Page1Next = ttk.Button(self, text ="Next Page",
                command = lambda : controller.show_frame(Validate))

                Page1Next["state"] = "disabled"
                Page1Next.grid(row = 6, column = 0, padx = 10, pady = 10, sticky=tk.E)

        def fileDialog(self):
                global filename, BasePath, AssemblyPath, ModDataExists

                #this clears the output of any previous file selections (for e.g. if there were any errors)
                PathLabel = self.grid_slaves(column = 1, row = 2)
                for i in PathLabel:
                        i.destroy()

                PathInput.delete('1.0', 'end')
                
                #basic file browse
                filename = filedialog.askopenfilename(initialdir =  "/", title = "Select A File", filetype = (("Snowtopia File","*.exe"),("all files","*.*")) )
                
                if filename[-13:] != 'Snowtopia.exe': #if the filename selected isn't snowtopia then it throws an error
                        PathLabel = ttk.Label(self, font = NORMALFONT, text = 'invalid dir(1)')
                        PathLabel.grid(column = 1, row = 3, sticky = 'E')
                        Page1Next["state"] = "disabled"
                        return
                
                BasePath = filename[:-13] #the path containing snowtopia.exe in game files
                AssemblyPath = BasePath+'Snowtopia_Data/Managed/Assembly-CSharp.dll' #the path containing the assembly (all c# game code)

                if not os.path.exists(AssemblyPath): #if the assmebly cannot be found then it throws an error
                        PathLabel = ttk.Label(self, font = NORMALFONT, text = 'invalid dir(2)')
                        PathLabel.grid(column = 0, row = 3, sticky = 'E')
                        Page1Next["state"] = "disabled"
                        return

                PathLabel = ttk.Label(self, font = NORMALFONT, text = 'valid dir') #everything is valid
                PathLabel.grid(column = 0, row = 3, sticky = 'E')

                PathLabel.forget()
                
                PathInput.insert('end', filename)

                Page1Next["state"] = "enabled" #enables next button


class Validate(tk.Frame):
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)

                #blank space (padding + filling)
                fill1 = ttk.Label(self, text='')
                fill1.grid(row = 0, column = 0, pady=25, padx = 220)

                #heading
                label = ttk.Label(self, text ="Validate", font = LARGEFONT)
                label.grid(row = 1, column = 0, padx = 110, pady = 10)

                WarningLabel = ttk.Label(self, text ="This may take a few moments", font = NORMALFONT)
                WarningLabel.grid(row = 2, column = 0, pady = 5)
                
                #validate button
                ValidateButton = ttk.Button(self, text ="Validate", command = self.Validate)

                ValidateButton.grid(row = 3, column = 0, padx = 10)
                
                #blank space (padding)
                fill2 = ttk.Label(self, text='')
                fill2.grid(row = 4, column = 0, pady=44)

                #help link
                helpLink = ttk.Label(self, text="Need Help?",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                helpLink.grid(row = 5, column = 0, padx = 10, sticky = tk.W)

                #make text an actual link
                helpLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://snowtopia-modders.fandom.com/wiki/Manual_installation_guide"))

                #Next button
                global Page2Next
                if os.path.exists(ModDataZip):
                        Page2Next = ttk.Button(self, text ="Next Page", command = lambda : controller.show_frame(Configure))
                else:
                        Page2Next = ttk.Button(self, text ="Next Page", command = lambda : controller.show_frame(InstallPage))

                Page2Next['state'] = 'disabled'
                Page2Next.grid(row = 5, column = 0, padx = 10, pady = 10, sticky = tk.E)

        def Validate(self):
                global ChangeLogs, CurrentChangeLog
                CurrentChangeLog = False
                ValidatingLabel = ttk.Label(self, text ="Validated...", font = NORMALFONT) #shows operation is complete
                ValidatingLabel.grid(row = 3, column = 0, sticky = tk.E)
                
                result = Validator.Validate(AssemblyPath) #this uses the other 'Validator.py' script to add up all the binary digits e.g. 10101 = 1+0+1+0+1 = 3

                if result == 3891662: #this is the original unmodded game and marks that only the install patch is required
                        ResultLabel = ttk.Label(self, text ="Validated Sucsessfully!", font = NORMALFONT)
                        Page2Next['state'] = 'enabled'
                
                elif os.path.exists(f'{current}\ChangeLogs\{result}.txt'): #this checks whether there is a patch available to unintsalla any current mods by Validation code (47586748.txt for example)
                        CurrentChangeLog = f'{result}.txt'
                        ResultLabel = ttk.Label(self, text ="Validated Sucsessfully!(Other mods detected will be overwritten)", font = NORMALFONT)
                        Page2Next['state'] = 'enabled'
                else:
                        ResultLabel = ttk.Label(self, text =f"got {str(result)} which is invalid", font = NORMALFONT)

                ResultLabel.grid(row = 4, column = 0, pady = 5) #this shows to the user any errors within the GUI

class Configure(tk.Frame): #this is for selecting maps
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)

                #blank space (padding + filling)
                fill1 = ttk.Label(self, text='')
                fill1.grid(row = 0, column = 0, pady=0, padx = 220)

                #heading
                label = ttk.Label(self, text ="Configuration - Select maps to install", font = LARGEFONT)
                label.grid(row = 1, column = 0, pady = 5)

                #selection canvas
                base = tk.Canvas(self)
                base.grid(row = 4, column = 0)

                #scrollbar for opt (Listbox)
                scroll = Scrollbar(base)
                scroll.pack(side=tk.RIGHT, fill=tk.Y)

                #this gets messy but it's because of having assigments in certain locations
                #selection pane
                opt = tk.Listbox(base, selectmode="multiple")
                opt.pack()

                #applying scrollbar to ListBox (opt)
                opt.config(yscrollcommand=scroll.set)
                scroll.config(command=opt.yview)

                #drop-down menu
                options = ['Select...','Top 10', 'Top 25', 'None', 'Custom']

                # datatype of menu text
                clicked = tk.StringVar()

                def UpdateSelectedMaps(): #this gets called when next button is pressed and basically configures selection for install
                        global SelectedMaps
                        if SelectedMaps != []: #null check
                                SelectedMaps = opt.selection_get().split('\n')

                #Next button
                NextPage = ttk.Button(self, text ="Next Page", state = 'disabled',
                command = lambda : [UpdateSelectedMaps(), controller.show_frame(InstallPage)])
                
                NextPage.grid(row = 6, column = 0, padx = 10, pady = 10, sticky = tk.E)

                #a bit messy but these functions have to be here

                def UpdateDropSelection(event):
                        clicked.set("Custom")
                        NextPage['state'] = 'enabled'

                opt.bind('<<ListboxSelect>>', UpdateDropSelection)

                def UpdateDropSelectionMaps(event):
                        if clicked.get() != 'Select...':
                                NextPage['state'] = 'enabled'
                        if clicked.get() == 'None':
                                opt.select_clear(0, tk.END)
                                return
                        with open('TopMaps.cfg') as f:
                                if clicked.get() == 'Top 10':
                                        opt.select_clear(0, tk.END)
                                        Top10Maps = f.readlines()[0].split(',')
                                        Top10Maps[-1] = Top10Maps[-1][:-1]
                                        for select in Top10Maps:
                                                opt.select_set(maps.index(select))
                                        return
                                if clicked.get() == 'Top 25':
                                        opt.select_clear(0, tk.END)
                                        Top25Maps = f.readlines()[1].split(',')
                                        for select in Top25Maps:
                                                opt.select_set(maps.index(select))
                                        return

                # Create Dropdown menu
                drop = ttk.OptionMenu(self, clicked, *options, command = UpdateDropSelectionMaps)
                drop.grid(row = 3, column = 0, pady = 7)

                #inflate map selection
                for i in maps:
                        opt.insert(tk.END, i)

                #padding
                padding = ttk.Label(self, text="")
                padding.grid(row = 5, column = 0, pady = 2)

                #help box (for both help and selection of maps)
                helpBox = Canvas(self)
                helpBox.grid(row = 6, column = 0, padx = 10, sticky = tk.W)

                #help link
                helpLink = ttk.Label(helpBox, text="Need Help?",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                helpLink.grid(row=0, column=0, padx=10)

                #make text an actual link
                helpLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://snowtopia-modders.fandom.com/wiki/Manual_installation_guide"))

                #maps link
                mapLink = ttk.Label(helpBox, text="Browse maps",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                mapLink.grid(row=0, column=1, padx=10)

                #make text an actual link
                mapLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://www.snowtopiamodding.com"))

class InstallPage(tk.Frame):
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)

                #blank space (padding + filling)
                fill1 = ttk.Label(self, text='')
                fill1.grid(row = 0, column = 0, pady=25, padx = 220)

                #heading
                label = ttk.Label(self, text ="Install", font = LARGEFONT)
                label.grid(row = 1, column = 0, pady = 10)

                WarningLabel = ttk.Label(self, text ="This may take a few moments", font = NORMALFONT)
                WarningLabel.grid(row = 2, column = 0, padx = 5, pady = 5)

                #Install button
                global InstallButton
                InstallButton = ttk.Button(self, text ="Install", command = self.Install)
                InstallButton.grid(row = 3, column = 0, padx = 10)

                #blank space (padding)
                fill2 = ttk.Label(self, text='')
                fill2.grid(row = 4, column = 0, pady=44)

                #help link
                helpLink = ttk.Label(self, text="Need Help?",font=('Helveticabold', 10),foreground = "blue", cursor="hand2")
                helpLink.grid(row=5, column=0, padx=10, sticky=tk.W)

                #make text an actual link
                helpLink.bind("<Button-1>", lambda e: webbrowser.open_new_tab("https://snowtopia-modders.fandom.com/wiki/Manual_installation_guide"))

                #next
                global FinalNext
                FinalNext = ttk.Button(self, text ="Next Page", state = "disabled",
                command = lambda : controller.show_frame(FinalPage))

                FinalNext.grid(row = 5, column = 0, padx = 10, pady = 10, sticky = tk.E)

        def Install(self):
                ValidatingLabel = ttk.Label(self, text ="Process finished!", font = NORMALFONT) #this will show regardless on the fact whether the function completes without errors
                ValidatingLabel.grid(row = 2, column = 0, sticky = tk.E)

                BtoB64.Convert(AssemblyPath) #this converts the binary of the Assmebly dll into base64 which is what all the patches are written with

                if CurrentChangeLog != False:
                        AssemblyPatch.Patch(f'ChangeLogs/{CurrentChangeLog}', "Assembly-CSharp.txt.b64") #this uninstalls any previous mods
                
                AssemblyPatch.Patch('ChangeLogs/Install.txt', "NewAssembly.txt.b64") #installs any new mods
                B64toB.Convert(AssemblyPath) #converts base64 patched dll back to binary

                if os.path.exists(ModDataZip):
                        if os.path.exists(f'{BasePath}/ModData'):
                                shutil.rmtree(f'{BasePath}/ModData') #removes current ModData from game files     
                        with zipfile.ZipFile(ModDataZip, 'r') as zip_ref: 
                                zip_ref.extractall(f'{BasePath}/ModData') #adds new ModData to game files (essentually a complete overwrite)
                        
                        for m in SelectedMaps:
                                if not os.path.exists(f'{BasePath}/ModData/maps/{m}'):
                                        shutil.copytree(f'maps/{m}', f'{BasePath}/ModData/maps/{m}') #adds selected Maps from Configiure class into ModData

                FinalNext['state'] = 'enabled'
                InstallButton['state'] = 'disabled'

                #cleaning up
                os.remove("Assembly-CSharp.txt.b64")
                os.remove("NewAssembly.txt.b64")

                ResultLabel = ttk.Label(self, text ="Installed Sucsessfully!", font = NORMALFONT)
                ResultLabel.grid(row = 4, column = 0)


class FinalPage(tk.Frame):
        def __init__(self, parent, controller):
                tk.Frame.__init__(self, parent)
                label = ttk.Label(self, text ="Thanks for Installing!", font = LARGEFONT)
                label.grid(row = 0, column = 0, padx = 80, pady = 70)

                # button to show frame 2 with text
                # layout2
                button1 = ttk.Button(self, text ="Exit", command = sys.exit)

                #blank space (padding)
                fill2 = ttk.Label(self, text='')
                fill2.grid(row = 1, column = 0, pady=45, padx = 220)

                # putting the button in its place by
                # using grid
                button1.grid(row = 2, column = 0, padx = 10, pady = 10, sticky = 'E')

# Driver Code
app = tkinterApp()
app.mainloop()
