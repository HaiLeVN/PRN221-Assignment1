using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SystemAccountDAO
    {
        // Use Singleton Pattern
        private static SystemAccountDAO instance;
        private static object instanceLock = new object();
        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                }
                return instance;
            }
        }

        // CRUD + Search Methods
        public List<SystemAccount> GetAll()
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.SystemAccounts.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error get all system account: " + ex.Message);
            }
        }
        public void Create(SystemAccount account)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                context.SystemAccounts.Add(account);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating account: " + ex.Message);
            }
        }

        public SystemAccount Read(short accountId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.SystemAccounts.Find(accountId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading account: " + ex.Message);
            }
        }

        public void Update(SystemAccount account)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                context.SystemAccounts.Update(account);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating account: " + ex.Message);
            }
        }

        public void Delete(short accountId)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                var account = context.SystemAccounts.Find(accountId);
                if (account != null)
                {
                    context.SystemAccounts.Remove(account);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting account: " + ex.Message);
            }
        }

        public List<SystemAccount> Search(string keyword)
        {
            try
            {
                using var context = new FunewsManagementDbContext();
                return context.SystemAccounts
                    .Where(a => a.AccountName.Contains(keyword) || a.AccountEmail.Contains(keyword))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching accounts: " + ex.Message);
            }
        }

    }
}
