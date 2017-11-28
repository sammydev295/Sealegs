using System;

using Sealegs.Utils;
using Sealegs.DataStore.Abstractions.SQLite;

namespace Sealegs.DataStore.Mock.Tables
{
    public class UserTable : BaseDB, IUserTable
    {
        #region CTOR

        public UserTable()
        {
            Connection.CreateTable<User>();
        }

        #endregion 

        #region InsertUser

        public void InsertUser(User user)
        {
            Connection.Insert(user);
        }

        #endregion

        #region GetUser

        public User GetUser()
        {
            try
            {
                User user = Connection.Table<User>().FirstOrDefault();
                return user;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region Update

        public bool Update(User user)
        {
            try
            {
                Connection.Update(user);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        #endregion

        #region DeleteAll

        public void DeleteAll()
        {
            Connection.DeleteAll<User>();
        }

        #endregion
    }
}
