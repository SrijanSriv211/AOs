#pragma once

class console
{
public:
    static WORD get_console_color();
    static void reset_console_color();
    static void set_console_color(WORD color);
    static void print(const std::string& message, const WORD& color, const bool& is_newline=true);
    static void throw_error(const std::string& details, const std::string& name_of_error);
};
