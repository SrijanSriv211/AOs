partial class Parser
{
    public struct Command(string[] cmd_names, string help_message, string[] usage, Dictionary<string[], string> supported_args=null, string[] default_values=null, bool is_flag=true, int min_args_length=0, int max_args_length=0, string location="internal", Delegate method=null, bool do_index=true)
    {
        public string[] Cmd_names { get; set; } = cmd_names;
        public string Help_message { get; set; } = help_message;
        public string[] Usage { get; set; } = usage;
        public Dictionary<string[], string> Supported_args { get; set; } = supported_args;
        public string[] Default_values { get; set; } = default_values;
        public bool Is_flag { get; set; } = is_flag;
        public int Max_args_length { get; set; } = max_args_length;
        public int Min_args_length { get; set; } = min_args_length;
        public string Location { get; set; } = location;
        public Delegate Method { get; set; } = method;
        public bool Do_index { get; set; } = do_index;
    }

    private Command FindMatchingCommand(string arg, bool only_indexed=false)
    {
        foreach (Command cmd_name in commands)
        {
            if (
                (only_indexed && cmd_name.Cmd_names.Contains(arg) && cmd_name.Do_index) ||
                (!only_indexed && cmd_name.Cmd_names.Contains(arg))
            ) return cmd_name;
        }

        return new Command();
    }
}
