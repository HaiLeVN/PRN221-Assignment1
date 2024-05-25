using BusinessObject.Models;
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

namespace LeThanhHaiWPF.StaffUtilities
{
    /// <summary>
    /// Interaction logic for NewsHistoryWindow.xaml
    /// </summary>
    public partial class NewsHistoryWindow : Window
    {
        private readonly IEnumerable<NewsArticle> _allNewsArticles;
        private IEnumerable<NewsArticle> _filteredNewsArticles;

        public NewsHistoryWindow(IEnumerable<NewsArticle> newsArticles)
        {
            InitializeComponent();
            _allNewsArticles = newsArticles;

            UpdateNewsHistoryDisplay();
        }

        private void UpdateNewsHistoryDisplay()
        {
            if (chkShowRemoved.IsChecked == true)
            {
                _filteredNewsArticles = _allNewsArticles;
            }
            else
            {
                _filteredNewsArticles = _allNewsArticles.Where(article => article.NewsStatus != false);
            }

            dataGridNewsHistory.ItemsSource = _filteredNewsArticles;
        }

        private void ChkShowRemoved_Checked(object sender, RoutedEventArgs e)
        {
            UpdateNewsHistoryDisplay();
        }

        private void ChkShowRemoved_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateNewsHistoryDisplay();
        }
    }
}
