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
    public partial class LogDetected : Form
    {
        public Log log { get; set; }
        private UserModel model;
        private string str1 = "No logout detected form last login on";
        public LogDetected(Log log)
        {
            InitializeComponent();
            this.log = log;
            model = new UserModel();
        }

        private void LogDetected_Load(object sender, EventArgs e)
        {
            str1 += log.timeLogin.ToString("dd-MM-yyyy") + " at " + log.timeLogin.ToString("hh:mm");
            
            lbdetecd.Text = str1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogDetected_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radioButton1.Checked)
            {
                log.crashID = 1;
            }
            else if (radioButton2.Checked)
            {
                log.crashID = 2;
            }
            log.description = txtReason.Text;
            model.UpdateLogDetail(log);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtReason.Text = "Software Crash";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtReason.Text = "System Crash";
        }
    }
}
