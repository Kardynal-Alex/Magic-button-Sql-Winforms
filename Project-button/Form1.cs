using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Project_button
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-339MSAS\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True";
        private SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
            customizeDesign();
            button2.Tag = "Hot Drink";
            button3.Tag = "Cold Drink";
            button6.Tag = "Hot Food";
            button5.Tag = "Cold Food";
        }
        private  void customizeDesign()
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel6.Visible = false;
        }
        private void hideSubMenu()
        {
            if (panel2.Visible == true)
                panel2.Visible = false;
            if (panel3.Visible == true)
                panel3.Visible = false;
            if (panel6.Visible == true)
                panel6.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
            openChildForm(new Form6());

        }
        
        private async void show_button_product(string catagory)
        {
            SqlDataReader sqlDataReader = null; 

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM [Table] WHERE Catagory='" + catagory + "'", sqlConnection);
            sqlDataReader = await sqlCommand.ExecuteReaderAsync();
            
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;

            while (await sqlDataReader.ReadAsync())
            {
                Button b = new Button();
                b.Size = new Size(100, 140);
               
                if (!string.IsNullOrEmpty(sqlDataReader["Image"].ToString()))
                {
                    Byte[] byteImage = (byte[])sqlDataReader["Image"];
                    MemoryStream memoryStream = new MemoryStream(byteImage);
                    Bitmap bitmap = new Bitmap(memoryStream);
                    b.Image = bitmap;
                    b.ImageAlign = ContentAlignment.TopCenter;
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                }
                b.Font = new Font("Times New Roman", 9);
                b.ForeColor = Color.Black;

                Label label = new Label();
                label.Size = new Size(100 - 2, 40 - 1);
                label.Location = new Point(b.Width - 100 + 1, b.Height - 40);
                label.Text = sqlDataReader["Description"].ToString().TrimEnd(' ') + "\n" + $"${Convert.ToDouble(sqlDataReader["Price"].ToString()):f2}";
                label.TextAlign = ContentAlignment.MiddleCenter;
                b.Controls.Add(label);

                b.TextAlign = ContentAlignment.BottomCenter;
                b.Tag = sqlDataReader["Description"];
                b.FlatStyle = FlatStyle.Flat;
                b.Click += new EventHandler(b_Click);

                flowLayoutPanel.Controls.Add(b);
            }
            flowLayoutPanel.AutoScroll = true;
            panel5.Controls.Add(flowLayoutPanel);
            sqlDataReader.Close();
        }

        private void b_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            openChildForm(new Form5(sqlConnection,b.Tag.ToString()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showSubMenu(panel2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            show_button_product(button2.Tag.ToString());
            hideSubMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            show_button_product(button3.Tag.ToString());
            hideSubMenu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            show_button_product(button6.Tag.ToString());
            hideSubMenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            show_button_product(button5.Tag.ToString());
            hideSubMenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            showSubMenu(panel3);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel5.Controls.Add(childForm);
            panel5.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            openChildForm(new Form3());
            hideSubMenu();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            openChildForm(new Form2(sqlConnection));
            hideSubMenu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            showSubMenu(panel6);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }
        private int count(string catagory)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [Table] WHERE [Catagory]=@Catagory", sqlConnection);
            sqlCommand.Parameters.AddWithValue("Catagory", catagory);
            return Convert.ToInt32(sqlCommand.ExecuteScalar());
        }

        private int count_hot_drink() =>count("Hot Drink");
        
        private int count_cold_drink() => count("Cold Drink");
        
        private int count_hot_food() => count("Hot Food");
 
        private int count_cold_food() => count("Cold Food");
        
        private async void button10_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();

            int n_hot_drink = await Task.Run(() => count_hot_drink());
            int n_cold_drink = await Task.Run(() => count_cold_drink());
            int n_hot_food = await Task.Run(() => count_hot_food());
            int n_cold_food = await Task.Run(() => count_cold_food());

            openChildForm(new Form4(n_hot_drink,n_cold_drink,n_hot_food,n_cold_food));
            hideSubMenu();
        }
    }
}
