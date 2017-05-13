using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            // TODO Pseudo-code:
            // 1. In a different class: pre-process all videos we have and load a MainIndex.dat file to project
            // 2. MainIndex is a dictionary mapping: <term, VideoDetails> of all the videos
            // 3. Deserialize MainIndex.dat to m_MainIndex

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
            List<string> parsedSearchText = Parser.Parse(searchBox.Text);
            RetrieveResults(parsedSearchText);
            SearchResultsGrid.Visibility = Visibility.Visible;
        }

        private void RetrieveResults(List<string> parsedSearchText)
        {
            foreach (string term in parsedSearchText)
            {
                if (!m_MainIndex.ContainsKey(term))
                    continue;

                List<VideoDetails> vdList = m_MainIndex[term];

                foreach (VideoDetails vd in vdList)
                {
                    TextBlock nameTB = new TextBlock();
                    nameTB.Cursor = Cursors.Hand;
                    nameTB.Text = vd.Name;

                    TextBlock keywordsTB = new TextBlock();
                    keywordsTB.Text = string.Join(" ", vd.Keywords);

                    StackPanel record = new StackPanel();
                    record.Name = vd.Name;
                    record.PreviewMouseDown += Record_PreviewMouseDown;
                    record.Children.Add(nameTB);
                    record.Children.Add(keywordsTB);

                    SearchResultsStackPanel.Children.Add(record);
                }
            }

            if (SearchResultsStackPanel.Children.Count == 0)    // No results
            {
                TextBlock noResults = new TextBlock();
                noResults.Text = "Oops, we couldn't find videos that match your search...";
                SearchResultsStackPanel.Children.Add(noResults);
            }
        }

        private void Record_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string videoName = (sender as StackPanel).Name;
            new VideoWindow(videoName).Show();
        }

        private void advSearch_Click(object sender, MouseButtonEventArgs e)
        {
            advSearchGrid.Visibility = (advSearchGrid.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
            if (SearchResultsGrid.Visibility == Visibility.Visible)
                SearchResultsGrid.Visibility = Visibility.Hidden;
        }
    }
}
