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
    public partial class CreateAccount : MaterialForm
    {
        UserRepository userRepository;
        public CreateAccount()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void CreateAccount_Load(object sender, EventArgs e)
        {
            userRepository = new UserRepository();
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {
            if(txtFName != null && txtLName != null && txtAge != null && txtEmail != null && txtCreateUsername != null && txtCreatePassword!=null)
            {
                
                var newUser = new User();
                newUser.First_Name = txtFName.Text;
                newUser.Last_Name = txtLName.Text;
                newUser.Age = decimal.Parse(txtAge.Text);
                newUser.Email = txtEmail.Text;
                newUser.Username = txtCreateUsername.Text;
                newUser.Password = txtCreatePassword.Text;
                newUser.UserID = userRepository.GetMaxUserID()+1;
                userRepository.CreateUser(newUser);
                MessageBox.Show("Account Created!");
                this.Close();
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //work in progress
            //Hide();
            //Login login = new Login();
            //login.Show();
        }
    }
}
