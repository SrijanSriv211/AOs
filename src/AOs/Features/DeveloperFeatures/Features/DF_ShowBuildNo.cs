using System.Text.Json;

partial class DeveloperFeatures
{
    private void ShowBuildNo()
    {
        string json_data = FileIO.FileSystem.ReadAllText("AOs.dev\\project.json");

        if (Utils.String.IsEmpty(json_data))
        {
            Console.WriteLine("null");
            return;
        }

        ProjectTemplate project_details = JsonSerializer.Deserialize<ProjectTemplate>(json_data);
        Console.WriteLine(project_details.build);
    }
}
