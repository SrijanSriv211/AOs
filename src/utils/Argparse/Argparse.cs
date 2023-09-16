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

    public void Add(string[] cmd_names, string help_message, string[] default_values=null, bool is_flag=true, bool required=false, int min_args_length=0, int max_args_length=0, Delegate method=null)
    {
        if (default_values != null || max_args_length > 0 || min_args_length > 0)
            is_flag = false;

        help_list.Add(cmd_names, help_message);
        arguments.Add(new Argument(cmd_names, help_message, default_values, is_flag, required, min_args_length, max_args_length, method));
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
                        values: new string[]{"true"},
                        is_flag: matching_argument.Is_flag,
                        required: matching_argument.Required,
                        min_args_length: matching_argument.Min_args_length,
                        max_args_length: matching_argument.Max_args_length,
                        method: matching_argument.Method
                    ));
                }

                else
                {
                    string[] out_values;
                    // if (Array.IndexOf(args, lowercase_arg) == args.Length - 1)
                    if (Utils.Array.IsEmpty(args))
                    {
                        if (matching_argument.Default_values == null)
                        {
                            Error.NoArgs(args[i]);
                            return new List<ParsedArgument>();
                        }

                        else
                            out_values = matching_argument.Default_values;
                    }

                    else
                        out_values = args;

                    if (matching_argument.Min_args_length > 0 && Utils.Array.Reduce(out_values).Length < matching_argument.Min_args_length)
                    {
                        Error.TooFewArgs(Utils.Array.Reduce(out_values));
                        return new List<ParsedArgument>();
                    }

                    else if (matching_argument.Max_args_length > 0 && Utils.Array.Reduce(out_values).Length > matching_argument.Max_args_length)
                    {
                        Error.TooManyArgs(Utils.Array.Reduce(out_values));
                        return new List<ParsedArgument>();
                    }

                    parsed_args.Add(new ParsedArgument(
                        names: matching_argument.Names,
                        values: out_values,
                        is_flag: matching_argument.Is_flag,
                        required: matching_argument.Required,
                        min_args_length: matching_argument.Min_args_length,
                        max_args_length: matching_argument.Max_args_length,
                        method: matching_argument.Method
                    ));

                    i++;
                }
            }

            else
            {
                parsed_args.Add(new ParsedArgument(
                    names: new string[]{args[i]},
                    values: new string[0],
                    is_flag: true,
                    required: false,
                    min_args_length: 0,
                    max_args_length: 0,
                    method: null,
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

        // for (int i = 0; i < args.Length; i++)
        // {
        //     string arg = args[i];
        //     if (arg_flags.Any(prefix => arg.StartsWith(prefix)))
        //     {
        //         Argument matching_argument = FindMatchingArgument(arg);

        //         if (matching_argument.Names == null)
        //         {
        //             error_func ??= Error.UnrecognizedArgs;
        //             error_func(arg);

        //             return new List<ParsedArgument>();
        //         }

        //         if (matching_argument.Is_flag)
        //             parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: "true", is_flag: matching_argument.Is_flag));

        //         else
        //         {
        //             string out_value;
        //             if (Array.IndexOf(args, arg) == args.Length - 1)
        //             {
        //                 if (matching_argument.Default_value == null)
        //                 {
        //                     new Error($"Missing value for argument: {arg}");
        //                     out_value = null;
        //                 }

        //                 else
        //                     out_value = matching_argument.Default_value;
        //             }

        //             else
        //                 out_value = args[Array.IndexOf(args, arg) + 1];

        //             parsed_args.Add(new ParsedArgument(names: matching_argument.Names, value: out_value, is_flag: matching_argument.Is_flag));
        //             i++;
        //         }
        //     }

        //     else
        //         parsed_args.Add(new ParsedArgument(new string[]{arg}));
        // }

        // List<string> missing_arg_list = arguments.Where(
        //     argument => argument.Required
        //     &&
        //     !parsed_args.Any(arg => arg.Names.SequenceEqual(argument.Names))).Select(argument => argument.Names[0]
        // ).ToList();

        // if (missing_arg_list.Count > 0)
        // {
        //     Error.TooFewArgs("");
        //     new Error($"Missing required argument(s): {string.Join(", ", missing_arg_list)}");
        //     return new List<ParsedArgument>();
        // }

        return parsed_args;
    }
}
