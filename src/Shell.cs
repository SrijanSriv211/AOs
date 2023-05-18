using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

class Shell
{
    public static void CommandPrompt(string GetProcess="")
    {
        ProcessStartInfo StartProcess = new ProcessStartInfo("cmd.exe", $"/c {GetProcess}");
        var Execute = Process.Start(StartProcess);
        Execute?.WaitForExit();
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
            if (Password != FileIO.FileSystem.Read(Path))
                new Error("Incorrect password.");

            else
                break;
        }
    }

    public static bool Scan()
    {
        List<string> Errors = new List<string>();
        string[] CheckFor = {
            $"{Obsidian.rootDir}\\Sysfail\\RECOVERY",
            $"{Obsidian.rootDir}\\Files.x72\\root\\Config.set",
            $"{Obsidian.rootDir}\\Sysfail\\rp",
            $"{Obsidian.rootDir}\\Sysfail\\rp\\safe.exe",
            $"{Obsidian.rootDir}\\Files.x72\\root\\ext\\ply.exe",
            $"{Obsidian.rootDir}\\Files.x72\\root\\ext\\wiki.exe",
            $"{Obsidian.rootDir}\\SoftwareDistribution\\UpdatePackages\\UPR.exe",
        };

        // Scan the system.
        for (int i = 0; i < CheckFor.Length; i++)
        {
            if ((Directory.Exists(CheckFor[i]) && !Directory.EnumerateFileSystemEntries(CheckFor[i], "*", SearchOption.AllDirectories).Any()) || (!Directory.Exists(CheckFor[i]) && !File.Exists(CheckFor[i])))
                Errors.Add(CheckFor[i]);
        }

        string[] MissingFiles = Errors.ToArray();

        // Check for corrupted files.
        if (!Collection.Array.IsEmpty(MissingFiles))
        {
            string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");

            FileIO.FileSystem.Write($"{Obsidian.rootDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, [{string.Join(", ", MissingFiles)}] file(s) were missing.\n");

            new Error($"{MissingFiles.Length} Errors were Found!\n{string.Join("\n", MissingFiles)}" + "\n" + "Your PC ran into a problem :(");
            Console.Write("Press any Key to Continue.");
            Console.ReadKey();
            Console.WriteLine();
            SYSRestore();

            Console.WriteLine("A restart is recommended.");
            return false;
        }

        return true;
    }

    public static void SYSRestore()
    {
        Console.WriteLine("Restoring.");
        Console.Write("Using 'Sysfail\\RECOVERY' to restore.");
        if (File.Exists($"{Obsidian.rootDir}\\Sysfail\\rp\\safe.exe") && Directory.Exists($"{Obsidian.rootDir}\\Sysfail\\RECOVERY"))
        {
            CommandPrompt($"call \"{Obsidian.rootDir}\\Sysfail\\rp\\safe.exe\" -r \"{Obsidian.rootDir}\"");
            Console.WriteLine("Restore successful.");
        }

        else
        {
            string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
            FileIO.FileSystem.Write($"{Obsidian.rootDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, RECOVERY DIRECTORY is missing or corrupted.\n");

            new Error("\n" + "Cannot restore this PC." + "\n" + "RECOVERY DIRECTORY is missing or corrupted.");
            Environment.Exit(0);
        }
    }
}
