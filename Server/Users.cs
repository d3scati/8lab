using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Users
    {
        public List<User> UsersList { get; set;} = new List<User>();
        public Users() 
        {
            //UsersList.Add(new User("FirstUser", "password1", "defaultUser"));
            //UsersList.Add(new User("SecondUser", "password2", "defaultUser"));
            //UsersList.Add(new User("ThirdUser", "password3", "defaultUser"));
            //UsersList.Add(new User("Owner", "9934", "administrator"));
        }
    }
}
