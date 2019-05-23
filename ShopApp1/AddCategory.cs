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
    public partial class AddCategory : Form
    {
        ShoppingEntities db = new ShoppingEntities();

        Category selectedCategory;

        public AddCategory()
        {
            InitializeComponent();
        }

        private void AddCategory_Load(object sender, EventArgs e)
        {
            FillDgCategories();
        }
        private void FillDgCategories()
        {
            dtgCategory.DataSource = db.Categories.Where(ct => ct.Status == 1)
                .Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();
            dtgCategory.Columns[0].Visible = false;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text;
            if (categoryName != string.Empty)
            {
                if (db.Categories.Any(ct => ct.Name == categoryName))
                {
                    lblError.Text = "Dear Admin.Yeke oglansin bu adda category adi db'da var ";
                    lblError.Visible = true;
                }
                else
                {
                    db.Categories.Add(new Category
                    {
                        Name = categoryName
                    });
                    db.SaveChanges();
                    MessageBox.Show("Category Name was successfully created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillDgCategories();
                }
            }
            else
            {
                lblError.Text = "Category name is not empty";
                lblError.Visible = true;
            }
        }


        private void ChangeMode(string mode)
        {
            if (mode == "neriman")
            {
                btnAddCategory.Visible = false;
                btnCategoryEdit.Visible = true;
                btnDeleteCategory.Visible = true;
            }
            else
            {
                btnAddCategory.Visible = true;
                btnCategoryEdit.Visible = false;
                btnDeleteCategory.Visible = false;
            }
        }
        private void DtgCategory_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int categoryId = (int)dtgCategory.Rows[e.RowIndex].Cells[0].Value;
            selectedCategory = db.Categories.FirstOrDefault(ct => ct.Id == categoryId);
            txtCategoryName.Text = selectedCategory.Name;
            ChangeMode("neriman");
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {

            if (selectedCategory != null)
            {
                if (txtCategoryName.Text != string.Empty)
                {
                    DialogResult result = MessageBox.Show("Dogurdan bu Category: " + txtCategoryName.Text + " silmək istəyirsən?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        selectedCategory.Status = 0;
                        db.SaveChanges();
                        FillDgCategories();
                        ChangeMode("add");
                        txtCategoryName.Text = "";
                    }

                }
                else
                {
                    lblError.Text = "Category name is not empty";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Please select a category";
                lblError.Visible = true;
            }
        }



        private void BtnCategoryEdit_Click(object sender, EventArgs e)
        {
            if (selectedCategory != null)
            {
                if (txtCategoryName.Text != string.Empty)
                {
                    selectedCategory.Name = txtCategoryName.Text;
                    db.SaveChanges();
                    FillDgCategories();
                    txtCategoryName.Text = "";
                    ChangeMode("add");

                }
                else
                {
                    lblError.Text = "Category Name is not empty";
                    lblError.Visible = true;
                }
            }

            else
            {
                lblError.Text = "Please select Category";
                lblError.Visible = true;
            }
        }

    }
}