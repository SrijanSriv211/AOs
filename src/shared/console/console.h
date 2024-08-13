#pragma once

namespace console
{
    enum color {
        BLACK = 0,
        BLUE = 1,
        GREEN = 2,
        CYAN = 3,
        RED = 4,
        MAGENTA = 5,
        YELLOW = 6,
        WHITE = 7,
        GRAY = 8,
        LIGHT_BLUE = 9,
        LIGHT_GREEN = 10,
        LIGHT_CYAN = 11,
        LIGHT_RED = 12,
        LIGHT_MAGENTA = 13,
        LIGHT_YELLOW = 14,
        LIGHT_WHITE = 15
    };

    color get_console_fore_color();
    color get_console_back_color();
    void reset_console_color();
    void set_console_color(const color& fore);
    void set_console_color(const color& fore, const color& back);
    void print(const std::string& message, const console::color& fore, const bool& endl=true);
    void print(const std::string& message, const console::color& fore, const console::color& back, const bool& endl=true);
    void throw_error(const std::string& details, const std::string& name_of_error);

    class readf
    {
    public:
        readf();
        std::string takeinput();
    };
};
