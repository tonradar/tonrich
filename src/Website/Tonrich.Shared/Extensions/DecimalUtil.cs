﻿namespace System;

public static class DecimalExtensions
{
    public static decimal ToNoneZeroDecimal(this decimal? value)
    {
        if (value == null)
            return 0m;
        return value.Value.ToNoneZeroDecimal();
    }

    public static decimal ToNoneZeroDecimal(this decimal value)
    {
        if (value == 0m)
            return 0m;
        
        var result = Math.Round(value, 2);
        if (result == 0.00m)
            return 0.01m;

        return result;
    }
}
