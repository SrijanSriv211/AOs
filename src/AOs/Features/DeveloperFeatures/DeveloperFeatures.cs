partial class DeveloperFeatures
{
    public readonly Parser parser;

    private readonly SystemUtils sys_utils;

    public DeveloperFeatures()
    {
        this.sys_utils = new();

        this.parser = new(CheckForError);
        this.LoadDevFeatures();
    }

    private void CheckForError(string input_cmd, string[] _)
    {
        new Error($"'{input_cmd}', Developer command does not exist");
    }
}
