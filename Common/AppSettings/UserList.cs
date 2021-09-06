using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;

namespace Amellar.Common.AppSettings
{
    public class UserList
    {
        private List<User> m_lstUsers;

        public UserList()
        {
            m_lstUsers = new List<User>();
            this.GetUserList();
        }

        public List<User> Users
        {
            get { return m_lstUsers; }
        }

        public void GetUserList()
        {
            m_lstUsers.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT usr_code, usr_ln, usr_fn, usr_mi FROM sys_users WHERE sys_code = 'C' ORDER BY usr_code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_lstUsers.Add(new User(result.GetString("usr_code").Trim(),
                        result.GetString("usr_ln").Trim(), result.GetString("usr_fn").Trim(),
                        result.GetString("usr_mi").Trim()));
                }
            }

            result.Close();
        }

        


    }
}
