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
        string value = string.Empty;

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg_flags.Any(prefix => arg.StartsWith(prefix)))
            {
                List<string> names = arguments.SelectMany(i => i.names).ToList();
                string name = names.Find(name => name==arg);

                if (Collection.String.IsEmpty(name))
                {
                    Error.Args(arg);
                    return new List<ParsedArgument>();
                }

                else
                {
                    foreach (var argument in arguments)
                    {
                        if (argument.is_flag && name == arg)
                            value = "true";

                        else if (!argument.is_flag && name == arg)
                        {
                            List<string> list = args.ToList();
                            int index = list.IndexOf(name);
                            if (index != -1 && index < args.Length-1)
                            {
                                if (!arg_flags.Any(prefix => args[index + 1].StartsWith(prefix)))
                                    value = args[index + 1];

                                else if (argument.default_value != null)
                                    parsed_args.Add(new ParsedArgument(argument.names, argument.default_value));

                                else
                                {
                                    new Error($"Missing value for argument: {name}");
                                    return new List<ParsedArgument>();
                                }
                            }

                            else
                            {
                                new Error($"Missing value for argument: {name}");
                                return new List<ParsedArgument>();
                            }
                        }

                        else
                            value = "false";

                        parsed_args.Add(new ParsedArgument(names.ToArray(), value));
                    }
                }
            }

            else
                parsed_args.Add(new ParsedArgument(new string[] { arg }, null));
        }

        foreach (var argument in arguments)
        {
            if (argument.required)
            {
                foreach (var i in parsed_args)
                {
                    if (i.names != argument.names)
                    {
                        new Error($"Missing required argument: {argument.names}");
                        return new List<ParsedArgument>();
                    }
                }
            }
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
