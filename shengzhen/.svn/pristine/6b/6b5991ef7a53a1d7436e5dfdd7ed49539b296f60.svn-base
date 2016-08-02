using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmMsg : Form
    {
        public frmMsg(string msg,int bookid,int msgid)
        {
            InitializeComponent();
            label1.Text = msg;
            BookID = bookid;
            MsgID = msgid;
        }
        private int BookID = -1;
        private int MsgID = -1;
        private void frmMsg_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            string sqlstring = "Insert Into UserMsg(UserName,MsgID)values('" + ClsBLL.UserName + "'," + MsgID + ")";
            try
            {
                SQLDbHelper.ExecuteSql(sqlstring);
            }
            catch { }
        }

        private void frmMsg_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BookID > -1)
            {
                frmBookLook fbn = new frmBookLook(BookID);
                fbn.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}