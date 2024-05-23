using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(short categoryId);
        List<Category> GetCategories();
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short categoryId);
    }
}
