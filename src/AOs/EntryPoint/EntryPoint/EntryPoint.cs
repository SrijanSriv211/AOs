partial class EntryPoint
{
    public readonly Parser parser;
    public static SettingsTemplate Settings;

    private readonly Obsidian AOs;
    private readonly string[] args;

    public EntryPoint(string[] args)
    {
        CheckRootPackages();

        this.args = args;
        AOs = new Obsidian();
        Features.AOs = AOs;

        parser = new(CheckForError);

        CheckForSetup();
        LoadFeatures();
        Startup();
    }

    // This function is called when the input command is not found in the feature-set of AOs,
    // it then checks if the user is trying to call a system env variable or app and runs it if it is being called.
    // Otherwise, it throws an error the the command does not exist.
    private void CheckForError(Lexer.Tokenizer.Token cmd, Lexer.Tokenizer.Token[] args)
    {
        cmd.Name = SystemUtils.CheckForEnvVarAndEXEs(cmd.Name);

        for (int i = 0; i < args.Length; i++)
            args[i].Name = SystemUtils.CheckForEnvVarAndEXEs(args[i].Name);

        if (!SystemUtils.RunSysOrEnvApps(cmd.Name, args.Select(x => x.Name).ToArray()))
        {
            string current_line = cmd.Name + " " + string.Join("", args.Select(x => x.Name));
            Error.Command(current_line, cmd.Name);
        }
    }
}
