partial class Obsidian
{
    public static void Credits()
    {
        string[] CreditCenter = [
             "_________ Team AOS ________",
             "Author     -> Srijan Srivastava",
             "Github     -> https://github.com/SrijanSriv211/AOs",
             "Contact    -> QCoreNest@gmail.com",
             "",
             "____________________ Note (For Developers) ____________________",
            $"|| {about_AOs}",
             "|| All code is licensed under an MIT license.",
             "|| This allows you to re-use the code freely, remixed in both commercial and non-commercial projects.",
             "|| The only requirement is to include the same license when distributing.",
             "",
             "____________________ Note (For All) ____________________",
             "|| Warning - Do not Delete any File",
             "|| or it may Cause Corruption",
             "|| and may lead to instability.",
             "",
             "Type 'help' to get information about all supported command."
        ];

        Terminal.Print(string.Join("\n", CreditCenter), ConsoleColor.White);
    }
}
