//! Test for now. No use. Improve it later.
partial class Features
{
    public class CmdNames
    {
        public string[] cmd_names { get; set; }
        public string help_message { get; set; }
        public List<List<object>> supported_args { get; set; }
        public string[] default_values { get; set; }
        public bool is_flag { get; set; }
        public int min_arg_len { get; set; }
        public int max_arg_len { get; set; }
        public string method { get; set; }
        public string exec_engine { get; set; }
    }

    public class CmdNamesConfig
    {
        public List<CmdNames> cmds { get; set; }
    }
}
