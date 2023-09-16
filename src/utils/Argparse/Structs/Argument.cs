partial class Argparse
{
    private struct Argument
    {
        public string[] Names { get; set; }
        public string Help { get; set; }
        public string Default_value { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }

        public Argument(string[] cmd_names, string help_message, string default_value=null, bool is_flag=true, bool required=true)
        {
            this.Names = cmd_names;
            this.Help = help_message;
            this.Default_value = default_value;
            this.Is_flag = is_flag;
            this.Required = required;
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
