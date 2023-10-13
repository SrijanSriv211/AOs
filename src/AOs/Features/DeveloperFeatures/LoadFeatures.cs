partial class DeveloperFeatures
{
    private void LoadDevFeatures()
    {
        this.parser.Add(
            new string[]{"new"}, "Create a new project",
            default_values: new string[]{"."},
            min_args_length: 1, max_args_length: 2, method: this.New
        );
        this.parser.Add(new string[]{"git", "github"}, "Use git to maintain version control", is_flag: false, min_args_length: -1, method: this.RunGit);
        this.parser.Add(new string[]{"cloc", "countlinesofcode"}, "Count the lines of code in a project directory", is_flag: false, min_args_length: -1, method: this.RunCLOC);
        this.parser.Add(new string[]{"clean"}, "Delete temp/unnecessary files created by the programming language in the project", is_flag: true, method: this.CleanProject);
        this.parser.Add(new string[]{"ver", "version"}, "Show the current build number of the project", is_flag: true, method: this.ShowBuildNo);
        this.parser.Add(new string[]{"server"}, "Start a local web-server", is_flag: true, method: this.StartLocalServer);
    }
}
