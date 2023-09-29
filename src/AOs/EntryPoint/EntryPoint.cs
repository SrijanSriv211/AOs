partial class EntryPoint
{
    public readonly Parser parser;
    public readonly Features features;

    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, Parser, List<(string, string[])>> run_method;
    private readonly SystemUtils sys_utils = Obsidian.sys_utils;

    public EntryPoint(string[] args, Action<Obsidian, Parser, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;
        this.AOs = new Obsidian();

        this.features = new(this.AOs, this.sys_utils);
        this.parser = new(CheckForError);

        CheckRootPackages();
        LoadFeatures();
        Startup();
    }

    private void CheckForError(string input_cmd, string[] input_args)
    {
        input_cmd = SystemUtils.CheckForSysOrEnvApps(input_cmd);

        for (int i = 0; i < input_args.Length; i++)
            input_args[i] = SystemUtils.CheckForSysOrEnvApps(input_args[i]);

        if (!this.sys_utils.RunSysOrEnvApps(input_cmd, input_args))
            Error.Command(input_cmd);
    }
}
