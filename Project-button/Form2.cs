using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Project_button
{
    public partial class Form2 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-339MSAS\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";
        private SqlConnection sqlConnection=null;
        public Form2(SqlConnection sqlConnection_)
        {    
            InitializeComponent();
            sqlConnection = sqlConnection_;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [Table]", sqlConnection);
            sqlDataAdapter.Fill(dataSet, "Table");
            dataGridView1.DataSource = dataSet.Tables["Table"].DefaultView;
            fill_autosize_datagrid();
            
            comboBox1.Items.AddRange(new string[] { "Hot Drink", "Cold Drink", "Hot Food", "Cold Food" });
            comboBox2.Items.AddRange(new string[] { "Hot Drink", "Cold Drink", "Hot Food", "Cold Food" });
            textBox2.Validating += textBox2_Validating;
            textBox4.Validating += textBox4_Validating;
            comboBox1.Validating += comboBox2_Validating;
        }

        private void fill_autosize_datagrid()
        {
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                comboBox1.Text = "";
                update_data();
            } 
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void update_data()
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [Table]", connectionString);
            sqlDataAdapter.Fill(dataSet, "Table");
            dataGridView1.DataSource = dataSet.Tables["Table"].DefaultView;
        }
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            update_data();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(comboBox1.Text)&&!string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Table] Where Catagory LIKE '%"+comboBox1.Text+"%'", sqlConnection);
                int check = sqlCommand.ExecuteNonQuery();
                if (check != 0)
                {
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("ERROR");
                }
            }
            else
            if(string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                update_data();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dataGridView1.CurrentRow.Selected = true;
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                    textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[4].FormattedValue.ToString()))
                    {
                        pictureBox1.Visible = true;
                        byteImage = (byte[])dataGridView1.Rows[e.RowIndex].Cells[4].Value;
                        MemoryStream memory = new MemoryStream(byteImage);
                        Bitmap bitmap = new Bitmap(memory);
                        pictureBox1.Image = bitmap;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception)
            {
                
            }    
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            int res;
            if (int.TryParse(textBox1.Text, out res) && textBox1.Text != "" && res>0)
            {
                SqlCommand sqlCommand = new SqlCommand("DELETE FROM[Table] WHERE [Id]=@Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", textBox1.Text);
                int check = await sqlCommand.ExecuteNonQueryAsync();
                if (check != 0)
                {
                    Form7 form7 = new Form7("Successfully Deleted");
                    form7.Show();
                }
                else
                {
                    MessageBox.Show("ERROR");
                }
                update_data();
                
                textBox1.Text = "";
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            int res1; double res2;
            if (int.TryParse(textBox2.Text, out res1) && double.TryParse(textBox4.Text, out res2) &&
                textBox2.Text != "" && textBox3.Text != "" &&
                textBox4.Text != "" && comboBox2.Text != "") 
            {
                SqlCommand sqlCommand = new SqlCommand("UPDATE [Table] SET [Description]=@Description, [Catagory]=@Catagory, [Price]=@price, [Image]=@Image WHERE [Id]=@Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", textBox2.Text);
                sqlCommand.Parameters.AddWithValue("Description", textBox3.Text);
                sqlCommand.Parameters.AddWithValue("Catagory", comboBox2.Text);
                sqlCommand.Parameters.AddWithValue("Price",Convert.ToDouble(textBox4.Text));
                sqlCommand.Parameters.AddWithValue("Image", byteImage);

                try
                {
                    int check=await sqlCommand.ExecuteNonQueryAsync();
                    if(check!=0)
                    {
                        Form7 form7 = new Form7("Successfully Updated");
                        form7.Show();
                        clear_info();
                    }
                    else
                    {
                        MessageBox.Show("ERROR");
                    }
                    update_data();
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR");
                }                
            }
        }
        private byte[] byteImage;
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if(dialogResult==DialogResult.OK)
            {
                FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                byteImage = new Byte[fileStream.Length];
                fileStream.Read(byteImage, 0, byteImage.Length);
                fileStream.Close();
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                MemoryStream memoryStream = new MemoryStream(byteImage);
                pictureBox1.Image = Image.FromStream(memoryStream);
            }
        }
        private void clear_info()
        {
            textBox2.Text = ""; textBox3.Text = ""; comboBox2.Text = "";
            textBox4.Text = ""; pictureBox1.Visible = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            clear_info();
        }

        private void textBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int res;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Cannot be Empty");
            }
            else
            if (!int.TryParse(textBox2.Text, out res))
            {
                errorProvider1.SetError(textBox2, "Format exception");
            }
            else
            if (int.TryParse(textBox2.Text, out res))
            {
                if (Convert.ToInt32(textBox2.Text) <= 0)
                    errorProvider1.SetError(textBox2, "Id can not be less than 0");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox4_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double res;
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                errorProvider1.SetError(textBox4, "Cannot be Empty");
            }
            else
            if (!double.TryParse(textBox4.Text, out res))
            {
                errorProvider1.SetError(textBox4, "Format exception");
            }
            else
            if (double.TryParse(textBox4.Text, out res))
            {
                if (Convert.ToDouble(textBox4.Text) <= 0)
                    errorProvider1.SetError(textBox4, "Price can not be less than 0");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void comboBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                errorProvider1.SetError(comboBox2, "Empty");
            }
            else
            if (!comboBox2.Text.Contains("Hot Drink") && !comboBox2.Text.Contains("Cold Drink") &&
                !comboBox2.Text.Contains("Hot Food") && !comboBox2.Text.Contains("Cold Food"))
            {
                errorProvider1.SetError(comboBox2, "There are not such catagory");
            }
            else
            {
                errorProvider1.Clear();
            }
        }
    }
}
