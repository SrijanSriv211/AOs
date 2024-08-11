#include "aospch.h"
#include "console.h"

// https://stackoverflow.com/a/4053879/18121288
void console::print(const std::string& message, const WORD& color, const bool& is_newline)
{
    WORD default_color = console::get_console_color();
    console::set_console_color(color);

    std::cout << message;

    if (is_newline)
        std::cout << std::endl;

    console::set_console_color(default_color);
}

void console::throw_error(const std::string& details, const std::string& name_of_error)
{
    console::print(name_of_error + " error:\n", 12, false);
    console::print(details, console::get_console_color());
}

// Get console color.
WORD console::get_console_color()
{
    HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    CONSOLE_SCREEN_BUFFER_INFO csbi;
    GetConsoleScreenBufferInfo(hOut, &csbi);

    return csbi.wAttributes;
}

// Reset console color.
void console::reset_console_color()
{
    set_console_color(7);
}

// Set console color.
void console::set_console_color(WORD color)
{
    HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    SetConsoleTextAttribute(hOut, color);
}
