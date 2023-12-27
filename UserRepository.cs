using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieTracker
{
    interface CRUD
    {
        void CreateUser(User user);
        User FindUser(string username, string password);
        void AddMovie (Movie newMovie);
        decimal GetMaxUserID();
        decimal GetMaxMovieID();
        User FindUserID(decimal ID);
    }
    class UserRepository : CRUD
    {
        MoviesdbEntities entities;
        public UserRepository()
        {
            entities = new MoviesdbEntities();
        }
        public void CreateUser(User newUser)
        {
            entities.Users.Add(newUser);
            entities.SaveChanges();
        }
        public User FindUser(string username, string password)
        {
            try
            {
                return entities.Users.First(x => x.Username == username && x.Password == password );
                
            }
            catch (Exception ex)
            {
                return null;
            }
            //return entities.Users.Find(username.ToString());
        }
         public void AddMovie(Movie newMovie)
        {

            entities.Movies.Add(newMovie);
            entities.SaveChanges();
        }
        public decimal GetMaxUserID()
        {
            
            return entities.Users.Max(x => x.UserID);
            
        }
        public decimal GetMaxMovieID()
        {
            return entities.Movies.Max(x => x.MovieID);
        }
        public User FindUserID(decimal ID)
        {
            return entities.Users.Find(ID);
        }

    }
}
