using BusinessObject.Models;
using Repositories.Interfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.IdentityModel.Tokens;

namespace LeThanhHaiWPF.StaffManagement
{
    public partial class CreateUpdateNewsArticleWindow : Window
    {
        public event EventHandler NewsArticleUpdated;

        private readonly NewsArticle _newsArticle;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private bool _IsUpdating;
        private SystemAccount _currentAccount;

        public CreateUpdateNewsArticleWindow(NewsArticle newsArticle, bool isUpdating, SystemAccount currentAccount)
        {
            InitializeComponent();
            _newsArticleRepository = new NewsArticleRepository(); // Initialize with DI or service locator
            _tagRepository = new TagRepository(); // Initialize with DI or service locator
            _categoryRepository = new CategoryRepository(); // Initialize with DI or service locator
            _IsUpdating = isUpdating;

            if (_IsUpdating)
            {
                txtNewsArticleId.IsReadOnly = true;
                _newsArticle = newsArticle;
                txtNewsArticleId.Text = newsArticle.NewsArticleId;
                txtTitle.Text = newsArticle.NewsTitle;
                txtContent.Text = newsArticle.NewsContent;
                PopulateTagsCheckBoxes(_tagRepository.GetAllTags(), newsArticle.Tags); // Pass article tags
                PopulateCategoryComboBox(newsArticle.CategoryId);
            }
            else
            {
                txtNewsArticleId.IsReadOnly = false;
                PopulateTagsCheckBoxes(_tagRepository.GetAllTags());
                PopulateCategoryComboBox();
            }
            _currentAccount = currentAccount;
        }

        private void PopulateTagsCheckBoxes(IEnumerable<Tag> allTags, IEnumerable<Tag> articleTags)
        {
            foreach (var tag in allTags)
            {
                var checkBox = new CheckBox
                {
                    Content = tag.TagName,
                    IsChecked = articleTags.Any(t => t.TagId == tag.TagId)
                };
                listBoxTags.Items.Add(checkBox);
            }
        }

        private void PopulateTagsCheckBoxes(IEnumerable<Tag> articleTags)
        {
            var allTags = _tagRepository.GetAllTags();
            PopulateTagsCheckBoxes(allTags, articleTags);
        }

        private void PopulateCategoryComboBox(short? selectedCategoryId = null)
        {
            var categories = _categoryRepository.GetCategories();
            cmbCategory.ItemsSource = categories;

            if (selectedCategoryId.HasValue)
            {
                cmbCategory.SelectedValue = selectedCategoryId.Value;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_IsUpdating)
                {
                    if (txtNewsArticleId.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("Field News Article ID is required", "Validator");
                        return;
                    }

                    if(txtTitle.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("News Title is required", "Validator");
                        return;
                    }

                    if (txtContent.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("News Content is required", "Validator");
                        return;
                    }

                    if(cmbCategory.SelectedValue == null)
                    {
                        MessageBox.Show("Please select one category", "Validator");
                        return;
                    }
                    var newsArticleId = txtNewsArticleId.Text.Trim();

                    // Check if the news article ID already exists
                    if (_newsArticleRepository.GetNewsArticleById(newsArticleId) != null)
                    {
                        MessageBox.Show($"News article with ID '{newsArticleId}' already exists. Please use a different ID.");
                        return;
                    }

                    // Create new news article
                    var newArticle = new NewsArticle
                    {
                        NewsArticleId = newsArticleId,
                        NewsTitle = txtTitle.Text,
                        NewsContent = txtContent.Text,
                        CreatedById = _currentAccount.AccountId,
                        NewsStatus = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CategoryId = (short)cmbCategory.SelectedValue
                    };
                    if (newArticle.Tags == null)
                    {
                        MessageBox.Show("Please select at least one tag.");
                        return;
                    }
                    _newsArticleRepository.CreateNewsArticle(newArticle, GetSelectedTags());
                }
                else
                {
                    if (txtTitle.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("News Title is required", "Validator");
                        return;
                    }

                    if (txtContent.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("News Content is required", "Validator");
                        return;
                    }
                    // Update existing news article
                    _newsArticle.NewsTitle = txtTitle.Text;
                    _newsArticle.NewsContent = txtContent.Text;
                    _newsArticle.Tags = GetSelectedTags();
                    _newsArticle.ModifiedDate = DateTime.Now;
                    _newsArticle.CategoryId = (short)cmbCategory.SelectedValue;
                    _newsArticleRepository.UpdateNewsArticle(_newsArticle);
                }

                NewsArticleUpdated?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private ICollection<Tag> GetSelectedTags()
        {
            var selectedTags = new List<Tag>();
            foreach (CheckBox checkBox in listBoxTags.Items)
            {
                if (checkBox.IsChecked == true)
                {
                    var tagName = checkBox.Content.ToString();
                    var existingTag = _tagRepository.GetTagByName(tagName);
                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }
            return selectedTags;
        }
    }
}
