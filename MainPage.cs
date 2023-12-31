using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using RestSharp;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MovieTracker
{
    public partial class MainPage : Form
    {
        UserRepository userRepository;
        private HttpClient client = new HttpClient();
        public MainPage()
        {
            InitializeComponent();
        }
        //test
        private async void MainPage_Load(object sender, EventArgs e)
        {
            dataGridSearch.Visible = false;
            userRepository = new UserRepository();
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/popular?language=en-US&page=1");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIyMmZmOWQ4YzY1NWZiNWJkZTBjNTQyN2EzMzcwMTc2NiIsInN1YiI6IjY1N2I0YTUzOGUyYmE2MDBlMWZjZTg5MSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.S8ahBGESJ7OR1zRtpROj5dXZj72oVlJlOPcFf8145nI");
            var response = await client.GetAsync(request);
            Root data = JsonConvert.DeserializeObject<Root>(response.Content);
            dataGridViewMovies.DataSource = data.results;
            dataGridViewMovies.Columns[0].Visible = false; // is adult movie?
            dataGridViewMovies.Columns[1].Visible = false; // backdrop path
            dataGridViewMovies.Columns[2].Visible = false; // id number
            dataGridViewMovies.Columns[3].Visible = false; // language
            dataGridViewMovies.Columns[7].Visible = false; // poster path
            dataGridViewMovies.Columns[9].Visible = false; // video??
            dataGridViewMovies.Columns[10].Visible = false; // vote average
            dataGridViewMovies.Columns[11].Visible = false; // vote count
        }

        private async void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e) //search
        {
            dataGridSearch.Visible = true;
            dataGridViewMovies.Visible = false;
            string userInput = txtTitle.Text;
            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/search/movie?query={userInput}&api_key=22ff9d8c655fb5bde0c5427a33701766";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response to MovieResponse
                    MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(responseContent);

                    // Bind the movies from the response to the DataGridView
                    
                    dataGridSearch.DataSource = movieResponse.Results;
                    dataGridSearch.Columns[3].Visible = false;
                    dataGridSearch.Columns[4].Visible = false;
                }
                else
                {
                    MessageBox.Show($"Failed to fetch data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridViewMovies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newMovie = new Movie();
                newMovie.MovieID = userRepository.GetMaxMovieID() + 1;
                newMovie.Title = dataGridViewMovies.CurrentRow.Cells[4].Value.ToString();
                newMovie.Overview = dataGridViewMovies.CurrentRow.Cells[5].Value.ToString();
                newMovie.Release_Date = dataGridViewMovies.CurrentRow.Cells[8].Value.ToString();
                var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
                userRepository.AddMovie(newMovie, userToFind.UserID);
            }
            catch(Exception ex)
            {
                var newMovie = new Movie();
                newMovie.MovieID = userRepository.GetMaxMovieID() + 1;
                newMovie.Title = dataGridSearch.CurrentRow.Cells[0].Value.ToString();
                newMovie.Overview = dataGridSearch.CurrentRow.Cells[2].Value.ToString();
                newMovie.Release_Date = dataGridSearch.CurrentRow.Cells[1].Value.ToString();
                var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
                userRepository.AddMovie(newMovie,userToFind.UserID);
            }
                       
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            
            UserMovieList userMovieList = new UserMovieList();
            userMovieList.Show();
            
        }
    }
}
