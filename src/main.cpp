#include "aopch.h"
#include "core/entrypoint/entrypoint.h"

int main(int argc, char const *argv[])
{
    std::vector<std::string> args(argv, argv + argc);
    args.erase(args.begin());
    int return_code = 0;

    while (true)
    {
        return_code = take_entry(args);
        if (return_code == 0)
            break;

        else if (return_code == -1)
            return 1;
    }

    return return_code;
}
