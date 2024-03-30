namespace Utils
{
    public class Maths
    {
        public static int CalculatePadding(int count, int max_padding_len=60)
        {
            return Math.Max(max_padding_len - (int)Math.Log10(count), 0);
        }
    }
}
