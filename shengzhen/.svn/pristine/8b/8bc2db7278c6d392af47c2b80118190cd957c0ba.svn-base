using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 后台服务程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection sqlconnect = null;
        string connstring = string.Empty;
        private void Form1_Load(object sender, EventArgs e)
        {
            connstring = AppConfig.ConfigGetValue(Application.StartupPath + "\\Workshop.exe", "Connectstring");
            button1_Click(null, null);
        }

        private SqlConnection MyCon
        {
            get
            {
                if (sqlconnect == null)
                {
                    try
                    {
                        sqlconnect = new SqlConnection(connstring);
                        sqlconnect.Open();
                        lblmsg.Text = "数据库连接成功！";
                    }
                    catch (Exception Err)
                    {
                       lblmsg.Text = "数据库连接错误！";
                    }
                }
                else
                {
                    if (sqlconnect.State == ConnectionState.Closed)
                    {
                        sqlconnect.Close();
                    }
                }
                return sqlconnect;
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.Visible = true;
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.notifyIcon1.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("你确定要关闭程序吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Interval = 45 * 60 * 1000;
            timer3.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand mycmd = new SqlCommand("Sp_ChangeState", MyCon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    if (MyCon.State == ConnectionState.Open)
                    {
                        mycmd.ExecuteNonQuery();
                        lblmsg.Text = "正常运行中！" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
            catch (SqlException Err)
            {
                //timer1.Enabled = false;
                lblmsg.Text = Err.Message;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand mycmd = new SqlCommand("StatWorkHours", MyCon))
                {
                    mycmd.CommandType = CommandType.StoredProcedure;
                    if (MyCon.State == ConnectionState.Open)
                    {
                        mycmd.ExecuteNonQuery();
                        lblmsg2.Text = "正常运行中！" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
            catch (SqlException Err)
            {
                //timer2.Enabled = false;
                lblmsg2.Text = Err.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 13 || DateTime.Now.Hour == 16 || DateTime.Now.Hour == 19)
                {
                    AppConfig.DbBackup();
                    if (DateTime.Now.Day % 2 == 0)
                    {
                        AppConfig.DelDataBack();
                        lblmsg3.Text = "数据库备份成功！" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
            catch (Exception Err)
            {
                lblmsg3.Text = Err.Message;
                //timer3.Enabled = false;
            }
        }
    }
}