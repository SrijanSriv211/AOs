partial class Argparse
{
    public struct ParsedArgument
    {
        public string[] Names { get; set; }
        public string[] Values { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }
        public int Max_args_length { get; set; }
        public int Min_args_length { get; set; }
        public Delegate Method { get; set; }
        public string KnownType { get; set; } = "Known";

        public ParsedArgument(string[] names, string[] values, bool is_flag, bool required, int min_args_length, int max_args_length, Delegate method, string known_type="Known")
        {
            this.Names = names ?? new string[0];
            this.Values = values;
            this.Is_flag = is_flag;
            this.Required = required;
            this.Max_args_length = max_args_length;
            this.Min_args_length = min_args_length;
            this.Method = method;
            this.KnownType = known_type;
        }
    }
}
