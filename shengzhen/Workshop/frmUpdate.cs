using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("确认密码不对！");
                return;
            }
            string sqlstring = "Select count(*) from SysUser where UserID='"+ ClsBLL.UserID +"' and Pwd='"+ textBox1.Text +"'";
            try
            {
                if (int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString()) == 0)
                {
                    MessageBox.Show("原密码错误！");
                    textBox1.Text = string.Empty;
                    textBox1.Focus();
                    return;
                }
                sqlstring = "Update SysUser Set Pwd='" + textBox2.Text + "' Where UserID='" + ClsBLL.UserID + "'";
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    MessageBox.Show("修改成功！");
                    this.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}