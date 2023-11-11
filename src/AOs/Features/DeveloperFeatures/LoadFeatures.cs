partial class DeveloperFeatures
{
    private void LoadDevFeatures()
    {
        this.parser.Add(
            new string[]{"new"}, "Create a new project",
            default_values: new string[]{"."},
            min_args_length: 1, max_args_length: 1, method: this.New
        );
        this.parser.Add(
            new string[]{"-t", "tasks"}, "List or run custom task in the developer environment",
            is_flag: false, default_values: new string[0]{},
            method: this.Tasks
        );
        this.parser.Add(new string[]{"clean"}, "Delete temporary/unnecessary files created in the project", is_flag: true, method: this.CleanProject);
        this.parser.Add(new string[]{"-v", "ver", "version"}, "Show the current build number of the project", is_flag: true, method: this.ShowBuildNo);
    }
}
