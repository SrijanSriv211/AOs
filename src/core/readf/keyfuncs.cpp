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
        if (this->vector3.i <= 0)
            return;

        this->vector3.i--;
        this->vector3.x--;
        text_buffer.erase(this->vector3.i, 1);
    }
}
