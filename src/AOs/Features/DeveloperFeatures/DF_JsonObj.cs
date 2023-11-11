partial class DeveloperFeatures
{
    public class ProjectTemplate
    {
        public string project_name { get; set; }
        public string project_path { get; set; }
        public string github_URL { get; set; }
        public string github_branch { get; set; }
        public int build { get; set; }
        public List<string> clean_project_waste { get; set; }
    }

    public class TasksDetails
    {
        public string description { get; set; }
        public string command { get; set; }
        public bool update_build_number { get; set; }
        public List<string> call_other_tasks { get; set; }
    }

    public class TasksConfig
    {
        public Dictionary<string, TasksDetails> tasks { get; set; }
    }
}
