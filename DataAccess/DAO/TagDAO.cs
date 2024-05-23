using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class TagDAO
    {
        private static TagDAO instance;
        private static object instanceLock = new object();

        public static TagDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TagDAO();
                    }
                }
                return instance;
            }
        }

        public List<Tag> GetAll()
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.Tags.Include(s => s.NewsArticles).ToList(); 
            } catch (Exception ex)
            {
                throw new Exception("Error get List of tag: " + ex.Message);
            }
        }

        public Tag GetTagByName(string tagName)
        {
            using var context = new FunewsManagementDbContext();
            return context.Tags.Include(s => s.NewsArticles).SingleOrDefault(t => t.TagName == tagName);
        }

        public void CreateTag(Tag tag)
        {
            using var context = new FunewsManagementDbContext();
            context.Tags.Add(tag);
            context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            using var context = new FunewsManagementDbContext();
            context.Tags.Update(tag);
            context.SaveChanges();
        }
    }
}
