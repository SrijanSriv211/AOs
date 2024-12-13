#include "aopch.h"
#include "console.h"

namespace console
{
    void print(const std::string& message, const color& fore, const bool& endl)
    {
        color default_fore = get_console_fore_color();
        set_console_color(fore);

        std::cout << message;

        set_console_color(default_fore);
        if (endl) std::cout << std::endl;
    }

    // https://stackoverflow.com/a/4053879/18121288
    void print(const std::string& message, const color& fore, const color& back, const bool& endl)
    {
        color default_fore = get_console_fore_color();
        color default_back = get_console_back_color();
        set_console_color(fore, back);

        std::cout << message;

        set_console_color(default_fore, default_back);
        if (endl) std::cout << std::endl;
    }
}
