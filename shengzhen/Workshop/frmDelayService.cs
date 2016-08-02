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
                ClsBLL.IniCombox(comboBox1, "�ж�ԭ��");
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Bt.Tag.ToString() == "�ж�")  //������ж�״̬�������ӳٵ�����Ķ���
            {
                ClsBLL.ServicePauseStart(RecordID);
            }
            string sqlstring = string.Empty;
            try
            {
                if (comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("�ӳ��ж�ԭ����Ϊ�գ�");
                    return;
                }
                sqlstring = "Insert into DelayService(BookID,Worker,StartServiceTime,PlanCompleteTime,DelayReason) select ID,Worker,StartServiceTime,PlanCompleteTime,'"+ comboBox1.Text +"' from Booking where ID=" + RecordID;
                sqlstring += ";Update booking set State='��ʱ������',DelayComplete='��ʱ������' where ID=" + RecordID;

                Bt.BackColor = Color.Orange;
                Bt.Tag = "��ʱ������";
                ClsBLL.AddMsg(RecordID, "���ƺ���:" + CarNo + "ά����ʱ������--" + ClsBLL.UserName);

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