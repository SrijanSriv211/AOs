using System.Text.Json;

partial class Error
{
    public static readonly List<string> AllCommands = [];

    public static void GetAllCommands()
    {
        string cmds_filepath = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\cmds.json");
        string json_data = FileIO.FileSystem.ReadAllText(cmds_filepath);

        CmdNamesConfig AllCMDNames = JsonSerializer.Deserialize<CmdNamesConfig>(json_data);
        AllCommands.AddRange(AllCMDNames.cmds.SelectMany(cmd => cmd.cmd_names));
    }

    private record CmdNames(string[] cmd_names);
    private record CmdNamesConfig(List<CmdNames> cmds);
}
