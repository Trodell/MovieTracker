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
        public static string SetValueForText1 = "";
        public static string SetValueForText2 = "";
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
        }

        private void btnCreateAcc_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            CreateAccount createAccount = new CreateAccount();
            createAccount.Show();
            
        }
        
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            SetValueForText1 = txtUsername.Text;
            SetValueForText2 = txtPassword.Text;
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            var user = userRepository.FindUser(username,password);
            if (user != null)
            {
                this.Hide();
                MainPage newForm = new MainPage();
                newForm.Show();
            }
            else
                MessageBox.Show("That user doesn't exist");
            

        }

    }
}
