# <img title="AOs" src="https://github.com/Light-Lens/AOs/blob/master/img/AOs.ico?raw=true" width="32" height="32"> AOs

A Command-line utility for improved efficiency and productivity. It is Simple and Powerful, capable of easing many tasks such as calculation, and opening any app installed on the host machine just from the command line.

> Note:
> ---
>
> Our development is ongoing and making great progress, but some features may not be fully implemented yet. If you encounter any bugs or have any suggestions, please feel free to open an _issue_ or submit a _pull request_. Your contributions are greatly appreciated!

## :gear: Getting started for people to use.
### :bangbang: Prerequisites
You need to install the following on your machine.
- [Dotnet Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### :eyes: Usage
1. Download the latest version of [AOs](https://github.com/Light-Lens/AOs/releases/) by clicking on the releases link.
2. Extract `AOs.zip` file and run `AOs.exe` file.
3. To see a list of all supported commands, type `help` in the AOs window:
```console
AOs 2023 [Version 2.3]
$ help
```

4. To use the backslash character inside double or single quotes in AOs, use double backslashes or single forward slash:

:heavy_check_mark: Correct way:
```console
AOs 2023 [Version 2.3]
$ cd "path\\to\\dir"
$ cd "path/to/dir"
```

:x: Wrong way:
```console
AOs 2023 [Version 2.3]
$ cd "path\to\dir"
```

This follows the same string handling conventions as in other programming languages.

## :toolbox: Getting Started for Developers
AOs is now officially complete and ready for use, as of now it is focused to run only on Windows, but you can modify it to work cross-platform. Visual Studio Code is recommended for development.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Mingw-w64](https://github.com/niXman/mingw-builds-binaries/releases)
- [Python 3.10](https://www.python.org/downloads/release/python-3109/)
- [Dotnet Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Visual Studio Code](https://code.visualstudio.com/)

### :pencil: Getting Started
AOs is now officially complete and ready for use, as of now it is focused to run only on Windows, but you can modify it to work cross-platform. Visual Studio Code is recommended for development.

1. Clone the repository with `git clone https://github.com/Light-Lens/AOs`.
2. Create a virtual environment and activate it (e.g. conda or venv):

```console
cd AOs
$ python -m venv .venv
$ .venv\Scripts\activate
```

3. Install python dependencies:
```console
$ pip install -r requirements.txt
```

4. Compile the program with:
Don't use `dotnet run`, instead run:
```console
$ build.bat
```

This will create an executable in the `AOs` folder with all required files to run.

## :warning: License and Contributions
All code is licensed under an MIT license. This allows you to re-use the code freely, remixed in both commercial and non-commercial projects. The only requirement is to include the same license when distributing.

We welcome any contributions to AOs development through pull requests on GitHub. Most of our active development is in the master branch, so we prefer to take pull requests there.
