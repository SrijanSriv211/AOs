partial class SystemUtils
{
    public bool RunSysOrEnvApps(string input_cmd, string[] input_args)
    {
        if (File.Exists(input_cmd))
        {
            List<string> args_to_be_passed = [
                $"\"{input_cmd}\"",
                " ",
                .. input_args,
            ];

            if (input_cmd.EndsWith(".aos"))
            {
                CommandPrompt(Obsidian.AOs_binary_path, args_to_be_passed.ToArray());
                return true;
            }

            else
            {
                string[] file_exts = [".exe", ".msi", ".bat", ".cmd"];
                if (file_exts.Any(input_cmd.EndsWith))
                    CommandPrompt(input_cmd, input_args);

                else
                    CommandPrompt($"start {string.Join("", args_to_be_passed)}");

                return true;
            }
        }

        return false;
    }

    public static string CheckForSysOrEnvApps(string input)
    {
        if (input.StartsWith("%") && input.EndsWith("%") && input.Length > 1 && !Utils.String.IsEmpty(Environment.GetEnvironmentVariable(input.Substring(1, input.Length-2).ToLower())))
            return Environment.GetEnvironmentVariable(input.Substring(1, input.Length-2).ToLower());

        else
        {
            string[] file_exts = { "", ".aos", ".exe", ".msi", ".bat", ".cmd" }; // "" empty string at the start of this array means that an extention as already passed.
            foreach (string ext in file_exts)
            {
                string exec_name = LocateExecutable((input + ext).ToLower());;
                if (!Utils.String.IsEmpty(exec_name))
                    return exec_name;
            }
        }

        return input;
    }

    public static string LocateExecutable(string Filename)
    {
        List<string> folder_paths = new() {
            Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\PowerToys")
        };

        folder_paths.AddRange(Environment.GetEnvironmentVariable("path")?.Split(';').ToList());

        foreach (string folder in folder_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        return "";
    }
}
