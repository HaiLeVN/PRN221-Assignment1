using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISystemAccountRepository
    {
        void Create(SystemAccount account);
        SystemAccount Read(short accountId);
        void Update(SystemAccount account);
        void Delete(short accountId);
        List<SystemAccount> Search(string keyword);
        List<SystemAccount> GetAll();
    }
}
