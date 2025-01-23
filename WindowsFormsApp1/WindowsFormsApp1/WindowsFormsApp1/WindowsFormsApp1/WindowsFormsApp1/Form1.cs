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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      
        private void listele()//veritabanındaki kayıtların görüntülenmesi
        {
            baglanti.Open();
            SqlDataAdapter da1 = new SqlDataAdapter("Select *from Owners", baglanti);
            DataTable tablo1 = new DataTable();
            da1.Fill(tablo1);
            dataGridView1.DataSource = tablo1;
            baglanti.Close();

            baglanti.Open();
            SqlDataAdapter da2 = new SqlDataAdapter("Select *from Vehicles", baglanti);
            DataTable tablo2 = new DataTable();
            da2.Fill(tablo2);
            dataGridView2.DataSource = tablo2;
            baglanti.Close();

            baglanti.Open();
            SqlDataAdapter da3 = new SqlDataAdapter("Select *from Registrations", baglanti);
            DataTable tablo3 = new DataTable();
            da3.Fill(tablo3);
            dataGridView3.DataSource = tablo3;
            baglanti.Close();

            baglanti.Open();
            SqlDataAdapter da4 = new SqlDataAdapter("Select *from Payments", baglanti);
            DataTable tablo4 = new DataTable();
            da4.Fill(tablo4);
            dataGridView4.DataSource = tablo4;
            baglanti.Close();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            String t2 = textBox2.Text;  //Name
            String t3 = textBox3.Text;  //ContactInfo
            String t4 = textBox4.Text;  //address


            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("INSERT INTO Owners (Name, ContactInfo, address) VALUES('" + t2 + "','" + t3 + "','" + t4 + "')", baglanti);
            komut1.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=NUMAN ;Initial Catalog=RVD;Integrated Security=True");

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String t2 = textBox2.Text;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM Owners WHERE Name=('" + t2 + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String t2 = textBox2.Text;  //Name
            String t3 = textBox3.Text;  //ContactInfo
            String t4 = textBox4.Text;  //address

            baglanti.Open();
            SqlCommand komut = new SqlCommand("UPDATE Owners SET Name='" + t2 + "', ContactInfo='" + t3 + "', Address='" + t4 + "' WHERE Name='" + t2 + "' ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            String t6 = textBox6.Text;  //LicensePlate
            String t7 = textBox7.Text;  //VehicleType
            String t8 = textBox8.Text;  //Model
            String t9 = textBox9.Text;  //Year

            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("INSERT INTO Vehicles (LicensePlate, VehicleType, Model, Year) VALUES('" + t6 + "','" + t7 + "','" + t8 + "','" + t9 + "')", baglanti);
            komut2.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string t10 = textBox10.Text; // VehicleID
            string t11 = textBox11.Text; // OwnerID
            string t1 = textBox1.Text;   // PaymentID
            DateTime registrationDate = dateTimePicker1.Value; // RegistrationDate için
            DateTime expiryDate = dateTimePicker2.Value;       // ExpiryDate için

            // Status kontrolü
            string status;
            if (expiryDate < DateTime.Now)
            {
                status = "Expired";
            }
            else
            {
                status = "Paid";
            }

            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // SQL komutu
                string query = "INSERT INTO Registrations (VehicleID, OwnerID, PaymentID, Statuss, ExpiryDate, RegistrationDate) " +
                               "VALUES (@VehicleID, @OwnerID, @PaymentID, @Statuss, @ExpiryDate, @RegistrationDate)";

                SqlCommand komut3 = new SqlCommand(query, baglanti);
                komut3.Parameters.AddWithValue("@VehicleID", t10);
                komut3.Parameters.AddWithValue("@OwnerID", t11);
                komut3.Parameters.AddWithValue("@PaymentID", t1);
                komut3.Parameters.AddWithValue("@Statuss", status); // Dinamik olarak hesaplanan status
                komut3.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                komut3.Parameters.AddWithValue("@RegistrationDate", registrationDate);

                // Sorguyu çalıştır
                komut3.ExecuteNonQuery();

               
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                baglanti.Close();
            }

            // Listeleme işlemi
            listele();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            // DateTimePicker'den tarih al
            string t17 = textBox17.Text;              // Amount bilgisi TextBox'tan alınır
            string t18 = textBox18.Text;       // PaymentStatus bilgisi TextBox'tan alınır
            DateTime paymentDate = dateTimePicker3.Value; // PaymentDate için DateTimePicker


            // Veritabanı bağlantısını aç
            baglanti.Open();

            // Parametreli sorgu kullanımı
            SqlCommand komut4 = new SqlCommand("INSERT INTO Payments (Amount, PaymentStatus, PaymentDate) VALUES ( '" + t17 + "','" + t18 + "', @PaymentDate)", baglanti);
            komut4.Parameters.AddWithValue("@PaymentDate", paymentDate);

            // Sorguyu çalıştır
            komut4.ExecuteNonQuery();

            // Veritabanı bağlantısını kapat
            baglanti.Close();

            // Listeleme işlemi
            listele();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox6.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textBox7.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            textBox8.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            textBox9.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox10.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            textBox11.Text = dataGridView3.CurrentRow.Cells[2].Value.ToString();
            textBox1.Text = dataGridView3.CurrentRow.Cells[3].Value.ToString();
            if (dataGridView3.CurrentRow.Cells[4].Value is DateTime cellDateTime1)
            {
                dateTimePicker1.Value = cellDateTime1;
            }

            if (dataGridView3.CurrentRow.Cells[5].Value is DateTime cellDateTime2)
            {
                dateTimePicker2.Value = cellDateTime2;
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentRow.Cells[1].Value is DateTime cellDateTime3)
            {
                dateTimePicker3.Value = cellDateTime3;
            }
            textBox17.Text = dataGridView4.CurrentRow.Cells[2].Value.ToString();
            textBox18.Text = dataGridView4.CurrentRow.Cells[3].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String t6 = textBox6.Text;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM Vehicles WHERE LicensePlate=('" + t6 + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String t10 = textBox10.Text;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM Registrations WHERE VehicleID=('" + t10 + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            String t18 = textBox18.Text;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("DELETE FROM Payments WHERE PaymentStatus=('" + t18 + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String t6 = textBox6.Text;  //LicensePlate
            String t7 = textBox7.Text;  //VehicleType
            String t8 = textBox8.Text;  //Model
            String t9 = textBox9.Text;  //Year

            baglanti.Open();
            SqlCommand komut = new SqlCommand("UPDATE Vehicles SET LicensePlate='" + t6 + "', VehicleType='" + t7 + "', Model='" + t8 + "', Year='" + t9 + "' WHERE LicensePlate='" + t6 + "' ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string t10 = textBox10.Text; // VehicleID
            string t11 = textBox11.Text; // OwnerID
            string t1 = textBox1.Text;   // PaymentID
            DateTime registrationDate = dateTimePicker1.Value; // RegistrationDate
            DateTime expiryDate = dateTimePicker2.Value;       // ExpiryDate

            // Status kontrolü
            string status;
            if (expiryDate < DateTime.Now)
            {
                status = "Expired";
            }
            else
            {
                status = "Paid";
            }

            // SQL sorgusu (parametreli)
            string query = "UPDATE Registrations " +
                           "SET OwnerID = @OwnerID, PaymentID = @PaymentID, Statuss = @Statuss, RegistrationDate = @RegistrationDate, ExpiryDate = @ExpiryDate " +
                           "WHERE VehicleID = @VehicleID";

            // SQL komutu ve bağlantı
            try
            {
                using (SqlCommand komut = new SqlCommand(query, baglanti))
                {
                    // Parametreleri ekle
                    komut.Parameters.AddWithValue("@VehicleID", t10);
                    komut.Parameters.AddWithValue("@OwnerID", t11);
                    komut.Parameters.AddWithValue("@PaymentID", t1);
                    komut.Parameters.AddWithValue("@Statuss", status); // Dinamik status
                    komut.Parameters.AddWithValue("@RegistrationDate", registrationDate);
                    komut.Parameters.AddWithValue("@ExpiryDate", expiryDate);

                    // Veritabanı bağlantısını aç
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                }

                
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }

            // Listeyi yenile
            listele();

        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Yeni verileri al
            string t17 = textBox17.Text;  // Yeni Amount
            string t18 = textBox18.Text;  // Yeni PaymentStatus
            DateTime paymentDate = dateTimePicker3.Value;  // Yeni PaymentDate

            // Seçilen satırdaki PaymentID'yi al
            string paymentID = dataGridView4.CurrentRow?.Cells[0]?.Value?.ToString(); // ID sütunu 0. hücrede olmalı

           

            string query = "UPDATE Payments " +
                           "SET Amount = @Amount, PaymentStatus = @PaymentStatus, PaymentDate = @PaymentDate " +
                           "WHERE PaymentID = @PaymentID";

            using (SqlCommand komut = new SqlCommand(query, baglanti))
            {
                // Parametreleri ekle
                komut.Parameters.AddWithValue("@Amount", t17);
                komut.Parameters.AddWithValue("@PaymentStatus", t18);
                komut.Parameters.AddWithValue("@PaymentDate", paymentDate);
                komut.Parameters.AddWithValue("@PaymentID", paymentID);

                // Sorguyu çalıştır
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
            }

            // Listeyi yenile
            listele();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DateTime registrationDate2 = dateTimePicker4.Value; // RegistrationDate
            DateTime expiryDate2 = dateTimePicker5.Value; // ExpiryDate

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("SELECT * FROM Registrations WHERE ExpiryDate < @expiryDate2 AND registrationDate > @registrationDate2", baglanti);
            // Parametreleri ekleyin
            komut1.Parameters.AddWithValue("@registrationDate2", registrationDate2);
            komut1.Parameters.AddWithValue("@expiryDate2", expiryDate2);

            SqlDataAdapter da = new SqlDataAdapter(komut1);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Veriyi listele (Örneğin bir DataGridView'e bağlıyorsanız)
            dataGridView5.DataSource = dt;

            baglanti.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            baglanti.Open(); // Bağlantıyı aç

            // SQL sorgusu
            string query = "SELECT VehicleType, COUNT(*) AS VehicleCount FROM Vehicles GROUP BY VehicleType";

            // Komut nesnesi oluştur
            SqlCommand komut = new SqlCommand(query, baglanti);

            // Veriyi çekmek için SqlDataAdapter kullan
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt); // Gelen veriyi DataTable'a doldur

            // DataGridView veya uygun kontrol üzerine veriyi bağla
            dataGridView5.DataSource = dt; // Örnek olarak DataGridView kullanıldı

            baglanti.Close(); // Bağlantıyı kapat
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int year = dateTimePicker6.Value.Year; // Yalnızca yıl değerini al

            baglanti.Open();

            // SQL sorgusu: Bu yılın toplam gelirini hesaplar
            string query = "SELECT ISNULL(SUM(Amount), 0) AS TotalRevenue FROM Payments WHERE YEAR(PaymentDate) = @year";

            SqlCommand komut = new SqlCommand(query, baglanti);
            komut.Parameters.AddWithValue("@year", year); // Parametreye yıl değerini ekle

            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();

            da.Fill(dt);

            // DataGridView'e veriyi bağla
            dataGridView5.DataSource = dt;

            baglanti.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            // SQL sorgusu
            string query = @"
        SELECT o.Name, COUNT(r.VehicleID) AS VehicleCount 
        FROM Owners o 
        JOIN Registrations r ON o.OwnerID = r.OwnerID 
        GROUP BY o.OwnerID, o.Name
        HAVING COUNT(r.VehicleID) > 1;";

            // Komut nesnesini oluştur
            SqlCommand komut = new SqlCommand(query, baglanti);

            // Veriyi çekmek için SqlDataAdapter kullan
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // DataGridView'e veriyi bağla
            dataGridView5.DataSource = dt;

            // Bağlantıyı kapat
            baglanti.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
