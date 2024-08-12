#include "aospch.h"

namespace AOs
{
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

    std::string about_AOs = "A command-line tool built to control your OS directly through the command-line";
    std::string AOs_repo_link = "https://github.com/SrijanSriv211/AOs";
}
