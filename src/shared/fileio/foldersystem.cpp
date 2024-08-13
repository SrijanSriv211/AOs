#include "aospch.h"
#include "foldersystem.h"
#include "console/console.h"

namespace foldersystem
{
    void create(const std::filesystem::path& folderpath)
    {
        // Check if the folder already exists
        if (std::filesystem::exists(folderpath))
            return;

        // Create the folder
        try
        {
            if (!std::filesystem::create_directory(folderpath))
                console::throw_error("Failed to create folder.", "Folder system IO");
        }

        catch(const std::exception& e)
        {
            console::throw_error(e.what(), "C++ folder system IO");
        }
    }
}
