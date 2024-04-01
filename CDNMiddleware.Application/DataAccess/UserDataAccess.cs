using System;
using CDNMiddleware.Application.DataAccess.Interfaces;
using CDNMiddleware.Application.DataAccess;
using Microsoft.EntityFrameworkCore;
using CDNMiddleware.DataAccess;
using CDNMiddleware.DataAccess.Models;
using Microsoft.EntityFrameworkCore.Storage;
using CDNMiddleware.DataAccess.Helpers;

namespace CDNMiddleware.Application.DataAccess
{
	public class UserDataAccess : IUserDataAccess
	{
        private readonly IDbContextFactory<CDNMiddlewareDbContext> _dbContext;

        public UserDataAccess(IDbContextFactory<CDNMiddlewareDbContext> dbContext)
		{
            _dbContext = dbContext;
        }

        #region SELECT - Single
        public User? GetUser(Int64 id)
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                return dbContext.Users
                                .Where(user => user.ID == id)
                                .FirstOrDefault();
            }
        }

        public User? FindUserById(Int64 Id)
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                return dbContext.Users
                                .Where(user => user.ID == Id)
                                .FirstOrDefault();
            }
        }
        #endregion

        #region INSERT - Single
        public User? Store(User? user)
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    return user;
                }
            }
        }
        #endregion

        #region UPDATE - Single
        public User? Update(User user)
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    return user;
                }
            }
        }
        #endregion

        #region DELETE - Single
        public void Delete(User user)
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
            }
        }
        #endregion

        #region SELECT - Multiple
        public List<User>? GetUsers()
        {
            using (CDNMiddlewareDbContext dbContext = _dbContext.CreateDbContext())
            {
                var data =  dbContext.Users.ToList();
                data.ForEach(user => user.PhoneNumber = GlobalFunction.DecryptString(user.PhoneNumber));

                return data;
            }
        }
        #endregion
    }
}

