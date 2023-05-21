using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

class Shell
{
    public static string LocateEXE(string Filename)
    {
        string[] path_folders = Environment.GetEnvironmentVariable("path")?.Split(';');
        foreach (string folder in path_folders)
        {
            string exec_path = Path.Combine(folder, Filename);
            if (File.Exists(exec_path))
                return exec_path;
        }

        return string.Empty;
    }

    public static bool SysEnvApps(string input_cmd, string[] input_args)
    {
        string[] extentions = {string.Empty, ".exe", ".msi", ".bat", ".cmd"};
        string lower_input = input_cmd.ToLower();
        if (File.Exists(lower_input))
        {
            Console.WriteLine(CommandPrompt($"\"{lower_input}\" {string.Join(" ", input_args)}"));
            return true;
        }

        else if (!Collection.String.IsEmpty(Environment.GetEnvironmentVariable(lower_input)))
        {
            if (Collection.Array.IsEmpty(input_args))
                Console.WriteLine(Environment.GetEnvironmentVariable(lower_input));

            else
                Error.UnrecognizedArgs(input_args);

            return true;
        }

        else
        {
            foreach (string ext in extentions)
            {
                string exe_name_with_ext = lower_input + ext;
                if (!Collection.String.IsEmpty(LocateEXE(exe_name_with_ext)))
                {
                    Console.WriteLine(CommandPrompt($"\"{exe_name_with_ext}\" {string.Join(" ", input_args)}"));
                    return true;
                }
            }

            // If no exe is located then try running a cmd command.
            string output = CommandPrompt($"\"{lower_input}\" {string.Join(" ", input_args)}", Obsidian.default_else_shell);
            if (Collection.String.IsEmpty(output))
                return false;

            Console.WriteLine(output);
            return true;
        }
    }

    public static string CommandPrompt(string args, string shell="cmd.exe")
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = shell;
            process.StartInfo.Arguments = "/C " + args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            process.WaitForExit();
            return process.StandardOutput.ReadToEnd().Trim();
        }
    }

    public static void RootPackages()
    {
        string[] DirectoryList = new string[]
        {
            "Files.x72\\etc",
            "Files.x72\\root\\tmp",
            "Files.x72\\root\\StartUp",
            "SoftwareDistribution\\RestorePoint"
        };

        string[] FileList = new string[]
        {
            "Files.x72\\root\\.history",
            "Files.x72\\root\\tmp\\BOOT.log",
            "Files.x72\\root\\StartUp\\.startlist",
            "Files.x72\\root\\tmp\\Crashreport.log"
        };

        foreach (string path in DirectoryList)
            FileIO.FolderSystem.Create(Path.Combine(Obsidian.rootDir, path));

        foreach (string path in FileList)
            FileIO.FileSystem.Create(Path.Combine(Obsidian.rootDir, path));
    }

    public static void AskPass()
    {
        string Path = $"{Obsidian.rootDir}\\Files.x72\\root\\User.set";
        if (!File.Exists(Path))
            return;

        while (true)
        {
            Console.Write("Enter password: ");
            string Password = Console.ReadLine();
            if (Password != FileIO.FileSystem.ReadAllText(Path))
                new Error("Incorrect password.");

            else
                break;
        }
    }
}
