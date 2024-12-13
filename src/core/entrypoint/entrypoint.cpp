#include "aopch.h"
#include "entrypoint.h"
#include "ao.h"

#include "argparse/argparse.h"
#include "fileio/foldersystem.h"
#include "fileio/filesystem.h"
#include "console/console.h"

#include "core/readf/readf.h"
#include "core/execute/execute.h"
#include "core/settings/settings.h"

bool print_new_line = true;

int take_entry(const std::vector<std::string> args)
{
    // parse arguments passed to AO
    argparse parser = argparse("AO", "A developer tool made by a developer for developers", unrecognized_argument_error);
    parser.add({"-h", "--help", "/?", "-?"}, "Show help message", "", true, false);
    parser.add({"-i", "--init"}, "Initialize in current directory", "", true, false);
    parser.add({"-a", "--api"}, "Run in server mode and accept API requests", "", true, false);

    std::vector<argparse::parsed_argument> parsed_args = parser.parse(args);
    std::vector<std::string> history = {};
    int is_running = 0; // 0 = false; 1 = true; 2 = reload

    UINT code_page = GetConsoleOutputCP();

    // change code page to UTF-8
    if (code_page != 65001)
    {
        // https://stackoverflow.com/a/388500/18121288
        // https://stackoverflow.com/questions/45575863/how-to-print-utf-8-strings-to-stdcout-on-windows
        SetConsoleOutputCP(65001);
    }

    if (parsed_args.size() > 0)
        return exec_parsed_args(parser, parsed_args);

    else
    {
        setup(); // show a setup screen with some basic details on first boot
        AO::clear_console();
        run_ao_scripts(settings::load()["startlist"]);
        is_running = 1;

        while (is_running == 1)
        {
            AO::print_prompt();
            console::readf readf = console::readf(settings::get_all_suggestions());

            readf.history_list = history;
            std::vector<lex::token> input_tokens = readf.takeinput();
            history = readf.history_list;

            print_new_line = true;
            is_running = execute_tokens(input_tokens);

            if (is_running == 1 && print_new_line)
                std::cout << std::endl;
        }
    }

    SetConsoleCP(code_page); // reset code page to the original one.
    return is_running;
}

void unrecognized_argument_error(const std::string& err)
{
    console::errors::throw_error(err, "Unrecognized argument");
}

// initialize .ao folder with it's subdirectories
// this folder will contain all the settings for AO
void init_folders()
{
    foldersystem::create(".ao");
    foldersystem::create(".ao\\etc");
    filesystem::write(".ao\\settings.json", settings::format);
}
