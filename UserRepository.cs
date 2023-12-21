using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    interface CRUD
    {
        void CreateUser(User user);
        User FindUser(string username);
    }
    class UserRepository : CRUD
    {
        MoviedbEntities5 entities;
        public UserRepository()
        {
            entities = new MoviedbEntities5();
        }
        public void CreateUser(User newUser)
        {
            entities.Users.Add(newUser);
            entities.SaveChanges();
        }
        public User FindUser(string username)
        {
            //return entities.Users.First(x=>x.Username==username.ToString());
            return entities.Users.Find(username.ToString());
        }
    }
}
