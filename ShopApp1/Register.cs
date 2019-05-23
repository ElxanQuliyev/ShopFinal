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
    public partial class Register : Form
    {
        ShoppingEntities db = new ShoppingEntities();
        public Register()
        {
            InitializeComponent();
        }

    

        private void Button1_Click(object sender, EventArgs e)
        {
            
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            string email = txtEmail.Text;
            string password  = txtPassword.Text;
            string repeatpassword = txtRepeat.Text;
            string phone = txtPhone.Text;
            bool allEmpty = Extensions.isNotEmpty(new string[] {
                firstname, lastname, email, repeatpassword, phone, password
            }, String.Empty);
            if (allEmpty)
            {
                lblError.Visible = false;

                if (password.Length >= 8)
                {
                    if(password== repeatpassword)
                    {
                        Customer cusEmail = db.Customers.FirstOrDefault(a => a.Email == email);
                        if (cusEmail == null)
                        {
                            Customer cus = new Customer();
                            cus.Firstname = firstname;
                            cus.Surname = lastname;
                            cus.Email = email;
                            cus.Phone = phone;
                            cus.Password = password.hashMe();
                            db.Customers.Add(cus);
                            db.SaveChanges();
                            MessageBox.Show(firstname + " " + lastname + " was created successfully","Custumer Created",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Email already exsist";

                        }

                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Password and Repeat password does not contain";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Password should be long 8 charachter";
                }

            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please,field input";
            }
              
        }

        private void TxtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar<48 || e.KeyChar>57) && e.KeyChar != 8 && e.KeyChar!=43)
            {
                e.Handled = true;
            }
        }

     

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
