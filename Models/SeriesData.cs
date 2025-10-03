using SkiaSharp;

namespace FinanceApp.Models
{
    public class SeriesData
    {
        public double[] Points { get; set; } = Array.Empty<double>();
        public SKColor Color { get; set; }
        public float Stroke { get; set; } = 2f;
    }
}
