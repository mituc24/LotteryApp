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

        private void ApplyRange_Btn_Click(object sender, RoutedEventArgs e)
        {
            int start = int.Parse(startNumber_TxtBox.Text);
            int end = int.Parse(endNumber_TxtBox.Text);
            mainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                mainWindow.ChangeRange(start, end);
            });
        }
    }
}
