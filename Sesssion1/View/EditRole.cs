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
    public partial class EditRole : Form
    {
        private User user;
        private UserModel model;
        private OfficeModel officeModel;
        public EditRole(User user)
        {
            InitializeComponent();

            this.user = user;

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

        private void EditRole_Load(object sender, EventArgs e)
        {
            txtEmail.Text = user.email;
            txtFirst.Text = user.firstName;
            txtLast.Text = user.lastName;
            ComboboxItem t = new ComboboxItem();
            t.Value = user.officeID;
            cbxOffice.SelectedIndex = cbxOffice.FindString(OfficeModel.GetOfficeByID(user.officeID).Title);

            bool check = ((user.roleID == 1) ? (rdbAdmin.Checked = true) : (rdbUser.Checked = true));
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
            else if (CheckInput(txtFirst))
            {
                MessageBox.Show("Ban chua nhap Ho dem");
            }
            else if (CheckInput(txtLast))
            {
                MessageBox.Show("Ban chua nhap Ten");
            }
            else
            {
                ComboboxItem selectedValue = cbxOffice.SelectedItem as ComboboxItem;

                if (selectedValue.Value <= 0)
                {
                    MessageBox.Show("Ban chua chon Buu dien");
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void btnAppl_Click(object sender, EventArgs e)
        {
            if(CheckValidateData())
            {
                user.email = txtEmail.Text;
                user.firstName = txtFirst.Text;
                user.lastName = txtFirst.Text;
                ComboboxItem selectedValue = cbxOffice.SelectedItem as ComboboxItem;
                user.officeID = selectedValue.Value;
                if (rdbAdmin.Checked)
                {
                    user.roleID = 1;
                }
                else
                {
                    user.roleID = 2;
                }
                if(!model.UpdateUser(user))
                {
                    MessageBox.Show("Khong the cap nhat thong tin.");
                }
                this.Close();
            }
        }
    }
}
