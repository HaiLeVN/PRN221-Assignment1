using BusinessObject.Models;
using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for CategoryManagementControl.xaml
    /// </summary>
    public partial class CategoryManagementControl : UserControl
    {
        private readonly ICategoryRepository _categoriesRepository;
        private readonly INewsArticleRepository _newsArticleRepository;
        private List<Category> _categories;

        public CategoryManagementControl()
        {
            InitializeComponent();
            _categoriesRepository = new CategoryRepository();
            _newsArticleRepository = new NewsArticleRepository();
            LoadCategories();
        }

        private void LoadCategories()
        {
            _categories = _categoriesRepository.GetCategories();
            dataGridCategories.ItemsSource = _categories;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var createCategoryWindow = new CreateUpdateCategoryWindow(null, false);
            if (createCategoryWindow.ShowDialog() == true)
            {
                LoadCategories();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCategories.SelectedItem is Category selectedCategory)
            {
                var updateCategoryWindow = new CreateUpdateCategoryWindow(selectedCategory, true);
                if (updateCategoryWindow.ShowDialog() == true)
                {
                    LoadCategories();
                }
            }
            else
            {
                MessageBox.Show("Please select a category to update.", "Update Category", MessageBoxButton.OK);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCategories.SelectedItem is Category selectedCategory)
            {
                if (MessageBox.Show("Are you sure want to delete this category?", "Delete", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    if (!_newsArticleRepository.GetNewsArticlesByCategoryId(selectedCategory.CategoryId).IsNullOrEmpty())
                    {
                        MessageBox.Show("This category is being used by news articles.", "Delete", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    try
                    {
                        _categoriesRepository.DeleteCategory(selectedCategory.CategoryId);
                        LoadCategories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Delete Category", MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.", "Delete Category", MessageBoxButton.OK);
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            var filteredCategories = _categories.Where(c => c.CategoryName.ToLower().Contains(searchText)).ToList();
            dataGridCategories.ItemsSource = filteredCategories;
        }

        private void DataGridCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCategories.SelectedItem is Category selectedCategory)
            {
                txtCategoryName.Text = selectedCategory.CategoryName;
                txtDescription.Text = selectedCategory.CategoryDesciption;
            }
        }
    }
}