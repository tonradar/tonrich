namespace System;

public static class DateTimeExtensions
{
    public static int MonthCount(this DateTimeOffset EndDateTime, DateTimeOffset StartDateTime)
    {
        return (Math.Abs((EndDateTime.Month - StartDateTime.Month) + 12 * (EndDateTime.Year - StartDateTime.Year))) + 1;
    }
}
