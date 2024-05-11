using System;
using System.Threading.Tasks;
using System.Windows;

namespace ToastLikeNotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int DISPLAY_TIME = 2000;
        const int FADEOUT_TIME = 500;

        public MainWindow()
        {
            InitializeComponent();

            textBlock.Text = string.Join(" ", Environment.GetCommandLineArgs()[1..]);
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Top = SystemParameters.WorkArea.Bottom - Height - 200;
            Left = (SystemParameters.WorkArea.Right - Width) / 2;

            await Task.Delay(DISPLAY_TIME);
            FadeOut();
        }

        private async void FadeOut()
        {
            while (Opacity > 0)
            {
                Opacity -= 0.01;
                await Task.Delay((int)(FADEOUT_TIME / (1 / 0.01)));
            }

            Close();
        }
    }
}
