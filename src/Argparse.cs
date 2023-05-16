using System;
using System.Linq;
using System.Collections.Generic;

class Argparse
{
    private string cmd_name = string.Empty;
    private string cmd_desc = string.Empty;
    private List<string> help_list = new List<string>();
    private List<Argument> arguments = new List<Argument>();

    private struct Argument
    {
        public string[] names { get; set; }
        public string help { get; set; }
        public bool required { get; set; }
        public string default_value { get; set; }
        public bool is_flag { get; set; }

        public Argument(string[] names, string help, bool required, string default_value)
        {
            this.names = names;
            this.help = help;
            this.required = required;
            this.default_value = default_value;
            this.is_flag = (default_value == null);
        }
    }

    public struct ParsedArgument
    {
        public string[] names { get; set; }
        public string value { get; set; }

        public ParsedArgument(string[] names, string value)
        {
            this.names = names;
            this.value = value;
        }
    }

    public Argparse(string name, string description)
    {
        cmd_name = name;
        cmd_desc = description;

        help_list.Add($"{cmd_name} -> {description}");
    }

    public void Add(string[] names, string help="", bool required=false, string default_value=null)
    {
        arguments.Add(new Argument(names, help, required, default_value));
    }

    public List<ParsedArgument> Parse(string[] args)
    {
        List<ParsedArgument> parsed_args = new List<ParsedArgument>();
        string[] arg_flags = { "--", "-", "/" };

        foreach (string arg in args)
        {
            if (arg_flags.Any(prefix => arg.StartsWith(prefix)))
            {
                List<string> names = arguments.SelectMany(i => i.names).ToList();

                string name = names.Find(name => name==arg);
                if (name == null)
                {
                    new Error($"Invalid argument: {arg}");
                    return new List<ParsedArgument>();
                }

                else
                {
                    Console.WriteLine(name);
                }
            }

            else
                parsed_args.Add(new ParsedArgument(new string[] { arg }, null));
        }

        return parsed_args;
    }

    private Argument FindMatchingArgument(string arg)
    {
        foreach (Argument argument in arguments)
        {
            if (argument.names.Contains(arg))
                return argument;
        }

        return new Argument();
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
        string[] help_flags = { "/?", "-h", "--help", "??" };
        return help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAskingForHelp(string[] args)
    {
        string[] help_flags = { "/?", "-h", "--help", "??" };
        foreach (string arg in args)
        {
            if (help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
