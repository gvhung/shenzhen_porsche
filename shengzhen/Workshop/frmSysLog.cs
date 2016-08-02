using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmSysLog : Form
    {
        public frmSysLog(string carno,int bookid)
        {
            InitializeComponent();
            this.Text = carno + "²Ù×÷ÈÕÖ¾";
            BookID = bookid;
        }
        int BookID = -1;
        private void frmSysLog_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select BookID,Message,Creator,SUBSTRING(CONVERT(nvarchar(50), CreateDate, 120), 6, 11) as CreateDate from SysLog Where BookID=" + BookID + " Order by CreateDate";
            try
            {
                dataGridView1.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
    }
}