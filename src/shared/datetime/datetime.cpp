#include "aopch.h"
#include "datetime.h"

namespace datetime
{
    std::string datetime(const std::string& format)
    {
        const auto now = std::chrono::system_clock::now();
        const std::time_t t_c = std::chrono::system_clock::to_time_t(now);

        std::tm now_tm = *std::localtime(&t_c);
        std::stringstream ss;
        ss << std::put_time(&now_tm, format.c_str());
        return ss.str();
    }
}
