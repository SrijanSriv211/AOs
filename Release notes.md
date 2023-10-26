# A brand new entry in the AOs 2 series with a all new command-line experience.
Today, I am excited to announce the release of AOs 2.5! A brand new entry in the AOs 2 series designed two-fold: to be more efficient and powerful, and to be a powerful Developer Tool Built for Developers by a Developer.

AOs 2.5 feature a wide range of changes all of which might be hard to discuss but can be reviewed in the **Changelog**: https://github.com/Light-Lens/AOs/compare/v2.4...v2.5

## Release Notes
1. Highlights
2. New Features
3. Improvements
4. Bug Fixes
5. Usage

## Highlights
1. Developer commands
2. Run multiple commands in a single line
3. Removal of some features from AOs 2.4

## New Features
1. Developer commands
2. Encrypt or Decrypt any text
3. Control host system volume
4. Run multiple commands in a single line
5. Execute a command direcly from the command-line

## Improvements
1. Less crashes
2. Cleaner source code
3. Improved performance
4. Faster default-else-shell execution
5. Improved file execution from the command-line
6. Improved error handling, traceback and logging
7. Independent executable that does not require .NET Framework to run

## Bug Fixes
1. Improper error logging
2. Crashing on very edgy unexpected inputs
3. Not executing when suitable .NET Framework is not installed on the host machine

## Usage
1. Developer commands:
    The all new `developer` command is designed to help developers develop their apps.

    ```console
    AOs 2023 [Version 2.5]  (User)
    $ dev help
    Type `help <command-name>` for more information on a specific command
    1. new                                                         Create a new project
    2. git, github                                                 Use git to maintain version control
    3. cloc, countlinesofcode                                      Count the lines of code in a project directory
    4. clean                                                       Delete temp/unnecessary files created by the programming language in the project
    5. ver, version                                                Show the current build number of the project
    6. server                                                      Start a local web-server
    ```

2. Encrypt or Decrypt any text:
    Now I introduce a new command, the `filer` command. Designed to encrypt or decrypt text using a very powerful custom text encryption and decryption algorithm with the help of the Absolute Number Disorder (AND) random number generator. I build Filer back in 2021, and was planned to be a part of AOs 1.7 but unfortunately it couldn't be. However, from AOs 2.5, Filer is officially integrated into the powerful environment of AOs.

    Filer could be found on [github](https://github.com/Light-Lens/Filer.git)

    ```console
    AOs 2023 [Version 2.5]  (User)
    $ help filer
    Name:
    filer                                                       A powerful text encryption and decryption program.

    Details:
    filer [OPTIONS]

    Maximum arguments: ∞
    Minimum arguments: 0

    Options:
    -h, --help                                                  Show help message
    -s                                                          A random seed in the range (0, 1) that acts like a password
    -o                                                          Place the output into <file>
    -m                                                          The maximum length of a string in each chunk
    -t                                                          Text input from the command line
    -f                                                          Takes a text file as an input
    -e                                                          Encrypt the message
    -d                                                          Decrypt the message
    ```

3. Control host system volume:
    AOs had the `volume` command as an experimental command (previously known as overload command) since `AOs 2.3.8`. However, now in AOs 2.5, the `vol` command is officially becoming a mainstream command of AOs, in other words, the `volume` command is no more an experimental command.

    This command is used to control the volume of the host operating system volume through the command-line of AOs. It gives you the ability to mute/unmute your operating system volume, as well as, it allows you to dynamically control the volume of your system by increasing or decreasing the volume by the given levels. Also it allows you to set the volume to a specific level.

    ```console
    AOs 2023 [Version 2.5]  (User)
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

4. Run multiple commands in a single line:
    Save your valuable time by using semicolon `;` execute multiple AOs commands in a single file. This will automatically execute those commands without you waiting for the first one to finish then execute the second manually one-by-one. AOs follows the same multi-line execution conventions as in C#.

    ```console
    AOs 2023 [Version 2.5]  (User)
    $ sfc /scannow; DISM /Online /Cleanup-image /Restorehealth
    ```

5. Execute a command direcly from the command-line:
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
