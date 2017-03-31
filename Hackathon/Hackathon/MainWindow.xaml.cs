using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Hackathon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _defaultText = "Search in our lectures...";

        public MainWindow()
        {
            InitializeComponent();
            searchBox.Foreground = Brushes.LightSlateGray;
            searchBox.Text = _defaultText;
            searchBox.VerticalContentAlignment = VerticalAlignment.Center;
            advSearchGrid.Visibility = Visibility.Hidden;
        }

        private void searchBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            searchBox.Foreground = Brushes.Black;
            if (searchBox.Text == _defaultText)
                searchBox.Text = string.Empty;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(2000);
            ResultsPic.Visibility = Visibility.Visible;
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                searchBtn_Click(sender, null);
        }

        private void advSearch_Click(object sender, MouseButtonEventArgs e)
        {
            advSearchGrid.Visibility = (advSearchGrid.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void Pic_Click(object sender, MouseButtonEventArgs e)
        {
            new VideoWindow().Show();
        }
    }
}
