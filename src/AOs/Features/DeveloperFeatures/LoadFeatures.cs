partial class DeveloperFeatures
{
    private void LoadDevFeatures()
    {
        this.parser.Add(
            ["new"], "Create a new project",
            default_values: ["."],
            min_args_length: 1, max_args_length: 1, method: this.New
        );
        this.parser.Add(
            ["-t", "tasks"], "List or run custom task in the developer environment",
            is_flag: false, default_values: [],
            method: this.Tasks
        );
        this.parser.Add(["clean"], "Delete temporary/unnecessary files created in the project", is_flag: true, method: this.CleanProject);
        this.parser.Add(["-v", "ver", "version"], "Show the current build number of the project", is_flag: true, method: this.ShowBuildNo);
    }
}
