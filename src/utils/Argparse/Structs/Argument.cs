partial class Argparse
{
    private struct Argument
    {
        public string[] Names { get; set; }
        public string Help { get; set; }
        public string[] Default_values { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }
        public int Max_args_length { get; set; }
        public int Min_args_length { get; set; }
        public Delegate Method { get; set; }

        public Argument(string[] cmd_names, string help_message, string[] default_values=null, bool is_flag=true, bool required=true, int min_args_length=0, int max_args_length=0, Delegate method=null)
        {
            this.Names = cmd_names;
            this.Help = help_message;
            this.Default_values = default_values;
            this.Is_flag = is_flag;
            this.Required = required;
            this.Max_args_length = max_args_length;
            this.Min_args_length = min_args_length;
            this.Method = method;
        }
    }

    private Argument FindMatchingArgument(string arg)
    {
        foreach (Argument argument in arguments)
        {
            if (argument.Names.Contains(arg))
                return argument;
        }

        return new Argument();
    }
}
