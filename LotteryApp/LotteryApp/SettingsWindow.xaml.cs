using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace LotteryApp
{
    public sealed partial class SettingsWindow : Window
    {
        private MainWindow mainWindow;

        public SettingsWindow(MainWindow mainWnd)
        {
            this.InitializeComponent();
            this.mainWindow = mainWnd;
        }

        private void MarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (TopMarginSlider != null && LeftMarginSlider != null)
            {
                int topMargin = (int)TopMarginSlider.Value;
                int bottomMargin = 0;
                int leftMargin = (int)LeftMarginSlider.Value;
                int rightMargin = 0;

                mainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    mainWindow.UpdateBorderMargin(new Thickness(leftMargin, topMargin, rightMargin, bottomMargin));
                });
            }
        }

        private void SizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (WidthSlider != null && HeightSlider != null)
            {
                double Width = WidthSlider.Value;
                double Height = HeightSlider.Value;

                mainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    mainWindow.UpdateBorderSize(Width, Height);
                });
            }
        }

        private void TextColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            mainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                mainWindow.UpdateTextColor(new SolidColorBrush(args.NewColor));
            });
        }
    }
}
