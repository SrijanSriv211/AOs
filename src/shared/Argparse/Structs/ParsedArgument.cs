partial class Argparse
{
    public struct ParsedArgument
    {
        public string[] Names { get; set; }
        public string Value { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }
        public string KnownType { get; set; } = "Known";

        public ParsedArgument(string[] names, string value, bool is_flag, bool required, string known_type="Known")
        {
            this.Names = names ?? [];
            this.Value = value;
            this.Is_flag = is_flag;
            this.Required = required;
            this.KnownType = known_type;
        }
    }
}
