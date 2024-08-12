#include "aospch.h"
#include "entrypoint.h"
#include "argparse/argparse.h"
#include "console/console.h"
#include "fileio/foldersystem.h"
#include "core/lexer/lex.h"
#include "strings/strings.h"

void unrecognized_argument_error(const std::string& err)
{
    console::throw_error(err, "Unrecognized argument");
}

// initialize .si folder with it's subdirectories
// this folder will contain all the settings for AOs
void init_folders()
{
    foldersystem::create(".aos");
    foldersystem::create(".aos/files.x72");
    foldersystem::create(".aos/files.x72/root");
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

        else
        {
            unrecognized_argument_error(arg.names[0] + ": Please type 'aos --help' for more information");
            return 1;
        }
    }

    return 0;
}

std::string load_file(const std::string& filename)
{
    std::fstream file;
    std::string current_line;
    std::vector<std::string> code;

    file.open(filename);

    // if (!file)
    //     Pastel::errors::open_file(filename);

    while (std::getline(file, current_line))
        code.push_back(current_line);

    file.close();
    return strings::join("\n", code);
}

int take_entry(std::vector<std::string> args)
{
    argparse parser = argparse("AOs", "A developer tool made by a developer for developers", unrecognized_argument_error);
    parser.add({"-h", "--help", "/?", "-?"}, "Show help message", "", true, false);
    parser.add({"--init"}, "Create AOs dir in cwd.", "", true, false);

    std::vector<argparse::parsed_argument> parsed_args = parser.parse(args);

    if (parsed_args.size() > 0) return exec_parsed_args(parser, parsed_args);
    else
    {
        //*NOTE: CHANGE IT LATER. BY DEFAULT AOS WILL OPEN IN CLI MODE.
        lex lexer(load_file("test.si"));
    }

    return 0;
}
