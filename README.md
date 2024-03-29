# <img title="AOs" src="https://github.com/Light-Lens/AOs/blob/master/img/AOs.ico?raw=true" width="32" height="32"> AOs

A simple tool built to control UI based tasks directly through the command-line, for example changeing system wallpaper, enabling/disabling dark mode, opening any app or website from the command-line just like you do with the UI of some application launcher such as [Ueli](https://github.com/oliverschwendener/ueli), doing basic math calculations on the command-line, and etc.

***

## :gear: Getting AOs
### :eyes: Usage
1. Download the latest version of [AOs](https://github.com/Light-Lens/AOs/releases) by clicking on the releases link.
2. Extract `AOs.zip` file and run `AOs.exe` file.
3. To see a list of all supported commands, type `help` in the AOs window:
```console
AOs 2024 [Version 2.6]  (User)
$ help
```

4. To use the backslash character inside double or single quotes in AOs, use double backslashes or single forward slash:

:heavy_check_mark: Correct way:
```console
AOs 2024 [Version 2.6]  (User)
$ "path\\to\\dir"
$ "path/to/dir"
```

:x: Wrong way:
```console
AOs 2024 [Version 2.6]  (User)
$ "path\to\dir"
```

AOs follows the same string handling conventions as in C#.

***

## :toolbox: Getting Started for Developers
AOs is officially untested on other development environments whilst we focus on a Windows build, but you can modify it to work cross-platform. Visual Studio Code is recommended for development.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Dotnet Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Python 3.12](https://www.python.org/downloads/release/python-3122)
- [Visual Studio Code](https://code.visualstudio.com)

### :pencil: Getting Started
<ins>**1. Downloading the repository:**</ins>

Start by cloning the repository with `git clone --recursive https://github.com/Light-Lens/AOs`.

If the repository was cloned non-recursively previously, use `git submodule update --init --recursive` to clone the necessary submodules.

<ins>**2. Configuring the dependencies:**</ins>

1. Create a virtual environment and activate it (e.g. conda or venv):
```console
$ python -m venv .venv
$ .venv\Scripts\activate
```
2. Install python dependencies:
```console
$ pip install -r requirements.txt
```

<ins>**3. Running and Compiling AOs:**</ins>

1. Download the latest version of [AOs](https://github.com/Light-Lens/AOs#gear-getting-aos) and then run it on the cloned directory.
2. All AOs compilation tasks:
```console
AOs 2024 [Version 2.6]  (User)
$ dev
Type `help <command-name>` for more information on a specific command
1. new                                                         Create a new project
2. -t, tasks                                                   List or run custom task in the developer environment
3. clean                                                       Delete temporary/unnecessary files created in the project
4. -v, ver, version                                            Show the current build number of the project
$ dev -t
1. run                                                         Run AOs
2. srun                                                        Silent run AOs
3. compile                                                     Compile AOs
4. execute                                                     Execute AOs
```
3. To run the program use:
```console
AOs 2024 [Version 2.6]  (User)
$ dev -t run
```
4. To compile the program don't use `dotnet build`, instead run:
```console
AOs 2024 [Version 2.6]  (User)
$ dev -t compile
```
This will create an executable in the `AOs` folder with all required files to run.
5. To execute the compiled program use:
```console
AOs 2024 [Version 2.6]  (User)
$ dev -t execute
```
This is will first build AOs if it already isn't and it will execute the `AOs\AOs.exe` executable.

***

## :notebook_with_decorative_cover: The Plan
I'm building AOs because I'm working on virtual assistant called [WINTER](https://github.com/Light-Lens/WINTER) for which I want such a tool which can help me automate tasks that require GUI to be involved.

The goal of AOs is to let the user do simple things such as changing the wallpaper, switching to dark/light mode, launching an app or website like [Ueli](https://github.com/oliverschwendener/ueli) does, do some basic math calculations directly from the command-line.

The reason why I'm building AOs and WINTER both separately is because I thought AOs as an independent tool maybe useful of many people.
