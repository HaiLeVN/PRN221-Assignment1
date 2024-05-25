using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public void CreateCategory(Category category) => CategoryDAO.Instance.CreateCategory(category);
        public void DeleteCategory(short categoryId) => CategoryDAO.Instance.DeleteCategory(categoryId);

        public List<Category> GetCategories() => CategoryDAO.Instance.GetCategories();

        public Category GetCategoryById(short categoryId) => CategoryDAO.Instance.GetCategoryById(categoryId);

        public void UpdateCategory(Category category) => CategoryDAO.Instance.UpdateCategory(category);
    }
}
