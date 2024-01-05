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
using MaterialSkin;
using MaterialSkin.Controls;

namespace MovieTracker
{
    public partial class MainPage : MaterialForm
    {
        UserRepository userRepository;
        private HttpClient client = new HttpClient();
        public MainPage()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            
        }
        private async void MainPage_Load(object sender, EventArgs e)
        {
            dataGridSearch.Visible = false; //data grid for search is not visible
            userRepository = new UserRepository();
            var options = new RestClientOptions("https://api.themoviedb.org/3/movie/popular?language=en-US&page=1"); //link for the trending movies
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
            dataGridViewMovies.Columns[4].HeaderText = "Title";
            dataGridViewMovies.Columns[5].HeaderText = "Description";
            dataGridViewMovies.Columns[6].HeaderText = "Popularity";
            dataGridViewMovies.Columns[7].Visible = false; // poster path
            dataGridViewMovies.Columns[8].HeaderText = "Release";
            dataGridViewMovies.Columns[9].Visible = false; // video??
            dataGridViewMovies.Columns[10].Visible = false; // vote average
            dataGridViewMovies.Columns[11].Visible = false; // vote count
            dataGridViewMovies.Columns[4].Width = 140;
            dataGridViewMovies.Columns[5].Width = 297;
            dataGridViewMovies.Columns[6].Width = 58;
            dataGridViewMovies.Columns[8].Width = 65;

        }
        private async void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnSearch_Click(object sender, EventArgs e) //search
        {
            dataGridSearch.Visible = true;
           
            dataGridViewMovies.Visible = false;
            string userInput = txtTitle.Text;
            try
            {
                string apiUrl = $"https://api.themoviedb.org/3/search/movie?query={userInput}&api_key=22ff9d8c655fb5bde0c5427a33701766"; //link for the for searching movies
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(responseContent); //Deserialize the JSON response to MovieResponse
                    dataGridSearch.DataSource = movieResponse.Results; //Attach the movies from the response to a data grid
                    dataGridSearch.Columns[0].Width = 140;
                    dataGridSearch.Columns[2].Width = 355;
                    dataGridSearch.Columns[1].Width = 65;
                    dataGridSearch.Columns[2].DisplayIndex = 1;
                    dataGridSearch.Columns[1].DisplayIndex = 2;
                    dataGridSearch.Columns[0].HeaderText = "Title";
                    dataGridSearch.Columns[1].HeaderText = "Release";
                    dataGridSearch.Columns[2].HeaderText = "Description";
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

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if(dataGridSearch.Visible == false)
            {
                var newMovie = new Movie();
                newMovie.MovieID = userRepository.GetMaxMovieID() + 1;
                newMovie.Title = dataGridViewMovies.CurrentRow.Cells[4].Value.ToString();
                newMovie.Overview = dataGridViewMovies.CurrentRow.Cells[5].Value.ToString();
                newMovie.Release_Date = dataGridViewMovies.CurrentRow.Cells[8].Value.ToString();
                var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
                userRepository.AddMovie(newMovie, userToFind.UserID);
            }
            else
            {
                var newMovie = new Movie();
                newMovie.MovieID = userRepository.GetMaxMovieID() + 1;
                newMovie.Title = dataGridSearch.CurrentRow.Cells[0].Value.ToString();
                newMovie.Overview = dataGridSearch.CurrentRow.Cells[2].Value.ToString();
                newMovie.Release_Date = dataGridSearch.CurrentRow.Cells[1].Value.ToString();
                var userToFind = userRepository.FindUser(Login.SetValueForText1.ToString(), Login.SetValueForText2.ToString());
                userRepository.AddMovie(newMovie, userToFind.UserID);
            }
                
            
                       
        }

        private void btnList_Click_1(object sender, EventArgs e)
        {
            Close();
            UserMovieList userMovieList = new UserMovieList();
            userMovieList.Show();
            
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnLoginPage_Click(object sender, EventArgs e)
        {
            Close();
            Login login = new Login();
            login.Show();
        }
    }
}
