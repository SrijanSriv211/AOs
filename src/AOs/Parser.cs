class Parser
{
    private readonly Dictionary<string[], string> help_list = new();
    private readonly List<Command> command_details = new();
    private readonly Action<string> error_function;

    public Parser(Action<string> error_function)
    {
        this.error_function = error_function;
    }

    public void Add(string[] cmd_names, string help_message, string[] default_values=null, bool is_flag=true, int max_args_length=-1, Delegate method=null)
    {
        if (default_values != null)
            is_flag = false;

        help_list.Add(cmd_names, help_message);
        command_details.Add(new Command(cmd_names, help_message, default_values, is_flag, max_args_length, method));
    }

    public ParsedCommand Parse(string cmd_name, string[] args)
    {
        ParsedCommand parsed_cmd = new();
        Command matching_cmd = FindMatchingCommand(cmd_name);

        // Return if no matching command was found.
        if (matching_cmd.Cmd_names == null)
        {
            error_function(cmd_name);
            return new ParsedCommand();
        }

        if (matching_cmd.Is_flag)
        {
            if (args.Length > 0)
            {
                Error.UnrecognizedArgs(args);
                return new ParsedCommand();
            }

            parsed_cmd.Cmd_name = cmd_name;
            parsed_cmd.Values = new string[]{"true"};
            parsed_cmd.Is_flag = matching_cmd.Is_flag;
            parsed_cmd.Max_args_length = matching_cmd.Max_args_length;
            parsed_cmd.Method = matching_cmd.Method;
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

            if (matching_cmd.Max_args_length > 0)
            {
                if (Utils.Array.Reduce(parsed_cmd.Values).Length > matching_cmd.Max_args_length)
                {
                    Error.TooManyArgs(Utils.Array.Reduce(parsed_cmd.Values));
                    return new ParsedCommand();
                }

                else if (Utils.Array.Reduce(parsed_cmd.Values).Length < matching_cmd.Max_args_length)
                {
                    Error.TooFewArgs(Utils.Array.Reduce(parsed_cmd.Values));
                    return new ParsedCommand();
                }
            }

            parsed_cmd.Cmd_name = cmd_name;
            parsed_cmd.Is_flag = matching_cmd.Is_flag;
            parsed_cmd.Max_args_length = matching_cmd.Max_args_length;
            parsed_cmd.Method = matching_cmd.Method;
        }

        for (int i = 0; i < parsed_cmd.Values.Length; i++)
        {
            string value = parsed_cmd.Values[i];
            if (Utils.String.IsString(value))
                parsed_cmd.Values[i] = Utils.String.Strings(value);
        }

        return parsed_cmd;
    }

    public void GetHelp(string[] cmd_names)
    {
        cmd_names = Utils.Array.Reduce(cmd_names);

        if (Utils.Array.IsEmpty(cmd_names))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");

            int count = 1;
            foreach (var item in help_list)
            {
                string[] command_names = item.Key;
                string description = item.Value;

                Console.Write("{0}. {1,-25}", count, $"{string.Join(", ", command_names)}");
                new TerminalColor(description, ConsoleColor.DarkGray);
                count++;
            }
        }

        else
        {
            int count = 1;
            foreach (var name in cmd_names)
            {
                bool match_found = false;
                foreach (var item in help_list)
                {
                    string[] command_names = item.Key;
                    string description = item.Value;

                    if (command_names.Contains(name))
                    {
                        Console.Write("{0}. {1,-25}", count, $"{string.Join(", ", command_names)}");
                        new TerminalColor(description, ConsoleColor.DarkGray);

                        count++;
                        match_found = true;
                        break;
                    }

                }

                if (match_found == false)
                    new Error($"No information for command '{name}'");
            }
        }
    }

    private Command FindMatchingCommand(string arg)
    {
        foreach (Command cmd_name in command_details)
        {
            if (cmd_name.Cmd_names.Contains(arg))
                return cmd_name;
        }

        return new Command();
    }

    public struct ParsedCommand
    {
        public string Cmd_name { get; set; }
        public string[] Values { get; set; }
        public bool Is_flag { get; set; }
        public int Max_args_length { get; set; }
        public Delegate Method { get; set; }

        public ParsedCommand(string cmd_name, string[] values, bool is_flag, int max_args_length, Delegate method)
        {
            this.Cmd_name = cmd_name;
            this.Values = values;
            this.Is_flag = is_flag;
            this.Max_args_length = max_args_length;
            this.Method = method;
        }
    }

    private struct Command
    {
        public string[] Cmd_names { get; set; }
        public string Help_message { get; set; }
        public string[] Default_values { get; set; }
        public bool Is_flag { get; set; }
        public int Max_args_length { get; set; }
        public Delegate Method { get; set; }

        public Command(string[] cmd_names, string help_message, string[] default_values=null, bool is_flag=true, int max_args_length=-1, Delegate method=null)
        {
            this.Cmd_names = cmd_names;
            this.Help_message = help_message;
            this.Default_values = default_values;
            this.Is_flag = is_flag;
            this.Max_args_length = max_args_length;
            this.Method = method;
        }
    }
}
