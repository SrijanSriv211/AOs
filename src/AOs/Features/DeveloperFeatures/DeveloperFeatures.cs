partial class DeveloperFeatures
{
    public readonly Parser parser;

    public DeveloperFeatures()
    {
        this.parser = new(CheckForError);
        this.LoadDevFeatures();
    }

    private void CheckForError(string input_cmd, string[] _)
    {
        new Error($"'{input_cmd}', Command does not exist");
    }
}
