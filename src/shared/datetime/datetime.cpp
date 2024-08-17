#include "aospch.h"
#include "datetime.h"

namespace datetime
{
    std::string datetime()
    {
        const auto now = std::chrono::system_clock::now();
        const std::time_t t_c = std::chrono::system_clock::to_time_t(now);

        std::tm now_tm = *std::localtime(&t_c);
        std::stringstream ss;
        ss << std::put_time(&now_tm, "%a, %d %b %Y, %H:%M:%S");
        return ss.str();
    }
}
