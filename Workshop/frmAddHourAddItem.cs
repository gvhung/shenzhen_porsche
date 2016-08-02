using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmAddHourAddItem : Form
    {
        public frmAddHourAddItem(int recordid)
        {
            InitializeComponent();
            RecordID = recordid;
        }
        private int RecordID = -1;
        private string state = string.Empty;
        string worker = string.Empty;
        private DateTime StartServiceTime = new DateTime();
        private DateTime PlanCompleteTime = new DateTime();
        private void frmAddHour_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select Worker,ServiceItem,StartServiceTime,PlanCompleteTime,EndServiceTime,ServiceHour,CarNo,State from Booking where ID=" + RecordID;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                txtServiceHour.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));
                txtItem.Text = Dt.Rows[0]["ServiceItem"].ToString();
                worker = Dt.Rows[0]["Worker"].ToString();
                state = Dt.Rows[0]["State"].ToString();
                PlanCompleteTime = DateTime.Parse(Dt.Rows[0]["PlanCompleteTime"].ToString());
                StartServiceTime = DateTime.Parse(Dt.Rows[0]["StartServiceTime"].ToString());
                this.Text = Dt.Rows[0]["CarNo"].ToString() + " " + this.Text;
                txtAddItem.Focus();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            decimal servicehours = 0;
            string sqlstring = string.Empty;
            if (state == "中断" || state == "过时")
            {
                ClsBLL.ServicePauseStart(RecordID);//如果是中断则结束中断
            }
            if (txtHours.Text != string.Empty)
            {
                servicehours = decimal.Parse(txtHours.Text);
                if (servicehours < 50)
                {
                    MessageBox.Show("维修工时错误！");
                    return;
                }
                else
                {
                    servicehours = decimal.Parse(servicehours.ToString()) / 100;
                }
            }
            decimal hours = servicehours + decimal.Parse(txtServiceHour.Text) / 100;
            //新的计划完成时间=当前时间+追加工时
            DateTime plancompletetime = PlanCompleteTime.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
            if (state == "中断")
            {
                plancompletetime = plancompletetime.AddMinutes(ClsBLL.Pausemins(RecordID, StartServiceTime));
            }
            if (plancompletetime.CompareTo(DateTime.Today.AddHours(9)) == -1)  //如果小于今天
            {
                plancompletetime = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
            }
            try
            {
                sqlstring = "Update booking set State='维修进行中',ServiceHour=" + hours + ",PlanCompleteTime='" + plancompletetime + "',EndServiceTime=Null,ServiceItem=ServiceItem + '," + txtItem.Text + "',Remark=isnull(Remark,'')+'，追加维修项目' where ID=" + RecordID;
                if (txtHours.Text != string.Empty)
                {
                    sqlstring += ";Insert Into ServiceAddHours(BookID,OldHours,AddHours,AddItem,Worker,AddTime)values(" + RecordID + "," + decimal.Parse(txtServiceHour.Text) / 100 + "," + decimal.Parse(txtHours.Text) / 100 + ",'"+ txtAddItem.Text +"','" + worker + "','" + DateTime.Today.ToShortTimeString() + "')";
                }
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    DialogResult = DialogResult.OK;
                    ClsBLL.AddMsg(RecordID, "车牌号码:" + this.Text + txtHours.Text + "TU--" + ClsBLL.UserName);
                    this.DialogResult = DialogResult.OK;
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

        private void frmAddHour_Activated(object sender, EventArgs e)
        {
            txtHours.Focus();
        }
    }
}