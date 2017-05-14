using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Hackathon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        private const string _defaultText = "Search our videos...";
        private Dictionary<string, List<VideoDetails>> m_MainIndex = new Dictionary<string, List<VideoDetails>>();
        #endregion

        public MainWindow()
        {
            
            // Set Font
          
            InitializeComponent();
            
            HandleDesign();
            HandleLogic();
        }

        private void HandleDesign()
        {
            searchBox.Foreground = Brushes.LightSlateGray;
            searchBox.Text = _defaultText;
            searchBox.VerticalContentAlignment = VerticalAlignment.Center;
            advSearchGrid.Visibility = Visibility.Hidden;
            SearchResultsGrid.Visibility = Visibility.Hidden;
        }

        private void HandleLogic()
        {
            byte[] index = Properties.Resources.MainIndex;
            using (Stream stream = new MemoryStream(index))
            {
                BinaryFormatter reader = new BinaryFormatter();
                m_MainIndex = (Dictionary<string, List<VideoDetails>>)reader.Deserialize(stream);
            }
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
                searchBtn_Click(sender, null);
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchTerms = string.Empty;
            if (searchBox.Text != _defaultText)
                searchTerms = searchBox.Text;
            List<string> parsedSearchText = Parser.Parse(searchTerms);
            RetrieveResults(parsedSearchText);
            if (advSearchGrid.Visibility == Visibility.Visible)
                advSearchGrid.Visibility = Visibility.Hidden;
            SearchResultsGrid.Visibility = Visibility.Visible;
        }

        private void RetrieveResults(List<string> parsedSearchText)
        {
            SearchResultsStackPanel.Children.Clear();
            foreach (string term in parsedSearchText)
            {
                if (!m_MainIndex.ContainsKey(term))
                    continue;

                List<VideoDetails> vdList = m_MainIndex[term];

                foreach (VideoDetails vd in vdList)
                {
                    TextBlock nameTB = new TextBlock();
                    nameTB.Text = vd.Name;
                    nameTB.Cursor = Cursors.Hand;
                    nameTB.FontSize = 20;
                    nameTB.FontWeight = FontWeights.Bold;
                    nameTB.TextDecorations = TextDecorations.Underline;
                    nameTB.Foreground = Brushes.Blue;

                    TextBlock keywordsTB = new TextBlock();
                    keywordsTB.Inlines.Add(new Run("Keywords:")
                    {
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        TextDecorations = TextDecorations.Underline
                    });
                    keywordsTB.Inlines.Add(new Run(" " + string.Join(", ", vd.Keywords))
                    {
                        FontSize = 16
                    });

                    StackPanel record = new StackPanel();
                    record.Name = vd.Name;
                    record.PreviewMouseDown += Record_PreviewMouseDown;
                    record.Children.Add(nameTB);
                    record.Children.Add(keywordsTB);
                    record.Margin = new Thickness(0, 0, 0, 15);

                    SearchResultsStackPanel.Children.Add(record);
                }
            }

            if (SearchResultsStackPanel.Children.Count == 0)    // No results
            {
                TextBlock noResults = new TextBlock();
                noResults.FontSize = 16;
                noResults.FontWeight = FontWeights.Bold;
                noResults.Text = "Oops, we couldn't find videos that match your search...";
                SearchResultsStackPanel.Children.Add(noResults);
            }
        }

        private void Record_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string videoName = (sender as StackPanel).Name;
            VideoWindow vidWin = new VideoWindow(videoName);
            vidWin.Show();
            vidWin.Topmost = true;
        }

        private void advSearch_Click(object sender, MouseButtonEventArgs e)
        {
            advSearchGrid.Visibility = (advSearchGrid.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            if (SearchResultsGrid.Visibility == Visibility.Visible)
                SearchResultsGrid.Visibility = Visibility.Hidden;
        }
    }
}
