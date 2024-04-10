partial class EntryPoint
{
    // A dictionary containng all the the method names from settings.json alongwith their respective functions.
    private Dictionary<string, Delegate> InternalMethods()
    {
        return new Dictionary<string, Delegate>() {
            { "RunAOsAsAdmin", Features.Admin },
            { "ClearConsole", AOs.ClearConsole },
            { "ShutdownHost", Features.Shutdown },
            { "RestartHost", Features.Restart },
            { "LockHost", Features.Lock },
            { "GetCurrentTime", Features.GetTime },
            { "GetTodayDate", Features.GetDate },
            { "Diagxt", Features.Diagxt },
            { "Scannow", Features.Scan },
            { "CheckForAOsUpdates", Features.CheckForAOsUpdates },
            { "Shout", Features.Shout },
            { "GetSetHistory", Features.GetSetHistory },
            { "RunInTerminal", Features.RunInTerminal },
            { "RunApp", Features.RunApp },
            { "ChangeTerminalTitle", Features.ChangeTitle },
            { "ChangeTerminalColor", Features.ChangeColor },
            { "ThreadSleep", Features.Wait },
            { "PauseTerminal", Features.Pause },
            { "Cat", Features.Cat },
            { "ChangeTerminalPrompt", Features.ModifyPrompt },
            { "LS", Features.LS },
            { "ChangeCurrentDir", Features.ChangeCurrentDir },
            { "ChangeToPrevDir", Features.ChangeToPrevDir },
            { "Touch", Features.Touch },
            { "Delete", Features.Delete },
            { "Move", Features.Move },
            { "Copy", Features.Copy },
            { "Pixelate", Features.Pixelate },
            { "ReadFile", Features.Read },
            { "CommitFile", Features.Commit },
            { "WinRAR", Features.WinRAR },
            { "TerminateRunningProcess", Features.Terminate },
            { "ControlVolume", Features.ControlVolume },
            { "ItsMagic", Features.ItsMagic },
            { "SwitchApp", Features.SwitchApp }
        };
    }

    private void LoadFeatures()
    {
        /*
        * The following features which are not available in settings.json and are immutable, meaning they can't be changed in anyway unless the source code of AOs is modified.
          These features are immutable because some features are fundamental to AOs though such as the quit, version, about or credits.
          The rest of the features will be mutable and can be changed at any time.
        * NOTE: After modifying settings.json AOs needs to be reloaded to apply those changes.
        */

        // Default-else shell command
        parser.Add(
            ["!"], "Switch the default-else shell",
            supported_args: new Dictionary<string[], string>
            {
                {["cmd"], "Switch the default-else shell to cmd.exe"},
                {["ps", "powershell"], "Switch the default-else shell to powershell.exe"}
            },
            default_values: [""], max_args_length: 1, method: Features.SwitchElseShell
        );

        // Flagged commands
        parser.Add(["settings"], "Displays the AOs settings", method: Features.PrintAOsSettings);
        parser.Add(["version", "ver", "-v"], "Displays the AOs version", method: Features.PrintVersion);
        parser.Add(["about", "info"], "About AOs", method: Features.About);
        parser.Add(["reload", "refresh"], "Restart AOs", method: Features.Refresh);
        parser.Add(["credits"], "Credit for AOs", method: Obsidian.Credits);
        parser.Add(["quit", "exit"], "Exit AOs", method: Features.Exit);

        // Load all the features from settings.json
        foreach (var cmd in Settings.cmds)
        {
            if (cmd.location == "internal")
            {
                parser.Add(
                    cmd_names: cmd.cmd_names,
                    help_message: cmd.help_message,
                    usage: cmd.usage,
                    supported_args: cmd.supported_args?.ToDictionary(arg => arg.args, arg => arg.help_message),
                    default_values: cmd.default_values,
                    is_flag: cmd.is_flag,
                    min_args_length: cmd.min_arg_len,
                    max_args_length: cmd.max_arg_len,
                    method: InternalMethods()[cmd.method],
                    do_index: cmd.do_index
                );
            }

            else if (cmd.location == "external")
            {
                if (!File.Exists(cmd.method))
                {
                    new Error($"Cannot find external app: '{cmd.method}', for command: '{string.Join(", ", cmd.cmd_names)}'", "boot error");
                    Terminal.Print("Please type 'diagxt' command to find the location of 'settings.json' and fix the mistake", ConsoleColor.DarkGray);
                    Environment.Exit(1);
                }
            }
        }
    }
}
