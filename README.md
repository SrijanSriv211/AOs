# <img title="AOs" src="https://github.com/Light-Lens/AOs/blob/master/img/AOs.ico?raw=true" width="32" height="32"> AOs

A Developer Command-line Tool Built for Developers by a Developer. It is Simple and Powerful, capable of easing tasks such as calculation or opening an app from the command line.

***

## :gear: Getting AOs
### :eyes: Usage
1. Download the latest version of [AOs](https://github.com/Light-Lens/AOs/releases/) by clicking on the releases link.
2. Extract `AOs.zip` file and run `AOs.exe` file.
3. To see a list of all supported commands, type `help` in the AOs window:
```console
AOs 2023 [Version 2.5]
$ help
```

4. To use the backslash character inside double or single quotes in AOs, use double backslashes or single forward slash:

:heavy_check_mark: Correct way:
```console
AOs 2023 [Version 2.5]
$ "path\\to\\dir"
$ "path/to/dir"
```

:x: Wrong way:
```console
AOs 2023 [Version 2.5]
$ "path\to\dir"
```

AOs follows the same string handling conventions as in any programming language.

***

## :toolbox: Getting Started for Developers
AOs is officially untested on other development environments whilst we focus on a Windows build, but you can modify it to work cross-platform. Visual Studio Code is recommended for development.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Dotnet Core 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Python 3.10](https://www.python.org/downloads/release/python-3109/)
- [Visual Studio Code](https://code.visualstudio.com/)

### :pencil: Getting Started
<ins>**1. Downloading the repository:**</ins>

Start by cloning the repository with `git clone --recursive https://github.com/Light-Lens/AOs`.

If the repository was cloned non-recursively previously, use `git submodule update --init` to clone the necessary submodules.

<ins>**2. Configuring the dependencies:**</ins>

1. After cloning the necessary submodules for AOs:
```console
cd "src\vendor\Filer\src\vendor"
git submodule update --init
```
To clone the necessary submodules for Filer (submodule for AOs).
2. Open a new Terminal instance in the root `AOs` directory.
3. Create a virtual environment and activate it (e.g. conda or venv):
```console
$ python -m venv .venv
$ .venv\Scripts\activate
```
4. Install python dependencies:
```console
$ pip install -r requirements.txt
```

<ins>**3. Running and Compiling AOs:**</ins>

1. To run the program use:
```console
$ python scripts\build.py run
```
2. To compile the program don't use `dotnet build`, instead run:
```console
$ python scripts\build.py
```
This will create an executable in the `AOs` folder with all required files to run.
3. To execute the compiled program use:
```console
$ python scripts\build.py execute
```
This is will first build AOs if it already isn't and it will execute the `AOs\AOs.exe` executable.

***

## :warning: License and Contributions
All code is licensed under an MIT license. This allows you to re-use the code freely, remixed in both commercial and non-commercial projects. The only requirement is to include the same license when distributing.

We welcome any contributions to AOs development through pull requests on GitHub. Our development is ongoing and making great progress, but some features may not be fully implemented yet. If you encounter any bugs or have any suggestions, please feel free to open an _issue_ or submit a _pull request_. Your contributions are greatly appreciated! Most of our active development is in the master branch, so we prefer to take pull requests there.
