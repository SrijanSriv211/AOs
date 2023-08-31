class Argparse
{
    private readonly string name = "";
    private readonly string desc = "";

    private readonly List<string> help_list = new List<string>();
    private readonly List<Argument> arguments = new List<Argument>();

    public Argparse(string name, string description)
    {
        this.name = name;
        this.desc = description;
    }

    public void Add(string[] names, string help="", string default_value=null, bool is_flag=false, bool required=false, Delegate method=null)
    {
        arguments.Add(new Argument(names, help, default_value, is_flag, required, method));
        help_list.Add($"{string.Join(", ", names)} -> {help}");
    }

    public List<ParsedArgument> Parse(string[] args, Action<string> error_func=null)
    {
        List<ParsedArgument> parsed_args = new();

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            Argument matching_argument = FindMatchingArgument(arg);

            if (matching_argument.Names == null)
            {
                error_func ??= Error.UnrecognizedArgs;
                error_func(arg);

                return new List<ParsedArgument>();
            }

            if (matching_argument.Is_flag)
                parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: "true", is_flag: matching_argument.Is_flag, method: matching_argument.Method));

            else
            {
                string out_value;
                if (Array.IndexOf(args, arg) == args.Length - 1)
                {
                    if (matching_argument.Default_value == null)
                    {
                        new Error($"Missing value for argument: {arg}");
                        return new List<ParsedArgument>();
                    }

                    else
                        out_value = matching_argument.Default_value;
                }

                else
                    out_value = args[Array.IndexOf(args, arg) + 1];

                parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: out_value, is_flag: matching_argument.Is_flag, method: matching_argument.Method));
                i++;
            }
        }

        List<string> missing_arg_list = arguments.Where(
            argument => argument.Required
            &&
            !parsed_args.Any(arg => arg.Names.SequenceEqual(argument.Names))).Select(argument => argument.Names[0]
        ).ToList();

        if (missing_arg_list.Count > 0)
        {
            Error.TooFewArgs("");
            new Error($"Missing required argument(s): {string.Join(", ", missing_arg_list)}");
            return new List<ParsedArgument>();
        }

        return parsed_args;
    }

    public struct ParsedArgument
    {
        public string[] Names { get; set; }
        public string Value { get; set; }
        public bool Is_flag { get; set; }
        public Delegate Method { get; set; }

        public ParsedArgument(string[] names=null, string value=null, bool is_flag=false, Delegate method=null)
        {
            this.Names = names ?? new string[0];
            this.Value = value;
            this.Is_flag = is_flag;
            this.Method = method;
        }
    }

    private struct Argument
    {
        public string[] Names { get; set; }
        public string Help { get; set; }
        public string Default_value { get; set; }
        public bool Is_flag { get; set; }
        public bool Required { get; set; }
        public Delegate Method { get; set; }

        public Argument(string[] names, string help="", string default_value=null, bool is_flag=false, bool required=false, Delegate method=null)
        {
            this.Names = names;
            this.Help = help;
            this.Default_value = default_value;
            this.Is_flag = is_flag;
            this.Required = required;
            this.Method = method;
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

    public void PrintHelp()
    {
        new TerminalColor("Name:", ConsoleColor.Yellow);
        Console.WriteLine($"{this.name}\n");
        new TerminalColor("Description:", ConsoleColor.Cyan);
        Console.WriteLine($"{this.desc}\n");
        new TerminalColor("Usage:", ConsoleColor.Blue);
        Console.WriteLine($"{this.name} [OPTIONS]\n");
        new TerminalColor("Options:", ConsoleColor.Magenta);

        foreach (var argument in arguments)
        {
            string argName = string.Join(", ", Collection.Array.Reduce(argument.Names));
            string defaultValue = argument.Default_value != null ? $" (default: {argument.Default_value})" : "";
            string isRequired = argument.Required != false ? $" (required: true)" : "";
            string isFlag = argument.Is_flag != false ? $" (is flag: true)" : "";

            Console.WriteLine($"{argName}: {argument.Help}{defaultValue}{isRequired}{isFlag}");
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
