using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieTracker
{
    public partial class UserMovieList : MaterialForm
    {
        UserRepository userRepository = new UserRepository();
        public UserMovieList()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            this.Text = "Movie List";
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void UserMovieList_Load(object sender, EventArgs e)
        {
            var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
            decimal userID = userToFind.UserID;
            List<Movie> userMovies = userRepository.GetUserMovies(userID);
            moviesGrid.DataSource = userMovies;
            moviesGrid.Columns[0].HeaderText = "Title";
            moviesGrid.Columns[1].HeaderText = "Release";
            moviesGrid.Columns[2].HeaderText = "Description";
            moviesGrid.Columns[1].DisplayIndex = 2;
            moviesGrid.Columns[2].DisplayIndex = 1;
            moviesGrid.Columns[0].Width = 140;
            moviesGrid.Columns[2].Width = 355;
            moviesGrid.Columns[1].Width = 65;
            moviesGrid.Columns[3].Visible = false; //Id
            moviesGrid.Columns[4].Visible = false; //??
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString()); //Pulls the user's ID
            decimal userID = userToFind.UserID;
            var id = moviesGrid.CurrentRow.Cells[3].Value;
            var movieToDelete = userRepository.GetMovieID((decimal)id);
            userRepository.DeleteMovie(movieToDelete, movieToDelete.Movie, userID);
            MessageBox.Show("Movie Deleted");
            moviesGrid.DataSource = userRepository.GetUserMovies(userID);
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e) //btnSearch
        {

            var searchTerm = txtTitle.Text;
            List<MovieTracker.Movie> moviesList = (List<MovieTracker.Movie>)moviesGrid.DataSource;
            List<MovieTracker.Movie> filteredMoviesList = moviesList.Where(movie => movie.Title.Contains(searchTerm)).ToList();
            filteredMoviesList.AddRange(moviesList.Except(filteredMoviesList));
            moviesGrid.DataSource = filteredMoviesList;
        }

    

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
            MainPage mainPage = new MainPage();
            mainPage.Show();
        }
    }
}
