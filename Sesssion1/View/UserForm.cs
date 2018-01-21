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
    public partial class UserForm : Form
    {
        private string str1 = "], Welcome to AMONIC Airlines Automation System.";

        public User user { get; set; }
        private UserModel model;
        private Log log;
        private int crash = 0;

        public UserForm(User user)
        {
            InitializeComponent();
            this.user = user;
            model = new UserModel();
            log = model.getLog(user);
        }

        private TimeSpan TimeSpendOnSys(DateTime begin, DateTime end)
        {
            TimeSpan time = end - begin;
            return time;
        }

        private string TimeSpendOnSystem(DateTime begin, DateTime end)
        {
            TimeSpan time;
            try
            {
                time = end - begin;

                int hours = (int)time.TotalHours;
                int min = (int)time.TotalMinutes;
                int sec = (int)time.TotalSeconds;
                return hours + ":" + min + ":" + sec;
            }
            catch { }
            return string.Empty;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            try
            {
                Log l = model.getTheFirstRowBefor(user);
                if (l.timeLogout == null)
                {
                    new LogDetected(l).ShowDialog();
                }

                lbWellcome.Text = "Hi [" + model.GetName(user.ID) + str1;
                timer1.Start();

                List<Log> list = model.getAllLogByID(user.ID);
                int i = 0;
                try
                {
                    foreach (var item in list)
                    {
                        this.dataGridView1.Rows.Add();
                        if ((item.timeLogout == null) && i != 0)
                        {
                            ChangeBackcolor(i);
                            dataGridView1.Rows[i].Cells[0].Value = item.timeLogin.ToString("dd-MM-yyyy");
                            dataGridView1.Rows[i].Cells[1].Value = item.timeLogin.ToString("hh:mm");
                            dataGridView1.Rows[i].Cells[3].Value = ((item.timeLogout == null) ? "*" : item.timeLogin.ToString("dd-MM-yyyy hh:mm"));
                            dataGridView1.Rows[i].Cells[2].Value = ((item.timeLogout == null) ? "*" : TimeSpendOnSystem(item.timeLogin, item.timeLogout.Value));
                            dataGridView1.Rows[i].Cells[4].Value = ((item.description == null) ? "*" : item.description);
                            crash++;
                        }
                        else if (i != 0)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = item.timeLogin.ToString("dd-MM-yyyy");
                            dataGridView1.Rows[i].Cells[1].Value = item.timeLogin.ToString("hh:mm");
                            dataGridView1.Rows[i].Cells[3].Value = TimeSpendOnSystem(item.timeLogin, item.timeLogout.Value);
                            dataGridView1.Rows[i].Cells[2].Value = (item.timeLogout == null) ? "*" : item.timeLogout.Value.ToString("hh:mm");
                            dataGridView1.Rows[i].Cells[4].Value = "";
                        }
                        i++;

                    }
                }
                catch { }
                lbCrash.Text = crash.ToString();
            }
            catch
            {
                lbCrash.Text = "0";
            }
        }

        private void ChangeBackcolor(int index)
        {
            try
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
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            lbTime.Text = TimeSpendOnSystem(log.timeLogin, DateTime.Now);
        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {

                if (MessageBox.Show("Ban co chac chan muon thoat?", "Thong bao", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    Log l = model.getLog(user);
                    model.UpdateLog(user, l);
                    this.Dispose();
                    this.Close();
                    Application.Exit();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
