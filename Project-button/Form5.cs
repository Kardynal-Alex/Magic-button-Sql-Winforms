using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
namespace Project_button
{
    public partial class Form5 : Form
    {
        SqlConnection sqlConnection;
        string Description;
        public Form5(SqlConnection sqlConnection_,string description)
        {
            InitializeComponent();
            Description = description;
            sqlConnection = sqlConnection_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void Form5_Load(object sender, EventArgs e)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * From[Table] WHERE Description='"+Description+"'", sqlConnection);
                sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                await sqlDataReader.ReadAsync();
                label1.Text = "Id : " + sqlDataReader["Id"].ToString();
                label2.Text = "Description : " + sqlDataReader["Description"].ToString();
                label3.Text = "Catagory : " + sqlDataReader["Catagory"].ToString();
                label4.Text = "Price : " + $"{sqlDataReader["Price"]:f2}" + " $";
                if (!string.IsNullOrEmpty(sqlDataReader["Image"].ToString()))
                {
                    Byte[] byteImage = (byte[])sqlDataReader["Image"];
                    MemoryStream memoryStream = new MemoryStream(byteImage);
                    Bitmap bitmap = new Bitmap(memoryStream);
                    pictureBox1.Image = bitmap;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
            finally
            {
                sqlDataReader.Close();
            }
        }
    }
}
