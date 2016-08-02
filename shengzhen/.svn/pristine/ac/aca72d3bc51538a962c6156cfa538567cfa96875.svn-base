using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmAddHour2Day : Form
    {
        public frmAddHour2Day(int recordid)
        {
            InitializeComponent();
            RecordID = recordid;
        }
        private int RecordID = -1;
        string worker = string.Empty;
        string state = string.Empty;
        private void frmAddHour_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select Worker,ServiceItem,StartServiceTime,PlanCompleteTime,EndServiceTime,ServiceHour,CarNo,State from Booking where ID=" + RecordID;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                txtHours.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));
                worker = Dt.Rows[0]["Worker"].ToString();
                txtOldWorker.Text = worker;
                
                state = Dt.Rows[0]["State"].ToString();
                this.Text = Dt.Rows[0]["CarNo"].ToString() + " " + this.Text;

                sqlstring = "Select WorkerName,Worker.WorkerCode from WorkerPlan inner join Worker on WorkerPlan.WorkerCode=Worker.WorkerCode Where Wyear=" + DateTime.Today.Year + " and Wmonth=" + DateTime.Today.Month + " and Wday=" + DateTime.Today.Day + " and IsWork=1 and Workergroup in ('" + ClsBLL.UserGroup + "') order by Workergroup desc";
                Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(Dt.Rows[i]["WorkerName"].ToString());
                }
                comboBox1.Text = worker;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            decimal addhours = 0;
            decimal newhours = 0;
            decimal starthours = 0;
            if (state == "中断" || state == "过时")
            {
                ClsBLL.ServicePauseStart(RecordID);//如果是中断则结束中断
            }
            if (txtAddHours.Text != string.Empty)
            {
                addhours = decimal.Parse(txtAddHours.Text);
                addhours = decimal.Parse(addhours.ToString()) / 100;
            }
            newhours = addhours + decimal.Parse(txtHours.Text) / 100;
            //新的计划完成时间=当前时间+追加工时
            starthours = ClsBLL.GetFactHours(RecordID);
            if (newhours > starthours)
            {
                starthours = newhours - starthours;  //预计维修工时减实际做过
            }
            DateTime plancompletetime = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(starthours * 60)));
            try
            {
                string sqlstring = "Update booking set State='维修进行中',StartServiceTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',Worker='" + comboBox1.Text + "' ,ServiceHour=" + newhours + ",PlanCompleteTime='" + plancompletetime + "',Remark=isnull(Remark,'')+'，" + this.Text + txtAddHours.Text + "TU' where ID=" + RecordID;
                if (txtAddHours.Text != string.Empty)
                {
                    sqlstring += ";Insert Into ServiceAddHours(BookID,OldHours,AddHours,Worker,AddTime)values(" + RecordID + "," + decimal.Parse(txtHours.Text) / 100 + "," + decimal.Parse(txtAddHours.Text) / 100 + ",'" + comboBox1.Text + "','" + DateTime.Today.ToShortTimeString() + "')";
                }
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    DialogResult = DialogResult.OK;
                    ClsBLL.AddMsg(RecordID, "车牌号码:" + this.Text + txtAddHours.Text + "TU--" + ClsBLL.UserName);
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
            txtAddHours.Focus();
        }
    }
}