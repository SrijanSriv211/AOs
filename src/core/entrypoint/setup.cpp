#include "aospch.h"
#include "entrypoint.h"
#include "aos.h"
#include "fileio/filesystem.h"
#include "console/console.h"
#include "strings/strings.h"

void pause(const std::string& msg)
{
    std::cout << msg;
    std::cin.get();
}

void setup()
{
    std::string initbootfile = AOs::get_root_path() + "\\initboot";
    if (std::filesystem::exists(initbootfile))
        return;

    filesystem::write(initbootfile, "1");
    system("cls");

    // https://onlinetools.com/ascii/convert-text-to-ascii-art#tool
    std::vector<std::string> HelloText = {
        " _    _      _ _       _ ",
        "| |  | |    | | |     | |",
        "| |__| | ___| | | ___ | |",
        "|  __  |/ _ \\ | |/ _ \\| |",
        "| |  | |  __/ | | (_) |_|",
        "|_|  |_|\\___|_|_|\\___/(_)",
        "                          "
    };

    std::vector<std::string> WelcomeToAOs = {
        " __      __   _                    _           _   ___       ",
        " \\ \\    / /__| |__ ___ _ __  ___  | |_ ___    /_\\ / _ \\ ___  ",
        "  \\ \\/\\/ / -_) / _/ _ \\ '  \\/ -_) |  _/ _ \\  / _ \\ (_) (_-<_ ",
        "   \\_/\\_/\\___|_\\__\\___/_|_|_\\___|  \\__\\___/ /_/ \\_\\___//__(_)",
        "                                                           "
    };

    console::print(strings::join("\n", HelloText), console::color::LIGHT_WHITE);
    console::print(strings::join("\n", WelcomeToAOs), console::color::LIGHT_YELLOW);

    pause("Press any key to continue.");

    console::print("The ", console::color::GRAY, false);
    console::print("help ", console::color::LIGHT_WHITE, false);
    console::print("command will give you information about any command that is indexed by AOs.", console::color::GRAY);
    console::print("So, to know which commands are indexed and which are not and to change that setting you can open ", console::color::GRAY, false);
    console::print("`.aos/root/settings.json`.", console::color::LIGHT_WHITE);
    console::print("And furthermore you can visit ", console::color::GRAY, false);
    console::print("`https://github.com/SrijanSriv211/AOs` ", console::color::LIGHT_WHITE, false);
    console::print("to get more information about AOs ", console::color::GRAY, false);
    console::print("`settings.json`.", console::color::LIGHT_WHITE);

    pause("\nNow you know the basics of how to use AOs.\nPress any key to continue.");

    std::vector<std::string> ThankYou = {
        "  _____ _              _    __   __          ",
        " |_   _| |_  __ _ _ _ | |__ \\ \\ / /__ _  _   ",
        "   | | | ' \\/ _` | ' \\| / /  \\ V / _ \\ || |_ ",
        "   |_| |_||_\\__,_|_||_|_\\_\\   |_|\\___/\\_,_(_)",
        "                                             "
    };

    console::print(strings::join("\n", ThankYou), console::color::LIGHT_GREEN);
    pause("");
}
