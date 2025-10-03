using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FinanceApp.Models;
using FinanceApp.Services;
using SkiaSharp;
using static FinanceApp.Helpers.Enums;

namespace FinanceApp.ViewModels;

public class HomePageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void Raise([CallerMemberName] string n = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

    //paramns
    private double _initialPrice = 100;
    public double InitialPrice { get => _initialPrice; set { _initialPrice = value; Raise(); } }

    private double _vol = 0.2;
    public double Vol { get => _vol; set { _vol = value; Raise(); } }

    private double _mean = 0.05;
    public double Mean { get => _mean; set { _mean = value; Raise(); } }

    private int _numDays = 200;
    public int NumDays { get => _numDays; set { _numDays = Math.Max(2, value); Raise(); } }

    private int _simulations = 1;
    public int Simulations { get => _simulations; set { _simulations = Math.Clamp(value, 1, 20); Raise(); } }

    private bool _normalize = false;
    public bool Normalize { get => _normalize; set { _normalize = value; Raise(); } }

    private float _strokeWidth = 2f;
    public float StrokeWidth { get => _strokeWidth; set { _strokeWidth = MathF.Max(1f, value); Raise(); } }

    // personalização
    private SKColor _selectedColor = SKColors.SteelBlue;
    public SKColor SelectedColor { get => _selectedColor; set { _selectedColor = value; Raise(); } }

    private LineStyle _selectedStyle = LineStyle.Solid;
    public LineStyle SelectedStyle { get => _selectedStyle; set { _selectedStyle = value; Raise(); } }

    private bool _applyColorToAll = false;
    public bool ApplyColorToAll { get => _applyColorToAll; set { _applyColorToAll = value; Raise(); } }

    public IList<LineStyle> LineStyles { get; } = Enum.GetValues(typeof(LineStyle)).Cast<LineStyle>().ToList();

    private string _selectedColorHex = "#4682B4"; 
    public string SelectedColorHex { get => _selectedColorHex; set { _selectedColorHex = value; Raise(); } }
  
    public ICommand SetColorCommand => new Command<string>(hex =>
    {
        if (string.IsNullOrWhiteSpace(hex)) return;

        SelectedColorHex = hex;

        var c = Color.FromArgb(hex);
        var sk = new SKColor((byte)(c.Red * 255), (byte)(c.Green * 255), (byte)(c.Blue * 255));
        SelectedColor = sk;

        if (Series.Count > 0)
        {
            if (ApplyColorToAll || Simulations == 1)
                foreach (var s in Series) s.Color = sk;
            else
                Series[0].Color = sk;

            Raise(nameof(Series));
            OnRedrawRequested?.Invoke();
        }
    });

    public SKColor[] Palette { get; } = new[]
    {
        SKColors.SteelBlue, SKColors.IndianRed, SKColors.SeaGreen, SKColors.SlateBlue,
        SKColors.DarkOrange, SKColors.MediumVioletRed, SKColors.Teal, SKColors.Goldenrod
    };

    public ObservableCollection<SeriesData> Series { get; } = new();

    public ICommand SimulateCommand { get; }

    private readonly IPricePathSimulator _simulator;

    public HomePageViewModel(IPricePathSimulator simulator)
    {
        _simulator = simulator;
        SimulateCommand = new Command(() =>
        {
            Series.Clear();

            var many = _simulator.GenerateMany(Simulations, Vol, Mean, InitialPrice, NumDays);
            for (int i = 0; i < many.Count; i++)
            {
                var pts = many[i];
                if (Normalize && pts.Length > 0)
                {
                    var baseVal = pts[0];
                    if (baseVal != 0)
                        pts = pts.Select(v => v / baseVal).ToArray();
                }

                Series.Add(new SeriesData
                {
                    Points = pts,
                    Color = (ApplyColorToAll || Simulations == 1) ? SelectedColor : Palette[i % Palette.Length],
                    Stroke = StrokeWidth
                });
            }

            Raise(nameof(Series));
            OnRedrawRequested?.Invoke();
        });
    }

    public event Action? OnRedrawRequested;
}
