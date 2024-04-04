partial class EntryPoint
{
    // A dictionary containng all the the method names from settings.json alongwith their respective functions.
    private Dictionary<string, Delegate> GetInternalMethod()
    {
        return new Dictionary<string, Delegate>() {
            { "RunAOsAsAdmin", features.Admin },
            { "ClearConsole", AOs.ClearConsole },
            { "ShutdownHost", features.Shutdown },
            { "RestartHost", features.Restart },
            { "LockHost", features.Lock },
            { "GetCurrentTime", features.GetTime },
            { "GetTodayDate", features.GetDate },
            { "Diagxt", features.Diagxt },
            { "Scannow", features.Scan },
            { "CheckForAOsUpdates", features.CheckForAOsUpdates },
            { "Shout", features.Shout },
            { "GetSetHistory", features.GetSetHistory },
            { "RunInTerminal", features.RunInTerminal },
            { "RunApp", features.RunApp },
            { "ChangeTerminalTitle", features.ChangeTitle },
            { "ChangeTerminalColor", features.ChangeColor },
            { "ThreadSleep", features.Wait },
            { "PauseTerminal", features.Pause },
            { "Cat", features.Cat },
            { "ChangeTerminalPrompt", features.ModifyPrompt },
            { "LS", features.LS },
            { "ChangeCurrentDir", features.ChangeCurrentDir },
            { "ChangeToPrevDir", features.ChangeToPrevDir },
            { "Touch", features.Touch },
            { "Delete", features.Delete },
            { "Move", features.Move },
            { "Copy", features.Copy },
            { "Pixelate", features.Pixelate },
            { "ReadFile", features.Read },
            { "CommitFile", features.Commit },
            { "WinRAR", features.WinRAR },
            { "TerminateRunningProcess", features.Terminate },
            { "Filer", features.Filer },
            { "ControlVolume", features.ControlVolume },
            { "ItsMagic", features.ItsMagic },
            { "SwitchApp", features.SwitchApp },
        };
    }

    private void LoadFeatures()
    {
        /*
        * The following features which are not available in settings.json are immutable, meaning they can't be changed in anyway unless the code is modified.
          These features are immutable because they are in some sense the USP of AOs meaning these features are what makes AOs different from other Terminals.
          Some features are fundamental for AOs though such as the quit, version, about or credits.
          The rest of the features will be mutable and can be changed at any time.
        * NOTE: After modifying settings.json AOs needs to be reloaded to apply those changes.
        */

        // Experimental commands
        // parser.Add(
        //     ["@"], "Experimental/Overload commands",
        //     supported_args: new Dictionary<string[], string>
        //     {
        //         {["-h", "--help"], "Show information about a developer command"},
        //         {["itsmagic"], "It's magic, it's magic."},
        //         {["switch"], "Switch between applications using the App IDs"},
        //         {["studybyte"], "Starts Studybyte"},
        //         {["cpix"], "Starts Cpix"}
        //     },
        //     default_values: ["help"], is_flag: false, method: features.ExperimentCMD
        // );

        // Default-else shell command
        parser.Add(
            ["!"], "Switch the default-else shell",
            supported_args: new Dictionary<string[], string>
            {
                {["cmd"], "Switch the default-else shell to cmd.exe"},
                {["ps", "powershell"], "Switch the default-else shell to powershell.exe"}
            },
            default_values: [""], max_args_length: 1, method: features.SwitchElseShell
        );

        // Developer commands
        parser.Add(
            ["dev", "developer"], "Developer tools",
            supported_args: new Dictionary<string[], string>
            {
                {["-h", "--help"], "Show information about a developer command"},
                {["new"], "Create a new project"},
                {["-t", "tasks"], "List or run custom task in the developer environment"},
                {["clean"], "Delete temporary/unnecessary files created in the project"},
                {["-v", "ver", "version"], "Show the current build number of the project"},
            },
            default_values: ["help"], is_flag: false, method: features.DevCMD
        );

        // Flagged commands
        parser.Add(["settings"], "Displays the AOs settings", method: features.PrintAOsSettings);
        parser.Add(["version", "ver", "-v"], "Displays the AOs version", method: features.PrintVersion);
        parser.Add(["about", "info"], "About AOs", method: features.About);
        parser.Add(["reload", "refresh"], "Restart AOs", method: features.Refresh);
        parser.Add(["credits"], "Credit for AOs", method: Obsidian.Credits);
        parser.Add(["quit", "exit"], "Exit AOs", method: features.Exit);

        // Load all the features from settings.json
        foreach (var cmd in Settings.cmds)
        {
            if (cmd.location == "internal")
            {
                parser.Add(
                    cmd_names: cmd.cmd_names,
                    help_message: cmd.help_message,
                    // usage: cmd.usage,
                    supported_args: cmd.supported_args?.ToDictionary(arg => arg.args, arg => arg.help_message),
                    default_values: cmd.default_values,
                    is_flag: cmd.is_flag,
                    min_args_length: cmd.min_arg_len,
                    max_args_length: cmd.max_arg_len,
                    method: GetInternalMethod()[cmd.method]
                    // index_cmd: cmd.index_cmd
                );
            }

            else if (cmd.location == "default-else-shell")
            {
            }

            else if (cmd.location == "external")
            {
            }

            else if (cmd.location == "powertoys")
            {
            }
        }
    }
}
