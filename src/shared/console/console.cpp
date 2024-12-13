#include "aopch.h"
#include "console.h"

namespace console
{
    // get console color.
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
        return color::BLACK; // default to black if unable to retrieve
    }

    // set console color.
    void set_console_color(const color& fore)
    {
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(hOut, fore);
    }

    void set_console_color(const color& fore, const color& back)
    {
        HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        WORD color = (back << 4) | fore; // background color is shifted to the upper 4 bits
        SetConsoleTextAttribute(hOut, color);
    }

    // reset console color.
    color original_fore = get_console_fore_color();
    color original_back = get_console_back_color();
    void reset_console_color()
    {
        set_console_color(original_fore, original_back);
    }

    int get_console_window_width()
    {
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
        return csbi.srWindow.Right - csbi.srWindow.Left + 1;
    }

    int get_console_window_height()
    {
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
        return csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
    }

    void set_cursor_pos(const COORD& c)
    {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleCursorPosition(h, c);
    }

    // https://stackoverflow.com/a/35800317/18121288
    COORD get_cursor_pos()
    {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_SCREEN_BUFFER_INFO cbsi;

        if (GetConsoleScreenBufferInfo(h, &cbsi))
            return cbsi.dwCursorPosition;
        return {0, 0}; // else return this
    }

    // https://stackoverflow.com/a/41213165/18121288
    bool getconchar(KEY_EVENT_RECORD& krec)
    {
        DWORD cc;
        INPUT_RECORD irec;
        HANDLE h = GetStdHandle(STD_INPUT_HANDLE);

        // console not found
        if (h == NULL)
            return false;

        for (;;)
        {
            ReadConsoleInput(h, &irec, 1, &cc);
            if (irec.EventType == KEY_EVENT && ((KEY_EVENT_RECORD &)irec.Event).bKeyDown)
            {
                krec = (KEY_EVENT_RECORD &)irec.Event;
                return true;
            }
        }

        return false;
    }

    // https://stackoverflow.com/a/18703309/18121288
    // helper function to get the state of modifier keys
    DWORD get_modifier_state(KEY_EVENT_RECORD& krec)
    {
        DWORD state = 0;
        if (krec.dwControlKeyState & LEFT_CTRL_PRESSED)
            state = LEFT_CTRL_PRESSED;

        else if (krec.dwControlKeyState & RIGHT_CTRL_PRESSED)
            state = RIGHT_CTRL_PRESSED;

        else if (krec.dwControlKeyState & LEFT_ALT_PRESSED)
            state = LEFT_ALT_PRESSED;

        else if (krec.dwControlKeyState & RIGHT_ALT_PRESSED)
            state = RIGHT_ALT_PRESSED;

        else if (krec.dwControlKeyState & SHIFT_PRESSED)
            state = SHIFT_PRESSED;

        return state;
    }
}
