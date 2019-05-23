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
    public partial class Login : Form
    {
        ShoppingEntities db = new ShoppingEntities();

        public Login()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            bool allEmpty = Extensions.isNotEmpty(new string[] {
              email, password
            }, String.Empty);

            if (allEmpty)
            {
              
                string adminName = db.AdminSettings.First(ad => ad.Id == 1).Email;
                string adminPassword = db.AdminSettings.First(ad => ad.Id == 1).Password;
                if(email==adminName && password == adminPassword)
                {
                    AdminDashboard ads = new AdminDashboard();
                    this.Close();
                    ads.ShowDialog();
                }
                else
                {
                    Customer cus = db.Customers.FirstOrDefault(a => a.Email == email);

                    if (cus != null)
                    {
                        if (cus.Password == password.hashMe())
                        {
                            if (chkRemember.Checked)
                            {
                                Properties.Settings.Default.email = email;
                                Properties.Settings.Default.password = password;
                                Properties.Settings.Default.remember = true;

                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                Properties.Settings.Default.email = "";
                                Properties.Settings.Default.password = "";
                                Properties.Settings.Default.remember = false;

                                Properties.Settings.Default.Save();
                            }
                            Dashboard das = new Dashboard(cus);
                            das.ShowDialog();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Password is not correct";
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Email is not correct";
                    }
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Please all the fields";
            }

           
        }

        private void FormLoad(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.remember)
            {
                txtEmail.Text = Properties.Settings.Default.email;
                txtPassword.Text = Properties.Settings.Default.password;
                chkRemember.Checked = true;
            }
        }
    }
}
