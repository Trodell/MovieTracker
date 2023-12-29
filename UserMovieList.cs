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
    public partial class UserMovieList : Form
    {
        UserRepository userRepository = new UserRepository();
        public UserMovieList()
        {
            InitializeComponent();
        }

        private void UserMovieList_Load(object sender, EventArgs e)
        {
            var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
            decimal userID = userToFind.UserID;
            List<Movie> userMovies = userRepository.GetUserMovies(userID);
            moviesGrid.DataSource = userMovies;
            moviesGrid.Columns[3].Visible = false; //Id
            moviesGrid.Columns[4].Visible = false; //??
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
            decimal userID = userToFind.UserID;
            var id = moviesGrid.CurrentRow.Cells[3].Value;
            var movieToDelete = userRepository.GetMovieID((decimal)id);
            userRepository.DeleteMovie(movieToDelete);
            MessageBox.Show("Movie Deleted");
            moviesGrid.DataSource = userRepository.GetUserMovies(userID);
        }
    }
}
