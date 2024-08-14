#include "aospch.h"
#include "console/console.h"
#include "core/lexer/lex.h"
#include "readf.h"

namespace console
{
    // std::vector<int> cursor_start_pos = {x, y}
    readf::readf(std::vector<int> cursor_start_pos, std::vector<std::string> suggestions_list, std::map<lex::token_type, console::color> color_codes)
    {
        this->text_buffer = "";
        this->config.x = cursor_start_pos[0];
        this->config.y = cursor_start_pos[1];
    }

    std::vector<lex::token> readf::takeinput()
    {
        KEY_EVENT_RECORD key;

        while (true)
        {
            if (!this->getconchar(key))
                continue;

            // https://stackoverflow.com/a/18703309/18121288
            if (key.wVirtualKeyCode == VK_RETURN)
            {
                std::cout << std::endl;
                break;
            }
        }

        return lex(this->text_buffer).tokens;
    }

    // https://stackoverflow.com/a/41213165/18121288
    bool readf::getconchar(KEY_EVENT_RECORD &krec)
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
}
