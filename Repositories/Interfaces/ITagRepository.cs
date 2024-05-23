using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        Tag GetTagByName(string tagName);
        void CreateTag(Tag tag);
    }
}
