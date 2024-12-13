#include "aopch.h"
#include "filesystem.h"
#include "console/console.h"

namespace filesystem
{
    void create(const std::filesystem::path& filepath)
    {
        // check if the file already exists
        if (std::filesystem::exists(filepath))
            return;

        // create the folder
        try
        {
            std::ofstream file(filepath);
            file.close();
        }

        catch(const std::exception& e)
        {
            console::errors::throw_error(e.what(), "C++ file system IO");
        }
    }

    void write(const std::filesystem::path& filepath, const std::string& content)
    {
        // check if the file already exists
        filesystem::create(filepath);

        // create the folder
        try
        {
            std::ofstream file(filepath);
            file << content;
            file.close();
        }

        catch(const std::exception& e)
        {
            console::errors::throw_error(e.what(), "C++ file system IO");
        }
    }
}
