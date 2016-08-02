using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmPause : Form
    {
        public frmPause(int recordid)
        {
            InitializeComponent();
            RecordID = recordid;
        }
        private int RecordID = -1;
        private string CarNo = string.Empty;
        private string Worker = string.Empty;
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmPause_Load(object sender, EventArgs e)
        {
            ClsBLL.IniCombox(comboBox1, "�ж�ԭ��");
            DataTable Dt = SQLDbHelper.Query("Select Worker,CarNo from Booking Where ID=" + RecordID).Tables[0];
            CarNo = Dt.Rows[0]["CarNo"].ToString();
            Worker=Dt.Rows[0]["Worker"].ToString();
            this.Text = CarNo + "�ж�";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("��ѡ���ж�ԭ��");
                    return;
                }

                ClsBLL.AddServicePause(RecordID,Worker, comboBox1.Text);
                ClsBLL.AddMsg(RecordID, "���ƺ���:" + CarNo + "ά���жϣ�ԭ����" + comboBox1.Text + "--" + ClsBLL.UserName);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}