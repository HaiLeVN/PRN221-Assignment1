using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        IEnumerable<NewsArticle> GetReportByDateRange(DateTime startDate, DateTime endDate);
        List<NewsArticle> GetNewsArticles();
        NewsArticle GetNewsArticleById(string id);
        void CreateNewsArticle(NewsArticle newsArticlem, ICollection<Tag> tags);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(string newsArticleId);
        List<NewsArticle> GetNewsArticleByCreatorId(short creatorId); 
        List<NewsArticle> GetNewsArticlesByCategoryId(short categoryId);
    }
}
