using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows.Forms;

namespace MovieTracker
{
    public partial class Login : Form
    {
        
        UserRepository userRepository;
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            userRepository = new UserRepository();
        }

        private void btnCreateAcc_Click(object sender, EventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            createAccount.ShowDialog();
            this.Close();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            var user = userRepository.FindUser(username,password);
            if (user != null)
            {
                
                MainPage mainPage = new MainPage();
                mainPage.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("That user doesn't exist");
            

        }
    }
}
