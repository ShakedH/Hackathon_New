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
        private const string _defaultText = "Search in the video...";
        Program p = new Program(new APIClient());

        public VideoWindow()
        {
            InitializeComponent();
            searchBox.Foreground = Brushes.LightSlateGray;
            searchBox.Text = _defaultText;
            searchBox.VerticalContentAlignment = VerticalAlignment.Center;
            p.LoadFromFile(Environment.CurrentDirectory);
        }

        private DispatcherTimer timerVideoTime;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(0.1);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            minionPlayer.Play();
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
            searchBox.Foreground = Brushes.Black;
            if (searchBox.Text == _defaultText)
                searchBox.Text = string.Empty;
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GetSearchResults();
        }

        private void GetSearchResults()
        {
            GenerateColumns();
            string term = searchBox.Text.ToLower();
            List<TimeInVid> allTimes = p.SearchWord(term);
            List<string[]> itemsSource = new List<string[]>();
            foreach (TimeInVid tiv in allTimes)
            {
                string[] data = new string[2];
                data[0] = tiv.ToString();
                data[1] = p.GetSentence(term, tiv);
                itemsSource.Add(data);
            }
            txtSearchResults.Visibility = Visibility.Visible;
            txtSearchResults.ItemsSource = itemsSource;
        }

        private void GenerateColumns()
        {
            txtSearchResults.Columns.Clear();

            DataGridTextColumn col = new DataGridTextColumn();
            col.Header = "Time";
            col.Binding = new Binding("[0]");
            txtSearchResults.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Phrase";
            col.Binding = new Binding("[1]");
            txtSearchResults.Columns.Add(col);
            col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void txtSearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string[] selectedRow = (string[])(((DataGrid)sender).SelectedItems[0]);
            string timeToJump = selectedRow[0];
            minionPlayer.Position = TimeSpan.Parse(timeToJump);
            ShowPosition();
        }
    }
}
