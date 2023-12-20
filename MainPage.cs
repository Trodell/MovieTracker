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

namespace MovieTracker
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }
        //test
        private async void MainPage_Load(object sender, EventArgs e)
        {
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
    }
}
