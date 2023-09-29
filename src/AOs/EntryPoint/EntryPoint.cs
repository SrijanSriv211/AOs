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
        this.parser = new(CheckForError);

        CheckRootPackages();
        LoadFeatures();
        Startup();
    }

    private void CheckForError(string input_cmd, string[] input_args)
    {
        input_cmd = SystemUtils.CheckForSysOrEnvApps(input_cmd);

        for (int i = 0; i < input_args.Length; i++)
        {
            if (input_args[i].StartsWith("%") && input_args[i].EndsWith("%"))
                input_args[i] = SystemUtils.CheckForSysOrEnvApps(input_args[i]);
        }

        if (!this.sys_utils.RunSysOrEnvApps(input_cmd, input_args))
            Error.Command(input_cmd);
    }
}
