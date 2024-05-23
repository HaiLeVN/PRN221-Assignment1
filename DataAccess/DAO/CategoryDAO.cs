using BusinessObject.Models;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        // Use Singleton pattern
        private static CategoryDAO instance;
        private static object instanceLock = new object();
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                }
                return instance;
            }
        }

        public Category GetCategoryById(short categoryId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            } catch (Exception ex)
            {
                throw new Exception("Error get category: " + ex.Message);
            }
        }

        public List<Category> GetCategories()
        {
            using var context = new FunewsManagementDbContext();
            return context.Categories.ToList();
        }

        public void CreateCategory(Category category)
        {
            using var context = new FunewsManagementDbContext();
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            using var context = new FunewsManagementDbContext();
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void DeleteCategory(short categoryId)
        {
            using var context = new FunewsManagementDbContext();
            var category = context.Categories.Find(categoryId);
            if (category != null && !context.NewsArticles.Any(na => na.CategoryId == categoryId))
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Cannot delete category that is used in news articles.");
            }
        }
    }
}