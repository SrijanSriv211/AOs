using System.Data;
using System.Text.RegularExpressions;

namespace Utils
{
    class Utils
    {
        public static string[] SimplifyString(string[] str)
        {
            List<string> temp_args = new();
            for (int i = 0; i < str.Length; i++)
                temp_args.Add(String.Strings(str[i]));

            return temp_args.ToArray();
        }
    }

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

    public class Array
    {
        // Check whether a string is empty.
        public static bool IsEmpty(string[] arr)
        {
            if (arr.Length == 0 || arr == null) return true;
            else return false;
        }

        // Remove empty strings from an array.
        public static string[] Reduce(string[] arr)
        {
            return arr.Where(x => !String.IsEmpty(x)).ToArray();
        }

        // Remove duplicate elements from an array.
        public static string[] Filter(string[] arr)
        {
            return arr.Distinct().ToArray();
        }

        // Trim the leading part of an array.
        public static string[] TrimStart(string[] arr)
        {
            int Count = 0;
            foreach (string i in arr)
            {
                if (String.IsEmpty(i)) Count++;
                else break;
            }

            return arr.Skip(Count).ToArray();
        }

        // Trim the trailing part of an array.
        public static string[] TrimEnd(string[] arr)
        {
            int Count = 0;
            foreach (string i in arr.ToArray().Reverse())
            {
                if (String.IsEmpty(i)) Count++;
                else break;
            }

            return arr.ToArray().Reverse().Skip(Count).Reverse().ToArray();
        }

        // Trim the leading and trailing part of an array.
        public static string[] Trim(string[] arr)
        {
            // No need to search through nothing
            if (arr == null || arr.Length == 0) return System.Array.Empty<string>();

            // Define predicate to test for non-empty strings
            Predicate<string> IsNotEmpty = s => !String.IsEmpty(s);

            var FirstIndex = System.Array.FindIndex(arr, IsNotEmpty);

            // Nothing to return if it's all whitespace anyway
            if (FirstIndex < 0) return System.Array.Empty<string>();

            var LastIndex = System.Array.FindLastIndex(arr, IsNotEmpty);

            // Calculate size of the relevant middle-section from the indices
            var NewArraySize = LastIndex - FirstIndex + 1;

            // Create new array and copy items to it
            var Results = new string[NewArraySize];
            System.Array.Copy(arr, FirstIndex, Results, 0, NewArraySize);

            return Results;
        }
    }
}
