import random
import time
import sys
import os
import os.path
from os import environ
environ['PYGAME_HIDE_SUPPORT_PROMPT'] = '1'
import pygame
from tqdm import tqdm

aDrive = '''Note: All these files and folders cannot be accessed.
AOs.set
Patch.set
Users
xFiles
xFiles.72'''

AOsSet = '''Note: All these files and folders cannot be accessed.
Addz
Boot
Sys72
Software.Distribution'''
PatchSet = "You don't have the permission to access this folder."

Users = '''Public
xBookA13Pro'''

xFiles = '''Note: All these files and folders cannot be accessed.
AOsProtable
AOs System controller
Attz
Commons
coreSecurity
Lens
PixStore
relp
temp'''

xFiles72 = '''Note: All these files and folders cannot be accessed.
A32
AOs.Access
Commons
Sys64
sys.temp
relp.func'''

xBookA13Pro = '''Desktop
Downloads
Documents
Pictures
Links
Music
Search
Videos'''

Desktop = '''This PC
Core Settings
Network-Base'''

Documents = "This folder is empty."

Downloads = "This folder is empty."

Pictures = "This folder is empty."

Links = "You don't have the permission to access this folder."

Music = "This folder is empty."

Search = "You don't have the permission to access this folder."

Videos = "This folder is empty."

helpSet = '''For more information on a specific command, type _help command-name
_clear          Clean all the unwanted text.
_generate       Generate a new random number.
_ran            Displays machine specific properties and configuration.

_shout          Prints a text on to the screen.
_get            Takes input from the user.
_var            Stores date taken from the user to use it later.

_search         Helps to open files and directory quickly.
_scan           Scans the system to check for viruses, malwares, spywares and etc.
_repair         Repairs any issue in your pc.

_fixerror       Repairs any issue in your pc.
_color          Changes the user-interface theme.
_update         Updates your pc to the latest version.

_maths          Calculate integers given by user.
_timer          Creats a stop-watch for users.
_calander       Displays current time and date.

_refresh        Optimizes the pc for better performance.
_corner         A web-browser to browse certain websites.
_shutdown       Close all the apps and turns of your PC.

_quit           Close all the apps and turns of your PC.
_restart        Restarts your pc if it's not working properly.
_lock           Locks your pc so others can't access your system.

_builder        Provides user a notepad to store multi-line data.
_play           A game where you have to guess the number.
_optimize       Optimizes your pc drives for better peformance.

_ping           Checks whether your internet is working properly or not.
_organize       Organizes system files and folders for fast search.
_read           Deletes all the the temporary files from the system.

_run            Launches a certain program for the user.
_reset          Reinstall AOs in your system. Your system files will be not affected.
_restore        Restores the last back up of your system.

_terminate      Force shutdowns the system.
_process        It starts other services to make system more easier to function.
_random         An artificial intelligence which will talk with you.

_pixstore       Helps you to download applications easily.
_sound          Allows you to play audio files.
_AOs1000        AOs is made on 1000 lines of code.

_credits        Provides Credit to Developers.
_admin          Starts admin version of AOs.
_help           Provides user a list of commands.
'''

creditSet = '''__________ AOS-Team __________
Developer - Srijan Srivastava

Found on - 15 June 2020

__________ Note(For Developers Only) __________
|| AOs - Terminal based Operating System
|| Contact: Srivastavavsrijan321@gmail.com

|| You can reuse the code source of the program at condition you say in your
|| program "Based on AOs Kernel 1.3"
__________________________________________
'''

def AOs():
    os.system('title AOs')
    os.system('cls')
    value = True
    while value:
        check = input("$")

        if check == "" or check == " ":
            pass

        elif check == "_generate":
            generate = random.randint(-1, 10000000)
            print(generate)

        elif check == "_clear":
            AOs()

        elif check == "_ran":
            os.system('systeminfo')

        elif check == "_aDrive":
            print(aDrive)

        elif check == "_aDrive-AOs.set":
            print(AOsSet)

        elif check == "_aDrive-Patch.set":
            print(PatchSet)

        elif check == "_aDrive-Users":
            print(Users)

        elif check == "_aDrive-xFiles":
            print(xFiles)

        elif check == "_aDrive-xFiles.72":
            print(xFiles72)

        elif check == "_aDrive-Users-xBookA13Pro":
            print(xBookA13Pro)

        elif check == "_aDrive-Users-xBookA13Pro-Desktop":
            print(Desktop)

        elif check == "_aDrive-Users-xBookA13Pro-Documents":
            print(Documents)

        elif check == "_aDrive-Users-xBookA13Pro-Downloads":
            print(Downloads)

        elif check == "_aDrive-Users-xBookA13Pro-Pictures":
            print(Pictures)

        elif check == "_aDrive-Users-xBookA13Pro-Links":
            print(Links)

        elif check == "_aDrive-Users-xBookA13Pro-Music":
            print(Music)

        elif check == "_aDrive-Users-xBookA13Pro-Music":
            print(Music)

        elif check == "_aDrive-Users-xBookA13Pro-Search":
            print(Search)

        elif check == "_aDrive-Users-xBookA13Pro-Videos":
            print(Videos)

        elif check == "_shout":
            say = input(">>> ")
            print(say)

        elif check == "_get":
            getData = input(">>> ")

        elif check == "_shout-get":
            print(getData)

        elif check == "_var":
            getVar = input(">>> ")

        elif check == "_shout-var":
            print(getVar)

        elif check == "_get-var":
            sysVar = input(getVar + " ")

        elif check == "_shout-get-var":
            print(getVar)
            print(sysVar)

        elif check == "_search":
            find = input(">>> ")
            if find == "aDrive":
                print(aDrive)

            elif find == "AOs.set":
                print(AOsSet)

            elif find == "Patch.set":
                print(PatchSet)

            elif find == "Users":
                print(Users)

            elif find == "xFiles":
                print(xFiles)

            elif find == "xFiles.72":
                print(xFiles72)

            elif find == "xBookA13Pro":
                print(xBookA13Pro)

            elif find == "Desktop":
                print(Desktop)

            elif find == "Documents":
                print(Documents)

            elif find == "Downloads":
                print(Downloads)

            elif find == "Pictures":
                print(Pictures)

            elif find == "Links":
                print(Links)

            elif check == "Music":
                print(Music)

            elif check == "Videos":
                print(Videos)

            elif check == "Search":
                print(Search)

            else:
                print("Search not found!")

        elif check == "_scan":
            os.system('''echo The %date% at %time% Scanner performed a task to scan this PC. >>Temp.txt''')
            os.system('''echo Task = _scan >>Temp/scan.tmp''')
            os.system('''echo Process = This PC was scanned at %date% %time% >>Temp/scan.tmp''')
            Scan()

        elif check == "_repair" or check == "_fixerror":
            os.system('''echo Task = _repair >>Temp/amd35.tmp''')
            os.system('''echo Process = This PC was repaired at %date% %time% >>Temp/amd35.tmp''')
            Repair()

        elif check == "_color":
            os.system('''echo The %date% at %time% UI Theme was changed. >>Temp.txt''')
            os.system('''echo Task = _color >>Temp/uitheme.tmp''')
            os.system('''echo Process = UI for This PC was changed at %date% %time% >>Temp/uitheme.tmp''')
            Colors()

        elif check == "_help":
            os.system('''echo Task = _help >>Temp/helporinfo.tmp''')
            os.system('''echo Process = User recalled all the AOs functions on a list. >>Temp/helporinfo.tmp''')
            Help()

        elif check == "_update":
            os.system('''echo The %date% at %time% PC was checked for updates/updated. >>Temp.txt''')
            os.system('''echo Task = _update >>Temp/updateft.tmp''')
            os.system('''echo Process = This PC was updated at %date% %time%. >>Temp/updateft.tmp''')
            Update()

        elif check == "_maths":
            os.system('''echo The %date% at %time% Calculations were performed. >>Temp.txt''')
            os.system('''echo Task = _maths >>Temp/calculator.tmp''')
            os.system('''echo Process = Calculations were performed at %date% %time%. >>Temp/calculator.tmp''')
            Maths()

        elif check == "_timer":
            os.system('''echo The %date% at %time% User started StopWatch. >>Temp.txt''')
            os.system('''echo Task = _timer >>Temp/stopwatchortimer.tmp''')
            os.system('''echo Process = User started StopWatch at %date% %time%. >>Temp/stopwatchortimer.tmp''')
            StopWatch()

        elif check == "_calander":
            os.system('''echo The %date% at %time% User checked for current Time and Date. >>Temp.txt''')
            os.system('''echo Task = _calander >>Temp/calander.tmp''')
            os.system('''echo Process = User checked for current Time and Date at %date% %time%. >>Temp/calander.tmp''')
            Calander()

        elif check == "_refresh":
            os.system('tree')

        elif check == "_corner":
            os.system('''echo The %date% at %time% Online searching was performed by the User. >>Temp.txt''')
            os.system('''echo Task = _corner >>Temp/networkcorner.tmp''')
            os.system('''echo Process = Online searching was performed by the User at %date% %time%. >>Temp/networkcorner.tmp''')
            Coner()

        elif check == "_lock":
            os.system('''echo The %date% at %time% PC was locked due to security purpose. >>Temp.txt''')
            os.system('''echo Task = _lock >>Temp/passlockkey.tmp''')
            os.system('''echo Process = PC was locked due to security purpose at %date% %time%. >>Temp/passlockkey.tmp''')
            lockPc()

        elif check == "_builder":
            os.system('''echo The %date% at %time% User took notes. >>Temp.txt''')
            os.system('''echo Task = _builder >>Temp/txtpad.tmp''')
            os.system('''echo Process = User took notes at %date% %time%. >>Temp/txtpad.tmp''')
            Builder()

        elif check == "_play":
            os.system('''echo The %date% at %time% User played games. >>Temp.txt''')
            os.system('''echo Task = _play >>Temp/gameplaybase.tmp''')
            os.system('''echo Process = User played games at %date% %time%. >>Temp/gameplaybase.tmp''')
            Game()

        elif check == "_terminal":
            os.system('''echo The %date% at %time% Commands were used to perform certain tasks. >>Temp.txt''')
            os.system('''echo Task = _terminal >>Temp/console.tmp''')
            os.system('''echo Process = Commands were used to perform certain tasks at %date% %time%. >>Temp/console.tmp''')
            Terminal()

        elif check == "_optimize":
            os.system('''echo Task = _optimize >>Temp/amdx32.tmp''')
            os.system('''echo Process = This PC was optimized at %date% %time%. >>Temp/amdx32.tmp''')
            Optimize()

        elif check == "_organize":
            os.system('''echo Task = _organize >>Temp/amdx72.tmp''')
            os.system('''echo Process = System files were organized automatically at %date% %time%. >>Temp/amdx72.tmp''')
            Organize()

        elif check == "_ping":
            os.system('''echo The %date% at %time% User checked Internet speed. >>Temp.txt''')
            os.system('''echo Task = _ping >>Temp/networkinternetcheck.tmp''')
            os.system('''echo Process = User checked Internet speed at %date% %time%. >>Temp/networkinternetcheck.tmp''')
            InternetSpeedTest()

        elif check == "_read":
            os.system('''echo Task = _read >>Temp/deltmpread.tmp''')
            os.system('''echo Process = Temporary files were deleted at %date% %time%. >>Temp/deltmpread.tmp''')
            Read()

        elif check == "_run":
            os.system('''echo Task = _run >>Temp/deltastart.tmp''')
            os.system('''echo Process = User ran a program at %date% %time%. >>Temp/deltastart.tmp''')
            Run()

        elif check == "_reset":
            Reset()

        elif check == "_restore":
            Restore()

        elif check == "_terminate":
            Terminate()

        elif check == "_Process":
            Process()

        elif check == "_random":
            os.system('''echo The %date% at %time% User talked with Random(Chatbot). >>Temp.txt''')
            os.system('''echo Task = _random >>Temp/aipacbrandom.tmp''')
            os.system('''echo Process = User talked with Random(Chatbot) at %date% %time%. >>Temp/aipacbrandom.tmp''')
            Chatbot()

        elif check == "_AOs1000":
            print("Congratulations for hitting 1000 lines of code in AOs.")
            print("It is your first program to hit 1000 lines of code.")

        elif check == "_credits":
            os.system('cls')
            print(creditSet)
            input("Continue.")
            os.system('cls')
            AOs()

        elif check == "_admin":
            Sysadmin()

        elif check == "_pixstore":
            PixShop()

        elif check == "_sound":
            PlaySong()

        elif check == "_shutdown" or check == "_quit":
            Timer()
            print("Shutting Down...")
            input("Done!")
            sys.exit()

        elif check == "_restart":
            Restart()

        else:
            print("Command do not exist!")

def Restart():
    BootLog()
    BIOS()
    Timer()
    print("Restarting....")
    print("Done!")
    input()
    AOs()

def Scan():
    Timer()
    print("Scanning your pc...")
    print("Done scanning!")
    time.sleep(1)
    randomNumber = random.randint(0, 1)

    if randomNumber == 1:
        print("Your pc is working fine.")
        input("Continue...")
        AOs()

    else:
        print("Your pc ran into problem!")
        Repair()

def Repair():
    input("Press Enter to repair your pc.")
    print("Repairing your pc...")
    Timer()
    input('''Done! Your pc has been repaired.
It needs to restart.''')
    Restart()

def Colors():
    print("There are currently two types of color format.")
    print('''0 = Default
1 = Black - White
2 = Red - White
3 = White - Red
4 = Green - White
5 = Blue - White''')
    colorChanger = input(">>> ")

    if colorChanger == "0":
        os.system('color 07')

    elif colorChanger == "1":
        os.system('color f0')

    elif colorChanger == "2":
        os.system('color f4')

    elif colorChanger == "3":
        os.system('color 4f')

    elif colorChanger == "4":
        os.system('color fa')

    elif colorChanger == "5":
        os.system('color f9')

    else:
        input("Theme not found!")

    input("Continue.")

def Help():
    print(helpSet)
    input("Continue.")

def Timer():
    sec = 0
    while sec < 100:
        sec = sec + 1
        print(sec)
        os.system('cls')
        if sec == 100:
            pass

def StartUp():
    print("Booting...")
    print("Done!")
    input()

def Update():
    updates = random.randint(0, 7)
    print("Checking for updates...")
    time.sleep(3)

    if updates == 0:
        print("Updates are available.")
        input("Update.")
        print("Please wait...")
        time.sleep(5)
        locate = os.path.isdir("UpdatePackages/AOs.old")
        if locate == True:
            UPR()

        else:
            os.chdir("UpdatePackages")
            os.system('mkdir AOs.old')
            os.chdir("..")
            UPR()

    else:
        input("No updates are available.")
        AOs()

def UPR():
    os.system('robocopy "." "UpdatePackages/AOs.old" /XD "UpdatePackages"  /E /S')
    os.system('cls')
    print("AOS_1.3_UPDATE_PACKAGE")
    print("[]")
    print("Getting things ready...")
    time.sleep(1)
    os.system('echo This got updated at %date% %time%>>UpdatePackages/Data/Update.log')
    os.system('echo This got updated at %date% %time%>>UpdatePackages/UPR.sys')
    os.system('echo This got updated at %date% %time%>>UpdatePackages/SOUND.sys')
    for lLfjs in tqdm(range(1000)):
        time.sleep(0.01)
    time.sleep(3)
    print("[]")
    print("Copying...")
    time.sleep(3)
    os.system('call UpdatePackages/UPR.bat')
    time.sleep(5)
    print("[]")
    print("Downloading...")
    time.sleep(3)
    print("AOS_1.3_UPDATE_PACKAGE_DOWNLOAD_EXECUTION")
    for late in tqdm(range(1357)):
        time.sleep(0.01)
    time.sleep(3)
    print("TOTAL_1357_FILES_DOWNLOADED")
    time.sleep(5)
    print("[]")
    print("Verifying...")
    time.sleep(1)
    print("AOS_1.3_UPDATE_PACKAGE_VERIFICATION_EXECUTION")
    for KIME in tqdm(range(1357)):
        time.sleep(0.01)
    time.sleep(3)
    print("TOTAL_1357_FILES_VERIFIED")
    time.sleep(5)
    print("[]")
    print("Extracting...")
    time.sleep(1)
    print("AOS_1.3_UPDATE_PACKAGE_EXTRACTION_EXECUTION")
    for KIME in tqdm(range(1357)):
        time.sleep(0.01)
    time.sleep(3)
    print("TOTAL_3793_FILES_EXTRACTED.")
    time.sleep(5)
    print("[]")
    print("Installing...")
    time.sleep(5)
    print("AOS_1.3_UPDATE_PACKAGE_INSTALLATION_EXECUTION")
    for land in tqdm(range(3793)):
        time.sleep(0.01)
    time.sleep(9)
    print("[]")
    print("Updating...")
    time.sleep(7)
    print("AOS_1.3_UPDATE_PACKAGE_EXECUTION")
    for lastofus2 in tqdm(range(100)):
        time.sleep(0.1)
    time.sleep(7)
    os.system('echo Bug fixes>>UpdatePackages/Change.log')
    print("[]")
    print("Restart to Update.")
    input()
    os.system('call UPR.exe')
    time.sleep(1)
    os.system('del UPR.exe')
    os.system('Start AOs.py')
    sys.exit()

def Maths():
    print('''1 = Add
2 = Subtract
3 = Multiply
4 = Divide''')

    math = input(">>> ")
    
    if math == "1":
        addA = int(input("Number1 = "))
        addB = int(input("Number2 = "))

        addAns = addA + addB
        print(addAns)

    elif math == "2":
        subA = int(input("Number1 = "))
        subB = int(input("Number2 = "))

        subAns = subA - subB
        print(subAns)

    elif math == "3":
        multA = int(input("Number1 = "))
        multB = int(input("Number2 = "))

        multAns = multA * multB
        print(multAns)

    elif math == "4":
        dvdA = int(input("Number1 = "))
        dvdB = int(input("Number2 = "))

        dvdAns = dvdA / dvdB
        print(dvdAns)

    else:
        input("Invalid number!")

def StopWatch():
    input("Set time.")
    defaultTime = 0
    sec = int(input("Seconds = "))
    while defaultTime < sec:
        defaultTime = defaultTime + 1
        time.sleep(1)
        os.system('cls')
        print(defaultTime)
        if defaultTime == sec:
            input('''Times up...
Continue.''')
            AOs()

def Calander():
    os.system('time /t')
    os.system('date /t')

def InternetSpeedTest():
    os.system('ping www.google.com')
    os.system('ping www.yahoo.com')
    os.system('ping www.bing.com')
    os.system('ping www.youtube.com')
    os.system('ping www.wikipedia.org')

def Coner():
    coner = input(">>> ")

    if coner == "google":
        print("Opening Google...")
        os.system('start www.google.com')

    elif coner == "youtube":
        print("Opening Youtube...")
        os.system('start www.youtube.com')

    elif coner == "wikipedia":
        print("Opening Wikipedia...")
        os.system('start www.wikipedia.org')

    elif coner == "gmail":
        print("Opening Gmail...")
        os.system('start www.gmail.com')

    elif coner == "yahoo":
        print("Opening Yahoo...")
        os.system('start www.yahoo.com')

    elif coner == "bing":
        print("Opening Bing...")
        os.system('start www.bing.com')

    elif coner == "facebook":
        print("Opening Facebook...")
        os.system('start www.facebook.com')

    elif coner == "scratch":
        print("Opening Scratch...")
        os.system('start scratch.mit.edu')

    elif coner == "twitter":
        print("Opening Twitter...")
        os.system('start twitter.com')

    elif coner == "instagram":
        print("Opening Instagram...")
        os.system('start www.instagram.com')

    elif coner == "twitch":
        print("Opening Twitch...")
        os.system('start www.twitch.tv')

    elif coner == "encyclopedia britannica":
        print("Opening Encyclopedia Britannica...")
        os.system('start www.britannica.com')

    elif coner == "amazon":
        print("Opening Amazon...")
        os.system('start www.amazon.in')

    elif coner == "flipkart":
        print("Opening Flipkart...")
        os.system('start www.flipkart.com')

    elif coner == "pexels":
        print("Opening Pexels...")
        os.system('start www.pexels.com')

    elif coner == "wix":
        print("Opening Wix...")
        os.system('start www.wix.com')

    elif coner == "wix logo maker":
        print("Opening Wix Logo Maker...")
        os.system('start www.wix.com/logo/maker')

    elif coner == "unity":
        print("Opening Unity...")
        os.system('start unity.com')

    elif coner == "unreal engine":
        print("Opening Unreal engine...")
        os.system('start www.unrealengine.com')

    elif coner == "godot":
        print("Opening Godot")
        os.system('start godotengine.org')

    elif coner == "gamemaker studio":
        print("Opening Gamemaker studio...")
        os.system('start www.yoyogame.com')

    elif coner == "python":
        print("Opening Python...")
        os.system('start www.python.org')

    elif coner == "java":
        print("Opening Java...")
        os.system('start www.java.com')

    elif coner == "dotnet":
        print("Opening Dotnet...")
        os.system('start dotnet.microsoft.com')

    elif coner == "microsoft":
        print("Opening Microsoft...")
        os.system('start www.microsoft.com/en-in')

    elif coner == "apple":
        print("Opening Apple...")
        os.system('start www.apple.com/in')

    elif 'google doodle' in coner:
        print("Opening Google Doodle...")
        os.system('start www.google.com/doodles')

    elif 'google search ' in coner:
        print("Searching...")
        instant = coner.replace("google search ", "")
        os.system('start www.google.com/search?q=' + instant)

    elif 'youtube search ' in coner:
        print("Searching...")
        instant = coner.replace("youtube search ", "")
        os.system('start www.youtube.com/results?search_query=' + instant)

    elif 'britanica search ' in coner:
        print("Searching...")
        instant = coner.replace("britanica search ", "")
        os.system('start www.britannica.com/search?query=' + instant)

    else:
        print("Unknown search!")

def lockPc():
    os.system('cls')
    print("Enter a password here.")
    pinLock = input(">>> ")
    os.system('cls')
    askPass(pinLock)
    
def askPass(text):
    print("Enter your password here.")
    askPin = input(">>> ")

    if askPin == text:
        AOs()

    else:
        print("Wrong password!")
        input("Continue.")
        os.system('cls')
        askPass(text)

def Builder():
    print("__________ BUILDER __________")
    print("PLease provide extention name.")
    print("PLease don't use white spaces.")
    name = input('''Please enter your file name.
> ''')
    said = True
    while said:
        txt = input()
        if txt == "AOS_ADMIN_QUIT-BUILDER(TRUE, 1);":
            input(name + " just got saved!")
            said = False

        else:
            os.system('echo ' + txt + ">>log/" + name)

#     print('''__________ Buillder __________
# 0          Build01
# 1          Build02
# 2          Build03
# ''')

#     buildData = input(">>> ")

#     if buildData == "0":
#         os.system('cls')
#         global build1
#         global build_A
#         global build_B
#         global build_C
#         global build_D
#         global build_E
#         global build_F
#         global build_G
#         global build_H
#         global build_I

#         build1 = input()
#         build_A = input()
#         build_B = input()
#         build_C = input()
#         build_D = input()
#         build_E = input()
#         build_F = input()
#         build_G = input()
#         build_H = input()
#         build_I = input()

#         # if build1 == "/close" or build_A == "/close" or build_B == "/close" or build_C == "/close" or build_D == "/close" or build_E == "/close" or build_F == "/close" or build_G == "/close" or build_H == "/close" or build_I == "/close":
#         #     os.system('cls')
#         #     print("Saving Progress...")
#         #     time.sleep(1)
#         #     os.system('cls')
#         #     AOs()

#     elif buildData == "1":
#         os.system('cls')
#         global build2
#         global build_A1
#         global build_B1
#         global build_C1
#         global build_D1
#         global build_E1
#         global build_F1
#         global build_G1
#         global build_H1
#         global build_I1

#         build2 = input()
#         build_A1 = input()
#         build_B1 = input()
#         build_C1 = input()
#         build_D1 = input()
#         build_E1 = input()
#         build_F1 = input()
#         build_G1 = input()
#         build_H1 = input()
#         build_I1 = input()

#     elif buildData == "2":
#         os.system('cls')
#         global build3
#         global build_A2
#         global build_B2
#         global build_C2
#         global build_D2
#         global build_E2
#         global build_F2
#         global build_G2
#         global build_H2
#         global build_I2

#         build3 = input()
#         build_A2 = input()
#         build_B2 = input()
#         build_C2 = input()
#         build_D2 = input()
#         build_E2 = input()
#         build_F2 = input()
#         build_G2 = input()
#         build_H2 = input()
#         build_I2 = input()

#     if buildData == "/read_build01":
#         print(build1 + "\n" + build_A + "\n" + build_B + "\n" + build_C + "\n" + build_D + "\n" + build_E + "\n" + build_F + "\n" + build_G + "\n" + build_H + "\n" + build_I + "\n")

#     elif buildData == "/read_build02":
#         print(build2 + "\n" + build_A1 + "\n" + build_B1 + "\n" + build_C1 + "\n" + build_D1 + build_E1 + build_F1 + build_G1 + build_H1 + build_I1 + "\n")

#     elif buildData == "/read_build03":
#         print(build3 + "\n" + build_A2 + "\n" + build_B2 + "\n" + build_C2 + "\n" + build_D2 + build_E2 + build_F2 + build_G2 + build_H2 + build_I2 + "\n")

#     elif buildData == "/edit_build01":
#         print("Note: Edit changes will not save.")
#         input(build1 + "\n" + build_A + "\n" + build_B + "\n" + build_C + "\n" + build_D + "\n" + build_E + "\n" + build_F + "\n" + build_G + "\n" + build_H + "\n" + build_I + "\n")

#     elif buildData == "/edit_build02":
#         print("Note: Edit changes will not save.")
#         input(build2 + "\n" + build_A1 + "\n" + build_B1 + "\n" + build_C1 + "\n" + build_D1 + build_E1 + build_F1 + build_G1 + build_H1 + build_I1 + "\n")

#     elif buildData == "/edit_build03":
#         print("Note: Edit changes will not save.")
#         input(build3 + "\n" + build_A2 + "\n" + build_B2 + "\n" + build_C2 + "\n" + build_D2 + build_E2 + build_F2 + build_G2 + build_H2 + build_I2 + "\n")

def Game():
    number = random.randint(0, 10)
    print("Guess the number!")
    ans = input(">>> ")

    if ans == number:
        print("Good Job!")
        input()
        pass

    else:
        print("Try again!")
        input()
        Game()

def Terminal():
    os.system('call cmd.exe')

def Optimize():
    print("Optimizing...")
    time.sleep(5)
    input('''Your pc is optimized.
''')
    os.system('tree')
    os.system('cls')
    input()
    AOs()

def Organize():
    print("Organizing...")
    time.sleep(5)
    input('''System files and folders are organized
''')
    os.system('tree')
    os.system('cls')
    input()
    AOs()

def Read():
    os.system('call Packages/data/oza/tmp.cmd')

def Run():
    randomBuild = random.randint(0, 1000)
    print("Please insert a path for your location.")
    codePath = input("#")

    if codePath == "Desktop" or codePath == "desktop":
        os.startfile('C:\\Users\\my pc\\Desktop')

    elif codePath == "Documents" or codePath == "documents":
        os.startfile('C:\\Users\\my pc\\Documents')

    elif codePath == "Music" or codePath == "music":
        os.startfile('C:\\Users\\my pc\\Music')

    elif codePath == "Pictures" or codePath == "pictures":
        os.startfile('C:\\Users\\my pc\\Pictures')

    elif codePath == "Videos" or codePath == "videos":
        os.startfile('C:\\Users\\my pc\\Videos')

    elif codePath == "cDrive" or codePath == "aDrive":
        os.startfile("C:\\")

    elif codePath == "Users" or codePath == "uesrs":
        os.startfile('C:\\Users')

    elif codePath == "Custom" or codePath == "custom":
        getPath = input(">>> ")
        os.startfile(getPath)

    else:
        input('''Cannot find your desired directory =(
''')

def BIOS():
    os.system('title AOs')
    os.system('cls')
    biosFunc = random.randint(0, 7)

    if biosFunc == 0:
        os.system('title BIOS')
        os.system('color 17')
        print("System cannot start due to driver faliur.")
        input("Continue.")
        sysControl()

    else:
        os.system('call Packages/data/oza/Az.cmd')
        pass

def sysControl():
    os.system('cls')
    print('''0 = Restart
1 = Reset
2 = Restore
3 = Terminate
''')
    sysType = input()

    if sysType == "0":
        os.system('color 07')
        Restart()

    elif sysType == "1":
        os.system('color 07')
        Reset()

    elif sysType == "2":
        os.system('color 07')
        Restore()

    elif sysType == "3":
        os.system('color 07')
        Terminate()

    else:
        sysControl()

def Reset():
    os.system('cls')
    print("Are you sure to reset?")
    print("It will reinstall AOs in your system. Your files will be not affected.")
    print("Y to allow deny N to deny.")

    Verificatiion = input()

    if Verificatiion == "Y" or Verificatiion == "y":
        Timer()
        print("Reseted.")
        print("Done!")
        input()
        time.sleep(1)
        Restart()

    elif Verificatiion == "N" or Verificatiion == "n":
        sysControl()

    else:
        print("Data Type is valid.")
        input()
        sysControl()

def Restore():
    os.system('cls')
    print("Restoring...")
    time.sleep(5)
    Timer()
    print("Restored.")
    print("Done!")
    input()
    print("Restart needed. Your pc will restart in 5 Seconds!")
    time.sleep(5)
    Restart()

def Terminate():
    os.system('cls')
    print("Terminating...")
    time.sleep(5)
    input()
    sys.exit()

def Process():
    os.system('ping www.google.com')
    os.system('systeminfo')
    os.system('ver')
    print("Processing...")
    time.sleep(5)
    input('''Processed.
Done!
''')

def Chatbot():
    print("Please use lowercase letters.")
    while True:
        get = input("You: ")

        if 'hello' in get or 'hi' in get or 'hey' in get:
            print("Random: Hi! How are you?")

        elif 'how are you' in get:
            print("Random: I'm fine! Thanks for asking =)")

        elif 'am fine' in get:
            print("Random: Sounds good.")

        elif 'am i fine' in get:
            print("Random: Ummm... I can't tell your fellings.")

        elif get == "" or get == " " or '  ' in get:
            print("Random: Please say something.")

        elif 'who are you' in get:
            print("Random: My name is Random. I'm an artificial intelligence.")

        elif 'you fine' in get:
            print("Random: Yes, i'm fine.")

        elif 'do you feel' in get:
            print("Random: I can't feel.")

        elif 'made you' in get:
            print("Random: Srijan Srivastava made me.")

        elif 'is this place' in get:
            print("Random: This is AOs. An terminal based Operating System")

        elif 'you know' in get:
            print("Random: What?")

        elif 'send' in get:
            print("Random: Nope.")

        elif 'open google' in get:
            os.system('start www.google.com')

        elif 'open youtube' in get:
            os.system('start www.youtube.com')

        elif 'open wikipedia' in get:
            os.system('start en.wikipedia.org')

        elif 'open google doodle' in get:
            os.system('start www.google.com/doodles')

        elif 'google ' in get:
            instant = get.replace("search ", "")
            os.system('start www.google.com/search?q=' + instant)

        elif 'youtube search ' in get:
            instant = get.replace("youtube search ", "")
            os.system('start www.youtube.com/results?search_query=' + instant)

        elif 'britanica search ' in get:
            instant = get.replace("britanica search ", "")
            os.system('start www.britannica.com/search?query=' + instant)

        elif 'bye' in get:
            print("Random: Bye!")
            time.sleep(3)
            os.system('cls')
            AOs()

        else:
            print("Random: Sorry, I can't understand you =(")

def Sysadmin():
    os.system('call adm.exe')
    os.system('title AOs')
    os.system('cls')

def PixShop():
    print("__________ PIXSTORE __________")
    print("PLease don't use white spaces.")
    print("Please don't use Lowercase keywords.")
    pix = True
    while pix:
        store = input("> ")
        if 'PIXSTORE_INSTALL ' in store:
            node = store.replace("PIXSTORE_INSTALL ", "")
            print("PIXSTORE_INSTALL " + node + "(TRUE, 1);")
            time.sleep(1)
            print("Downloading " + node +"...")
            time.sleep(7)
            print("Installing " + node +"...")
            time.sleep(5)
            print(node + " got Installed.")
            os.system('echo ' + node + ' got Installed at %date% %time%.>>Packages/appdata/PIXSTORE_APPDATA_' + node + '.log')
            time.sleep(3)
            input()

        elif store == "PIXSTORE_QUIT(TRUE, 1);":
            pix = False

        elif 'PIXSTORE_UNINSTALL ' in store:
            node = store.replace("PIXSTORE_UNINSTALL ", "")
            print("Please wait...")
            time.sleep(1)
            take = os.path.isfile('Packages/appdata/PIXSTORE_APPDATA_' + node + '.log')
            if take == True:
                print("Uninstalling " + node + "...")
                time.sleep(5)
                os.chdir('Packages/appdata')
                os.system('del PIXSTORE_APPDATA_' + node + '.log')
                print(node + " got uninstalled.")
                os.chdir('..')
                os.chdir('..')
                os.system('dir')
                time.sleep(3)
                input("Done.")

            else:
                print("PIXSTORE_ERROR-0x015")

        else:
            print("PIXSTORE_ERROR-0x016")
            pix = False

def BootLog():
    os.system('title AOs')
    os.system('call Packages/data/oza/server.bat')
    os.system('echo AOs booted at %date% %time%. >>log/set/BOOT.log')
    os.system('call Packages/data/oza/Config.cmd')
    os.system('color 07')

def PlaySong():
    print("Please enter the file location and it's name.")
    print("Please don't use Quotation Mark(“)")
    audio = input("> ")
    base = os.path.isfile(audio)
    if base == True:
        pygame.init()
        os.system('cls')
        print("Loading sound...")
        time.sleep(5)
        os.system('cls')
        pygame.mixer.music.load(audio)
        pygame.mixer.music.play()
        os.system('cls')

    else:
        input("Audio file does not exist.")

if __name__ == '__main__':
    BootLog()
    BIOS()
    Timer()
    StartUp()
    AOs()
