#include "aopch.h"
#include "lex.h"

#include "strings/strings.h"
#include "console/console.h"

bool lex::any_token_type(const lex::token_type& str, const std::vector<lex::token_type>& iter)
{
    for (const lex::token_type& i : iter)
    {
        if (str == i)
            return true;
    }

    return false;
}

bool lex::is_math_expr(const std::string& str)
{
    return strings::only(str, {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "_", ".", "-", "+", "*", "/", "(", ")", " "});
}

bool lex::is_valid_string(const std::string& str)
{
    if (str.length() == 1)
    {
        this->error = "unexpected end of tokens after " + std::string(1, str.front());

        if (this->break_at_error)
        {
            console::errors::syntax(this->error);
            return false;
        }
    }

    else if (str.front() != str.back())
    {
        this->error = "missing terminating " + std::string(1, str.front()) + " character";

        if (this->break_at_error)
        {
            console::errors::syntax(this->error);
            return false;
        }
    }

    return true;
}

std::string lex::unescape_string(const std::string& str)
{
    std::map<char, char> escape_chars = {
        {'\\', '\\'},
        {'"', '"'},
        {'\'', '\''},
        {'n', '\n'},
        {'n', '\n'},
        {'0', '\0'},
        {'t', '\t'},
        {'r', '\r'},
        {'b', '\b'},
        {'a', '\a'},
        {'f', '\f'}
    };

    std::string unescaped_str = "";
    bool found_escape_char = false;

    for (int i = 0; i < static_cast<int>(str.size()); i++)
    {
        if (str[i] == '\\')
        {
            i++;

            for (const auto& [key, val] : escape_chars)
            {
                if (str[i] == key)
                {
                    unescaped_str += val;
                    found_escape_char = true;
                    break;
                }
            }

            if (!found_escape_char)
                unescaped_str += "\\" + str[i];
        }

        else
            unescaped_str += str[i];

        found_escape_char = false;
    }

    return unescaped_str;
}

std::string lex::create_env_filename(const std::string& filename)
{
    const std::string exts[] = {"", ".ao", ".exe", ".msi", ".bat", ".cmd"};

    for (const std::string& ext : exts)
    {
        if (std::filesystem::exists(filename + ext))
            return filename + ext;
    }

    return "";
}

lex::token lex::get_env_var_val(const std::string& str)
{
    // it will have at least 3 chars '`' and the env char,
    // therefore no need to check if str is greater than 3 chars or not.
    const std::string env_var_name = str.substr(1, str.size() - 2);
    const char* env_var_val = std::getenv(env_var_name.c_str());

    if (env_var_val != nullptr)
    {
        // push it as an identifier since it won't be wrapped inside any string literals
        //*NOTE: It may change and become more dynamic, so if the env val is an int,
        //* then the token type will be an int. but that's for future, not now.
        return {env_var_val, lex::IDENTIFIER};
    }

    // check if the file already exists
    const std::string filename = this->create_env_filename(env_var_name);
    if (!strings::is_empty(filename))
        return {filename, lex::IDENTIFIER};

    return {str, lex::STRING};
}
