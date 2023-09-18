partial class Parser
{
    public void Execute(ParsedCommand parsed_cmd)
    {
        if (parsed_cmd.Cmd_name == null || parsed_cmd.Method == null)
            return;

        if (parsed_cmd.Is_flag)
        {
            var action = parsed_cmd.Method as Action; // Cast the stored delegate to Action
            action?.Invoke(); // Invoke the delegate with no arguments
        }

        else if (parsed_cmd.Values.Length > 1 || parsed_cmd.Min_args_length > 1 || parsed_cmd.Max_args_length > 1)
        {
            var action = parsed_cmd.Method as Action<string[]>; // Cast the stored delegate to Action<string[]>
            action?.Invoke(parsed_cmd.Values); // Invoke the delegate with the provided arguments (i.args)
        }

        else if (parsed_cmd.Values.Length == 1 && (parsed_cmd.Min_args_length == 1 || parsed_cmd.Max_args_length == 1))
        {
            var action = parsed_cmd.Method as Action<string>; // Cast the stored delegate to Action<string>
            action?.Invoke(parsed_cmd.Values.First()); // Invoke the delegate with the provided arguments (i.args[0])
        }
    }
}
