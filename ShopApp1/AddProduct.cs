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
    public partial class AddProduct : Form
    {
        ShoppingEntities db = new ShoppingEntities();
        Product selectedProduct;
        public AddProduct()
        {
            InitializeComponent();
        }
        private void FillCategoryCombo()
        {
            cmbCatName.Items.AddRange(db.Categories.Select(ct => ct.Name).ToArray());
        }
        private void AddProduct_Load(object sender, EventArgs e)
        {
            FillCategoryCombo();
            FillDtgProduct();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string productName = txtProName.Text;
            string price = txtPrice.Text;
            string comboName = cmbCatName.Text;
            int productPrice;
            if (productName != string.Empty && price != string.Empty)
            {
                if(int.TryParse(price,out productPrice))
                {
                    lblError.Visible = false;
                    if (db.Products.Any(pro => pro.Name == productName))
                    {
                        lblError.Visible = true;
                        lblError.Text = "This Product has already been exsist in Database";
                    }
                    else
                    {

                        int selectedcategory = db.Categories.FirstOrDefault(sc => sc.Name == comboName).Id;
                        MessageBox.Show(selectedcategory.ToString());

                        db.Products.Add(new Product
                        {
                            Name = productName,
                            Category_id = selectedcategory,
                            Price = productPrice
                        });
                        db.SaveChanges();
                        MessageBox.Show("Product have been added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillDtgProduct();
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Please enter number";
                }
                
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please field all input";
            }
        }
        private void FillDtgProduct() {
            dtgView.DataSource = db.Products.Where(st=>st.Status==1).Select(ab => new
            {
                ab.Id,
                ab.Name,
                ab.Price

            }).ToList();
            dtgView.Columns[0].Visible = false;
        }
        private void ChangeMode(string mode)
        {
            if (mode == "edit")
            {
                btnAdd.Visible = false;
                btnDelete.Visible = true;
                btnEdit.Visible = true;
            }
            else
            {
                btnAdd.Visible = true;
                btnDelete.Visible = false;
                btnEdit.Visible = false;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            string price = txtPrice.Text;
            string CategoryName = cmbCatName.Text;
            int productPrice;
            if (selectedProduct != null)
            {
                if (price != string.Empty && CategoryName != string.Empty && txtProName.Text != string.Empty)
                {
                    if(int.TryParse(price,out productPrice))
                    {
                        selectedProduct.Price = productPrice;
                        selectedProduct.Name = txtProName.Text;
                        selectedProduct.Category.Name = CategoryName;
                        db.SaveChanges();
                        FillDtgProduct();
                       
                        ChangeMode("add");
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "zehmet olmasa reqem secin";
                    }
                }
                else
                {

                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "zehmet olmasa mehsul secin";
            }
        
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            string price = txtPrice.Text;
            string CategoryName = cmbCatName.Text;
            if (selectedProduct != null)
            {
                if (price != string.Empty && CategoryName != null)
                {
                    DialogResult result = MessageBox.Show("Dogurdan bu Category: " + CategoryName + " silmək istəyirsən?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        selectedProduct.Status = 0;
                        db.SaveChanges();
                        ChangeMode("add");
                        CategoryName = "";
                        FillDtgProduct();
                    }
                }
            }
        }

        private void DtgView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int productId = (int)dtgView.Rows[e.RowIndex].Cells[0].Value;
            selectedProduct = db.Products.FirstOrDefault(ct => ct.Id == productId);
            txtProName.Text = selectedProduct.Name;
            cmbCatName.Text = selectedProduct.Category.Name;
            txtPrice.Text = selectedProduct.Price.ToString();
            ChangeMode("edit");
        }
    }
}
