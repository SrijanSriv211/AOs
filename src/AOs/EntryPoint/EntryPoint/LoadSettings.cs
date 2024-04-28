using System.Text.Json;

partial class EntryPoint
{
    public void LoadSettings()
    {
        // Read 'settings.json'
        string SettingsFilepath = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\settings.json");
        string JsonData = FileIO.FileSystem.ReadAllText(SettingsFilepath);

        // Deserialize all the json data from 'settings.json' into a usable object
        Settings = JsonSerializer.Deserialize<SettingsTemplate>(JsonData);
    }

    public class SettingsTemplate
    {
        public string default_else_shell { get; set; }
        public string username { get; set; }
        public string[] startlist { get; set; }
        public ReadLineTemplate readline { get; set; }
        public List<CmdsTemplate> cmds { get; set; }
    }

    public class ReadLineTemplate
    {
        public bool color_coding { get; set; }
        public bool auto_complete_suggestions { get; set; }
    }

    public class CmdsTemplate
    {
        public string[] cmd_names { get; set; }
        public string help_message { get; set; }
        public string[] usage { get; set; }
        public List<SupportedArgsTemplate> supported_args { get; set; }
        public string[] default_values { get; set; }
        public bool is_flag { get; set; }
        public int min_arg_len { get; set; }
        public int max_arg_len { get; set; }
        public string method { get; set; }
        public string location { get; set; }
        public bool do_index { get; set; }
    }

    public class SupportedArgsTemplate
    {
        public string[] args { get; set; }
        public string help_message { get; set; }
    }
}
