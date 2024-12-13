#include "aopch.h"
#include "math.h"

#include "strings/strings.h"
#include "console/console.h"
#include "tinyexpr/tinyexpr.h"

namespace math
{
    int calc_padding(const int& count, const int max_padding_len)
    {
        return std::max(max_padding_len - static_cast<int>(std::log10(count)), 0);
    }

    std::string eval(const std::string& expression, const bool& return_expr_on_error)
    {
        // preprocess expression
        std::string expr = strings::replace_all(expression, "_", "");
        expr = strings::replace_all(expr, " ", "");

        // use tinyexpr to evaluate the expression
        double result = te_interp(expr.c_str(), 0);
        std::string result_s = std::format("{}", result); // _s means converted to string

        if ((result_s == "nan" || result_s == "inf") && return_expr_on_error)
            return expr;

        return result_s;
    }
}
