# <img title="AOs" src="https://github.com/Light-Lens/AOs/blob/master/img/AOs.ico?raw=true" width="32" height="32"> AOs

A Command-line utility for improved efficiency and productivity. It is Simple and Powerful, capable of easing many tasks such as calculation, and opening any app installed on the host machine just from the command line.

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

***

## :toolbox: Getting Started for Developers
AOs is officially untested on other development environments whilst we focus on a Windows build, but you can modify it to work cross-platform. Visual Studio Code is recommended for development.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Dotnet Core 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Visual Studio Code](https://code.visualstudio.com/)

### :pencil: Getting Started
1. Clone the repository with `git clone https://github.com/Light-Lens/AOs`.
2. `cd AOs`
3. To run the program use:
```console
$ dotnet run
```
4. To compile the program don't use `dotnet build`, instead run:
```console
$ python scripts\build.py
```

This will create an executable in the `AOs` folder with all required files to run.

***

## :warning: License and Contributions
All code is licensed under an MIT license. This allows you to re-use the code freely, remixed in both commercial and non-commercial projects. The only requirement is to include the same license when distributing.

We welcome any contributions to AOs development through pull requests on GitHub. Our development is ongoing and making great progress, but some features may not be fully implemented yet. If you encounter any bugs or have any suggestions, please feel free to open an _issue_ or submit a _pull request_. Your contributions are greatly appreciated! Most of our active development is in the master branch, so we prefer to take pull requests there.
