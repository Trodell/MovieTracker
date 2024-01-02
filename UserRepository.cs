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
            var existingMovie = entities.Movies.FirstOrDefault(m => m.Title == movie.Title && m.Release_Date == movie.Release_Date); //does the movie exist in the movies table

            if (existingMovie != null) //if the movie does exist
            {
                
                decimal movieID = existingMovie.MovieID; //grabs the existing movie's ID

                
                var userMovieExists = entities.UserMovies1.Any(um => um.UserID == userID && um.MovieID == movieID); //checks if the movie ID is already linked to the user ID by comparing each entity in the UserMovie table

                if (!userMovieExists) //movie ID is not linked to the user
                {
                    UserMovies newUserMovie = new UserMovies //creates a new entity in the UserMovie table
                    {
                        UserID = userID, //assigns the User ID
                        MovieID = movieID //assigns to movie ID
                    };

                    entities.UserMovies1.Add(newUserMovie); //adds the the new entity to User Movie table
                    entities.SaveChanges(); //saves changes
                }
                else //movie is linked to the User in the UserMovie table
                {
                    MessageBox.Show("This movie already exists");
                }
            }
            else //if the the movie doesn't exist
            {
                entities.Movies.Add(movie); //add movie to the movie table
                entities.SaveChanges(); // save changes

                decimal newMovieID = movie.MovieID; // Retrieve the newly created Movie ID from the main page

                UserMovies newUserMovie = new UserMovies //creates a new entity in the UserMovie table
                {
                    UserID = userID, //assigns the User ID
                    MovieID = newMovieID //assigns to movie ID
                };

                entities.UserMovies1.Add(newUserMovie); //adds the the new entity to User Movie table
                entities.SaveChanges(); //saves changes
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
