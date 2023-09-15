partial class Argparse
{
    private struct Argument
    {
        public string[] Names { get; set; }
        public string Help { get; set; }
        public string Default_value { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }

        public Argument(string[] names, string help="", string default_value=null, bool is_flag=false, bool required=false)
        {
            this.Names = names;
            this.Help = help;
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
