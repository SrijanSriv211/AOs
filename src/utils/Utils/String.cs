using System.Text.RegularExpressions;

namespace Utils
{
    public class String
    {
        // Count the number of times a substring appears in a string.
        public static int Count(string source, string what_to_count)
        {
            return Regex.Matches(source, Regex.Escape(what_to_count)).Count;
        }

        // Remove extra spaces from a string.
        public static string Reduce(string line)
        {
            return string.Join(" ", line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
        }

        // Check whether a string is empty.
        public static bool IsEmpty(string line)
        {
            return string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line);
        }

        // Check whether a string is just a whitespace.
        public static bool IsWhiteSpace(string line)
        {
            return line != "" && line.Trim() == "";
        }

        // Check if the string is wrapped with string quotes or not.
        public static bool IsString(string line)
        {
            return (line.StartsWith("\"") && line.EndsWith("\"")) || (line.StartsWith("'") && line.EndsWith("'"));
        }

        // Remove string quotes.
        public static string Strings(string line)
        {
            return IsString(line) ? line.Substring(1, line.Length - 2) : line;
        }
    }
}