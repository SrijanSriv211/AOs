partial class DeveloperFeatures
{
    private void RunCLOC(string[] args)
    {
        Console.WriteLine("Run CLOC");
        Console.WriteLine(string.Join(", ", args));
        Console.WriteLine();
    }

    private void RunGit(string[] args)
    {
        Console.WriteLine("Run GIT");
        Console.WriteLine(string.Join(", ", args));
        Console.WriteLine();
    }
}
