using BusinessObject.Models;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LeThanhHaiWPF
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private readonly ISystemAccountRepository _accountRepository;
        private SystemAccount _account;
        private bool IsUpdate;

        public AccountWindow(ISystemAccountRepository accountRepository, SystemAccount account, bool isUpdate)
        {
            InitializeComponent();
            _accountRepository = accountRepository;
            _account = account;
            if (_account != null)
            {
                LoadAccountData();
            }
            IsUpdate = isUpdate;
        }

        private void LoadAccountData()
        {
            NameTextBox.Text = _account.AccountName;
            EmailTextBox.Text = _account.AccountEmail;
            PasswordBox.Password = _account.AccountPassword;
            RoleComboBox.SelectedIndex = _account.AccountRole == 1 ? 0 : 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate that no fields are empty
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _account = _account ?? new SystemAccount();

            _account.AccountName = NameTextBox.Text;
            _account.AccountEmail = EmailTextBox.Text;
            _account.AccountPassword = PasswordBox.Password;
            _account.AccountRole = ((ComboBoxItem)RoleComboBox.SelectedItem).Tag.ToString() == "1" ? 1 : 2;

            if (!IsUpdate)
            {
                int maxAccountId = _accountRepository.GetAll().Max(a => a.AccountId);
                // Set the new AccountID
                _account.AccountId = (short)(maxAccountId + 1);

                _accountRepository.Create(_account);
            }
            else
            {
                _accountRepository.Update(_account);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regular expression for validating an Email
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
