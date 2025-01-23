using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        SqlConnection baglanti = new SqlConnection("Data Source=NUMAN ;Initial Catalog=RVD;Integrated Security=True");

        public Form2()
        {
            InitializeComponent();
        }

        private void listele()//veritabanındaki kayıtların görüntülenmesi
        {
            baglanti.Open();
            SqlDataAdapter da1 = new SqlDataAdapter("Select * from Owners", baglanti);
            DataTable tablo1 = new DataTable();
            da1.Fill(tablo1);
            dataGridView1.DataSource = tablo1;
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }
        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text; // Kullanıcıdan adı al

            try
            {
                baglanti.Open();

                // SQL sorgusu
                string query = @"
            SELECT 
                Owners.Name,
                Vehicles.VehicleID,
                Vehicles.LicensePlate,
                Vehicles.VehicleType,
                Vehicles.Model,
                Registrations.ExpiryDate,
                DATEDIFF(MONTH, GETDATE(), Registrations.ExpiryDate) AS MonthsLeft,
                CASE 
                    WHEN GETDATE() > Registrations.ExpiryDate THEN 'Expired'
                    ELSE 'Active'
                END AS Status
            FROM 
                Owners
            INNER JOIN 
                Registrations ON Owners.OwnerID = Registrations.OwnerID
            INNER JOIN 
                Vehicles ON Registrations.VehicleID = Vehicles.VehicleID
            WHERE 
                Owners.Name = @Name";

                SqlCommand komut = new SqlCommand(query, baglanti);
                komut.Parameters.AddWithValue("@Name", name);

                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable tablo = new DataTable();
                da.Fill(tablo);

                // DataGridView'e sonuçları yükle
                dataGridView1.DataSource = tablo;

                // Bildirim için tabloyu dolaş
                StringBuilder message = new StringBuilder();
                foreach (DataRow row in tablo.Rows)
                {
                    string licensePlate = row["LicensePlate"].ToString();
                    string expiryDate = Convert.ToDateTime(row["ExpiryDate"]).ToString("dd/MM/yyyy");
                    string status = row["Status"].ToString();

                    if (status == "Expired")
                    {
                        message.AppendLine($"License Plate: {licensePlate}, Expiry Date: {expiryDate}, Status: {status}");
                    }
                    else
                    {
                        int monthsLeft = Convert.ToInt32(row["MonthsLeft"]);
                        message.AppendLine($"LicensePlate: {licensePlate}, Expiry Date: {expiryDate}, Months Left: {monthsLeft}");
                    }
                }

                MessageBox.Show(message.ToString(), "Expiry Date Information");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}
