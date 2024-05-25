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
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public void Create(SystemAccount account)
        {
            SystemAccountDAO.Instance.Create(account);
        }

        public SystemAccount Read(short accountId)
        {
            return SystemAccountDAO.Instance.Read(accountId);
        }

        public void Update(SystemAccount account)
        {
            SystemAccountDAO.Instance.Update(account);
        }

        public void Delete(short accountId)
        {
            SystemAccountDAO.Instance.Delete(accountId);
        }

        public List<SystemAccount> Search(string keyword)
        {
            return SystemAccountDAO.Instance.Search(keyword);
        }

        public List<SystemAccount> GetAll()
        {
            return SystemAccountDAO.Instance.GetAll();
        }
    }
}
