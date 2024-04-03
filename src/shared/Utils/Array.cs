using Lexer;

namespace Utils
{
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

        // https://stackoverflow.com/a/70991343/18121288
        // Trim the leading and trailing part of an array.
        public static string[] Trim(string[] arr)
        {
            // No need to search through nothing
            if (arr == null || arr.Length == 0)
                return [];

            // Define predicate to test for non-empty strings
            static bool IsNotEmpty(string s) => !String.IsEmpty(s);

            var FirstIndex = System.Array.FindIndex(arr, IsNotEmpty);

            // Nothing to return if it's all whitespace anyway
            if (FirstIndex < 0)
                return [];

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
