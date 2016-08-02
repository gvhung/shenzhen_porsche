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
            ClsBLL.IniCombox(comboBox1, "中断原因");
            DataTable Dt = SQLDbHelper.Query("Select Worker,CarNo from Booking Where ID=" + RecordID).Tables[0];
            CarNo = Dt.Rows[0]["CarNo"].ToString();
            Worker=Dt.Rows[0]["Worker"].ToString();
            this.Text = CarNo + "中断";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("请选择中断原因！");
                    return;
                }

                ClsBLL.AddServicePause(RecordID,Worker, comboBox1.Text);
                ClsBLL.AddMsg(RecordID, "车牌号码:" + CarNo + "维修中断，原因是" + comboBox1.Text + "--" + ClsBLL.UserName);

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