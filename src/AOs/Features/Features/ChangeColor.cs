partial class Features
{
    public void ChangeColor(string color_name)
    {
        static void help_for_color()
        {
            new Error("`color` value was not defined. Please use a defined color value");
            string[] list_of_colors = new string[]
            {
                "Black", "DarkBlue", "DarkGreen", "DarkCyan", "DarkRed",
                "DarkMagenta", "DarkYellow", "Gray", "DarkGray", "Blue",
                "Green", "Cyan", "Red", "Magenta", "Yellow", "White"
            };

            for (int i = 0; i < list_of_colors.Length; i++)
            {
                Console.Write($"{i}. ");
                if (i == 0)
                {
                    var default_color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.White;

                    new TerminalColor(list_of_colors[i], (ConsoleColor)i);

                    Console.BackgroundColor = default_color;
                }

                else
                    new TerminalColor(list_of_colors[i], (ConsoleColor)i);
            }
        }

        if (Utils.String.IsEmpty(color_name))
            AOs.Default_color = Obsidian.original_color_of_terminal;

        else
        {
            if (int.TryParse(color_name, out int color_num))
            {
                if (color_num < 0 || color_num > 15)
                    help_for_color();

                else
                    AOs.Default_color = (ConsoleColor)color_num;
            }

            else
                help_for_color();
        }

        Console.ForegroundColor = AOs.Default_color;
    }
}
