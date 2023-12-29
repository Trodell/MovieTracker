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
        void AddUserMovies(UserMovies userMovies);
        List<Movie> GetUserMovies(decimal userID);
        void DeleteMovie(UserMovies movie);
        UserMovies GetMovieID(decimal movieID);
        Movie GetAllMovies(string movieTitle, string movieRelease);
    }
    class UserRepository : CRUD
    {
        MoviesdbEntities entities;
        public UserRepository()
        {
            entities = new MoviesdbEntities();

        }
        public Movie GetAllMovies(string movieTitle, string movieRelease)
        {
            try
            {
                return entities.Movies.First(x => x.Title == movieTitle && x.Release_Date == movieRelease);
            }
            catch (Exception ex)
            {
                return null;
            }
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
        public void AddUserMovies(UserMovies userMovies)
        {

            entities.UserMovies1.Add(userMovies);
            entities.SaveChanges();
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
        public List<Movie> GetUserMovies(decimal userID)
        {
            var userMovies = entities.UserMovies1.Where(um => um.UserID == userID).ToList();

            // Retrieve movies based on UserMovies for the given user
            List<Movie> moviesForUser = new List<Movie>();
            foreach (var userMovie in userMovies)
            {
                var movie = entities.Movies.FirstOrDefault(m => m.MovieID == userMovie.MovieID);
                if (movie != null)
                {
                    moviesForUser.Add(movie);
                }
            }
            return moviesForUser;
        }
        public void DeleteMovie(UserMovies movie)
        {
            entities.UserMovies1.Remove(movie);
            entities.SaveChanges();
        }
        public UserMovies GetMovieID(decimal movieID)
        {
            return entities.UserMovies1.First(x => x.MovieID == movieID);
        }
    }
}
