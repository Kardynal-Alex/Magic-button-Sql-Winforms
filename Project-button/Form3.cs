using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Project_button
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            add_to_combobox();
            pictureBox1.Visible = true;
            textBox1.Validating += textBox1_Validating;
            textBox3.Validating += textBox3_Validating;
            comboBox1.Validating += comboBox1_Validating;
        }
        private string connectionString = @"Data Source=DESKTOP-339MSAS\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";
        private SqlConnection sqlConnection = null;
        private void button2_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        private void add_to_combobox()
        {
            comboBox1.Items.AddRange(new string[] { "Hot Drink", "Cold Drink", "Hot Food", "Cold Food" });
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }
        private Byte[] byteImage;
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                byteImage = new Byte[fileStream.Length];
                fileStream.Read(byteImage, 0, byteImage.Length);
                fileStream.Close();
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Visible = true;
                MemoryStream memoryStream = new MemoryStream(byteImage);
                pictureBox1.Image = Image.FromStream(memoryStream);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            int res1; double res2;
            if (int.TryParse(textBox1.Text, out res1) && textBox1.Text != "" &&
                double.TryParse(textBox3.Text, out res2) && textBox3.Text != "" &&
                comboBox1.Text != "" && textBox2.Text != "") 
            {
                SqlCommand sqlCommand = new SqlCommand("INSERT INTO [Table] (Id,Description,Catagory,Price,Image) VALUES(@Id,@Description,@Catagory,@Price,@Image)", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", textBox1.Text);
                sqlCommand.Parameters.AddWithValue("Description", textBox2.Text);
                sqlCommand.Parameters.AddWithValue("Catagory", comboBox1.Text);
                sqlCommand.Parameters.AddWithValue("Price", Convert.ToDouble(textBox3.Text));
                sqlCommand.Parameters.AddWithValue("Image", byteImage);
                int check = await sqlCommand.ExecuteNonQueryAsync();
                if (check == 0)
                {
                    MessageBox.Show("Error");
                }
                else
                {
                    Form7 form7 = new Form7("Sucsessfully Added");
                    form7.Show();
                    clear_info();
                }
            }
        }
        private void clear_info()
        {
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = "";
            comboBox1.Text = ""; pictureBox1.Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            clear_info();
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int res;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Cannot be Empty");
            }
            else
            if(!int.TryParse(textBox1.Text,out res))
            {
                errorProvider1.SetError(textBox1, "Format exception");
            }
            else
            if (int.TryParse(textBox1.Text, out res))
            {
                if(Convert.ToInt32(textBox1.Text) <= 0) 
                    errorProvider1.SetError(textBox1, "Id can not be less than 0");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox3_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double res;
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "Cannot be Empty");
            }
            else
            if(!double.TryParse(textBox3.Text,out res))
            {
                errorProvider1.SetError(textBox3,"Format exception");
            }
            else
            if (double.TryParse(textBox3.Text,out res))
            {
                if(Convert.ToDouble(textBox3.Text) <= 0)
                    errorProvider1.SetError(textBox3, "Price can not be less than 0");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void comboBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                errorProvider1.SetError(comboBox1, "Empty");
            }
            else
            if (!comboBox1.Text.Contains("Hot Drink")&& !comboBox1.Text.Contains("Cold Drink")&&
                !comboBox1.Text.Contains("Hot Food")&& !comboBox1.Text.Contains("Cold Food"))
            {
                errorProvider1.SetError(comboBox1, "There are not such catagory");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
    }
}
