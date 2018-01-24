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
    public partial class Manager : Form
    {

        public User mainUser { get; set; }
        private Log log { get; set; }

        private UserModel userModel = new UserModel();
        private OfficeModel model = null;

        public Manager(User user)
        {
            InitializeComponent();

            this.mainUser = user;
            log = userModel.getLog(user);

            ComboboxItem item = new ComboboxItem();
            item.Text = "All Office";
            item.Value = 0;
            cbxOffice.Items.Add(item);
            model = new OfficeModel();
            
            foreach(Office i in model.GetOffices())
            {
                ComboboxItem t = new ComboboxItem();
                t.Text = i.Title;
                t.Value = i.ID;
                cbxOffice.Items.Add(t);
            }
            cbxOffice.SelectedIndex = 0;
        }

        private static int Age(DateTime birthDate, DateTime laterDate)
        {
            int age;
            age = laterDate.Year - birthDate.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            return age;
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            //userModel = new UserModel();

            Log l = userModel.getTheFirstRowBefor(mainUser);
            if(l.timeLogout == null)
            {
                new LogDetected(l).ShowDialog();
            }

            int i = 0;
            foreach(var item in userModel.GetAllUsers(cbxOffice.SelectedIndex))
            {
                this.dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = item.firstName;
                dataGridView1.Rows[i].Cells[1].Value = item.lastName;
                dataGridView1.Rows[i].Cells[2].Value = Age(item.birthDate, DateTime.Now);
                dataGridView1.Rows[i].Cells[3].Value = ((item.roleID == 1) ? "Administrator" : "Other User");
                dataGridView1.Rows[i].Cells[4].Value = item.email;
                dataGridView1.Rows[i].Cells[5].Value = OfficeModel.GetOfficeByID(item.officeID).Title;
                i++;
            }
        }

        private void WriteLog()
        {
            userModel.UpdateLog(mainUser, log);
        }

        private void cbxOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            Load_data();
        }

        private void Load_data()
        {
            ComboboxItem selectedValue = cbxOffice.SelectedItem as ComboboxItem;

            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            int i = 0;
            foreach (var item in userModel.GetAllUsers(selectedValue.Value))
            {
                this.dataGridView1.Rows.Add();
                if (!item.isActive)
                {
                    ChangeBackcolor(i);
                    dataGridView1.Rows[i].Cells[0].Value = item.firstName;
                    dataGridView1.Rows[i].Cells[1].Value = item.lastName;
                    dataGridView1.Rows[i].Cells[2].Value = Age(item.birthDate, DateTime.Now);
                    dataGridView1.Rows[i].Cells[3].Value = ((item.roleID == 1) ? "Administrator" : "Other User");
                    dataGridView1.Rows[i].Cells[4].Value = item.email;
                    dataGridView1.Rows[i].Cells[5].Value = OfficeModel.GetOfficeByID(item.officeID).Title;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[0].Value = item.firstName;
                    dataGridView1.Rows[i].Cells[1].Value = item.lastName;
                    dataGridView1.Rows[i].Cells[2].Value = Age(item.birthDate, DateTime.Now);
                    dataGridView1.Rows[i].Cells[3].Value = ((item.roleID == 1) ? "Administrator" : "Other User");
                    dataGridView1.Rows[i].Cells[4].Value = item.email;
                    dataGridView1.Rows[i].Cells[5].Value = OfficeModel.GetOfficeByID(item.officeID).Title;
                }
                i++;
            }
        }
        //yyyy-MM-dd
        private void ChangeBackcolor(int index)
        {
            DataGridViewRow row = dataGridView1.Rows[index];

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = Color.FromArgb(255, Color.Red);
            style.ForeColor = Color.Black;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].Style = style;
            }
        }


        private void cbxOffice_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRole.Enabled = true;
            btnEnable.Enabled = true;
            
        }

        private void addUser_Click(object sender, EventArgs e)
        {

        }

        private void btnRole_Click(object sender, EventArgs e)
        {
            User user = new User();
            int index = dataGridView1.SelectedRows[0].Index;
            string email = dataGridView1.Rows[index].Cells[4].Value.ToString();
            string first = dataGridView1.Rows[index].Cells[0].Value.ToString();
            string last = dataGridView1.Rows[index].Cells[1].Value.ToString();

            user = userModel.GetUser(email, first, last);
            EditRole edit = new EditRole(user);  
            edit.ShowDialog();
            Load_data();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            User user = new User();
            int index = dataGridView1.SelectedRows[0].Index;
            string email = dataGridView1.Rows[index].Cells[4].Value.ToString();
            string first = dataGridView1.Rows[index].Cells[0].Value.ToString();
            string last = dataGridView1.Rows[index].Cells[1].Value.ToString();

            user = userModel.GetUser(email, first, last);
            user.isActive = !user.isActive;
            if (!userModel.UpdateUser(user))
            {
                MessageBox.Show("Khong the cap nhat thong tin.");
            }
            Load_data();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Ban co chac chan muon thoat?", "Thong bao", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                userModel.UpdateLog(mainUser, log);
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
