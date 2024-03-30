partial class EntryPoint
{
    public readonly Parser parser;
    public readonly Features features;

    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly SystemUtils sys_utils;

    public EntryPoint(string[] args)
    {
        this.args = args;
        this.AOs = new Obsidian();

        this.sys_utils = new();
        this.features = new(this.AOs);
        this.parser = new(this.CheckForError);

        CheckRootPackages();
        this.LoadFeatures();
        this.Startup();
    }

    // This function is called when the input command is not found in the feature-set of AOs,
    // it then checks if the user is trying to call a system env variable or app and runs it if it is being called.
    // Otherwise, it throws an error the the command does not exist.
    private void CheckForError(string cmd_name, string[] args)
    {
        cmd_name = SystemUtils.CheckForSysOrEnvApps(cmd_name);

        for (int i = 0; i < args.Length; i++)
            args[i] = SystemUtils.CheckForSysOrEnvApps(args[i]);

        if (!this.sys_utils.RunSysOrEnvApps(cmd_name, args))
        {
            string current_line = cmd_name + " " + string.Join("", args);
            Error.Command(current_line, cmd_name);
        }
    }
}
