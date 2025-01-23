using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class LoginPage : Form
    {

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }

        public List<User> users = new List<User>
        {
            new User { Username = "admin", Password = "admin123", IsAdmin = true },
            new User { Username = "user1", Password = "user123", IsAdmin = false },
        };
        public LoginPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                if (user.IsAdmin)
                {
                    this.Hide();
                    Form1 Form1 = new Form1();
                    Form1.ShowDialog(); 
                    this.Close(); // Admin formu kapandığında giriş ekranı tamamen kapanır
                }

                else
                {
                    // Kullanıcı sayfasına yönlendir
                    MessageBox.Show("Kullanıcı giriş yaptı!");
                    Form2 userPage = new Form2();
                    userPage.Show();
                    this.Hide(); // Giriş formunu gizler
                } 
                
            }
            else
            {
                // Hatalı giriş
                errorLabel.Text = "Username or password is incorrect!";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // NewUserForm oluşturuluyor
            NewUser newUserForm = new NewUser
            {
                UserList = users // Form1'deki kullanıcı listesini NewUserForm'a gönderiyoruz
            };

            // Formu non-modal olarak aç
            newUserForm.Show();
        }
    }
}
