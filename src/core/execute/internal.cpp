#include "aopch.h"
#include "execute.h"
#include "ao.h"

#include "core/entrypoint/entrypoint.h"
#include "console/console.h"
#include "strings/strings.h"
#include "array/array.h"

// https://stackoverflow.com/a/5459929/18121288
// only the first 2 macros for useful for me
#define STR_HELPER(x) #x
#define STR(x) STR_HELPER(x)

void AOs1000()
{
    console::print("AOs1000!", console::color::LIGHT_WHITE);
    console::print("CONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!", console::color::LIGHT_WHITE);
    console::print("It was my first ever program to reach these many LINES OF CODE!", console::color::LIGHT_WHITE);
}

void BadApple()
{
    system("start https://youtu.be/2Ni13dnAbSA");
}

void TommyViceCity()
{
    system("cls");
    console::print("Lucia... ", console::color::LIGHT_MAGENTA, false);
    console::print("  (Do you know why you're here?)  ", console::color::LIGHT_WHITE, false);
    console::print("Bad luck, I guess.", console::color::GRAY);
}

void Rockstar6()
{
    system("start https://youtu.be/QdBZY2fkU-0");
}

void itanimulli()
{
    console::print("spelling mistake kid. It's illuminati", console::color::LIGHT_WHITE);
}

void illuminati()
{
    system("start https://youtu.be/DCqty4-VjZc");
}

void wgpcgr()
{
    console::print("true", console::get_console_back_color(), true);
}

void mrstark()
{
    console::print("And IAmBatMan.", console::get_console_back_color(), true);
}

void itsmagic()
{
    system("start https://youtu.be/dQw4w9WgXcQ");
}

void diagxt()
{
    std::string date_v = STR(VERSION);
    std::string semantic_v = STR(STD);

    std::vector<std::string> details = {
        "NAME        : AO " + date_v,
        "VERSION     : " + semantic_v,
        "PROCESS     : AO.exe",
        "SYSTEM TYPE : x64",
        "",
        "AUTHOR           : SrijanSriv211 (Srijan Srivastava)",
        "REGISTERED OWNER : " + std::string(std::getenv("username")),
        "",
        "ROOT DIRECTORY     : " + AO::get_root_path(),
        "SETTINGS FILE      : " + AO::ao_env_path + "\\.ao\\settings.json",
        "",
        "GITHUB REPO     : " + AO::AO_repo_link,
        "SYSTEM LANGUAGE : en-in; English (India)"
    };

    console::print(strings::join("\n", details), console::color::LIGHT_WHITE);
}

void chdir(const std::vector<std::string>& path)
{
    std::filesystem::current_path(path.front());
}

void workspace(const std::vector<std::string>& workspace_path)
{
    if (array::is_empty(workspace_path))
        std::cout << AO::ao_env_path << std::endl;

    else
    {
        std::string name = workspace_path.front();
        if (strings::startswith_any(name, {"\"", "'", "`"}) && strings::endswith_any(name, {"\"", "'", "`"}))
            name = strings::trim(name, 1, 2);

        AO::ao_env_path = name;
    }
}

std::map<std::vector<std::string>, std::function<void()>> cmd_func_map = {
    {{"_AOs1000"}, AOs1000},
    {{"--setup"}, run_setup},
    {{"cls", "_clear", "_cls"}, AO::clear_console},
    {{"_BadApple"}, BadApple},
    {{"_tommyViceCity"}, TommyViceCity},
    {{"_R*6"}, Rockstar6},
    {{"_itanimulli"}, itanimulli},
    {{"_illuminati"}, illuminati},
    {{"_withgreatpowercomesgreatresponsibility", "-wgpcgr"}, wgpcgr},
    {{"_IAmIronMan", "-mrstark"}, mrstark},
    {{"_itsmagicitsmagic"}, itsmagic},
    {{"--diagxt", "-aod"}, diagxt},
    {{"_initAO", "--init", "-i"}, init_folders}
};

std::map<std::vector<std::string>, std::function<void(const std::vector<std::string>&)>> cmd_args_func_map = {
    {{"cd", "_chdir", "--chdir"}, chdir},
    {{"_cws", "--workspace"}, workspace}
};

std::function<void()> get_command_func(const std::string& cmd)
{
    for (const auto& [key, pair] : cmd_func_map)
    {
        if (strings::any(cmd, key, true))
            return pair;
    }

    return nullptr;
}

std::function<void(const std::vector<std::string>&)> get_cmd_args_func(const std::string& cmd)
{
    for (const auto& [key, pair] : cmd_args_func_map)
    {
        if (strings::any(cmd, key, true))
            return pair;
    }

    return nullptr;
}
