partial class EntryPoint
{
    // A dictionary containng all the the method names from settings.json alongwith their respective functions.
    private Dictionary<string, Delegate> InternalMethods()
    {
        return new Dictionary<string, Delegate>() {
            { "Rij", Features.Rij },
            { "RunAOsAsAdmin", Features.Admin },
            { "ClearConsole", AOs.ClearConsole },
            { "ShutdownHost", Features.Shutdown },
            { "RestartHost", Features.Restart },
            { "LockHost", Features.Lock },
            { "SleepHost", Features.Sleep },
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
            { "SwitchApp", Features.SwitchApp },
            { "SwitchElseShell", Features.SwitchElseShell },
            { "AO(s)ettings", Features.PrintAOsSettings },
            { "GetAOsVersion", Features.PrintVersion },
            { "AboutAOs", Features.About },
            { "RefreshAOs", Features.Refresh },
            { "ExitAOs", Features.Exit },
            { "AOsCredits", Obsidian.Credits }
        };
    }

    private void IndexPrivateFeatures(string[] _cmd_names, string _help_message, string[] _usage=null, Dictionary<string[], string> _supported_args=null, string[] _default_values=null, bool _is_flag=true, int _min_args_length=0, int _max_args_length=0, string _method="internal", bool _do_index=true)
    {
        List<SupportedArgsTemplate> supportedArgs = [];

        if (_supported_args != null)
        {
            foreach (var item in _supported_args)
                supportedArgs.Add(new SupportedArgsTemplate {args = item.Key, help_message = item.Value});
        }

        else
            supportedArgs = null;

        Settings.cmd_schema.Add(new CmdsTemplate {
            cmd_names = _cmd_names,
            help_message = _help_message,
            usage = _usage,
            supported_args = supportedArgs,
            default_values = _default_values,
            is_flag = _is_flag,
            min_arg_len = _min_args_length,
            max_arg_len = _max_args_length,
            method = _method,
            location = "internal",
            do_index = _do_index
        });
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
        IndexPrivateFeatures(
            _cmd_names: ["!"],
            _help_message: "Switch the default-else shell",
            _usage: null,
            _supported_args: new Dictionary<string[], string>
            {
                {["cmd"], "Switch the default-else shell to cmd.exe"},
                {["ps", "powershell"], "Switch the default-else shell to powershell.exe"}
            },
            _default_values: [""], _min_args_length: 0, _max_args_length: 1, _is_flag: false, _method: "SwitchElseShell"
        );

        // Private commands
        IndexPrivateFeatures(["settings"], "Displays the AOs settings", _method: "AO(s)ettings");
        IndexPrivateFeatures(["version", "ver", "-v"], "Displays the AOs version", _method: "GetAOsVersion");
        IndexPrivateFeatures(["about", "info"], "About AOs", _method: "AboutAOs");
        IndexPrivateFeatures(["reload", "refresh"], "Restart AOs", _method: "RefreshAOs");
        IndexPrivateFeatures(["credits"], "Credit for AOs", _method: "AOsCredits");
        IndexPrivateFeatures(["quit", "exit"], "Exit AOs", _method: "ExitAOs");

        // Load all the features from settings.json
        foreach (var cmd in Settings.cmd_schema)
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
        Console.ReadKey();
    }
}
