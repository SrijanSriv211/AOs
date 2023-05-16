using System;
using System.Linq;
using System.Collections.Generic;

class Argparse
{
    private string cmd_name = string.Empty;
    private string cmd_desc = string.Empty;
    private List<string> help_list = new List<string>();
    private List<Argument> arguments = new List<Argument>();

    public Argparse(string name, string description)
    {
        cmd_name = name;
        cmd_desc = description;

        help_list.Add($"{cmd_name} -> {description}");
    }

    public void Add(string name, string help="", bool required=false, string default_value=null)
    {
        arguments.Add(new Argument(name, help, required, default_value));
    }

    public List<ParsedArgument> Parse(string[] args)
    {
        List<ParsedArgument> parsed_args = new List<ParsedArgument>();
        foreach (string arg in args)
        {
            // code here.
        }

        return new List<ParsedArgument>();
    }

    public void GetHelp(string name="")
    {
        help_list.Sort();

        if (Collection.String.IsEmpty(name))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");
            foreach (string item in help_list)
            {
                string[] parts = item.Split(' ');
                string command = parts[0];
                string description = string.Join(" ", parts.Skip(2));
                Console.WriteLine("{0,-15} -> {1}", command, description);
            }
        }

        else
        {
            var matches = help_list.Where(s => s.StartsWith(name));
            if (matches.Any())
                Console.WriteLine(string.Join("\n", matches));

            else
                new Error($"No information for command '{name}'");
        }
    }

    public static bool IsAskingForHelp(string arg)
    {
        string[] help_flags = {"/?", "-h", "--help", "??"};
        return help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAskingForHelp(string[] args)
    {
        string[] help_flags = {"/?", "-h", "--help", "??"};
        foreach (string arg in args)
        {
            if (help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public struct ParsedArgument
    {
        public string name { get; set; }
        public string value { get; set; }
        public string type { get; set; }

        public ParsedArgument(string name, string value, string type)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }

    private struct Argument
    {
        public string name { get; set; }
        public string help { get; set; }
        public bool required { get; set; }
        public string defaultvalue { get; set; }
        public bool isflag { get; set; }

        public Argument(string name, string help, bool required, string default_value)
        {
            this.name = name;
            this.help = help;
            this.required = required;
            this.defaultvalue = default_value;
            this.isflag = (default_value == null);
        }
    }
}
