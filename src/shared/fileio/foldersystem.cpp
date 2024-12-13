#include "aopch.h"
#include "foldersystem.h"
#include "console/console.h"

namespace foldersystem
{
    void create(const std::filesystem::path& folderpath)
    {
        // check if the folder already exists
        if (std::filesystem::exists(folderpath))
            return;

        // create the folder
        try
        {
            if (!std::filesystem::create_directory(folderpath))
                console::errors::throw_error("Failed to create folder.", "Folder system IO");
        }

        catch(const std::exception& e)
        {
            console::errors::throw_error(e.what(), "C++ folder system IO");
        }
    }
}
