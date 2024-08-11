#include "aospch.h"
#include "math.h"

namespace math
{
    int calc_padding(const int& count, const int max_padding_len)
    {
        return std::max(max_padding_len - static_cast<int>(std::log10(count)), 0);
    }
}
