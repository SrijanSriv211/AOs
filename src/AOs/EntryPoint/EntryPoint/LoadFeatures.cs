partial class EntryPoint
{
    private void LoadFeatures()
    {
        this.parser.Add(
            new string[]{ "!" }, "Switch the default-else shell",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{ "cmd" }, "Switch the default-else shell to cmd.exe"},
                {new string[]{ "ps", "powershell" }, "Switch the default-else shell to powershell.exe"}
            },
            default_values: new string[]{""}, max_args_length: 1, method: this.features.SwitchElseShell
        );

        // Flagged commands
        this.parser.Add(new string[]{ "cls", "clear" }, "Clear the screen", method: this.AOs.ClearConsole);
        this.parser.Add(new string[]{ "version", "ver", "-v" }, "Displays the AOs version", method: features.PrintVersion);
        this.parser.Add(new string[]{ "about", "info" }, "About AOs", method: features.About);
        this.parser.Add(new string[]{ "shutdown" }, "Shutdown the host machine", method: this.features.Shutdown);
        this.parser.Add(new string[]{ "restart" }, "Restart the host machine", method: this.features.Restart);
        this.parser.Add(new string[]{ "quit", "exit" }, "Exit AOs", method: this.features.Exit);
        this.parser.Add(new string[]{ "admin" }, "Launch an AOs instance as administrator", method: this.features.Admin);
        this.parser.Add(new string[]{ "reload", "refresh" }, "Restart AOs", method: this.features.Refresh);
        this.parser.Add(new string[]{ "credits" }, "Credit for AOs", method: Obsidian.Credits);
        this.parser.Add(new string[]{ "time", "clock" }, "Display current time", method: this.features.GetTime);
        this.parser.Add(new string[]{ "date", "calendar" }, "Display today's date", method: this.features.GetDate);
        this.parser.Add(new string[]{ "datetime" }, "Display today's time and date", method: this.features.GetDateTime);
        this.parser.Add(new string[]{ "ran", "random", "generate" }, "Generate a random number between 0 and 1", method: this.features.GenRandomNum);
        this.parser.Add(new string[]{ "sysinfo", "systeminfo", "osinfo" }, "Display operating system configuration information", method: this.features.SysInfo);
        this.parser.Add(new string[]{ "tree" }, "Graphically display the directory structure of a drive or path", method: this.features.Tree);
        this.parser.Add(new string[]{ "diagxt" }, "Display AOs specific properties and configuration", method: this.features.Diagxt);
        this.parser.Add(new string[]{ "scan" }, "Scan the integrity of all protected system files", method: this.features.Scan);
        this.parser.Add(new string[]{ "update" }, "Check for AOs updates", method: this.features.CheckForAOsUpdates);

        // Unflagged commands
        this.parser.Add(new string[]{ "shout", "echo" }, "Display messages", is_flag: false, method: this.features.Shout);
        this.parser.Add(
            new string[]{ "history" }, "Display the history of Commands",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{ "-c", "--clear" }, "Clear the history"},
            },
            default_values: new string[]{""}, max_args_length: 1, method: this.features.GetSetHistory
        );
        this.parser.Add(new string[]{ ">", "console", "terminal", "cmd", "call" }, "Call a specified program or command, given the full or sysenv path in terminal", default_values: new string[0], method: this.features.RunInTerminal);
        this.parser.Add(new string[]{ "open", "run", "start" }, "Start a specified program or command, given the full or sysenv path", default_values: new string[0], method: this.features.RunApp);
        this.parser.Add(new string[]{ "title" }, "Change the title for AOs window", default_values: new string[0]{}, method: this.features.ChangeTitle);
        this.parser.Add(new string[]{ "color" }, "Change the default AOs foreground colors", default_values: new string[]{""}, max_args_length: 1, method: this.features.ChangeColor);
        this.parser.Add(new string[]{ "wait", "timeout" }, "Suspend processing of a command for the given number of seconds", default_values: new string[]{""}, max_args_length: 1, method: this.features.Wait);
        this.parser.Add(
            new string[]{ "pause" }, "Suspend processing of a command and display the message",
            default_values: new string[]{"Press any key to continue..."}, method: this.features.Pause
        );
        this.parser.Add(new string[]{ "cat", "allinstapps", "installedapps", "allinstalledapps" }, "Start an installed program from the system", default_values: new string[0]{}, method: this.features.Cat);
        this.parser.Add(
            new string[]{ "prompt" }, "Change the command prompt",
            supported_args: new Dictionary<string[], string>
            {
                {new string[] {"-h", "--help"}, "Display all supported arguments"},
                {new string[] {"-r", "--reset", "--restore", "--default"}, "$ (dollar sign, reset the prompt)"},
                {new string[] {"-u", "--username"}, "%username%"},
                {new string[] {"-s", "--space"}, "(space)"},
                {new string[] {"-b", "--backspace"}, "(backspace)"},
                {new string[] {"-v", "--version"}, "Current AOs version"},
                {new string[] {"-t", "--time"}, "Current time"},
                {new string[] {"-d", "--date"}, "Current date"},
                {new string[] {"-p", "--path"}, "Current path"},
                {new string[] {"-n", "--drive"}, "Current drive"}
            },
            default_values: new string[0]{}, method: this.features.ModifyPrompt
        );
        this.parser.Add(new string[]{ "ls", "dir" }, "Displays a list of files and subdirectories in a directory", default_values: new string[0]{}, method: this.features.LS);
        this.parser.Add(new string[]{ "cd" }, "Change the current directory", default_values: new string[]{""}, max_args_length: 1, method: this.features.ChangeCurrentDir);
        this.parser.Add(new string[]{ "cd.." }, "Change to previous directory", method: this.features.ChangeToPrevDir);
        this.parser.Add(new string[]{ "touch", "create" }, "Create a one or more files or folders", min_args_length: 1, method: this.features.Touch);
        this.parser.Add(new string[]{ "del", "delete", "rm", "rmdir" }, "Delete one or more files or folders", min_args_length: 1, method: this.features.Delete);
        this.parser.Add(new string[]{ "ren", "rename", "rn" }, "Rename a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Move);
        this.parser.Add(new string[]{ "mv", "move" }, "Move a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Move);
        this.parser.Add(new string[]{ "cp", "copy" }, "Copy a file or folder", min_args_length: 2, max_args_length: 2, method: this.features.Copy);
        this.parser.Add(
            new string[]{ "pixelate", "leaf", "corner" }, "Start a website in a web browser",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{"-e", "--engine"}, "Search for a query on a specific search engine (google, bing, duckduckgo, youtube, wikipedia)"},
                {new string[]{"-w", "--weather"}, "Display today's weather in a city"},
                {new string[]{"-t", "--temp", "--temperature"}, "Display today's temperature in a city"}
            },
            min_args_length: 1, method: this.features.Pixelate
        );
        this.parser.Add(
            new string[]{ "read", "type" }, "Display the contents of a text file",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{"-l", "--line"}, "Shows information about a specific line"},
            },
            min_args_length: 1, max_args_length: 3, method: this.features.Read
        );
        this.parser.Add(
            new string[]{ "commit", "write" }, "Edit the contents of a text file",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{"-l", "--line"}, "Edit specific line in a text file"},
            },
            min_args_length: 2, method: this.features.Commit
        );
        this.parser.Add(
            new string[]{ "zip", "rar", "winrar" }, "Compress or Decompress files or folders",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{"-u", "--uncompress", "--decompress"}, "Decompress zip files"},
            },
            min_args_length: 1, method: this.features.WinRAR
        );
        this.parser.Add(new string[]{ "terminate", "taskkill", "tasklist", "kill", "close" }, "Terminate a specific running process", default_values: new string[0]{}, method: this.features.Terminate);
        this.parser.Add(
            new string[]{ "filer" }, "A powerful text encryption and decryption program.",
            supported_args: new Dictionary<string[], string>
            {
                {new string[]{"-h", "--help"}, "Show help message"},
                {new string[]{"-s"}, "A random seed in the range (0, 1) that acts like a password"},
                {new string[]{"-o"}, "Place the output into <file>"},
                {new string[]{"-m"}, "The maximum length of a string in each chunk"},
                {new string[]{"-t"}, "Text input from the command line"},
                {new string[]{"-f"}, "Takes a text file as an input"},
                {new string[]{"-e"}, "Encrypt the message"},
                {new string[]{"-d"}, "Decrypt the message"},
            },
            min_args_length: 1, method: this.features.Filer
        );
    }
}
