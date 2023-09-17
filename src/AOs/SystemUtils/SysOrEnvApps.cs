using System.Diagnostics;

partial class SystemUtils
{
    public bool RunSysOrEnvApps(string input_cmd, string[] input_args)
    {
        if (File.Exists(input_cmd))
        {
            List<string> args_to_be_passed = new()
            {
                $"\"{input_cmd}\"",
                " "
            };

            args_to_be_passed.AddRange(input_args);

            foreach (var item in args_to_be_passed)
                Console.WriteLine($"[{item}]");

            if (input_cmd.EndsWith(".aos"))
            {
                string AOsBinaryFilepath = Process.GetCurrentProcess().MainModule.FileName;
                CommandPrompt(AOsBinaryFilepath, args_to_be_passed.ToArray());

                return true;
            }

            else
            {
                string[] file_exts = { ".exe", ".msi", ".bat", ".cmd" };
                if (file_exts.Any(input_cmd.EndsWith))
                    CommandPrompt(input_cmd, input_args);

                else
                    CommandPrompt($"start {string.Join("", args_to_be_passed)}");
                    // CommandPrompt($"start {input_cmd} {input_args}");

                return true;
            }
        }

        //? Maybe used in future but not for now.
        // else
        // {
        //     int return_val = CommandPrompt($"{input_cmd} {string.Join("", input_args)}", true);
        //     return return_val == 0;
        // }

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
        List<string> folder_paths = Environment.GetEnvironmentVariable("path")?.Split(';').ToList();
        folder_paths.Add(Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\PowerToys"));

        foreach (string folder in folder_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        return "";
    }
}
