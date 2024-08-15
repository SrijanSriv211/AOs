#include "aospch.h"
#include "readf.h"

#include "console/console.h"

namespace console
{
    void readf::render_tokens()
    {
    }

    void readf::clear_console()
    {
    }

    void readf::update_console()
    {
        if (this->text_buffer == this->ren_text_buffer)
            return;

        this->clear_console();
        this->render_tokens();
    }
}
