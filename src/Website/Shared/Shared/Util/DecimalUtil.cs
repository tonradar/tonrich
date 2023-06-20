namespace Tonrich.Shared.Util
{
    public static class DecimalUtil
    {
        public static decimal ToNoneZeroDecimal(this decimal? value)
        {
            if (value == null)
                return 0m;
           return value.Value.ToNoneZeroDecimal();
        }

        public static decimal ToNoneZeroDecimal(this decimal value)
        {
            var result = Math.Round(value, 2);
            if (result == 0.00m)
                return 0.01m;

            return result;
        }
    }
}
