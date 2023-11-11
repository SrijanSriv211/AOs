using System.Text.Json;

partial class DeveloperFeatures
{
    private void CleanProject()
    {
        string json_data = FileIO.FileSystem.ReadAllText("AOs.dev\\project.json");

        if (Utils.String.IsEmpty(json_data))
        {
            Console.WriteLine("null");
            return;
        }

        ProjectTemplate project_details = JsonSerializer.Deserialize<ProjectTemplate>(json_data);
        List<string> project_wastes = project_details.clean_project_waste;
        foreach (string waste in project_wastes)
        {
            if (Directory.Exists(waste))
                FileIO.FolderSystem.Delete(waste);

            else if (File.Exists(waste))
                FileIO.FileSystem.Delete(waste);
        }
    }
}
