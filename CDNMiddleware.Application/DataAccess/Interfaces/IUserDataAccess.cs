using System;
using CDNMiddleware.DataAccess.Models;

namespace CDNMiddleware.Application.DataAccess.Interfaces
{
	public interface IUserDataAccess
	{
        #region SELECT - Single
        User? GetUser(Int64 id);
        User? FindUserById(Int64 Id);
        #endregion

        #region INSERT - Single
        User? Store(User? user);
        #endregion

        #region UPDATE - Single
        User? Update(User user);
        #endregion

        #region DELETE - Single
        void Delete(User user);
        #endregion

        #region SELECT - Multiple
        List<User>? GetUsers();
        #endregion
    }
}

