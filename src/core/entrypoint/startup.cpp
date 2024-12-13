#include "aopch.h"
#include "entrypoint.h"
#include "ao.h"

#include "argparse/argparse.h"
#include "console/console.h"
#include "strings/strings.h"
#include "array/array.h"

#include "core/lexer/lex.h"
#include "core/server/server.h"
#include "core/execute/execute.h"

std::vector<std::string> load_file(const std::string& filename)
{
    if (strings::is_empty(filename))
        return {""};

    std::fstream file;
    std::string current_line;
    std::vector<std::string> code;

    file.open(filename);

    if (!file)
        console::errors::throw_error(filename + ": No such file or directory.", "File system io");

    while (std::getline(file, current_line))
        code.push_back(current_line);

    file.close();
    return code;
}

void run_ao_script(const std::string& filename)
{
    std::vector<std::string> code = load_file(filename);

    for (std::vector<std::string>::size_type i = 0; i < code.size(); i++)
    {
        lex lexer(code[i]);
        execute_tokens(lexer.tokens);
    }
}

void run_ao_scripts(const std::vector<std::string>& filenames)
{
    for (const std::string& filename : array::reduce(filenames))
    {
        console::print("> ", console::color::GRAY, false);
        console::print(filename, console::color::LIGHT_WHITE);
        run_ao_script(filename);
    }
}

int exec_parsed_args(argparse& parser, const std::vector<argparse::parsed_argument>& parsed_args)
{
    for (const auto& arg : parsed_args)
    {
        if (std::find(arg.names.begin(), arg.names.end(), "-h") != arg.names.end())
        {
            parser.print_help();
            return 0;
        }

        else if (std::find(arg.names.begin(), arg.names.end(), "--init") != arg.names.end())
            init_folders();

        else if (std::find(arg.names.begin(), arg.names.end(), "--api") != arg.names.end())
        {
            setup(); // show a setup screen with some basic details on first boot
            AO::clear_console();
            return start_server("127.0.0.1", 8000);
            // curl -X POST http://127.0.0.1:8000 -d "shout Hello world!"
        }

        else if (arg.names.front().ends_with(".ao"))
            run_ao_script(arg.names.front());

        else
        {
            unrecognized_argument_error(arg.names.front() + ": Please type 'ao --help' for more information");
            return 1;
        }
    }

    return 0;
}
