using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.cloud.register.Commons
{
    public static class Configuration
    {
        #region attributes
        private static string _usersTableName;
        #endregion

        #region properties
        public static string USERS_TABLE_NAME
        {
            get 
            {
                if (_usersTableName == null)
                {
                    _usersTableName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("USERS_TABLE_NAME"))
                        ?
                        "Challenge_Cloud_Users"
                        :
                        Environment.GetEnvironmentVariable("USERS_TABLE_NAME");                
                }
                return _usersTableName; 
            }
        }
        #endregion
    }
}
