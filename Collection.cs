using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

public class Collection
    {
        public class String
        {
            // Count the number of times a substring appears in a string.
            public static int Count(string _Source, string _SubString)
            {
                return Regex.Matches(_Source, Regex.Escape(_SubString)).Count;
            }

            // Remove extra spaces from a string.
            public static string Reduce(string _Line)
            {
                return string.Join(" ", _Line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
            }

            // Check whether a string is empty.
            public static bool IsEmpty(string _Line)
            {
                if (string.IsNullOrEmpty(_Line) || string.IsNullOrWhiteSpace(_Line)) return true;
                else return false;
            }

            // Check if a string has special chars like @, !, >.
            public static bool HasSpecialChars(string _Line)
            {
                return _Line.Any(c => !Char.IsLetterOrDigit(c));
            }

            // Check if the string is wrapped with string quotes or not.
            public static bool IsString(string _Line)
            {
                if (_Line.StartsWith("\"") && _Line.EndsWith("\"")) return true;
                else if (_Line.StartsWith("'") && _Line.EndsWith("'")) return true;
                return false;
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
