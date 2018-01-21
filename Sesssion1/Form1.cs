using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sesssion1.Code;
using Sesssion1.Model;
using Sesssion1.View;

namespace Sesssion1
{
    public partial class Form1 : Form
    {
        private SqlConnection conn = null;
        private int time = 15;
        private static int tt;
        private static int t = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new DTA().GetConnection();
        }

        private bool checkValidate(TextBox t)
        {
            return string.IsNullOrEmpty(t.Text);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (checkValidate(txtUsename))
            {
                MessageBox.Show("Your username is empty!!!", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (checkValidate(txtPassword))
                {
                    MessageBox.Show("Your password is empty!!!", "Warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    UserModel model = new UserModel();
                    if (model.CheckUser(txtUsename.Text, (encryptionMD5.md5(txtPassword.Text))) == -2)
                    {
                        lbStatus.Text = "Tai khoan khong ton tai";
                        lbStatus.ForeColor = Color.Red;
                    }
                    else if (model.CheckUser(txtUsename.Text, (encryptionMD5.md5(txtPassword.Text))) == -1)
                    {
                        lbStatus.Text = "Tai Khoan bi khoa";
                        lbStatus.ForeColor = Color.Red;
                    }
                    else if (model.CheckUser(txtUsename.Text, (encryptionMD5.md5(txtPassword.Text))) == 0)
                    {
                        lbStatus.Text = "Sai mat khau";
                        lbStatus.ForeColor = Color.Red;
                        t++;
                        if(t >= 3)
                        {
                            time = time * 2;
                            tt = time;
                            timer1.Enabled = true;
                            timer1.Interval = 1000;
                            timer1.Start();
                        }
                    }
                    else if(model.CheckUser(txtUsename.Text, (encryptionMD5.md5(txtPassword.Text))) == 1)
                    {
                        //MessageBox.Show("Dang nhap thanh cong");
                        User user = model.GetUser(txtUsename.Text, (encryptionMD5.md5(txtPassword.Text)));
                        model.WriteLog(user);
                        if(user.roleID == 1)
                        {

                            new Manager(user).Show();
                        }else if(user.roleID == 2)
                        {
                            new UserForm(user).Show();
                        }
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Co loi xay ra trong qua trinh dang nhap");
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                // Display the new time left
                // by updating the Time Left label.
                time = time - 1;
                lbStatus.ForeColor = Color.Red;
                lbStatus.Text ="Hay cho "+ time + " seconds de dang nhap lai.";
                btnLogin.Enabled = false;
            }
            else
            {
                t = 0;
                time = tt;
                timer1.Stop();
                timer1.Dispose();
                timer1.Enabled = false;
                btnLogin.Enabled = true;
                lbStatus.Text = "";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Ban co chac chan muon thoat?", "Thong bao", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
