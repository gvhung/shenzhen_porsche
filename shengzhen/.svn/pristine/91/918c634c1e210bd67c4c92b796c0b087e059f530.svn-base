using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            textBox1.Text = AppConfig.ConfigGetValue(Application.ExecutablePath, "LastUserID");
            if (textBox1.Text == string.Empty)
            {
                textBox1.Focus();
            }
            else
            {
                textBox2.Focus();
            }
            DateTime SysDBTime = DateTime.Parse(SQLDbHelper.ExecuteScalar("Select GetDate()").ToString());
            TimeSpan ts = SysDBTime.Subtract(DateTime.Now);
            if (Math.Abs(ts.Minutes)>3)
            {
                MessageBox.Show("你的电脑时间与服务器时间不一致，请调整时间！");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstring = "Select UserName from SysUser where UserID='" + textBox1.Text + "' and Pwd='"+ textBox2.Text +"'";
            try
            {
                object obj = SQLDbHelper.ExecuteScalar(sqlstring);
                if (obj == null)
                {
                    MessageBox.Show("用户名不存在或者密码错误！");
                    textBox1.Focus();
                    return;
                }
                else
                {
                    //将计算机名称保存到数据库中
                    if (int.Parse(SQLDbHelper.ExecuteScalar("Select count(*) from VersionUser where Computer='" + Environment.MachineName + "'").ToString()) == 0)
                    {
                        SQLDbHelper.ExecuteSql("Insert into VersionUser(Computer,Ver)values('" + Environment.MachineName + "',0)");
                    }
                    else
                    {
                        int userver = int.Parse(SQLDbHelper.ExecuteScalar("Select ver from VersionUser where Computer='" + Environment.MachineName + "'").ToString());
                        int sysver = int.Parse(SQLDbHelper.ExecuteScalar("Select ver from VersionSys").ToString());
                        if (userver < sysver)
                        {
                            if (MessageBox.Show("软件有新的版本可以升级！你需要升级吗？", "软件升级", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                SQLDbHelper.ExecuteSql("Update VersionUser Set Ver="+ sysver +" where Computer='" + Environment.MachineName + "'");
                                System.Diagnostics.Process.Start(Application.StartupPath + @"\Update.exe");
                                Application.Exit();
                            }
                        }
                    }
                    ClsBLL.UserID = textBox1.Text;
                    ClsBLL.UserName = obj.ToString();
                    ClsBLL.UserGroup = ClsBLL.GetUserGroup(ClsBLL.UserName);
                    AppConfig.ConfigSetValue(Application.ExecutablePath, "LastUserID",textBox1.Text);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void frmLogin_Activated(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                textBox2.Focus();
            }
        }
    }
}