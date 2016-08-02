using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmAddHourReser : Form
    {
        public frmAddHourReser(int recordid)
        {
            InitializeComponent();
            RecordID = recordid;
        }
        private int RecordID = -1;
        DateTime StartServiceTime = DateTime.Now;
        DateTime EndServiceTime = DateTime.Now;
        private string state = string.Empty;
        string worker = string.Empty;
        private void frmAddHour_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select Worker,ServiceItem,StartServiceTime,PlanCompleteTime,EndServiceTime,ServiceHour,CarNo,State from Booking where ID=" + RecordID;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                textBox1.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));
                StartServiceTime = DateTime.Parse(Dt.Rows[0]["StartServiceTime"].ToString());
                if(Dt.Rows[0]["EndServiceTime"].ToString()!=string.Empty)
                {
                    EndServiceTime = DateTime.Parse(Dt.Rows[0]["EndServiceTime"].ToString());
                }
                textBox2.Text = Dt.Rows[0]["ServiceItem"].ToString();
                worker = Dt.Rows[0]["Worker"].ToString();
                textBox3.Text = worker;
                state = Dt.Rows[0]["State"].ToString();
                this.Text = Dt.Rows[0]["CarNo"].ToString() + " " + this.Text;

                sqlstring = "Select WorkerName,Worker.WorkerCode from WorkerPlan inner join Worker on WorkerPlan.WorkerCode=Worker.WorkerCode Where Wyear=" + DateTime.Today.Year + " and Wmonth=" + DateTime.Today.Month + " and Wday=" + DateTime.Today.Day + " and IsWork=1 and Position like '%工人%' and Workergroup in ('" + ClsBLL.UserGroup + "') order by Workergroup desc";
                Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(Dt.Rows[i]["WorkerName"].ToString());
                }
                comboBox1.Text = textBox3.Text;
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
            decimal hours = servicehours + decimal.Parse(textBox1.Text) / 100;
            //新的计划完成时间=当前时间+追加工时
            DateTime plancompletetime = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
            try
            {
                sqlstring = "Insert Into Reservice(BookID,Worker,PlanCompleteTime,StartServiceTime,EndServiceTime,AddHours)Select ID,Worker,PlanCompleteTime,StartServiceTime,EndServiceTime," + servicehours + " From Booking Where ID=" + RecordID;
                sqlstring += ";Update booking set State='维修进行中',ServiceHour=" + hours + ",StartServiceTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',EndServiceTime=Null,PlanCompleteTime='" + plancompletetime + "',Worker='" + comboBox1.Text + "',Remark=isnull(Remark,'')+'，返修' where ID=" + RecordID;
                if (txtHours.Text != string.Empty)
                {
                    sqlstring += ";Insert Into ServiceAddHours(BookID,OldHours,AddHours,Worker,AddTime)values(" + RecordID + "," + decimal.Parse(textBox1.Text) / 100 + "," + decimal.Parse(txtHours.Text) / 100 + ",'" + comboBox1.Text + "','" + DateTime.Today.ToShortTimeString() + "')";
                }
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    DialogResult = DialogResult.OK;
                    ClsBLL.AddMsg(RecordID, "车牌号码:" + this.Text + txtHours.Text + "TU--" + ClsBLL.UserName);
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