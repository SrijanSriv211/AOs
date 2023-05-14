using System;
using System.Linq;
using System.Collections.Generic;

//! This module is deprecated/not ready for use.
class readline
{
    public string Out = "";
    private string Prompt = "";

    private static int count_for_tmp_history = 0;
    private static List<string> tmp_history_of_commands = new List<string>();

    public readline(string prompt="")
    {
        Prompt = prompt;
    }

    public string input()
    {
        Console.Write(Prompt); // Print the prompt

        // Take input
        int cursor_pos = 0;
        int count_for_autocomplete = 1;
        string complete_suggestion = string.Empty;
        string[] list_of_suggestions = new string[0];

        string CMD = string.Empty;
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Tab && cursor_pos == CMD.Length)
            {
                KeyHandler.Tab TabKey = new KeyHandler.Tab(CMD);
                string str_to_be_replaced = TabKey.Directories;
                list_of_suggestions = TabKey.List_of_Suggestions;

                if (Collection.Array.IsEmpty(list_of_suggestions)) continue;

                // '(count_for_autocomplete + 1) % list_of_suggestions.Length' here is very different from the count declaration.
                //* ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ -> Can be rewritten as '(((count_for_autocomplete + 1) % list_of_suggestions.Length) + 1) % list_of_suggestions.Length'
                string suggestion = list_of_suggestions[(count_for_autocomplete + 1) % list_of_suggestions.Length];
                string current_line = list_of_suggestions[count_for_autocomplete];
                complete_suggestion = CMD.Replace(str_to_be_replaced, suggestion);

                // Update the line.
                Console.Write(string.Concat(Enumerable.Repeat("\b \b", current_line.Length)));
                Console.Write(suggestion);

                // Move the cursor to the very end and update cursor position.
                cursor_pos = complete_suggestion.Length;
                Console.SetCursorPosition(Prompt.Length + cursor_pos, Console.CursorTop);

                // Update the count such that when it == list_of_suggestions.Length, then it resets back to 0;
                count_for_autocomplete = (count_for_autocomplete + 1) % list_of_suggestions.Length;
            }

            else
            {
                CMD = !Collection.Array.IsEmpty(list_of_suggestions) ? complete_suggestion : CMD;
                list_of_suggestions = new string[0];
                count_for_autocomplete = 0;

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    if (!Collection.String.IsEmpty(CMD))
                    {
                        tmp_history_of_commands.Add(CMD);
                        count_for_tmp_history++;
                    }

                    break;
                }

                // Handle typable-characters.
                else if (!Char.IsControl(keyInfo.KeyChar))
                {
                    CMD = CMD.Insert(cursor_pos, keyInfo.KeyChar.ToString());
                    cursor_pos++;

                    Console.Write(CMD.Substring(cursor_pos - 1));
                    Console.SetCursorPosition(Console.CursorLeft - CMD.Length + cursor_pos, Console.CursorTop);
                }

                // Handle backspace.
                else if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.Backspace && !Collection.String.IsEmpty(CMD) && Console.CursorLeft > Prompt.Length)
                {
                    int start_pos = cursor_pos - 1;
                    while (start_pos >= 0 && CMD[start_pos] == ' ') start_pos--;
                    while (start_pos >= 0 && CMD[start_pos] != ' ') start_pos--;
                    start_pos++;

                    string deleted = CMD.Substring(start_pos, cursor_pos - start_pos);
                    CMD = CMD.Remove(start_pos, cursor_pos - start_pos);
                    cursor_pos = start_pos;

                    Console.Write(string.Concat(Enumerable.Repeat("\b \b", deleted.Length)));
                    Console.Write(CMD.Substring(cursor_pos) + " ");
                    Console.Write(new string(' ', deleted.Length) + "\b", deleted.Length);
                    Console.SetCursorPosition(Prompt.Length + cursor_pos, Console.CursorTop);
                }

                else if (keyInfo.Key == ConsoleKey.Backspace && !Collection.String.IsEmpty(CMD) && Console.CursorLeft > Prompt.Length)
                {
                    CMD = CMD.Remove(cursor_pos - 1, 1);
                    cursor_pos--;

                    Console.Write("\b \b" + CMD.Substring(cursor_pos) + " ");
                    Console.SetCursorPosition(Prompt.Length + cursor_pos, Console.CursorTop);
                }

                // Handle arrow keys.
                else if (keyInfo.Key == ConsoleKey.UpArrow && !Collection.Array.IsEmpty(tmp_history_of_commands.ToArray()))
                {
                    Console.SetCursorPosition(Prompt.Length + CMD.Length, Console.CursorTop);
                    count_for_tmp_history = (count_for_tmp_history == 0) ? 0 : (count_for_tmp_history - 1);

                    Console.Write(string.Concat(Enumerable.Repeat("\b \b", CMD.Length)));
                    CMD = tmp_history_of_commands[count_for_tmp_history];
                    cursor_pos = CMD.Length;
                    Console.Write(CMD);
                }

                else if (keyInfo.Key == ConsoleKey.DownArrow && !Collection.Array.IsEmpty(tmp_history_of_commands.ToArray()))
                {
                    Console.SetCursorPosition(Prompt.Length + CMD.Length, Console.CursorTop);

                    int tmp_history_of_commands_length = tmp_history_of_commands.Count - 1;
                    count_for_tmp_history = (count_for_tmp_history == tmp_history_of_commands_length) ? tmp_history_of_commands_length : (count_for_tmp_history + 1);

                    Console.Write(string.Concat(Enumerable.Repeat("\b \b", CMD.Length)));
                    CMD = tmp_history_of_commands[count_for_tmp_history];
                    cursor_pos = CMD.Length;
                    Console.Write(CMD);
                }

                else if (keyInfo.Key == ConsoleKey.LeftArrow && Console.CursorLeft > Prompt.Length)
                {
                    cursor_pos--;
                    Console.Write("\b");
                }

                else if (keyInfo.Key == ConsoleKey.RightArrow && Console.CursorLeft < CMD.Length + 2)
                {
                    cursor_pos++;
                    Console.Write(CMD[cursor_pos - 1]);
                }
            }
        }

        return CMD.Trim() ?? "";
    }
}
