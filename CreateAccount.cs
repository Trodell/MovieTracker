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
using System.Text.RegularExpressions;


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
            lblAgeWarning.Visible = false;
            lblEmailWarning.Visible = false;
            lblPasswordWarning.Visible = false;
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {
            if (txtFName.Text != "" && txtLName.Text != "" && txtAge.Text != "" && txtEmail.Text != "" && txtCreateUsername.Text != "" && txtCreatePassword.Text != "")
            {

                var newUser = new User();
                newUser.First_Name = txtFName.Text;
                newUser.Last_Name = txtLName.Text;
                newUser.Age = decimal.Parse(txtAge.Text);
                newUser.Email = txtEmail.Text;
                newUser.Username = txtCreateUsername.Text;
                newUser.Password = userRepository.HashPassword(txtCreatePassword.Text);
                newUser.UserID = userRepository.GetMaxUserID() + 1;
                userRepository.CreateUser(newUser);
                MessageBox.Show("Account Created!");
                this.Close();
                Login login = new Login();
                login.Show();
            }
            else
                MessageBox.Show("Please enter all information correctly");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            Close();
            Login login = new Login();
            login.Show();
        }

        private void txtAge_Validating(object sender, CancelEventArgs e)
        {
            if(!int.TryParse(txtAge.Text, out int age) || int.Parse(txtAge.Text) <= 0)
            {
                e.Cancel = true;
                lblAgeWarning.Visible = true;
            }
            else
                lblAgeWarning.Visible=false;
        }


        private void txtFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mark the event as handled to stop the character from being processed
            }
        }

        private void txtLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mark the event as handled to stop the character from being processed
            }
        }
        private bool IsValidEmail(string email)
        {
            // Define the regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Check if the provided email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
        private bool PasswordValidation(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (IsValidEmail(txtEmail.Text))
            {
                lblEmailWarning.Visible = false;
            }
            else
            {
                e.Cancel = true;
                lblEmailWarning.Visible = true;
            }
        }

        private void txtCreatePassword_Validating(object sender, CancelEventArgs e)
        {
            if (PasswordValidation(txtCreatePassword.Text))
            {
                lblPasswordWarning.Visible = false;
            }
            else
            {
                e.Cancel = true;
                lblPasswordWarning.Visible = true;
            }
        }
    }
}
