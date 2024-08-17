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

    COORD readf::calc_xy_coord(const int& total_dist)
    {
        // calculate the exact x and y positions to put the cursor at.
        short y = total_dist / this->console_window_width();
        short x = total_dist - (y * this->console_window_width());

        return {x, y};
    }

    int readf::console_window_width()
    {
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
        return csbi.srWindow.Right - csbi.srWindow.Left + 1;
    }

    int readf::console_window_height()
    {
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &csbi);
        return csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
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
    // helper function to get the state of modifier keys
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

    // (token_idx, char_idx)
    std::pair<int, int> readf::get_token_diff(const std::string& text, const std::string& text2)
    {
        // tokenize the updated input text
        this->lexer = lex(text, false);

        // tokenize the updated rendered input text
        lex ren_lexer = lex(text2, false);

        int smallest_token_list_len = std::min(this->lexer.tokens.size(), ren_lexer.tokens.size());

        for (int token_idx = 0; token_idx < smallest_token_list_len; token_idx++)
        {
            std::string text_token_name = this->lexer.tokens[token_idx].name;
            std::string text2_token_name = ren_lexer.tokens[token_idx].name;

            lex::token_type text_token_type = this->lexer.tokens[token_idx].type;
            lex::token_type text2_token_type = ren_lexer.tokens[token_idx].type;

            if (text_token_type != text2_token_type)
                return {token_idx, 0};

            else if (text_token_name != text2_token_name)
                return {token_idx, this->get_text_diff(text_token_name, text2_token_name)};
        }

        return {smallest_token_list_len - 1, 0};
    }

    int readf::get_text_diff(const std::string& text, const std::string& text2)
    {
        for (int char_idx = 0; char_idx < std::min(text.length(), text2.length()); char_idx++)
        {
            if (text[char_idx] != text2[char_idx])
                return char_idx;
        }

        if (text.length() != text2.length())
            return std::min(text.length(), text2.length());

        return 0;
    }
}
