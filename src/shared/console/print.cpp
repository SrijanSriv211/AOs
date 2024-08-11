#include "aospch.h"
#include "console.h"

namespace console
{
    void print(const std::string& message, const color& fore, const bool& endl)
    {
        color default_fore = get_console_fore_color();
        set_console_color(fore);

        std::cout << message;
        if (endl) std::cout << std::endl;

        set_console_color(default_fore);
    }

    // https://stackoverflow.com/a/4053879/18121288
    void print(const std::string& message, const color& fore, const color& back, const bool& endl)
    {
        color default_fore = get_console_fore_color();
        color default_back = get_console_back_color();
        set_console_color(fore, back);

        std::cout << message;
        if (endl) std::cout << std::endl;

        set_console_color(default_fore, default_back);
    }

    void throw_error(const std::string& details, const std::string& name_of_error)
    {
        print(name_of_error + " error:\n", color::LIGHT_WHITE, color::LIGHT_RED, false);
        print(details, get_console_fore_color());
    }
}
