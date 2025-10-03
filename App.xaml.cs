using FinanceApp.Views;

#if WINDOWS
using Microsoft.Maui.Platform;
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif

namespace FinanceApp
{
    public partial class App : Application
    {
        private readonly HomePage _homePage;

        public App(HomePage homePage)
        {
            InitializeComponent();
            _homePage = homePage;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var navPage = new NavigationPage(_homePage)
            {
                BarBackgroundColor = Color.FromArgb("#001861"),
                BarTextColor = Colors.White
            };

            var window = new Window(navPage);

#if WINDOWS
            window.Created += (s, e) =>
            {
                var mauiWindow   = (Microsoft.Maui.Controls.Window)s;
                var nativeWindow = mauiWindow.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
                if (nativeWindow is null) return;

                var hwnd     = WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                var appWindow = AppWindow.GetFromWindowId(windowId);
                if (appWindow is null) return;

                try
                {
                    appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                }
                catch
                {
                    if (appWindow.Presenter is OverlappedPresenter overlapped)
                    {
                        overlapped.Maximize();
                    }
                    else
                    {
                        // Fallback bem amplo: ocupa toda área de trabalho se nada acima funcionar
                        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
                        var workArea = displayArea.WorkArea;
                        appWindow.MoveAndResize(workArea);
                    }
                }
            };
#endif

            return window;
        }
    }
}
