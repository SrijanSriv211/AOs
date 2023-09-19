partial class Parser
{
    public struct Command
    {
        public string[] Cmd_names { get; set; }
        public string Help_message { get; set; }
        public Dictionary<string[], string> Supported_args { get; set; }
        public string[] Default_values { get; set; }
        public bool Is_flag { get; set; }
        public int Max_args_length { get; set; }
        public int Min_args_length { get; set; }
        public Delegate Method { get; set; }

        public Command(string[] cmd_names, string help_message, Dictionary<string[], string> supported_args=null, string[] default_values=null, bool is_flag=true, int min_args_length=0, int max_args_length=0, Delegate method=null)
        {
            this.Cmd_names = cmd_names;
            this.Help_message = help_message;
            this.Supported_args = supported_args;
            this.Default_values = default_values;
            this.Is_flag = is_flag;
            this.Max_args_length = max_args_length;
            this.Min_args_length = min_args_length;
            this.Method = method;
        }
    }

    private Command FindMatchingCommand(string arg)
    {
        foreach (Command cmd_name in command_details)
        {
            if (cmd_name.Cmd_names.Contains(arg))
                return cmd_name;
        }

        return new Command();
    }
}
