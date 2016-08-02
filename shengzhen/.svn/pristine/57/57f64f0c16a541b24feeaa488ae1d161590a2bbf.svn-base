using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmParkSite : Form
    {
        public frmParkSite(int recordid,int tag,string carno)
        {
            InitializeComponent();
            RecordID = recordid;
            IntTag = tag;
            this.Text = carno + this.Text;
            CarNo = carno;
        }
        private int RecordID = -1;
        private int IntTag = 0;
        private string CarNo = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            string sqlstring = string.Empty;
            if (IntTag == 0)
            {
                sqlstring = "Update Booking set ParkSite='" + textBox1.Text + "' Where ID=" + RecordID;
            }
            else
            {
                sqlstring = "Update A set A.State='洗车',A.ParkSite='" + textBox1.Text + "',A.ClearCarTime='" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm") + "'";
                sqlstring += " From Booking A,(Select CarNo,BookTime From Booking Where ID=" + RecordID + ") B";
                sqlstring += " Where A.CarNo=B.CarNo And substring(convert(nvarchar(50),A.booktime,120),1,10)=substring(convert(nvarchar(50),B.booktime,120),1,10)";
            }
            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
            {
                DialogResult = DialogResult.OK;
            }
            ClsBLL.AddMsg(RecordID, "车牌号码:" + CarNo + "现在开始洗车--"+ClsBLL.UserName);

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmParkSite_Load(object sender, EventArgs e)
        {
            if (IntTag == 1)
            {
                string sqlstring = "Select ParkSite from Booking Where ID=" + RecordID;
                object obj = SQLDbHelper.ExecuteScalar(sqlstring);
                if (obj != null)
                {
                    textBox1.Text = obj.ToString();
                }
            }
            textBox1.Focus();
        }
    }
}