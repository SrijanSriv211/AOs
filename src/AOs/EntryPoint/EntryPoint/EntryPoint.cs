partial class EntryPoint
{
    public readonly Parser parser;
    public readonly Features features;
    public static SettingsTemplate Settings;

    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly SystemUtils sys_utils;

    public EntryPoint(string[] args)
    {
        CheckRootPackages();

        this.args = args;
        AOs = new Obsidian();

        sys_utils = new();
        features = new(AOs);
        parser = new(CheckForError);

        LoadFeatures();
        Startup();
    }

    // This function is called when the input command is not found in the feature-set of AOs,
    // it then checks if the user is trying to call a system env variable or app and runs it if it is being called.
    // Otherwise, it throws an error the the command does not exist.
    private void CheckForError(string cmd_name, string[] args)
    {
        cmd_name = SystemUtils.CheckForEnvVarAndEXEs(cmd_name);

        for (int i = 0; i < args.Length; i++)
            args[i] = SystemUtils.CheckForEnvVarAndEXEs(args[i]);

        if (!sys_utils.RunSysOrEnvApps(cmd_name, args))
        {
            string current_line = cmd_name + " " + string.Join("", args);
            Error.Command(current_line, cmd_name);
        }
    }
}
