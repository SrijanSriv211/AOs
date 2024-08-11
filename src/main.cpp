#include "aospch.h"
#include "core/entrypoint/entrypoint.h"

int main(int argc, char const *argv[])
{
    std::vector<std::string> args(argv, argv + argc);
    args.erase(args.begin());
    return take_entry(args);
}
