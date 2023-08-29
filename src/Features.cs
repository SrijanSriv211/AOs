class Features
{
    public static void shout(string[] args)
    {
        if (Collection.Array.IsEmpty(args))
            Error.NoArgs();

        else
            Console.WriteLine(string.Join(" ", Lexer.SimplifyString(args)));
    }

    public static void exit(string[] _)
    {
        Environment.Exit(0);
    }
}
