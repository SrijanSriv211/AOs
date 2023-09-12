partial class Features
{
    public void ChangeCurrentDir(string dirname)
    {
        if (Utils.String.IsEmpty(dirname))
            new TerminalColor(Directory.GetCurrentDirectory(), ConsoleColor.White);

        else
            Directory.SetCurrentDirectory(Utils.String.Strings(dirname));
    }
}
