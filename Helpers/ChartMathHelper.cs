namespace FinanceApp.Helpers;

public static class ChartMathHelper
{
    public static (double niceMin, double niceMax, double tick) NiceScale(double min, double max, int maxTicks = 6)
    {
        if (Math.Abs(max - min) < 1e-12) { max += 1; min -= 1; }
        double range = NiceNumber(max - min, false);
        double tick = NiceNumber(range / (maxTicks - 1), true);
        double niceMin = Math.Floor(min / tick) * tick;
        double niceMax = Math.Ceiling(max / tick) * tick;
        return (niceMin, niceMax, tick);
    }

    private static double NiceNumber(double x, bool round)
    {
        double exp = Math.Floor(Math.Log10(Math.Max(1e-12, x)));
        double f = x / Math.Pow(10, exp);
        double nf = round
            ? (f < 1.5 ? 1 : f < 3 ? 2 : f < 7 ? 5 : 10)
            : (f <= 1 ? 1 : f <= 2 ? 2 : f <= 5 ? 5 : 10);
        return nf * Math.Pow(10, exp);
    }
}
