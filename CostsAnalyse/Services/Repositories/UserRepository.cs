using CostsAnalyse.Models;
using CostsAnalyse.Models.Context;
using CostsAnalyse.Services.Abstracts;
using System;


namespace CostsAnalyse.Services.Repositories
{
    public class UserRepository<User> : IRepository<Models.User>
    {
        private readonly UserContext _appContext;
        public UserRepository(UserContext appContext)
        {
            this._appContext = appContext;
        }

        public bool Add(Models.User item)
        {
            try
            {
                this._appContext.Users.Add(item);
                this._appContext.SaveChanges(); return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(Models.User item)
        {
            try
            {
                this._appContext.Users.Remove(item);
                this._appContext.SaveChanges(); return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool Update(Models.User item)
        {
            try
            { 
                this._appContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Models.User IRepository<Models.User>.Get()
        {
            throw new NotImplementedException();
        }
    }
}
