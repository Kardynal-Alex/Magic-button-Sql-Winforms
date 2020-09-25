using System;
using System.Windows.Forms;

namespace Project_button
{
    public partial class Form7 : Form
    {
        public Form7(string text)
        {
            InitializeComponent();
            label1.Text = text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            this.Close();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            Top = 20;
            Left = Screen.PrimaryScreen.Bounds.Width - Width*4 - 20;
            timer1.Start();
        }
    }
}
