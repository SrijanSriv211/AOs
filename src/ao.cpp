#include "aopch.h"
#include "ao.h"

#include "core/entrypoint/entrypoint.h"

#include "console/console.h"
#include "datetime/datetime.h"

// https://stackoverflow.com/a/5459929/18121288
// only the first 2 macros for useful for me
#define STR_HELPER(x) #x
#define STR(x) STR_HELPER(x)

namespace AO
{
    const std::string about_AO = "A command-line tool built to control your OS directly through the command-line";
    const std::string AO_repo_link = "https://github.com/SrijanSriv211/AO";
    std::string ao_env_path = std::filesystem::current_path().string();

    // https://stackoverflow.com/q/50889647/18121288
    std::string get_root_path()
    {
        char buffer[MAX_PATH];
        // in the stackoverflow thread, the code snippet contained `GetModuleFileName` change it to `GetModuleFileNameA`.
        // `GetModuleFileName` is a macro that resolves to either `GetModuleFileNameA` or `GetModuleFileNameW` based on
        // the project's character set setting. `GetModuleFileNameA` is the ANSI version that works with char strings.
        //*NOTE: explanation by chatgpt
        GetModuleFileNameA(NULL, buffer, MAX_PATH);
        std::string::size_type pos = std::string(buffer).find_last_of("\\/");
        return (pos == std::string::npos) ? "" : std::string(buffer).substr(0, pos);
    }

    void clear_console()
    {
        system("cls");
        print_new_line = false;
        std::string date_v = STR(VERSION);
        std::string semantic_v = STR(STD);
        console::print("AO " + date_v + " [Version " + semantic_v + "]  ", console::color::LIGHT_YELLOW, false);
        console::print("(" + std::string(std::getenv("username")) + ")  ", console::color::LIGHT_WHITE, false);
        console::print(datetime::datetime("%a, %d %b %Y"), console::color::GRAY);

        // ask user to use AO in Windows Terminal if they are not already
        const char* env_var_val = std::getenv("WT_SESSION");
        if (env_var_val == nullptr || env_var_val[0] == '\0')
        {
            console::print("> ", console::color::GRAY, false);
            console::print("Please use AO in Windows Terminal for better experience.", console::color::RED);
        }
    }

    void print_prompt()
    {
        console::print(std::filesystem::current_path().string(), console::color::LIGHT_WHITE);
        console::print(datetime::datetime("%H:%M:%S"), console::color::LIGHT_WHITE);
        console::print("$ ", console::color::LIGHT_WHITE, false);
    }
}
