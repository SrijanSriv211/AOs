partial class Argparse
{
    private readonly string name = "";
    private readonly string desc = "";

    private readonly List<Argument> arguments = new();
    private readonly Dictionary<string[], string> help_list = new();
    private readonly Action<string> error_func;

    public Argparse(string name, string description, Action<string> error_func=null)
    {
        this.name = name;
        this.desc = description;

        this.error_func = error_func;
    }

    public void Add(string[] cmd_names, string help_message, string default_value=null, bool is_flag=true, bool required=false)
    {
        if (default_value != null)
            is_flag = false;

        help_list.Add(cmd_names, help_message);
        arguments.Add(new Argument(cmd_names, help_message, default_value, is_flag, required));
    }

    public List<ParsedArgument> Parse(string[] args)
    {
        List<ParsedArgument> parsed_args = new();
        string[] arg_flags = { "--", "-", "/" };

        for (int i = 0; i < args.Length; i++)
        {
            string lowercase_arg = args[i].ToLower();
            if (arg_flags.Any(lowercase_arg.StartsWith))
            {
                Argument matching_argument = FindMatchingArgument(lowercase_arg);

                // Return if no matching command was found.
                if (matching_argument.Names == null)
                {
                    error_func(args[i]);
                    return new List<ParsedArgument>();
                }

                if (matching_argument.Is_flag)
                {
                    parsed_args.Add(new ParsedArgument(
                        names: matching_argument.Names,
                        value: "true",
                        is_flag: matching_argument.Is_flag,
                        required: matching_argument.Required
                    ));
                }

                else
                {
                    string out_value;
                    if (Array.IndexOf(args, lowercase_arg) == args.Length - 1)
                    {
                        if (matching_argument.Default_value == null)
                        {
                            Error.NoArgs(args[i]);
                            return new List<ParsedArgument>();
                        }

                        else
                            out_value = matching_argument.Default_value;
                    }

                    else
                        out_value = args[Array.IndexOf(args, lowercase_arg) + 1];

                    parsed_args.Add(new ParsedArgument(
                        names: matching_argument.Names,
                        value: out_value,
                        is_flag: matching_argument.Is_flag,
                        required: matching_argument.Required
                    ));

                    i++;
                }
            }

            else
            {
                parsed_args.Add(new ParsedArgument(
                    names: new string[]{args[i]},
                    value: null,
                    is_flag: true,
                    required: false,
                    known_type: "Unknown"
                ));
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
        }

        return parsed_args;
    }
}
