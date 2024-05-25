using BusinessObject.Models;
using Repositories.Interfaces;
using Repositories;
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

namespace LeThanhHaiWPF.StaffManagement
{
    /// <summary>
    /// Interaction logic for NewsArticleManagementControl.xaml
    /// </summary>
    public partial class NewsArticleManagementControl : UserControl
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private List<NewsArticle> _newsArticles;
        private SystemAccount _currentAccountLogin;

        public NewsArticleManagementControl(SystemAccount account)
        {
            InitializeComponent();
            _newsArticleRepository = new NewsArticleRepository(); 
            LoadNewsArticles();
            _currentAccountLogin = account;
        }

        private void LoadNewsArticles()
        {
            _newsArticles = _newsArticleRepository.GetNewsArticles();
            dataGridNewsArticles.ItemsSource = _newsArticles;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateUpdateNewsArticleWindow(null, false, _currentAccountLogin);
            createWindow.NewsArticleUpdated += (s, ea) => LoadNewsArticles();
            createWindow.ShowDialog();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridNewsArticles.SelectedItem is NewsArticle selectedArticle)
            {
                var updateWindow = new CreateUpdateNewsArticleWindow(selectedArticle, true, _currentAccountLogin);
                updateWindow.NewsArticleUpdated += (s, ea) => LoadNewsArticles();
                updateWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a news article to update.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            if (dataGridNewsArticles.SelectedItem is NewsArticle selectedArticle)
            {
                if (MessageBox.Show("Are you sure want to delete this article?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    _newsArticleRepository.DeleteNewsArticle(selectedArticle.NewsArticleId);
                    LoadNewsArticles();
                }
            }
            else
            {
                MessageBox.Show("Please select a news article to delete.");
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            var filteredArticles = _newsArticles.Where(a => a.NewsTitle.ToLower().Contains(searchText) || a.NewsContent.ToLower().Contains(searchText)).ToList();
            dataGridNewsArticles.ItemsSource = filteredArticles;
        }

        private void DataGridNewsArticles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
