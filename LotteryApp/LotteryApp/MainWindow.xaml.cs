using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using WinRT.Interop;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Media.Imaging;

namespace LotteryApp
{
    public sealed partial class MainWindow : Window
    {
        private Random random = new Random();
        private CancellationTokenSource cts1 = new CancellationTokenSource();
        private CancellationTokenSource cts2 = new CancellationTokenSource();
        private CancellationTokenSource cts3 = new CancellationTokenSource();
        
        private AppWindow _appWindow;
        public int startNumber = 1;
        public int endNumber = 500;

        public MainWindow()
        {
            this.InitializeComponent();
            _appWindow = GetAppWindowForCurrentWindow();
        }

        private async void SpinButton_Click(object sender, RoutedEventArgs e)
        {
            int randomNum = random.Next(startNumber, endNumber+1);
            string numString = randomNum.ToString().PadLeft(3, '0');

            // Check if any column is spinning and stop them in sequence
            if (!cts1.Token.IsCancellationRequested)
            {
                cts1.Cancel();
                await SlowDownColumn(Column1, ""+numString[0]);
            }
            else if (!cts2.Token.IsCancellationRequested)
            {
                cts2.Cancel();
                await SlowDownColumn(Column2, "" + numString[1]);
            }
            else if (!cts3.Token.IsCancellationRequested)
            {
                cts3.Cancel();
                await SlowDownColumn(Column3, "" + numString[2]);
            }
            else
            {
                // Reset the CancellationTokenSource for each column
                cts1 = new CancellationTokenSource();
                cts2 = new CancellationTokenSource();
                cts3 = new CancellationTokenSource();

                // Start spinning all columns at the same speed
                _ = SpinColumn(Column1, cts1.Token);
                _ = SpinColumn(Column2, cts2.Token);
                _ = SpinColumn(Column3, cts3.Token);
            }
        }

        private async Task SpinColumn(Image column, CancellationToken token)
        {
            int interval = 40; // Initial interval for a smoother animation

            while (!token.IsCancellationRequested)
            {
                column.Source = new BitmapImage(new Uri("ms-appx:///Assets/Numbers/" + random.Next(0, 10) + ".png"));
                await Task.Delay(interval);
            }
        }

        private async Task SlowDownColumn(Image column, string num)
        {
            int duration = 2000; // Duration for the slowdown in milliseconds
            int elapsed = 0;
            int startInterval = 20; // Start interval for the slowdown
            int endInterval = 500; // End interval for the slowdown

            while (elapsed < duration)
            {
                double progress = (double)elapsed / duration;
                double sineValue = Math.Sin(progress * Math.PI / 2); // Sine function for smooth deceleration
                int interval = (int)(startInterval + (endInterval - startInterval) * sineValue);
                
                column.Source = new BitmapImage(new Uri("ms-appx:///Assets/Numbers/" + random.Next(0, 10) + ".png"));
                await Task.Delay(interval);

                elapsed += interval;
            }
            column.Source = new BitmapImage(new Uri("ms-appx:///Assets/Numbers/" + num + ".png"));
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e) {
            SettingsWindow settingsWindow = new SettingsWindow(this);
            settingsWindow.Activate();
        }

        public void UpdateBorderMargin(Thickness newPadding)
        {
            AdjustableBorder.Margin = newPadding;
        }

        public void UpdateBorderSize(double newWidth, double newHeight)
        {
            AdjustableBorder.Width = newWidth;
            AdjustableBorder.Height = newHeight;
        }

        private void Collapse_Click(object sender, RoutedEventArgs e)
        {
            Toolbar.Visibility = Visibility.Collapsed;
            Expand.Visibility = Visibility.Visible;
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            Expand.Visibility = Visibility.Collapsed;
            Toolbar.Visibility = Visibility.Visible;
        }

        public void ChangeRange(int start, int end)
        {
            startNumber = start;
            endNumber = end;
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (_appWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
            {
                _appWindow.SetPresenter(AppWindowPresenterKind.Default);
                FullScreenFontIcon.Glyph = "\uEE49";
            }
            else
            {
                _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                FullScreenFontIcon.Glyph = "\uEE47";
            }
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F11)
            {
                FullScreenButton_Click(sender, e);
            }
            else if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SpinButton_Click(sender, e);
            }
        }
    }

    
}
