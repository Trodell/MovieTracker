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
using System.Web.ModelBinding;
using System.Windows.Forms;

namespace MovieTracker
{
    public partial class Login : MaterialForm
    {
        public static string SetValueForText1 = ""; //Use this to keep track of the User's Username to pull their ID number
        public static string SetValueForText2 = ""; //Use this to keep track of the User's Username to pull their ID number
        UserRepository userRepository;
        public Login()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);


        }

    private void Form1_Load(object sender, EventArgs e)
        {
            userRepository = new UserRepository();
            btnHide.Visible = false;
        }

        private void btnCreateAcc_Click_1(object sender, EventArgs e)
        {
            
            CreateAccount createAccount = new CreateAccount();
            createAccount.StartPosition = FormStartPosition.CenterScreen;
            createAccount.Show();
            this.Hide(); //

        }
        
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = userRepository.HashPassword(txtPassword.Text);
            SetValueForText1 = txtUsername.Text;
            SetValueForText2 = password;
            var user = userRepository.FindUser(username,password);
            if (user != null)
            {
                bool passwordMatch = userRepository.VerifyPassword(txtPassword.Text,password);
                if (passwordMatch)
                {
                    
                    MainPage newForm = new MainPage();
                    newForm.StartPosition = FormStartPosition.CenterScreen;
                    newForm.Show();
                    this.Hide();
                }
               
            }
            else
                MessageBox.Show("That user doesn't exist");
            

        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            btnHide.Visible = false;
            btnPeek.Visible = true;
        }

        private void btnPeek_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            btnPeek.Visible = false;
            btnHide.Visible = true;
        }
    }
}
