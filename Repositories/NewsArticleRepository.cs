
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
    public class NewsArticleRepository : INewsArticleRepository
    {
        public IEnumerable<NewsArticle> GetReportByDateRange(DateTime startDate, DateTime endDate)
        {
            return NewsArticleDAO.Instance.GetByDateRange(startDate, endDate);
        }

        public List<NewsArticle> GetNewsArticles() => NewsArticleDAO.Instance.GetNewsArticles();

        public void CreateNewsArticle(NewsArticle newsArticle, ICollection<Tag> tags) => NewsArticleDAO.Instance.CreateNewsArticle(newsArticle, tags);

        public void UpdateNewsArticle(NewsArticle newsArticle) => NewsArticleDAO.Instance.UpdateNewsArticle(newsArticle);

        public void DeleteNewsArticle(string newsArticleId) => NewsArticleDAO.Instance.DeleteNewsArticle(newsArticleId);

        public NewsArticle GetNewsArticleById(string id) => NewsArticleDAO.Instance.GetNewsArticleById(id);

        public List<NewsArticle> GetNewsArticleByCreatorId(short creatorId) => NewsArticleDAO.Instance.GetNewsArticleByCreatorId(creatorId);

        public List<NewsArticle> GetNewsArticlesByCategoryId(short categoryId) => NewsArticleDAO.Instance.GetNewsArticlesByCategoryId(categoryId);
    }
}
