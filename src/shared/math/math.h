#pragma once

namespace math
{
    int calc_padding(const int& count, const int max_padding_len=60);
    std::string eval(const std::string& expression, const bool& return_expr_on_error=true);
};
