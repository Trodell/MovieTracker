using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
        void AddMovie (Movie newMovie, decimal userID);
        decimal GetMaxUserID();
        decimal GetMaxMovieID();
        void AddUserMovies(UserMovies userMovies);
        List<Movie> GetUserMovies(decimal userID);
        void DeleteMovie(UserMovies movie, Movie movie1, decimal userID);
        UserMovies GetMovieID(decimal movieID);
        //Movie GetAllMovies(string movieTitle, string movieRelease);
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
        public void AddUserMovies(UserMovies userMovies)
        {

            entities.UserMovies1.Add(userMovies);
            entities.SaveChanges();
        }
         public void AddMovie(Movie movie, decimal userID)
        {

            using (var context = new MoviesdbEntities())
            {
                var existingMovie = context.Movies.FirstOrDefault(m => m.Title == movie.Title && m.Release_Date == movie.Release_Date);

                if (existingMovie != null)
                {
                    // Movie exists in the Movies table
                    context.Entry(existingMovie).State = EntityState.Detached;
                    decimal movieID = existingMovie.MovieID;

                    var userMovieExists = context.UserMovies1.Any(um => um.UserID == userID && um.MovieID == movieID);

                    if (!userMovieExists)
                    {
                        // Create UserMovies entry
                        UserMovies newUserMovie = new UserMovies
                        {
                            UserID = userID,
                            MovieID = movieID
                        };

                        context.UserMovies1.Add(newUserMovie);
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("This movie already exists for this user");
                    }
                }
                else
                {
                    // Movie doesn't exist in the Movies table, add it

                    // Ensure that the primary key values are unique for each entity of type 'MovieTracker.Movie' in your database.
                    // Verify that the database-generated primary keys are configured correctly in the database and in the Entity Framework model.
                    // Use the Entity Designer for Database First/Model First configuration or use the 'HasDatabaseGeneratedOption" fluent API or 'DatabaseGeneratedAttribute' for Code First configuration.
                    context.Entry(movie).State = EntityState.Added;
                    context.SaveChanges();

                    // Retrieve the newly created MovieID
                    decimal newMovieID = movie.MovieID;

                    // Create UserMovies entry for the new movie
                    UserMovies newUserMovie = new UserMovies
                    {
                        UserID = userID,
                        MovieID = newMovieID
                    };

                    context.UserMovies1.Add(newUserMovie);
                    context.SaveChanges();
                }
            }
        
    }
        public decimal GetMaxUserID()
        {
            return entities.Users.Max(x => x.UserID); //Finds max User ID
            
        }
        public decimal GetMaxMovieID()
        {
            return entities.Movies.Max(x => x.MovieID); //Finds max movie ID
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
        public void DeleteMovie(UserMovies movie, Movie movieToDelete, decimal loggedInUserID)
        {

            //var movieToDelete = entities.UserMovies1.Where(x=> x.MovieID == movie.MovieID && x.UserID == userID);
            //if (movieToDelete!=null)
            //{
            //    entities.Movies.Remove(movie1);
            //    entities.SaveChanges();
            //}
            //else
            //{
            //    entities.UserMovies1.Remove(movie);
            //    entities.SaveChanges();
            //}
            var movieID = movie.MovieID;

            // Check if the movie ID exists in UserMovies for the logged-in user
            bool movieInUserMoviesForCurrentUser = entities.UserMovies1
                .Any(um => um.MovieID == movieID && um.UserID == loggedInUserID);

            if (movieInUserMoviesForCurrentUser)
            {
                // If the movie ID exists for the logged-in user, delete the UserMovies record
                var userMovieToDelete = entities.UserMovies1
                    .FirstOrDefault(um => um.MovieID == movieID && um.UserID == loggedInUserID);

                if (userMovieToDelete != null)
                {
                    entities.UserMovies1.Remove(userMovieToDelete);
                    
                    entities.SaveChanges();
                }
            }

            // Proceed with deleting from Movies in any case (if it's not present in any user's list)
            bool movieNotInUserMovies = !entities.UserMovies1.Any(um => um.MovieID == movieID);

            if (movieNotInUserMovies)
            {
                // If the movie ID is not present in UserMovies, delete it from Movies
                entities.Movies.Remove(movieToDelete);
                
                entities.SaveChanges();
            }
        }


    
        public UserMovies GetMovieID(decimal movieID)
        {
            return entities.UserMovies1.First(x => x.MovieID == movieID);
        }
    }
}
