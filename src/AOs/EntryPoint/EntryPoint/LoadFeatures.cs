partial class EntryPoint
{
    //TODO: Move all features to a json file and read that json file to make features implementation and AOs extention easier.
    private void LoadFeatures()
    {
        // Experimental commands
        this.parser.Add(
            ["@"], "Experimental/Overload commands",
            supported_args: new Dictionary<string[], string>
            {
                {["-h", "--help"], "Show information about a developer command"},
                {["itsmagic"], "It's magic, it's magic."},
                {["switch"], "Switch between applications using the App IDs"},
                {["studybyte"], "Starts Studybyte"},
                {["cpix"], "Starts Cpix"}
            },
            default_values: ["help"], is_flag: false, method: this.features.ExperimentCMD
        );

        // Default-else shell command
        this.parser.Add(
            ["!"], "Switch the default-else shell",
            supported_args: new Dictionary<string[], string>
            {
                {["cmd"], "Switch the default-else shell to cmd.exe"},
                {["ps", "powershell"], "Switch the default-else shell to powershell.exe"}
            },
            default_values: [""], max_args_length: 1, method: this.features.SwitchElseShell
        );

        // Flagged commands
        this.parser.Add(["cls", "clear"], "Clear the screen", method: this.AOs.ClearConsole);
        this.parser.Add(["version", "ver", "-v"], "Displays the AOs version", method: features.PrintVersion);
        this.parser.Add(["about", "info"], "About AOs", method: features.About);
        this.parser.Add(["shutdown"], "Shutdown the host machine", method: this.features.Shutdown);
        this.parser.Add(["restart"], "Restart the host machine", method: this.features.Restart);
        this.parser.Add(["lock", "deeplock"], "Lock the host machine", method: this.features.Lock);
        this.parser.Add(["quit", "exit"], "Exit AOs", method: this.features.Exit);
        this.parser.Add(["admin"], "Launch an AOs instance as administrator", method: this.features.Admin);
        this.parser.Add(["reload", "refresh"], "Restart AOs", method: this.features.Refresh);
        this.parser.Add(["credits"], "Credit for AOs", method: Obsidian.Credits);
        this.parser.Add(["time", "clock"], "Display current time", method: this.features.GetTime);
        this.parser.Add(["date", "calendar"], "Display today's date", method: this.features.GetDate);
        this.parser.Add(["datetime"], "Display today's time and date", method: this.features.GetDateTime);
        this.parser.Add(["ran", "random", "generate"], "Generate a random number between 0 and 1", method: this.features.GenRandomNum);
        this.parser.Add(["sysinfo", "systeminfo", "osinfo"], "Display operating system configuration information", method: this.features.SysInfo);
        this.parser.Add(["tree"], "Graphically display the directory structure of a drive or path", method: this.features.Tree);
        this.parser.Add(["diagxt"], "Display AOs specific properties and configuration", method: this.features.Diagxt);
        this.parser.Add(["scan", "deepscan"], "Scan the integrity of all protected system files", method: this.features.Scan);
        this.parser.Add(["update"], "Check for AOs updates", method: this.features.CheckForAOsUpdates);

        // Unflagged commands
        this.parser.Add(["shout", "echo"], "Display messages", is_flag: false, method: this.features.Shout);
        this.parser.Add(
            ["history"], "Display the history of Commands",
            supported_args: new Dictionary<string[], string>
            {
                {["-c", "--clear"], "Clear the history"},
            },
            default_values: [""], max_args_length: 1, method: this.features.GetSetHistory
        );
        this.parser.Add([">", "console", "terminal", "cmd", "call"], "Call a specified program or command, given the full or sysenv path in terminal", default_values: [], method: this.features.RunInTerminal);
        this.parser.Add(["open", "run", "start"], "Start a specified program or command, given the full or sysenv path", default_values: [], method: this.features.RunApp);
        this.parser.Add(["title"], "Change the title for AOs window", default_values: [], method: this.features.ChangeTitle);
        this.parser.Add(["color"], "Change the default AOs foreground colors", default_values: [""], max_args_length: 1, method: this.features.ChangeColor);
        this.parser.Add(["wait", "timeout"], "Suspend processing of a command for the given number of seconds", default_values: [""], max_args_length: 1, method: this.features.Wait);
        this.parser.Add(
            ["pause"], "Suspend processing of a command and display the message",
            default_values: ["Press any key to continue..."], method: this.features.Pause
        );
        this.parser.Add(["cat", "allinstapps", "installedapps", "allinstalledapps"], "Start an installed program from the system", default_values: [], method: this.features.Cat);
        this.parser.Add(
            ["prompt"], "Change the command prompt",
            supported_args: new Dictionary<string[], string>
            {
                {["-h", "--help"], "Display all supported arguments"},
                {["-r", "--reset", "--restore", "--default"], "$ (dollar sign, reset the prompt)"},
                {["-u", "--username"], "%username%"},
                {["-s", "--space"], "(space)"},
                {["-b", "--backspace"], "(backspace)"},
                {["-v", "--version"], "Current AOs version"},
                {["-t", "--time"], "Current time"},
                {["-d", "--date"], "Current date"},
                {["-p", "--path"], "Current path"},
                {["-n", "--drive"], "Current drive"}
            },
            default_values: [], method: this.features.ModifyPrompt
        );
        this.parser.Add(["ls", "dir"], "Displays a list of files and subdirectories in a directory", default_values: [], method: this.features.LS);
        this.parser.Add(["cd"], "Change the current directory", default_values: [""], max_args_length: 1, method: this.features.ChangeCurrentDir);
        this.parser.Add(["cd.."], "Change to previous directory", method: this.features.ChangeToPrevDir);
        this.parser.Add(["touch", "create"], "Create a one or more files or folders", min_args_length: 1, method: this.features.Touch);
        this.parser.Add(["del", "delete", "rm", "rmdir"], "Delete one or more files or folders", min_args_length: 1, method: this.features.Delete);
        this.parser.Add(["ren", "rename", "rn"], "Rename a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Move);
        this.parser.Add(["mv", "move"], "Move a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Move);
        this.parser.Add(["cp", "copy"], "Copy a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Copy);
        this.parser.Add(
            ["pixelate", "leaf", "corner"], "Start a website in a web browser",
            supported_args: new Dictionary<string[], string>
            {
                {["-e", "--engine"], "Search for a query on a specific search engine (google, bing, duckduckgo, youtube, wikipedia)"},
                {["-w", "--weather"], "Display today's weather in a city"},
                {["-t", "--temp", "--temperature"], "Display today's temperature in a city"}
            },
            min_args_length: 1, method: this.features.Pixelate
        );
        this.parser.Add(
            ["read", "type"], "Display the contents of a text file",
            supported_args: new Dictionary<string[], string>
            {
                {["-l", "--line"], "Shows information about a specific line"},
            },
            min_args_length: 1, max_args_length: 3, method: this.features.Read
        );
        this.parser.Add(
            ["commit", "write"], "Edit the contents of a text file",
            supported_args: new Dictionary<string[], string>
            {
                {["-l", "--line"], "Edit specific line in a text file"},
            },
            min_args_length: 2, method: this.features.Commit
        );
        this.parser.Add(
            ["zip", "rar", "winrar"], "Compress or Decompress files or folders",
            supported_args: new Dictionary<string[], string>
            {
                {["-u", "--uncompress", "--decompress"], "Decompress zip files"},
            },
            min_args_length: 1, method: this.features.WinRAR
        );
        this.parser.Add(["terminate", "taskkill", "tasklist", "kill", "close"], "Terminate a specific running process", default_values: [], method: this.features.Terminate);
        this.parser.Add(
            ["filer"], "A powerful text encryption and decryption program.",
            supported_args: new Dictionary<string[], string>
            {
                {["-h", "--help"], "Show help message"},
                {["-s"], "A random seed in the range (0, 1) that acts like a password"},
                {["-o"], "Place the output into <file>"},
                {["-m"], "The maximum length of a string in each chunk"},
                {["-t"], "Text input from the command line"},
                {["-f"], "Takes a text file as an input"},
                {["-e"], "Encrypt the message"},
                {["-d"], "Decrypt the message"},
            },
            min_args_length: 1, method: this.features.Filer
        );
        this.parser.Add(
            ["vol", "volume"], "Control the host operating system volume",
            supported_args: new Dictionary<string[], string>
            {
                {["-m"], "Mute/Unmute the system master volume"},
                {["-i"], "Increase/Decrease the volume by then given value"},
            },
            default_values: [], max_args_length: 2, method: this.features.ControlVolume
        );

        // Developer commands
        this.parser.Add(
            ["dev", "developer"], "Developer tools",
            supported_args: new Dictionary<string[], string>
            {
                {["-h", "--help"], "Show information about a developer command"},
                {["new"], "Create a new project"},
                {["-t", "tasks"], "List or run custom task in the developer environment"},
                {["clean"], "Delete temporary/unnecessary files created in the project"},
                {["-v", "ver", "version"], "Show the current build number of the project"},
            },
            default_values: ["help"], is_flag: false, method: this.features.DevCMD
        );
    }
}
