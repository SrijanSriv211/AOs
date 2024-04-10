partial class Terminal
{
    public static string TakeInput(ConsoleColor color=ConsoleColor.Gray, int cursor_start_pos=0)
    {
        ConsoleColor default_color = Console.ForegroundColor;
        Console.ForegroundColor = color;
        int cursor_pos = cursor_start_pos;
        string output = "";

        while (true)
        {
            ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

            if (KeyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            else if (KeyInfo.Key == ConsoleKey.Backspace)
            {
                if (cursor_pos > cursor_start_pos)
                {
                    int length = KeyInfo.Modifiers == ConsoleModifiers.Control ? Utils.Array.Reduce(output.Split()).Last().Length : 1;

                    void Backspace()
                    {
                        cursor_pos--;
                        Console.Write("\b \b");
                        Console.SetCursorPosition(cursor_pos, Console.CursorTop);
                        output = output.Remove(cursor_pos - cursor_start_pos, 1);
                    }

                    for (int i = 0; i < length; i++)
                    {
                        Backspace();

                        if (KeyInfo.Modifiers == ConsoleModifiers.Control && output.LastOrDefault() == ' ')
                            Backspace();
                    }
                }
            }

            else if (KeyInfo.Key == ConsoleKey.RightArrow)
            {
                if (cursor_pos < cursor_start_pos + output.Length)
                {
                    if (KeyInfo.Modifiers == ConsoleModifiers.Control)
                    {
                        int next_word_idx = output.IndexOf(' ', cursor_pos - cursor_start_pos);
                        if (next_word_idx == -1)
                            next_word_idx = output.Length;

                        cursor_pos = cursor_start_pos + next_word_idx;
                    }

                    else if (KeyInfo.Modifiers == ConsoleModifiers.Shift)
                    {
                        //TODO: Multi-char selection
                    }

                    else
                        cursor_pos++;

                    Console.SetCursorPosition(cursor_pos, Console.CursorTop);
                }
            }

            else if (KeyInfo.Key == ConsoleKey.LeftArrow)
            {
                if (cursor_pos > cursor_start_pos)
                {
                    if (KeyInfo.Modifiers == ConsoleModifiers.Control)
                    {
                        int previous_word_idx = output.LastIndexOf(' ', cursor_pos - cursor_start_pos - 1);
                        if (previous_word_idx == -1)
                            previous_word_idx = cursor_start_pos;

                        cursor_pos = cursor_start_pos + previous_word_idx + 1;
                    }

                    else if (KeyInfo.Modifiers == ConsoleModifiers.Shift)
                    {
                        //TODO: Multi-char selection
                    }

                    else
                        cursor_pos--;

                    Console.SetCursorPosition(cursor_pos, Console.CursorTop);
                }
            }

            else if (KeyInfo.Key == ConsoleKey.Home)
            {
                Console.SetCursorPosition(cursor_start_pos, Console.CursorTop);
                cursor_pos = cursor_start_pos;
            }

            else if (KeyInfo.Key == ConsoleKey.End)
            {
                Console.SetCursorPosition(cursor_start_pos + output.Length, Console.CursorTop);
                cursor_pos = cursor_start_pos + output.Length;
            }

            else
            {
                // Append the character to the end of the output
                if (cursor_pos == output.Length + cursor_start_pos)
                {
                    output += KeyInfo.KeyChar;
                    Console.Write(KeyInfo.KeyChar);
                }

                // Insert the character at the cursor position
                else
                {
                    output = output.Insert(cursor_pos - cursor_start_pos, KeyInfo.KeyChar.ToString());

                    Console.Write(output[(cursor_pos - cursor_start_pos)..]); // Update text after cursor
                    Console.SetCursorPosition(cursor_pos + 1, Console.CursorTop); // Move cursor back to correct position
                }

                cursor_pos++;
            }
        }

        Console.ForegroundColor = default_color;
        return output;
    }
}
