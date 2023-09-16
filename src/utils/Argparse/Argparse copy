partial class Argparse
{
    private readonly string name = "";
    private readonly string desc = "";
    private readonly List<string> help_list = new();
    private readonly List<Argument> arguments = new();

    public Argparse(string name, string description)
    {
        this.name = name;
        this.desc = description;
    }

    public void Add(string[] names, string help="", string default_value=null, bool is_flag=false, bool required=false)
    {
        arguments.Add(new Argument(names, help, default_value, is_flag, required));
        help_list.Add($"{string.Join(", ", names)} -> {help}");
    }

    public List<ParsedArgument> Parse(string[] args, Action<string> error_func=null)
    {
        List<ParsedArgument> parsed_args = new();
        string[] arg_flags = { "--", "-", "/" };

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (arg_flags.Any(prefix => arg.StartsWith(prefix)))
            {
                Argument matching_argument = FindMatchingArgument(arg);

                if (matching_argument.Names == null)
                {
                    error_func ??= Error.UnrecognizedArgs;
                    error_func(arg);

                    return new List<ParsedArgument>();
                }

                if (matching_argument.Is_flag)
                    parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: "true", is_flag: matching_argument.Is_flag));

                else
                {
                    string out_value;
                    if (Array.IndexOf(args, arg) == args.Length - 1)
                    {
                        if (matching_argument.Default_value == null)
                        {
                            new Error($"Missing value for argument: {arg}");
                            out_value = null;
                        }

                        else
                            out_value = matching_argument.Default_value;
                    }

                    else
                        out_value = args[Array.IndexOf(args, arg) + 1];

                    parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: out_value, is_flag: matching_argument.Is_flag));
                    i++;
                }
            }

            else
                parsed_args.Add(new ParsedArgument(new string[]{arg}));
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
}
