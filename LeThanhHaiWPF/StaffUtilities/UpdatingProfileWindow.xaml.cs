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
using System.Windows.Shapes;
using Microsoft.IdentityModel.Tokens;

namespace LeThanhHaiWPF.StaffUtilities
{
    /// <summary>
    /// Interaction logic for UpdatingProfileWindow.xaml
    /// </summary>
    public partial class UpdatingProfileWindow : Window
    {
        private readonly SystemAccount _currentAccount;
        private readonly ISystemAccountRepository _systemAccountRepository;

        public UpdatingProfileWindow(SystemAccount currentAccount)
        {
            InitializeComponent();
            _currentAccount = currentAccount;
            _systemAccountRepository = new SystemAccountRepository(); // Initialize with DI or service locator

            // Load current account information
            txtUserId.Text = _currentAccount.AccountId.ToString();
            txtName.Text = _currentAccount.AccountName;
            txtEmail.Text = _currentAccount.AccountEmail;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Field Name cannot be leave null here", "Validator", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtEmail.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("Field Email cannot be leave null here", "Validator", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Update account information
                _currentAccount.AccountName = txtName.Text;
                _currentAccount.AccountEmail = txtEmail.Text;

                _systemAccountRepository.Update(_currentAccount);

                MessageBox.Show("Profile updated successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
