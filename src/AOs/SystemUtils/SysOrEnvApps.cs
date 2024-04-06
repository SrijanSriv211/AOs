partial class SystemUtils
{
    public static bool RunSysOrEnvApps(string input_cmd, string[] input_args)
    {
        if (!File.Exists(input_cmd))
            return false;

        List<string> args_to_be_passed = [
            $"\"{input_cmd}\"",
            " ",
            .. input_args,
        ];

        if (input_cmd.EndsWith(".aos"))
        {
            CommandPrompt(Obsidian.AOs_binary_path, [.. args_to_be_passed]);
            return true;
        }

        string[] file_exts = [".exe", ".msi", ".bat", ".cmd"];
        if (file_exts.Any(input_cmd.EndsWith))
            CommandPrompt(input_cmd, input_args);

        else
            CommandPrompt($"start {string.Join("", args_to_be_passed)}");

        return true;
    }

    public static string CheckForEnvVarAndEXEs(string input)
    {
        // If the input is an environment variable then get it.
        // Remove the first and last char - which is '%' - from input string to get the actual value,
        // which we will be use to get the env var.
        string env_var = Environment.GetEnvironmentVariable(input[1..^1].ToLower());
        if (input.StartsWith("%") && input.EndsWith("%") && input.Length > 1 && !Utils.String.IsEmpty(env_var))
            return env_var;

        // If the input is not an env var, then check if it is an executable or not.
        // "" empty string at the start of this array means that an extention as already passed.
        string[] file_exts = ["", ".aos", ".exe", ".msi", ".bat", ".cmd"];
        foreach (string ext in file_exts)
        {
            string exec_name = FindExecutablePath((input + ext).ToLower());

            if (!Utils.String.IsEmpty(exec_name))
                return exec_name;
        }

        return input;
    }

    private static string FindExecutablePath(string Filename)
    {
        List<string> app_dir_paths = Environment.GetEnvironmentVariable("path")?.Split(';').ToList();

        foreach (string folder in app_dir_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        return "";
    }
}
