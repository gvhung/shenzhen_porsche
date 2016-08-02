using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmDelayService : Form
    {
        public frmDelayService(int recordid, DoubleClickButton bt)
        {
            InitializeComponent();
            RecordID = recordid;
            Bt = bt;
        }
        private int RecordID = -1;
        private string CarNo = string.Empty;
        private string worker = string.Empty;
        DoubleClickButton Bt;
        private void frmDelayService_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select PlanOutTime,ServiceItem,Worker,ServiceHour,CarNo,StartServiceTime from Booking where ID=" + RecordID;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                txtPlanOut.Text = Dt.Rows[0]["PlanOutTime"].ToString();
                txtServiceItem.Text = Dt.Rows[0]["ServiceItem"].ToString();
                txtHours.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));
                this.Text = Dt.Rows[0]["CarNo"].ToString() + this.Text;
                CarNo = Dt.Rows[0]["CarNo"].ToString();
                worker = Dt.Rows[0]["Worker"].ToString();
                ClsBLL.IniCombox(comboBox1, "中断原因");
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Bt.Tag.ToString() == "中断")  //如果是中断状态，做了延迟到明天的动作
            {
                ClsBLL.ServicePauseStart(RecordID);
            }
            string sqlstring = string.Empty;
            try
            {
                if (comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("延迟中断原因不能为空！");
                    return;
                }
                sqlstring = "Insert into DelayService(BookID,Worker,StartServiceTime,PlanCompleteTime,DelayReason) select ID,Worker,StartServiceTime,PlanCompleteTime,'"+ comboBox1.Text +"' from Booking where ID=" + RecordID;
                sqlstring += ";Update booking set State='延时到明天',DelayComplete='延时到明天' where ID=" + RecordID;

                Bt.BackColor = Color.Orange;
                Bt.Tag = "延时到明天";
                ClsBLL.AddMsg(RecordID, "车牌号码:" + CarNo + "维修延时到明天--" + ClsBLL.UserName);

                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtHours_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != char.Parse("."))
            {
                ClsBLL.Key_Number(e);
            }
        }
    }
}