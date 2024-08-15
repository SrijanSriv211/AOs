#include "aospch.h"
#include "readf.h"

namespace console
{
    void readf::handle_ctrl_enter()
    {
        std::cout << "Test!";
    }

    void readf::handle_backspace()
    {
        if (this->config.i <= 0)
            return;

        this->config.i--;
        this->config.x--;
        text_buffer.erase(this->config.i, 1);
    }
}
