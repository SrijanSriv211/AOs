#include "aospch.h"
#include "console.h"

namespace console
{
    // Get console color.
    color get_console_fore_color()
    {
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(hOut, &csbi);

        WORD attributes = csbi.wAttributes;
        color foreground_color = static_cast<color>(attributes);
        return foreground_color;
    }

    // code from chatgpt
    color get_console_back_color()
    {
        const WORD BACKGROUND_MASK = 0xF0; // 0xF0 masks the upper 4 bits
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        if (GetConsoleScreenBufferInfo(hOut, &csbi))
        {
            WORD attributes = csbi.wAttributes;
            color background_color = static_cast<color>((attributes & BACKGROUND_MASK) >> 4);
            return background_color;
        }
        return color::BLACK; // Default to black if unable to retrieve
    }

    // Set console color.
    void set_console_color(const color& fore)
    {
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(hOut, fore);
    }

    void set_console_color(const color& fore, const color& back)
    {
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        WORD color = (back << 4) | fore; // Background color is shifted to the upper 4 bits
        SetConsoleTextAttribute(hOut, color);
    }

    // reset console color.
    color original_fore = get_console_fore_color();
    color original_back = get_console_back_color();
    void reset_console_color()
    {
        set_console_color(original_fore, original_back);
    }
}
