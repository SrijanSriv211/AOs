namespace Utils
{
    public class Maths
    {
        public static int CalculatePadding(int count, int max_padding_len=60)
        {
            return Math.Max(max_padding_len - (int)Math.Log10(count), 0);
        }

        // Split the number into 2 parts (before and after the decimal point)
        public static (int, double) SplitNumber(double Number)
        {
            int BeforeDecimalPart = (int)Number;
            double FractionalPart = Number - BeforeDecimalPart;

            return (BeforeDecimalPart, FractionalPart);
        }
    }
}
