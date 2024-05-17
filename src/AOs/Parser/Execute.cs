partial class Parser
{
    public void Execute(ParsedCommand parsed_cmd)
    {
        if (parsed_cmd.Cmd_name == null)
            return;

        if (parsed_cmd.Method == null && !Utils.String.IsEmpty(parsed_cmd.Location))
        {
            ExecuteExternal(parsed_cmd);
            return;
        }

        if (parsed_cmd.Is_flag)
        {
            var action = parsed_cmd.Method as Action; // Cast the stored delegate to Action
            action?.Invoke(); // Invoke the delegate with no arguments
        }

        else if (parsed_cmd.Max_args_length == 0 || parsed_cmd.Max_args_length > 1)
        {
            var action = parsed_cmd.Method as Action<string[]>; // Cast the stored delegate to Action<string[]>
            action?.Invoke(parsed_cmd.Values); // Invoke the delegate with the provided arguments (i.args)
        }

        else
        {
            var action = parsed_cmd.Method as Action<string>; // Cast the stored delegate to Action<string>
            action?.Invoke(parsed_cmd.Values.First()); // Invoke the delegate with the provided arguments (i.args[0])
        }
    }

    private void ExecuteExternal(ParsedCommand parsed_cmd)
    {
        if (parsed_cmd.Is_flag)
            SystemUtils.RunSysOrEnvApps(parsed_cmd.Location, []);

        else if (parsed_cmd.Max_args_length == 0 || parsed_cmd.Max_args_length > 1)
            SystemUtils.RunSysOrEnvApps(parsed_cmd.Location, parsed_cmd.Values);

        else
            SystemUtils.RunSysOrEnvApps(parsed_cmd.Location, [parsed_cmd.Values.First()]);
    }
}
