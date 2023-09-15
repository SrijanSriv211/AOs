partial class Argparse
{
   public struct ParsedArgument
    {
        public string[] Names { get; set; }
        public string Value { get; set; }
        public bool Is_flag { get; set; }

        public ParsedArgument(string[] names=null, string value=null, bool is_flag=false)
        {
            this.Names = names ?? new string[0];
            this.Value = value;
            this.Is_flag = is_flag;
        }
    }
}
