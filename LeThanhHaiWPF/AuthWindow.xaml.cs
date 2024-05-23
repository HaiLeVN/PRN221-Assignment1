using BusinessObject.Models;
using Microsoft.Extensions.Configuration;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly string _adminEmail;
        private readonly string _adminPassword;
        public AuthWindow()
        {
            InitializeComponent();
            _accountRepository = new SystemAccountRepository();
            _newsArticleRepository = new NewsArticleRepository();
            _categoryRepository = new CategoryRepository();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _adminEmail = config["AdminCredentials:Email"];
            _adminPassword = config["AdminCredentials:Password"];
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            loginButton.IsEnabled = false;

            if (email == _adminEmail && password == _adminPassword)
            {
                OpenSpecificWindow(new SystemAccount { AccountRole = 3 });
            }
            else
            {
                var account = _accountRepository.GetAll().FirstOrDefault(a => a.AccountEmail == email && a.AccountPassword == password);

                if (account != null)
                {
                    OpenSpecificWindow(account);
                }
                else
                {
                    ErrorTextBlock.Visibility = Visibility.Visible;
                    loginButton.IsEnabled = true;
                }
            }
        }

        private void OpenSpecificWindow(SystemAccount account)
        {
            loginButton.IsEnabled = true;
            switch (account.AccountRole)
            {
                case 1: //  Staff role
                    if (MessageBox.Show("Login as staff successfully.", "Login", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        var mainWindow = new MainWindow(_categoryRepository, _newsArticleRepository, _accountRepository, false, account);
                        mainWindow.Show();
                        this.Close();
                    }
                    break;
                case 2: // Lecturer role
                    MessageBox.Show("Login success. Lecturer role auth");
                    break;
                case 3: // Admin role
                    if (MessageBox.Show("Login as admin successfully.", "Login", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        var adminWindow = new AdminWindow(_accountRepository, _newsArticleRepository, _categoryRepository);
                        adminWindow.Show();
                        this.Close();
                    }
                    break;
                default:
                    MessageBox.Show("Invalid role.");
                    return;
            }
            this.Close();
        }
    }
}
