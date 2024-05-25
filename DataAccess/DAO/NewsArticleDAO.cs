using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO instance;
        private static object instanceLock = new object();
        public static NewsArticleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleDAO();
                    }
                }
                return instance;
            }
        }

        public IEnumerable<NewsArticle> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.NewsArticles
                              .Where(na => na.CreatedDate >= startDate && na.CreatedDate <= endDate && na.NewsStatus == true) // Only get active articles
                              .OrderByDescending(na => na.CreatedDate)
                              .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error get news article by range: " + ex.Message);
            }
        }

        public List<NewsArticle> GetNewsArticles()
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.NewsArticles
                              .Include(s => s.Tags)
                              .Where(na => na.NewsStatus == true) // Only get active articles
                              .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error get news article: " + ex.Message);
            }
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.NewsArticles
                              .Include(s => s.Tags)
                              .Where(na => na.NewsStatus == true)
                              .FirstOrDefault(n => n.NewsArticleId.ToLower().Equals(id.ToLower()));
            } catch (Exception ex)
            {
                throw new Exception("Error get news article by id: " + ex.Message);
            }
        }

        public void CreateNewsArticle(NewsArticle newsArticle, ICollection<Tag> tags)
        {
            using var context = new FunewsManagementDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                foreach (var newTag in tags)
                {
                    var existingTag = context.Tags.FirstOrDefault(t => t.TagName == newTag.TagName);
                    if (existingTag != null)
                    {
                        // Tag already exists, associate it with the news article
                        newTag.TagId = existingTag.TagId; // Ensure newTag has the correct TagId
                        existingTag.NewsArticles.Add(newsArticle);
                    }
                    else
                    {
                        // Tag does not exist, add it to the database and associate it with the news article
                        context.Tags.Add(newTag);
                    }
                }

                // Add the news article
                context.NewsArticles.Add(newsArticle);
                context.SaveChanges();

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an exception occurs
                transaction.Rollback();
                throw new Exception("Error creating news article: " + ex.Message);
            }
        }


        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            using var context = new FunewsManagementDbContext();
            var existingArticle = context.NewsArticles.Include(a => a.Tags).SingleOrDefault(a => a.NewsArticleId == newsArticle.NewsArticleId);
            if (existingArticle != null)
            {
                // Update news article properties
                existingArticle.NewsTitle = newsArticle.NewsTitle;
                existingArticle.NewsContent = newsArticle.NewsContent;
                existingArticle.CategoryId = newsArticle.CategoryId;

                // Update associated tags
                var newTagIds = newsArticle.Tags.Select(t => t.TagId);
                var tagsToRemove = existingArticle.Tags.Where(t => !newTagIds.Contains(t.TagId)).ToList();
                foreach (var tagToRemove in tagsToRemove)
                {
                    existingArticle.Tags.Remove(tagToRemove);
                }
                foreach (var newTag in newsArticle.Tags)
                {
                    if (!existingArticle.Tags.Any(t => t.TagId == newTag.TagId))
                    {
                        var existingTag = context.Tags.Find(newTag.TagId);
                        if (existingTag != null)
                        {
                            existingArticle.Tags.Add(existingTag);
                        }
                        else
                        {
                            existingArticle.Tags.Add(newTag);
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        public void DeleteNewsArticle(string newsArticleId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                var newsArticle = context.NewsArticles
                                         .SingleOrDefault(n => n.NewsArticleId.ToLower().Equals(newsArticleId.ToLower()));

                if (newsArticle != null)
                {
                    // Soft deleting
                    newsArticle.NewsStatus = false;
                    context.NewsArticles.Update(newsArticle);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting news article: " + ex.Message);
            }
        }

        public List<NewsArticle> GetNewsArticleByCreatorId(short creatorId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                var articles = context.NewsArticles.Include(s => s.Tags).Include(s => s.Category).Where(a => a.CreatedById == creatorId).ToList();
                return articles;
            } catch (Exception ex)
            {
                throw new Exception("Error get news article by creator id: " + ex.Message);
            }
        }

        public List<NewsArticle> GetNewsArticlesByCategoryId(short categoryId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                var articles = context.NewsArticles.Where(a => a.CategoryId == categoryId).ToList();
                return articles;
            } catch (Exception ex)
            {
                throw new Exception("Error get news article by category id: " + ex.Message);
            }
        }
    }
}
