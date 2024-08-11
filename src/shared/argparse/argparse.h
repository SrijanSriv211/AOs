#pragma once

class argparse
{
public:
    struct argument
    {
        std::vector<std::string> names;
        std::string help;
        std::string default_value;
        bool is_flag;
        bool required;

        argument(std::vector<std::string> cmd_names, std::string help_message, std::string default_value = "", bool is_flag = true, bool required = true)
        {
            this->names = cmd_names;
            this->help = help_message;
            this->default_value = default_value;
            this->is_flag = is_flag;
            this->required = required;
        }
    };

    struct parsed_argument
    {
        std::vector<std::string> names;
        std::string value;
        bool is_flag;
        bool requierd;
        std::string known_type;

        parsed_argument(std::vector<std::string> names, std::string value, bool is_flag, bool required, std::string known_type="known")
        {
            this->names = names;
            this->value = value;
            this->is_flag = is_flag;
            this->requierd = requierd;
            this->known_type = known_type;
        }
    };

public:
    argparse(const std::string& name, const std::string& description, const std::function<void(std::string)>& error_func = nullptr);
    void add(const std::vector<std::string>& cmd_names, const std::string& help_message, const std::string& default_value = "", const bool& is_flag = false, const bool& required = false);
    std::vector<parsed_argument> parse(const std::vector<std::string>& args);
    void print_help();
    void print_help(const argument& details);
    void get_help(const std::vector<std::string>& cmd_names);

private:
    std::string name = "";
    std::string description = "";
    std::vector<argument> arguments;
    std::function<void(std::string)> error_func;

private:
    argument find_matching_argument(const std::string& arg);
};
