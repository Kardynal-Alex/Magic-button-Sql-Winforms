using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Project_button
{
    public partial class Form4 : Form
    {
        public Form4(int n_hot_drink,int n_cold_drink,int n_hot_food,int  n_cold_food)
        {
            InitializeComponent();
            chart1.Series[0].Points.Clear();
            
            DataPoint p1 = chart1.Series[0].Points.Add(n_hot_drink);
            p1.Label =n_hot_drink.ToString();
            DataPoint p2 = chart1.Series[1].Points.Add(n_cold_drink);
            p2.Label = n_cold_drink.ToString();
            DataPoint p3 = chart1.Series[2].Points.Add(n_hot_food);
            p3.Label = n_hot_food.ToString();
            DataPoint p4 = chart1.Series[3].Points.Add(n_cold_food);
            p4.Label = n_cold_food.ToString();

            chart2.Series[0].Points.Clear();
            DataPoint p01 = chart2.Series[0].Points.Add(n_hot_drink);
            p01.Label = "Hot Drink " + n_hot_drink.ToString();
            DataPoint p02 = chart2.Series[0].Points.Add(n_cold_drink);
            p02.Label = "Cold Drink " + n_cold_drink.ToString();
            DataPoint p03 = chart2.Series[0].Points.Add(n_hot_food);
            p03.Label = "Hot Food " + n_hot_food.ToString();
            DataPoint p04 = chart2.Series[0].Points.Add(n_cold_food);
            p04.Label = "Cold Food  " + n_cold_food.ToString();
        }
    }
}
