partial class ExperimentalFeatures
{
    public readonly Parser parser;

    private readonly SystemUtils sys_utils;

    public ExperimentalFeatures()
    {
        this.sys_utils = new();

        this.parser = new(CheckForError);
        this.LoadExperimentalFeatures();
    }

    private void CheckForError(string input_cmd, string[] _)
    {
        new Error($"'{input_cmd}', Experimental command does not exist");
    }
}
