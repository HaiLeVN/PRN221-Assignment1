using BusinessObject.Models;
using Repositories.Interfaces;
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

namespace LeThanhHaiWPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AdminWindow(ISystemAccountRepository accountRepository, INewsArticleRepository newsArticleRepository, ICategoryRepository categoryRepository)
        {
            InitializeComponent();
            _accountRepository = accountRepository;
            _newsArticleRepository = newsArticleRepository;
            _categoryRepository = categoryRepository;
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            var accounts = _accountRepository.GetAll();
            AccountsDataGrid.ItemsSource = accounts;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchQuery = SearchTextBox.Text;
            var accounts = _accountRepository.Search(searchQuery);
            AccountsDataGrid.ItemsSource = accounts;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var accountWindow = new AccountWindow(_accountRepository, null, false);
            bool? result = accountWindow.ShowDialog();
            if (result == true)
            {
                LoadAccounts();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is SystemAccount selectedAccount)
            {
                var accountWindow = new AccountWindow(_accountRepository, selectedAccount, true);
                bool? result = accountWindow.ShowDialog();
                if (result == true)
                {
                    LoadAccounts();
                }
            }
            else
            {
                MessageBox.Show("Please select an account to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is SystemAccount selectedAccount)
            {
                if (MessageBox.Show("Are you sure want to delete this account?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    _accountRepository.Delete(selectedAccount.AccountId);
                    LoadAccounts();
                }
            }
            else
            {
                MessageBox.Show("Please select an account to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            if (startDate > endDate)
            {
                MessageBox.Show("End date must be equal or higher than start date", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                var newsArticles = _newsArticleRepository.GetReportByDateRange(startDate.Value, endDate.Value);
                foreach (var article in newsArticles)
                {
                    // Fetch category name for each news article
                    if (article.CategoryId.HasValue)
                    {
                        var category = _categoryRepository.GetCategoryById(article.CategoryId.Value);
                        if (category != null)
                            article.Category = category;
                    }
                }
                NewsArticlesDataGrid.ItemsSource = newsArticles;
            }
            else
            {
                MessageBox.Show("Please select both start date and end date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
