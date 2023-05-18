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
            if (Password != FileIO.FileSystem.ReadAllText(Path))
                new Error("Incorrect password.");

            else
                break;
        }
    }
}
