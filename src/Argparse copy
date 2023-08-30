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
        public Delegate method { get; set; }

        public Argument(string[] names, string help, bool required, string default_value, bool is_flag, Delegate method=null)
        {
            this.names = names;
            this.help = help;
            this.required = required;
            this.default_value = default_value;
            this.is_flag = is_flag;
            this.method = method;
        }
    }

    public struct ParsedArgument
    {
        public string[] names { get; set; }
        public string value { get; set; }
        public bool is_flag { get; set; }
        public Delegate method { get; set; }

        public ParsedArgument(string[] names, string value, bool is_flag, Delegate method)
        {
            this.names = names;
            this.value = value;
            this.method = method;
            this.is_flag = is_flag;
        }
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

    public Argparse(string name, string description)
    {
        cmd_name = name;
        cmd_desc = description;
    }

    public void Add(string[] names, string help="", bool required=false, string default_value=null, bool is_flag=false, Delegate method=null)
    {
        arguments.Add(new Argument(names, help, required, default_value, is_flag, method));
        help_list.Add($"{string.Join(", ", names)} -> {help}");
    }

    public List<ParsedArgument> Parse(string[] args, Action<string> error_func=null)
    {
        List<ParsedArgument> parsed_args = new List<ParsedArgument>();
        string[] arg_flags = { "--", "-", "_", "/" };

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg_flags.Any(prefix => arg.StartsWith(prefix)))
            {
                Argument matching_argument = FindMatchingArgument(arg);
                if (matching_argument.names == null)
                {
                    if (error_func == null)
                        Error.UnrecognizedArgs(arg);

                    else
                        error_func(arg);

                    return new List<ParsedArgument>();
                }

                if (matching_argument.is_flag)
                    parsed_args.Add(new ParsedArgument(matching_argument.names.ToArray(), "true", matching_argument.is_flag, matching_argument.method));

                else
                {
                    if (Array.IndexOf(args, arg) == args.Length - 1 || arg_flags.Any(prefix => args[Array.IndexOf(args, arg) + 1].StartsWith(prefix)))
                    {
                        new Error($"Missing value for argument: {arg}");
                        return new List<ParsedArgument>();
                    }

                    parsed_args.Add(new ParsedArgument(matching_argument.names.ToArray(), args[Array.IndexOf(args, arg) + 1], matching_argument.is_flag, matching_argument.method));
                    i++;
                }
            }

            else
                parsed_args.Add(new ParsedArgument(new string[]{arg}, null, false, null));
        }

        List<string> missing_arg_list = arguments.Where(argument => argument.required && !parsed_args.Any(arg => arg.names.SequenceEqual(argument.names))).Select(argument => argument.names[0]).ToList();
        if (missing_arg_list.Count > 0)
        {
            Error.TooFewArgs("");
            new Error($"Missing required argument(s): {string.Join(", ", missing_arg_list)}");
            return new List<ParsedArgument>();
        }

        return parsed_args;
    }

    public void PrintHelp()
    {
        new TerminalColor("Name:", ConsoleColor.Yellow);
        Console.WriteLine($"{cmd_name}\n");
        new TerminalColor("Description:", ConsoleColor.Cyan);
        Console.WriteLine($"{cmd_desc}\n");
        new TerminalColor("Usage:", ConsoleColor.Blue);
        Console.WriteLine($"{cmd_name} [OPTIONS]\n");
        new TerminalColor("Options:", ConsoleColor.Magenta);

        foreach (var argument in arguments)
        {
            string argName = string.Join(", ", Collection.Array.Reduce(argument.names));
            string defaultValue = argument.default_value != null ? $" (default: {argument.default_value})" : "";
            string isRequired = argument.required != false ? $" (required: true)" : "";
            string isFlag = argument.is_flag != false ? $" (is flag: true)" : "";

            Console.WriteLine($"{argName}: {argument.help}{defaultValue}{isRequired}{isFlag}");
        }
    }

    public void GetHelp(string name="")
    {
        help_list.Sort();
        name = name.StartsWith('_') ? name.Substring(1) : name;

        if (Collection.String.IsEmpty(name))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");
            foreach (string item in help_list)
                Console.WriteLine(item);
        }

        else
        {
            if (name == ",")
            {
                Error.Syntax("Invalid syntax");
                return;
            }

            string match = "";
            foreach (string item in help_list)
            {
                if (!Collection.String.IsEmpty(match))
                {
                    Console.WriteLine(match);
                    break;
                }

                string[] parts = item.Split("->");
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Contains(name))
                    {
                        match = $"{Collection.String.Reduce(parts[i])} -> {Collection.String.Reduce(parts[i+1])}";
                        break;
                    }
                }
            }

            if (Collection.String.IsEmpty(match))
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
