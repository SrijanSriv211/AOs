#include "aospch.h"
#include "entrypoint.h"

#include "argparse/argparse.h"
#include "console/console.h"

std::vector<std::string> load_file(const std::string& filename)
{
    std::fstream file;
    std::string current_line;
    std::vector<std::string> code;

    file.open(filename);

    if (!file)
        console::throw_error(filename + ": No such file or directory.", "File system io");

    while (std::getline(file, current_line))
        code.push_back(current_line);

    file.close();
    return code;
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

        else if (arg.names[0].ends_with(".aos"))
        {
            std::vector<std::string> code = load_file(arg.names[0]);

            for (int i = 0; i < code.size(); i++)
            {
                std::string* line = new std::string(code[i]);
                exec_code(line);
                delete line;
            }
        }

        else
        {
            unrecognized_argument_error(arg.names[0] + ": Please type 'aos --help' for more information");
            return 1;
        }
    }

    return 0;
}
