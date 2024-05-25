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
    public class TagRepository : ITagRepository
    {
        public Tag GetTagByName(string tagName) => TagDAO.Instance.GetTagByName(tagName);

        public void CreateTag(Tag tag) => TagDAO.Instance.CreateTag(tag);

        public List<Tag> GetAllTags() => TagDAO.Instance.GetAll();
    }
}
