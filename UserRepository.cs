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
        }
        public void AddUserMovies(UserMovies userMovies)
        {

            entities.UserMovies1.Add(userMovies);
            entities.SaveChanges();
        }
         public void AddMovie(Movie movie, decimal userID)
        {

            using (var context = new MoviesdbEntities()) //Establishes a context to interact with the database
            {
                var existingMovie = context.Movies.FirstOrDefault(m => m.Title == movie.Title && m.Release_Date == movie.Release_Date); //returns the movie if it already exists in the movies table
                if (existingMovie != null) //Checks if the movie already exists in the Movies table , it does exist , for use if another user has this movie in their list
                {
                    context.Entry(existingMovie).State = EntityState.Detached; //Detaches the existing movie from the context
                    decimal movieID = existingMovie.MovieID; //Retrieves the MovieID of the existing movie
                    var userMovieExists = context.UserMovies1.Any(um => um.UserID == userID && um.MovieID == movieID); //return the movie if the User ID and Movie ID are linked in the UserMovie table
                    if (!userMovieExists) // Checks if the movie is linked to the specified user
                    {
                        UserMovies newUserMovie = new UserMovies {UserID = userID, MovieID = movieID}; //Create UserMovies entry
                        context.UserMovies1.Add(newUserMovie); //Adds the UserMovies entry
                        context.SaveChanges(); //Saves changes to the database
                    }
                    else //else the movie already exists as a linked movie to the User ID's list so it will not add
                    {
                        MessageBox.Show("This movie already exists for this user");
                    }
                }
                else // Movie doesn't exist in the Movies table, add it , for use if no other user has this movie to their list
                {
                    context.Entry(movie).State = EntityState.Added; //Marks the new movie as Added
                    context.SaveChanges(); //Saves changes to add the new movie to the Movies table
                    decimal newMovieID = movie.MovieID; //Retrieve the newly created MovieID
                    
                    UserMovies newUserMovie = new UserMovies {UserID = userID, MovieID = newMovieID}; //Create UserMovies entry for the new movie
                    context.UserMovies1.Add(newUserMovie); //Adds the UserMovies entry
                    context.SaveChanges(); //Saves changes to the UserMovies table
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
        public List<Movie> GetUserMovies(decimal userID) //Used for data grid display
        {
            var userMovies = entities.UserMovies1.Where(um => um.UserID == userID).ToList(); //where the entities User ID in the UserMovies table match with the current User ID, convert to list
            List<Movie> moviesForUser = new List<Movie>(); //create a list of movies
            foreach (var userMovie in userMovies) //foreach movie in the UserMovies table with the User's ID
            {
                var movie = entities.Movies.FirstOrDefault(m => m.MovieID == userMovie.MovieID); //returns the movie if the movie ID in the Movie table matches the movie ID in the UserMovie table
                if (movie != null) //if the movie is returned
                {
                    moviesForUser.Add(movie); //add the movie to the list
                }
            }
            return moviesForUser; //return list for data grid display
        }
        public void DeleteMovie(UserMovies movie, Movie movieToDelete, decimal loggedInUserID)
        {
            var movieID = movie.MovieID; //grabs the movie ID from the UserMovies table
            bool movieInUserMoviesForCurrentUser = entities.UserMovies1.Any(um => um.MovieID == movieID && um.UserID == loggedInUserID); //Checks if the movie ID exists in UserMovies for the logged-in user
            if (movieInUserMoviesForCurrentUser) //if the movie ID does exist in the UserMovies table
            { 
                var userMovieToDelete = entities.UserMovies1.FirstOrDefault(um => um.MovieID == movieID && um.UserID == loggedInUserID); //Checks if the movie ID exists for the logged-in user
                if (userMovieToDelete != null) //if the movie ID does exist
                {
                    entities.UserMovies1.Remove(userMovieToDelete); //delete the UserMovies table
                    entities.SaveChanges(); //saves changes
                }
            }
            bool movieNotInUserMovies = !entities.UserMovies1.Any(um => um.MovieID == movieID); //Proceed with deleting from Movies in any case (if it's not present in any user's list)
            if (movieNotInUserMovies) //If the movie ID does not exist in UserMovies table
            {
                entities.Movies.Remove(movieToDelete); //delete it from Movies table
                entities.SaveChanges(); //saves changes
            }
        }
        public UserMovies GetMovieID(decimal movieID)
        {
            return entities.UserMovies1.First(x => x.MovieID == movieID);
        }
    }
}
