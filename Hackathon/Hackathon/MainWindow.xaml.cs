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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hackathon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _defaultText = "Search in our videos...";
        public MainWindow()
        {
            InitializeComponent();
            //SearchScreen.Visibility = Visibility.Visible;
            //ResultsScreen.Visibility = Visibility.Hidden;
            SearchScreen.Visibility = Visibility.Hidden;
            ResultsScreen.Visibility = Visibility.Visible;
            searchBox.Foreground = Brushes.LightSlateGray;
            searchBox.Text = _defaultText;
            searchBox.VerticalContentAlignment = VerticalAlignment.Center;
        }

        private void searchBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            searchBox.Foreground = Brushes.Black;
            if (searchBox.Text == _defaultText)
                searchBox.Text = string.Empty;
        }
    }
}
