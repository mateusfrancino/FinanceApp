using FinanceApp.Helpers;
using FinanceApp.ViewModels;
using SkiaSharp;

namespace FinanceApp.Services.Charting;

public sealed class PriceChartRenderer : IPriceChartRenderer
{
    public void Draw(SKCanvas canvas, SKImageInfo info, IReadOnlyList<SeriesData> series, bool normalize, LineStyle style, ChartOptions? opts = null)
    {
        if (series == null || series.Count == 0) return;
        opts ??= new ChartOptions();

        // 1) agregados
        double gMin = double.MaxValue, gMax = double.MinValue;
        int maxLen = 0;
        foreach (var s in series)
        {
            if (s.Points.Length == 0) continue;
            maxLen = Math.Max(maxLen, s.Points.Length);
            foreach (var v in s.Points) { if (v < gMin) gMin = v; if (v > gMax) gMax = v; }
        }
        if (maxLen < 2) return;

        var (niceMin, niceMax, yTick) = ChartMathHelper.NiceScale(gMin, gMax, 6);

        using var axisPaint = new SKPaint { Color = opts.GridColor, StrokeWidth = 1, IsAntialias = true };
        using var axisLine = new SKPaint { Color = opts.AxisColor, StrokeWidth = opts.AxisStroke, IsAntialias = true };
        using var font = new SKFont { Size = opts.LabelFontSize };
        using var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true };

        // 2) medir rótulos Y
        var yLabels = new List<string>();
        for (double yv = niceMin; yv <= niceMax + 1e-9; yv += yTick)
            yLabels.Add(normalize ? yv.ToString("0.00") : yv.ToString("N2"));

        float Measure(string s) => font.MeasureText(s, textPaint);

        float maxLabelWidth = 0f;
        foreach (var s in yLabels) maxLabelWidth = Math.Max(maxLabelWidth, Measure(s));

        // 3) layout
        float left = opts.PaddingLeft + maxLabelWidth + 12f;
        float right = opts.PaddingRight;
        float top = opts.PaddingTop;
        float bottom = opts.PaddingBottom;

        float dw = Math.Max(10, info.Width - left - right);
        float dh = Math.Max(10, info.Height - top - bottom);

        // 4) grid + labels Y
        int lblIdx = 0;
        for (double yv = niceMin; yv <= niceMax + 1e-9; yv += yTick)
        {
            float y = top + (float)((niceMax - yv) / (niceMax - niceMin) * dh);
            canvas.DrawLine(left, y, left + dw, y, axisPaint);
            canvas.DrawText(yLabels[lblIdx++], left - 6, y, SKTextAlign.Right, font, textPaint);
        }

        // eixos
        canvas.DrawLine(left, top, left, top + dh, axisLine);
        canvas.DrawLine(left, top + dh, left + dw, top + dh, axisLine);

        // ticks X (0, 25, 50, 75, 100%)
        int[] xTicks = new[] { 0, maxLen / 4, maxLen / 2, (3 * maxLen) / 4, maxLen - 1 }.Distinct().OrderBy(i => i).ToArray();
        foreach (var t in xTicks)
        {
            float x = left + (float)t / (maxLen - 1) * dw;
            canvas.DrawLine(x, top + dh, x, top + dh + 4, axisLine);
            canvas.DrawText(t.ToString(), x, top + dh + 20, SKTextAlign.Center, font, textPaint);
        }

        // 5) séries
        foreach (var s in series)
        {
            var pts = s.Points;
            if (pts.Length < 2) continue;

            using var path = new SKPath();
            for (int i = 0; i < pts.Length; i++)
            {
                float x = left + (float)i / (pts.Length - 1) * dw;
                float y = top + (float)((niceMax - pts[i]) / (niceMax - niceMin) * dh);
                if (i == 0) path.MoveTo(x, y); else path.LineTo(x, y);
            }

            using var paint = new SKPaint
            {
                Color = s.Color,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = s.Stroke,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };
            switch (style)
            {
                case LineStyle.Dashed: paint.PathEffect = SKPathEffect.CreateDash(new float[] { 10, 6 }, 0); break;
                case LineStyle.Dotted: paint.PathEffect = SKPathEffect.CreateDash(new float[] { 2, 6 }, 0); break;
                case LineStyle.Solid:
                default: break;
            }
            canvas.DrawPath(path, paint);
        }
    }

}
