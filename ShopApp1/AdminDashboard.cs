using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp1
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void AddCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCategory adc = new AddCategory();
            adc.ShowDialog();
        }

        private void AddProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProduct addpr = new AddProduct();
            addpr.ShowDialog();
        }
    }
}
