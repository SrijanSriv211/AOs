#include "aospch.h"
#include "filesystem.h"
#include "console/console.h"

namespace filesystem
{
    void create(const std::filesystem::path& filepath)
    {
        // Check if the file already exists
        if (std::filesystem::exists(filepath))
            return;

        // Create the folder
        try
        {
            std::ofstream file(filepath);
            file.close();
        }

        catch(const std::exception& e)
        {
            console::throw_error(e.what(), "C++ file system IO");
        }
    }

    void write(const std::filesystem::path& filepath, const std::string& content)
    {
        // Check if the file already exists
        filesystem::create(filepath);

        // Create the folder
        try
        {
            std::ofstream file(filepath);
            file << content;
            file.close();
        }

        catch(const std::exception& e)
        {
            console::throw_error(e.what(), "C++ file system IO");
        }
    }
}
