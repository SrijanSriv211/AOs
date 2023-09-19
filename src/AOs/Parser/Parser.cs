partial class Parser
{
    private readonly List<Command> command_details = new();
    private readonly Action<string, string[]> error_function;

    public Parser(Action<string, string[]> error_function)
    {
        this.error_function = error_function;
    }

    public void Add(string[] cmd_names, string help_message, Dictionary<string[], string> supported_args=null, string[] default_values=null, bool is_flag=true, int min_args_length=0, int max_args_length=0, Delegate method=null)
    {
        if (supported_args != null || default_values != null || max_args_length > 0 || min_args_length > 0)
            is_flag = false;

        command_details.Add(new Command(cmd_names, help_message, supported_args, default_values, is_flag, min_args_length, max_args_length, method));
    }

    public ParsedCommand Parse(string cmd_name, string[] args)
    {
        string lowercase_cmd = cmd_name.ToLower();

        ParsedCommand parsed_cmd = new();
        Command matching_cmd = FindMatchingCommand(lowercase_cmd);

        // Return if no matching command was found.
        if (matching_cmd.Cmd_names == null)
        {
            error_function(cmd_name, args);
            return new ParsedCommand();
        }

        if (matching_cmd.Is_flag)
        {
            if (args.Length > 0)
            {
                Error.UnrecognizedArgs(args);
                return new ParsedCommand();
            }

            parsed_cmd.Values = new string[]{"true"};
        }

        else
        {
            if (Utils.Array.IsEmpty(args))
            {
                if (matching_cmd.Default_values == null)
                {
                    Error.NoArgs(cmd_name);
                    return new ParsedCommand();
                }

                else
                    parsed_cmd.Values = matching_cmd.Default_values;
            }

            else
                parsed_cmd.Values = args;

            if (matching_cmd.Min_args_length > 0 && Utils.Array.Reduce(parsed_cmd.Values).Length < matching_cmd.Min_args_length)
            {
                Error.TooFewArgs(Utils.Array.Reduce(parsed_cmd.Values));
                return new ParsedCommand();
            }

            else if (matching_cmd.Max_args_length > 0 && Utils.Array.Reduce(parsed_cmd.Values).Length > matching_cmd.Max_args_length)
            {
                Error.TooManyArgs(Utils.Array.Reduce(parsed_cmd.Values));
                return new ParsedCommand();
            }
        }

        parsed_cmd.Cmd_name = cmd_name;
        parsed_cmd.Is_flag = matching_cmd.Is_flag;
        parsed_cmd.Min_args_length = matching_cmd.Min_args_length;
        parsed_cmd.Max_args_length = matching_cmd.Max_args_length;
        parsed_cmd.Method = matching_cmd.Method;

        return parsed_cmd;
    }
}
