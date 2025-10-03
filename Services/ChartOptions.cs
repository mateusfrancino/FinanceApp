using SkiaSharp;

namespace FinanceApp.Services.Charting;

public sealed class ChartOptions
{
    public float PaddingLeft { get; set; } = 10f;
    public float PaddingRight { get; set; } = 20f;
    public float PaddingTop { get; set; } = 20f;
    public float PaddingBottom { get; set; } = 40f;

    public int MaxYTicks { get; set; } = 6;
    public SKColor GridColor { get; set; } = new SKColor(220, 220, 220);
    public SKColor AxisColor { get; set; } = new SKColor(80, 80, 80);
    public float AxisStroke { get; set; } = 1.5f;

    public float LabelFontSize { get; set; } = 14f;
}
