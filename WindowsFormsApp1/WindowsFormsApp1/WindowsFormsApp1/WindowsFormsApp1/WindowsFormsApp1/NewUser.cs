using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.LoginPage;

namespace WindowsFormsApp1
{
    public partial class NewUser : Form
    {

        public List<User> UserList { get; set; }

        public NewUser()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kullanıcı oluşturma işlemi burada yapılacak
            string newUsername = usernameRegTextBox.Text.Trim();
            string newPassword = passwordRegTextBox.Text.Trim();

            // Kullanıcı girişlerini kontrol et ve listeye ekle
            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz.");
                return;
            }

            if (UserList.Any(user => user.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Bu kullanıcı adı zaten mevcut.");
                return;
            }

            UserList.Add(new User { Username = newUsername, Password = newPassword, IsAdmin = false });
            MessageBox.Show("Kullanıcı başarıyla kaydedildi!");
            this.Close(); // Formu kapat
        }
    }
}
