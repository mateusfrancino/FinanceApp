using SkiaSharp;
using FinanceApp.ViewModels;

namespace FinanceApp.Services.Charting;

public interface IPriceChartRenderer
{
    void Draw(SKCanvas canvas, SKImageInfo info, IReadOnlyList<SeriesData> series, bool normalize, LineStyle style, ChartOptions? options = null);
}
