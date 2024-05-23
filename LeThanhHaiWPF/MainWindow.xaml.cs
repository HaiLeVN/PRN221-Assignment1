using BusinessObject.Models;
using LeThanhHaiWPF.StaffManagement;
using LeThanhHaiWPF.StaffUtilities;
using Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeThanhHaiWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly SystemAccount _currentAccountLogin;

        public MainWindow(ICategoryRepository categoryRepository, INewsArticleRepository newsArticleRepository, ISystemAccountRepository systemAccountRepository, bool contentLoaded, SystemAccount systemAccount)
        {
            InitializeComponent();
            _categoryRepository = categoryRepository;
            _newsArticleRepository = newsArticleRepository;
            _systemAccountRepository = systemAccountRepository;
            _contentLoaded = contentLoaded;
            _currentAccountLogin = systemAccount;
        }

        private void ListBoxManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxManagement.SelectedItem is ListBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Category Management":
                        contentControlManagement.Content = new CategoryManagementControl();
                        break;
                    case "News Article Management":
                        contentControlManagement.Content = new NewsArticleManagementControl(_currentAccountLogin);
                        break;
                    case "Profile Management":
                        //contentControlManagement.Content = new ProfileManagementControl();
                        break;
                    default:
                        contentControlManagement.Content = null;
                        break;
                }
            }
        }

        private void BtnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            var updateProfileWindow = new UpdatingProfileWindow(_currentAccountLogin);
            updateProfileWindow.ShowDialog();
        }

        private void BtnViewNewsHistory_Click(object sender, RoutedEventArgs e)
        {
            var newsHistory = GetNewsHistoryByAccountId(_currentAccountLogin.AccountId);
            var newsHistoryWindow = new NewsHistoryWindow(newsHistory);
            newsHistoryWindow.ShowDialog();
        }

        private IEnumerable<NewsArticle> GetNewsHistoryByAccountId(short accountId)
        {
            var newsArticleRepository = new NewsArticleRepository(); // Initialize with DI or service locator
            return newsArticleRepository.GetNewsArticleByCreatorId(accountId);
        }
    }
}
