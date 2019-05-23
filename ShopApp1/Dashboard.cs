using ShopApp1.Model;
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
    public partial class Dashboard : Form
    {
        ShoppingEntities db = new ShoppingEntities();
        Customer activeCustomer { get; set; }
        public Dashboard(Customer a)
        {
            activeCustomer=a;
            InitializeComponent();
        
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = string.Format("Dear, {0} {1} Xoş gelmisiz", activeCustomer.Firstname, activeCustomer.Surname);
            FillDataView(activeCustomer.Id);
            FillCheckCategory();
            FilterChecKCategory();
            nmMinPrice.Maximum = (int)db.Products.Max(a => a.Price);
            nmMaxPrice.Maximum = (int)db.Products.Max(a => a.Price);

        }
        private void FilterChecKCategory()
        {
            cmbFilterCategory.Items.AddRange(db.Categories.Where(a=>a.Status==1).Select(c => c.Name).ToArray());
        }
        private void FillCheckCategory()
        {
            cmbCategory.Items.AddRange(db.Categories.Where(a => a.Status == 1).Select(c => c.Name).ToArray());
        }
        private void FillDataView(int id)
        {
            dtgView.DataSource = db.Orders.Where(a=>a.Customer_id==id).Select(o =>new
            {
                o.Product.Name,
                o.Product.Price,
                o.Amount,
                o.Purchase_date,

            }).ToArray();
        }
      

        private void indexChanged(object sender, EventArgs e)
        {
            
            string categorName= cmbCategory.Text;
          int categoryID=  db.Categories.First(c => c.Name == categorName).Id;
            cmbProduct.Items.Clear();
            FillCheckProduct(categoryID);

        }
        private void FillCheckProduct(int id)
        {
            cmbProduct.Items.AddRange(db.Products.Where(pr=>pr.Category_id==id).Select(pr => pr.Name).ToArray());

        }

        private void CmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string productName = cmbProduct.Text;
            int price = (int)db.Products.First(pr => pr.Name == productName).Price;
            lblPrice.Text = price + " Azn";
        
                lblPrice.Visible = true;
                lblAmount.Visible = true;
           

        }



        private void CmbCategory_DropDownClosed(object sender, EventArgs e)
        {
            cmbProduct.Text = "";
            lblPrice.Visible = false;
        }

        private void TxtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar<48 || e.KeyChar>57) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
       
        private void BtnBuy_Click(object sender, EventArgs e)
        {
            string category = cmbCategory.Text;
            string product = cmbProduct.Text;
            string amount = txtAmount.Text;
            int amounts;
            bool allEmpty = Extensions.isNotEmpty(new string[]
            {
                category,product,amount
            },string.Empty);
            if (allEmpty)
            {
                if(int.TryParse(amount,out amounts))
                {
                    Product selectedProduct = db.Products.FirstOrDefault(pr => pr.Name == product);
                    int CategoryId = db.Categories.First(a => a.Name == category).Id;
                    if (selectedProduct != null)
                    {
                        Product prd = new Product();
                        prd.Name = product;
                        prd.Category_id = CategoryId;


                    }
                    else
                    {

                    }
                    lblError.Visible = false;
                    Order ord = new Order();
                    ord.Customer_id = activeCustomer.Id;
                    ord.Product_id = selectedProduct.Id;
                    ord.Purchase_date = DateTime.Now;
                    ord.Amount = amounts;
                    db.Orders.Add(ord);
                    db.SaveChanges();
                    string t = string.Format("Dear {0}.Siz {1} {2} ədəd uğurla alındı", activeCustomer.Firstname, selectedProduct.Name, amount);
                    MessageBox.Show(t, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillDataView(activeCustomer.Id);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Please add numeric text";
                }
               
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please fill all input";
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string categoryName = cmbFilterCategory.Text;
            int minPrice = (int)nmMinPrice.Value;
            int maxPrice = (int)nmMaxPrice.Value;
            int maxAllPrice = (int)db.Products.Max(m => m.Price);
            if (minPrice > maxPrice || maxPrice==0)
            {
                maxPrice = maxAllPrice;
                
            }
            dtgView.DataSource = db.Orders.Where(or => or.Customer_id == activeCustomer.Id &&
            or.Product.Category.Name.Contains(categoryName) && or.Product.Price >= minPrice &&
            or.Product.Price <= maxPrice).Select(or => new
            {
                or.Product.Name,
                or.Product.Price,
                or.Purchase_date,
                or.Amount


            }).ToList();
        }
    }
}
