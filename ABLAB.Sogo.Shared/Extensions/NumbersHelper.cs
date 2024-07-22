using System.Globalization;

namespace ABLAB.Sogo.Shared.Extensions;

public static class NumbersHelper
{
    public static string ToZlotys(this decimal value)
    {
        return value.ToString("C0", new CultureInfo("pl-PL"));
    }

    public static bool NotNullsOrZeros(params int?[] values)
    {
        return values.All(v => v > 0);
    }

    public static string SqrMeters(this decimal value)
    {
        return SqrMeters((decimal?)value);
    }

    public static string SqrMeters(this decimal? value)
    {
        if (value == null)
        {
            return "0 m²";
        }
        return value.Value.ToString("N2", new CultureInfo("pl-PL")) + " m²";
    }
}
