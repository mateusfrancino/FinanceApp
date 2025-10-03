using SkiaSharp;
using FinanceApp.Models;
using static FinanceApp.Helpers.Enums;

namespace FinanceApp.Services.Charting;

public interface IPriceChartRenderer
{
    void Draw(SKCanvas canvas, SKImageInfo info, IReadOnlyList<SeriesData> series, bool normalize, LineStyle style, ChartOptions? options = null);
}
