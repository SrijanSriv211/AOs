using System;

public class Error
{
        // No arguments.
        static public void NoArgs()
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"NoArgumentError: No arguments were passed");
            Console.ForegroundColor = Color;
        }

        // Unrecognized arguments.
        static public void Args(string[] _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unrecognized arguments: {string.Join(", ", Collection.Array.Reduce(_Flag))}");
            Console.ForegroundColor = Color;
        }

        static public void Args(string _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unrecognized arguments: {_Flag}");
            Console.ForegroundColor = Color;
        }

        // Not appropriate number of arguments.
        static public void TooFewArgs(string[] _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too few arguments: {string.Join(", ", Collection.Array.Reduce(_Flag))}");
            Console.ForegroundColor = Color;
        }

        static public void TooFewArgs(string _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too few arguments: {_Flag}");
            Console.ForegroundColor = Color;
        }

        static public void TooManyArgs(string[] _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too many arguments: {string.Join(", ", Collection.Array.Reduce(_Flag))}");
            Console.ForegroundColor = Color;
        }

        static public void TooManyArgs(string _Flag)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too many arguments: {_Flag}");
            Console.ForegroundColor = Color;
        }

        // Unrecognized Command.
        static public void Command(string _Command, string _Details="Command does not exist")
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"'{_Command}', {_Details}");
            Console.ForegroundColor = Color;
        }

        // Unrecognized syntax.
        static public void Syntax(string _Details)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SyntaxError: {_Details}");
            Console.ForegroundColor = Color;
        }

        // Division by 0.
        static public void ZeroDivision(string _Line)
        {
            var Color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ZeroDivisionError: Division by 0");
            Console.ForegroundColor = Color;
        }
}
