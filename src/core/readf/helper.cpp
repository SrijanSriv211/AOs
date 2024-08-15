#include "aospch.h"
#include "readf.h"

namespace console
{
    void readf::set_cursor_pos(const COORD& c)
    {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleCursorPosition(h, c);
    }

    // https://stackoverflow.com/a/35800317/18121288
    COORD readf::get_cursor_pos()
    {
        HANDLE h = GetStdHandle(STD_OUTPUT_HANDLE);
        CONSOLE_SCREEN_BUFFER_INFO cbsi;

        if (GetConsoleScreenBufferInfo(h, &cbsi))
            return cbsi.dwCursorPosition;
        return {0, 0}; // else return this
    }

    // https://stackoverflow.com/a/41213165/18121288
    bool readf::getconchar(KEY_EVENT_RECORD& krec)
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
    // Helper function to get the state of modifier keys
    DWORD readf::get_modifier_state(KEY_EVENT_RECORD& krec)
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
