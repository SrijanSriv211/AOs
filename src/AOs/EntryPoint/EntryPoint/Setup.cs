partial class EntryPoint
{
    private void CheckForSetup()
    {
        string InitBootConfigPath = Path.Combine(Obsidian.root_dir, "initbootconfig");
        if (File.Exists(InitBootConfigPath))
            return;

        FileIO.FileSystem.Write(InitBootConfigPath, "1");
        Setup();
    }

    private void Setup()
    {
        SystemUtils.CommandPrompt("cls");

        // https://onlinetools.com/ascii/convert-text-to-ascii-art#tool
        string[] HelloText = [
            " _    _      _ _       _ ",
            "| |  | |    | | |     | |",
            "| |__| | ___| | | ___ | |",
            "|  __  |/ _ \\ | |/ _ \\| |",
            "| |  | |  __/ | | (_) |_|",
            "|_|  |_|\\___|_|_|\\___/(_)",
            "                          "
        ];

        string[] WelcomeToAOs = [
            " __      __   _                    _           _   ___       ",
            " \\ \\    / /__| |__ ___ _ __  ___  | |_ ___    /_\\ / _ \\ ___  ",
            "  \\ \\/\\/ / -_) / _/ _ \\ '  \\/ -_) |  _/ _ \\  / _ \\ (_) (_-<_ ",
            "   \\_/\\_/\\___|_\\__\\___/_|_|_\\___|  \\__\\___/ /_/ \\_\\___//__(_)",
            "                                                           "
        ];

        Terminal.Print(string.Join("\n", HelloText), ConsoleColor.White);
        Terminal.Print(string.Join("\n", WelcomeToAOs), ConsoleColor.Yellow);

        Pause("Press any key to continue.");

        Terminal.Print("Type ", ConsoleColor.Gray, false);
        Terminal.Print("`help <command-name>` ", ConsoleColor.White, false);
        Terminal.Print("for more information on a specific command", ConsoleColor.Gray);

        Pause("Press any key to continue.");

        Terminal.Print("The ", ConsoleColor.Gray, false);
        Terminal.Print("help ", ConsoleColor.White, false);
        Terminal.Print("command will give you information about any command that is indexed by AOs.", ConsoleColor.Gray);
        Terminal.Print("So, to know which commands are indexed and which are not and to change that setting you can open ", ConsoleColor.Gray, false);
        Terminal.Print("`Files.x72\\root\\settings.json`.", ConsoleColor.White);
        Terminal.Print("And furthermore you can visit ", ConsoleColor.Gray, false);
        Terminal.Print("`https://github.com/SrijanSriv211/AOs` ", ConsoleColor.White, false);
        Terminal.Print("to get more information about AOs ", ConsoleColor.Gray, false);
        Terminal.Print("`settings.json`.", ConsoleColor.White);

        Pause("Press any key to continue.");

        string[] AOsShortcutKeysHeading = [
            "| Shortcut              | Comment                         |",
            "| --------------------- | --------------------------------|"
        ];

        string[] AOsShortcutKeys = [
            "| `End`                 | Send end of line                |",
            "| `Tab`                 | Change autocomplete suggestions |",
            "| `Home`                | Send start of line              |",
            "| `Escape`              | Clear suggestions               |",
            "| `Delete`              | Delete succeeding character     |",
            "| `Backspace`           | Delete previous character       |",
            "| `LeftArrow`           | Backward one character          |",
            "| `RightArrow`          | Forward one character           |",
            "| `Shift`+`Escape`      | Clear input and suggestions     |",
            "| `Ctrl`+`Enter`        | Accept current suggestion       |",
            "| `Ctrl`+`Spacebar`     | Show current suggestions        |",
            "| `Ctrl`+`Delete`       | Delete succeeding token         |",
            "| `Ctrl`+`Backspace`    | Delete previous token           |",
            "| `Ctrl`+`LeftArrow`    | Backward one token              |",
            "| `Ctrl`+`RightArrow`   | Forward one token               |"
        ];

        Terminal.Print("All the supported shortcut keys are the following:\n", ConsoleColor.Gray);
        Terminal.Print(string.Join("\n", AOsShortcutKeysHeading), ConsoleColor.White);
        Terminal.Print(string.Join("\n", AOsShortcutKeys), ConsoleColor.Gray);

        Pause("\nNow you know the basics of how to use AOs.\nPress any key to continue.");

        string[] ThankYou = [
            "  _____ _              _    __   __          ",
            " |_   _| |_  __ _ _ _ | |__ \\ \\ / /__ _  _   ",
            "   | | | ' \\/ _` | ' \\| / /  \\ V / _ \\ || |_ ",
            "   |_| |_||_\\__,_|_||_|_\\_\\   |_|\\___/\\_,_(_)",
            "                                             "
        ];

        Terminal.Print(string.Join("\n", ThankYou), ConsoleColor.Green);
        Console.ReadKey();
    }

    private void Pause(string message)
    {
        Console.Write(message);
        Console.ReadKey();
        Console.Write("\r");
        Console.Write(new string(' ', message.Length));
        Console.WriteLine("\n");
    }
}
