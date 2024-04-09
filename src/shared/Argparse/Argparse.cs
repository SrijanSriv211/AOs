partial class Argparse(string name, string description, Action<string> error_func = null)
{
    private readonly string name = name;
    private readonly string desc = description;

    private readonly List<Argument> arguments = [];
    private readonly Action<string> error_func = error_func;

    public void Add(string[] cmd_names, string help_message, string default_value=null, bool is_flag=false, bool required=false)
    {
        arguments.Add(new Argument(cmd_names, help_message, default_value, is_flag, required));
    }

    public List<ParsedArgument> Parse(string[] args)
    {
        List<ParsedArgument> parsed_args = [];
        string[] arg_flags = ["--", "-", "/"];

        for (int i = 0; i < args.Length; i++)
        {
            string lowercase_arg = args[i].ToLower();
            Argument matching_argument = FindMatchingArgument(lowercase_arg);

            // Return if no matching command was found.
            if (matching_argument.Names == null)
            {
                if (arg_flags.Any(lowercase_arg.StartsWith))
                {
                    error_func(args[i]);
                    return [];
                }

                else
                {
                    parsed_args.Add(new ParsedArgument(
                        names: [args[i]],
                        value: null,
                        is_flag: true,
                        required: false,
                        known_type: "Unknown"
                    ));

                    continue;
                }
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
                        return [];
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

            List<string> missing_arg_list = arguments.Where(
                argument => argument.Required
                &&
                !parsed_args.Any(arg => arg.Names.SequenceEqual(argument.Names))).Select(argument => argument.Names[0]
            ).ToList();

            if (missing_arg_list.Count > 0)
            {
                Error.TooFewArgs("");
                new Error($"Missing required argument(s): {string.Join(", ", missing_arg_list)}", "runtime error");
                return [];
            }
        }

        return parsed_args;
    }
}
