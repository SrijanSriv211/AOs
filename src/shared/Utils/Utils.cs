namespace Utils
{
    class Utils
    {
        public static string[] SimplifyString(string[] str)
        {
            List<string> temp_args = [];
            for (int i = 0; i < str.Length; i++)
                temp_args.Add(String.Strings(str[i]));

            return [.. temp_args];
        }
    }
}
