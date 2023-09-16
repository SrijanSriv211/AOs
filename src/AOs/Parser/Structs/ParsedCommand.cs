partial class Parser
{
    public struct ParsedCommand
    {
        public string Cmd_name { get; set; }
        public string[] Values { get; set; }
        public bool Is_flag { get; set; }
        public int Max_args_length { get; set; }
        public int Min_args_length { get; set; }
        public Delegate Method { get; set; }

        public ParsedCommand(string cmd_name, string[] values, bool is_flag, int min_args_length, int max_args_length, Delegate method)
        {
            this.Cmd_name = cmd_name;
            this.Values = values;
            this.Is_flag = is_flag;
            this.Max_args_length = max_args_length;
            this.Min_args_length = min_args_length;
            this.Method = method;
        }
    }
}
