# <img title="AOs" src="img/AOs.ico" width="32" height="32"> AOs

I started working on AOs as a hobby project back in 2020. Originally the project was written in Python but I shifted to C# for this one.

The goal of AOs is simple. To better customize command-line along with some basic features like syntax highlighliting and autocomplete suggestions.

***

## :gear: Getting AOs
### :eyes: Usage
1. Download the latest version of [AOs](https://github.com/SrijanSriv211/AOs/releases) by clicking on the releases link.
2. Extract `AOs.zip` file and run `AOs.exe` file.

> [!NOTE]
> You can further extend AOs features by installing [AOs-PowerToys](https://github.com/SrijanSriv211/AOs-PowerToys) into AOs as well.

<br>

3. All the supported shortcut keys are the following:

| Shortcut              | Comment                         |
| --------------------- | --------------------------------|
| `End`                 | Send end of line                |
| `Tab`                 | Change autocomplete suggestions |
| `Home`                | Send start of line              |
| `Escape`              | Clear suggestions               |
| `Delete`              | Delete succeeding character     |
| `Backspace`           | Delete previous character       |
| `LeftArrow`           | Backward one character          |
| `RightArrow`          | Forward one character           |
| `Shift`+`Escape`      | Clear input and suggestions     |
| `Ctrl`+`Enter`        | Accept current suggestion       |
| `Ctrl`+`Spacebar`     | Show current suggestions        |
| `Ctrl`+`Delete`       | Delete succeeding token         |
| `Ctrl`+`Backspace`    | Delete previous token           |
| `Ctrl`+`LeftArrow`    | Backward one token              |
| `Ctrl`+`RightArrow`   | Forward one token               |

For more details visit [Creadf's](https://github.com/SrijanSriv211/Creadf.git) github repo.

4. To see a list of all supported commands, type `help` in the AOs window:
```console
AOs 2024 [Version 2.7]  (User)
$ help
```
<br>

- To edit the supported commands view [`settings.json`](src/Files.x72/root/settings.json)

```jsonc
{
    // You can change it to powershell by 'ps.exe'.
    "default_else_shell": "cmd.exe",

    // If null then the default username is your system username.
    "username": null,

    // List of all AOs (.aos) scripts that will run at start of AOs.
    "startlist": [],

    // Enable/Disable syntax highlighting or autocomplete suggestions during input.
    "readline": {
        "color_coding": true,
        "auto_complete_suggestions": true
    },

    // This feature allows you to index some specific type of files from some specific folder on your machine.
    "search_index": {
        // All files contains the following extentions will be indexed.
        "extensions": [
            ".exe", ".msi", ".lnk"
        ],
        // All the files will be indexed only from the following folders.
        "search_paths": [
            "C:\\Users\\%username%\\Desktop",
            "C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs"
        ],
        // Files from the following folders will not be indexed and will be ignored.
        "excluded_items": [""]
    },

    // This section contains the schema of how commands will work in AOs.
    // All the commands that will be defined here will work as intended,
    // and will appear with all its details when the 'help' command is typed in AOs.
    // Though most of it is pretty obvious and is self-explanatory, I'll explain some parts.
    // NOTE: All the commands that will be defined here will be global and cannot be over-ridden by anything,
    // even any file/folder having the same name as the command.
    "cmd_schema": [
        {
            "cmd_names": [":", "rij"],
            "help_message": "A quick search engine for files on your machines",
            // If you don't want to give any usage information then you can set 'usage' to a null.
            "usage": null,
            // Same goes from 'supported_args'.
            "supported_args": null,
            // The following default values will be passed in the comamnd when no value was passed by the user.
            // NOTE: This list cannot be null or empty. At least it must have an empty string which will be ignore during parsing.
            "default_values": [""],
            // The flag can either be 'true' or 'false' (obviously).
            // This means as its name suggests that a command can either be flagged or non-flagged.
            // A flagged command needs no argument/parameter to be passed where as
            // a non-flagged command do need some arguments/parameters
            "is_flag": false,
            // The 'min_arg_len' & 'max_arg_len' are used to define the mininum and maximum number of arguments that a command can accept.
            // Their minimum value can only be 0 for both.
            // NOTE: When 'is_flag' = true, then this has no effects but when it is false,
            // min_arg_len = 0 & max_arg_len = 0 means that the command will accept infinitely many arguments.
            "min_arg_len": 0,
            "max_arg_len": 0,
            // The method parameter is based on location parameter.
            // Absolute/Relative path for an app or script will be accepted, as well as, a function identifier from the source code.
            "method": "Rij",
            // There are two locations in AOs.
            // 1. internal          2. external
            // internal means that the method/function that will be executed by AOs is built directly into AOs' source code.
            // enternal means that the method/function is an external app or script which can have any extention.
            "location": "internal",
            // This index option allows you to control whether you want this particular command to show up in help messages or not.
            // If true then index the command in help messages else don't.
            "do_index": true
        },
        {
            "cmd_names": ["zip", "rar", "winrar"],
            "help_message": "Compress or Decompress files or folders",
            // To give some usage info you have to write the usage in a list of strings.
            // However, the format is completely up to you.
            "usage": [
                "zip [OPTIONS] [target filepath (STRING)] [compressed filepath (STRING)]",
                "Note: AOs follows the same string handling conventions as in C#.\n",
                "Example usage:",
                "zip photos                         # Compress the photos folder",
                "zip photos compressed              # Compress the photos folder into compressed",
                "zip compressed photos -u           # Decompress the photos folder into compressed"
            ],
            // The supported arguments for a command are defined in the following way:
            "supported_args": [
                {
                    "args": ["-u", "--uncompress", "--decompress"],
                    "help_message": "Decompress zip files (is_flag: true)"
                }
            ],
            "default_values": null,
            "is_flag": false,
            "min_arg_len": 1,
            "max_arg_len": 0,
            "method": "WinRAR",
            "location": "internal",
            "do_index": true
        },
        ...
    ]
}
```

<br>

- Currently the following are all the supported internal methods:
```
1. RunAOsAsAdmin                13. GetSetHistory                   25. Touch
2. ClearConsole                 14. RunInTerminal                   26. Delete
3. ShutdownHost                 15. RunApp                          27. Move
4. RestartHost                  16. ChangeTerminalTitle             28. Copy
5. LockHost                     17. ChangeTerminalColor             29. Pixelate
6. SleepHost                    18. ThreadSleep                     30. ReadFile
7. GetCurrentTime               19. PauseTerminal                   31. CommitFile
8. GetTodayDate                 20. Cat                             32. WinRAR
9. Diagxt                       21. ChangeTerminalPrompt            33. TerminateRunningProcess
10. Scannow                     22. LS                              34. ControlVolume
11. CheckForAOsUpdates          23. ChangeCurrentDir                35. ItsMagic
12. Shout                       24. ChangeToPrevDir                 36. SwitchApp

37. Rij
```

<br>

> [!NOTE]
> AOs follows the same string handling conventions as in C#.
>
> :heavy_check_mark: Correct way:
> ```console
> AOs 2024 [Version 2.7]  (User)
> $ "path\\to\\dir"
> $ "path/to/dir"
> ```
>
> :x: Wrong way:
> ```console
> AOs 2024 [Version 2.7]  (User)
> $ "path\to\dir"
> ```

<br>

5. List of some internal methods (in-built features) of AOs:

- Control host system volume:
This command is used to control the volume of the host operating system volume through the command-line of AOs. It gives you the ability to mute/unmute your operating system volume, as well as, it allows you to dynamically control the volume of your system by increasing or decreasing the volume by the given levels. Also it allows you to set the volume to a specific level.

```console
AOs 2024 [Version 2.7]  (User)
$ help vol
Name:
vol, volume                                                 Control the host operating system volume

Details:
vol, volume [OPTIONS]

Maximum arguments: 2
Minimum arguments: 2

Options:
-m                                                          Mute/Unmute host operating system volume
-i                                                          Increase/Decrease the volume by then given value

$ vol -m # Mute the host operating system volume
$ vol -m # Unmute the host operating system volume
$ vol -i -20 # Decrease the host operating system volume by 20 levels
$ vol # When no argument is passed, then print the current host operating system volume
80
$ vol -i 20 # Increase the host operating system volume by 20 levels
$ vol
100
$ vol 10 # Set the host operating system volume to 10
$ vol
9.999999
$ vol 100 # Set the host operating system volume to 100
$ vol
100
```

<br>

- Run multiple commands in a single line:
Save your valuable time by using semicolon ; execute multiple AOs commands in a single file. This will automatically execute those commands without you waiting for the first one to finish then execute the second manually one-by-one. AOs follows the same multi-line execution conventions as in C#.

```console
AOs 2024 [Version 2.7]  (User)
$ sfc /scannow; DISM /Online /Cleanup-image /Restorehealth
```

<br>

- Execute a command direcly from the command-line:
Now apart from executing an AOs outside of AOs from the command-line, you can also execute any AOs command directly from the command-line. He following shows you how to do that.

```console
$ AOs --help
Name:
AOs

Description:
A Developer Command-line Tool Built for Developers by a Developer.

Usage:
AOs [OPTIONS]

Options:
-h, --help: Display all supported arguments (is flag: true)
-a, --admin: Run as administrator (is flag: true)
-c, --cmd: Program passed in as string
$ AOs --cmd "shout Hello world!"
Hello world!
```

<br>

- Encrypt or Decrypt any text:
Filer is a text encryption and decryption algorithm powered by the AND random number generator. Designed to encrypt or decrypt text using a very powerful custom text encryption and decryption algorithm with the help of my Absolute Number Disorder (AND) random number generator.

> [!WARNING]
> Filer is just a hobby project and is not meant to be used for any serious text encryption

Filer's source code and binaries could be found [here](https://github.com/SrijanSriv211/Filer.git)

```console
AOs 2024 [Version 2.7]  (User)
$ help filer
Name:
filer                                                       A powerful text encryption and decryption program

Details:
filer [OPTIONS]

Maximum arguments: ∞
Minimum arguments: 0

Options:
-h, --help                                                  Show help message
-p                                                          Strong password for better security
-o                                                          Place the output into <file>
-m                                                          The maximum length of a string in each chunk (default: 4)
```

<br>

- Switch between currently opened apps:
```console
AOs 2024 [Version 2.7]  (User)
$ # This will search for all currently active app throughout your PC then switch the focus to it.
$ switch <appname>
```

and many more...

***

## :toolbox: Getting Started for Developers
AOs is officially untested on other development environments whilst I focus on a Windows build, but you can modify it to work cross-platform.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Dotnet Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Python 3.12](https://www.python.org/downloads/release/python-3122)

### :pencil: Getting Started
<ins>**1. Downloading the repository:**</ins>

Start by cloning the repository with `git clone --recursive https://github.com/SrijanSriv211/AOs`.

If the repository was cloned non-recursively previously, use `git submodule update --init --recursive` to clone the necessary submodules.

<ins>**2. Running and Compiling AOs:**</ins>

1. To run the program use:
```console
python scripts\build.py run
```

2. To compile the program don't use dotnet build, instead run:
```console
python scripts\build.py
```
