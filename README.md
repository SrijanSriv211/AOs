# <img title="AOs" src="res/AOs.png" width="32" height="32"> AOs
> [!IMPORTANT]
> AOs is now moved to: https://github.com/SrijanSriv211/AO<br>
> No more updated will be pushed to this repository, all new changes will be pushed to the new repository.<br>
> The repository won't be marked as archived and will stay as it is.

> I started working on AOs as a hobby project back in 2020. Originally the project was written in Python but I shifted the project to C#.
> I work on AOs as to learn a programming language or just do time pass.
> Now I'm rewriting AOs in C++ I don't know why but it sounds cool.

***

## :toolbox: Getting Started
AOs is officially untested on other development environments whilst I focus on a Windows build, but you can modify it to work cross-platform.

### :bangbang: Prerequisites
You need to install the following on your machine.
- [Mingw-w64](https://www.mingw-w64.org/downloads/#mingw-builds)
- [Python](https://www.python.org/downloads) >= 3.12

### :pencil: Getting AOs
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
