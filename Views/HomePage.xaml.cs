using FinanceApp.ViewModels;
using FinanceApp.Services.Charting;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace FinanceApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _vm;
    private readonly IPriceChartRenderer _renderer;

    public HomePage(HomePageViewModel vm, IPriceChartRenderer renderer)
    {
        InitializeComponent();
        _vm = vm;
        _renderer = renderer;
        BindingContext = _vm;
        _vm.OnRedrawRequested += () => Canvas.InvalidateSurface();
    }

    private void OnCanvasPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        e.Surface.Canvas.Clear(SKColors.White);
        _renderer.Draw(e.Surface.Canvas, e.Info, _vm.Series, _vm.Normalize, _vm.SelectedStyle);
    }
}
