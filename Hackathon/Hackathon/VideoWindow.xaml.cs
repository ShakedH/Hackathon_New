using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Hackathon
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            InitializeComponent();
        }

        private DispatcherTimer timerVideoTime;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(0.1);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            minionPlayer.Stop();
        }

        private void minionPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            sbarPosition.Minimum = 0;
            sbarPosition.Maximum = minionPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            sbarPosition.Visibility = Visibility.Visible;
            Duration.Text = minionPlayer.NaturalDuration.TimeSpan.ToString();
        }

        private void ShowPosition()
        {
            sbarPosition.Value = minionPlayer.Position.TotalSeconds;
            txtPosition.Text = string.Format("{0:hh\\:mm\\:ss}", minionPlayer.Position);
        }

        private void EnableButtons(bool is_playing)
        {
            if (is_playing)
            {
                btnPlay.IsEnabled = false;
                btnPause.IsEnabled = true;
                btnPlay.Opacity = 0.5;
                btnPause.Opacity = 1.0;
            }
            else
            {
                btnPlay.IsEnabled = true;
                btnPause.IsEnabled = false;
                btnPlay.Opacity = 1.0;
                btnPause.Opacity = 0.5;
            }
            timerVideoTime.IsEnabled = is_playing;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Play();
            EnableButtons(true);
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Pause();
            EnableButtons(false);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Stop();
            EnableButtons(false);
            ShowPosition();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Stop();
            minionPlayer.Play();
            EnableButtons(true);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Position += TimeSpan.FromSeconds(10);
            ShowPosition();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            minionPlayer.Position -= TimeSpan.FromSeconds(10);
            ShowPosition();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ShowPosition();
        }

        private void searchBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
