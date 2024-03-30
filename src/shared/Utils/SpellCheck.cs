namespace Utils
{
    // https://github.com/b001io/wagner-fischer
    class SpellCheck(List<string> dictionary)
    {
        public List<(string, int)> Check(string word, int suggestions_len=10)
        {
            List<(string, int)> suggestions = [];

            foreach (var correctWord in dictionary)
            {
                int distance = WagnerFischer(word, correctWord);
                suggestions.Add((correctWord, distance));
            }

            suggestions.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            return suggestions.Take(suggestions_len).ToList();
        }

        private static int WagnerFischer(string s1, string s2)
        {
            int lenS1 = s1.Length;
            int lenS2 = s2.Length;
            if (lenS1 > lenS2)
            {
                (s1, s2) = (s2, s1);
                (lenS1, lenS2) = (lenS2, lenS1);
            }

            int[] currentRow = Enumerable.Range(0, lenS1 + 1).ToArray();
            for (int i = 1; i <= lenS2; i++)
            {
                int[] previousRow = currentRow;
                currentRow = new int[lenS1 + 1];
                currentRow[0] = i;
                for (int j = 1; j <= lenS1; j++)
                {
                    int add = previousRow[j] + 1;
                    int delete = currentRow[j - 1] + 1;
                    int change = previousRow[j - 1];
                    if (s1[j - 1] != s2[i - 1])
                        change++;

                    currentRow[j] = Math.Min(Math.Min(add, delete), change);
                }
            }

            return currentRow[lenS1];
        }
    }
}
