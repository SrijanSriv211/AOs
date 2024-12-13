#include "aopch.h"
#include "console.h"

namespace console
{
    namespace errors
    {
        std::string throw_error(const std::string& message, const std::string& name_of_error)
        {
            console::print(name_of_error + " error", console::color::BLACK, console::color::LIGHT_RED, false);
            console::print(": " + message, console::color::WHITE);

            return name_of_error + " error: " + message;
        }

        std::string runtime(const std::string& command, const std::string& message)
        {
            console::print(command + ": ", console::color::LIGHT_WHITE, false);
            return command + ": " + errors::throw_error(message, "runtime");
        }

        std::string syntax(const std::string& message)
        {
            return errors::throw_error(message, "syntax");
        }
    }
}
