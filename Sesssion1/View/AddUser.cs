using Sesssion1.Code;
using Sesssion1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sesssion1.View
{
    public partial class AddUser : Form
    {

        private User user;
        private UserModel model;
        private OfficeModel officeModel;
        public AddUser()
        {
            InitializeComponent();

            ComboboxItem item = new ComboboxItem();
            item.Text = "Office name";
            item.Value = 0;
            cbxOffice.Items.Add(item);

            officeModel = new OfficeModel();
            model = new UserModel();

            foreach (Office i in officeModel.GetOffices())
            {
                ComboboxItem t = new ComboboxItem();
                t.Text = i.Title;
                t.Value = i.ID;
                cbxOffice.Items.Add(t);
            }
            cbxOffice.SelectedIndex = 0;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {

        }

        private void cbShowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowpass.Checked)
            {
                txtPass.UseSystemPasswordChar = false;
            }
            else
            {
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private bool CheckInput(TextBox t)
        {
            return string.IsNullOrEmpty(t.Text);
        }

        private bool CheckValidateData()
        {
            if (CheckInput(txtEmail))
            {
                MessageBox.Show("Ban chua nhap Email");
            }
            else if (CheckInput(txtFirstname))
            {
                MessageBox.Show("Ban chua nhap Ten");
            }
            else if (CheckInput(txtLastname))
            {
                MessageBox.Show("Ban chua nhap Ho dem");
            }
            else if (CheckInput(txtDateBirth))
            {
                MessageBox.Show("Ban chua nhap ngay sinh");
            }
            else if (CheckInput(txtPass))
            {
                MessageBox.Show("Ban chua nhap Mat khau");
            }
            else
            {
                DateTime t = DateTime.Now;
                if (!DateTime.TryParse(txtDateBirth.Text, out t))
                {
                    MessageBox.Show("Dinh danh ngay khong dung");
                }
                else
                {
                    ComboboxItem selectedValue = cbxOffice.SelectedItem as ComboboxItem;

                    if (selectedValue.Value <= 0)
                    {
                        MessageBox.Show("Ban chua chon Bua dien");
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(CheckValidateData())
            {
                user = new User();
                user.email = txtEmail.Text;
                user.firstName = txtFirstname.Text;
                user.lastName = txtLastname.Text;
                user.birthDate = DateTime.Parse(txtDateBirth.Text);
                user.password = txtPass.Text;

                ComboboxItem selectedValue = cbxOffice.SelectedItem as ComboboxItem;

                user.officeID = selectedValue.Value;

                if(model.CheckUser(user.email))
                {
                    MessageBox.Show("Tai khoan da duoc dang ky.");
                }
                else
                {
                    model.InsertUser(user);
                }
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(CheckInput(txtEmail) || CheckInput(txtFirstname) || CheckInput(txtLastname)
                || CheckInput(txtDateBirth) || CheckInput(txtPass))
            {
                if(MessageBox.Show("Form co du lieu chua duoc luu. Ban co chac chan muon thoat?", "Thong bao", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void AddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckInput(txtEmail) || CheckInput(txtFirstname) || CheckInput(txtLastname)
                || CheckInput(txtDateBirth) || CheckInput(txtPass))
            {
                if (MessageBox.Show("Form co du lieu chua duoc luu. Ban co chac chan muon thoat?", "Thong bao", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
