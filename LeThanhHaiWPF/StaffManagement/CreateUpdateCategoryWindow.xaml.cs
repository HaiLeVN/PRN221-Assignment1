using BusinessObject.Models;
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
using System.Windows.Shapes;

namespace LeThanhHaiWPF.StaffManagement
{
    /// <summary>
    /// Interaction logic for CreateUpdateCategoryWindow.xaml
    /// </summary>
    public partial class CreateUpdateCategoryWindow : Window
    {
        private readonly ICategoryRepository _categoriesRepository;
        private readonly Category _categoryToUpdate;
        private bool _isUpdating;

        public CreateUpdateCategoryWindow(Category? categoryToUpdate, bool isUpdating)
        {
            InitializeComponent();
            _categoriesRepository = new CategoryRepository();
            _categoryToUpdate = categoryToUpdate;

            if (_categoryToUpdate != null)
            {
                txtCategoryName.Text = _categoryToUpdate.CategoryName;
                txtDescription.Text = _categoryToUpdate.CategoryDesciption;
            }
            _isUpdating = isUpdating;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = txtCategoryName.Text.Trim();
            var description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButton.OK);
                return;
            }

            if (!_isUpdating)
            {
                var newCategory = new Category
                {
                    CategoryName = categoryName,
                    CategoryDesciption = description
                };
                _categoriesRepository.CreateCategory(newCategory);
            }
            else
            {
                _categoryToUpdate.CategoryName = categoryName;
                _categoryToUpdate.CategoryDesciption = description;
                _categoriesRepository.UpdateCategory(_categoryToUpdate);
            }

            DialogResult = true;
            Close();
        }
    }
}
