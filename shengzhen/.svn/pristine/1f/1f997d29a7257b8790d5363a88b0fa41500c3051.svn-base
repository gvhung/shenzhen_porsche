using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmChangeState : Form
    {
        public frmChangeState(int recordid)
        {
            InitializeComponent();
            RecordID = recordid;
        }
        int RecordID = -1;
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == textBox1.Text || comboBox2.Text == string.Empty)
            {
                MessageBox.Show("修改状态不能为空或修改状态不能等于原状态！");
                return;
            }
            string sqlstring = "Update Booking Set State='" + comboBox2.Text + "' Where ID=" + RecordID;
           try
           {
               SQLDbHelper.ExecuteSql(sqlstring);
               if (textBox1.Text == "中断")
               {
                   ClsBLL.ServicePauseStart(RecordID);
               }
               ClsBLL.AddSysLog(RecordID, "修改状态,原状态:"+ textBox1.Text  +",现在状态:"+ comboBox2.Text +"，车牌号码:" + SQLDbHelper.ExecuteScalar("Select CarNo from Booking Where ID=" + RecordID).ToString());
               MessageBox.Show("修改成功！");
               this.DialogResult = DialogResult.OK;
               this.Close();
           }
           catch(Exception Err)
           {
               MessageBox.Show(Err.Message);
           }
        }

        private void frmCarTop_Load(object sender, EventArgs e)
        {
            string sqlstring = "Select Distinct State From Booking";
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    comboBox2.Items.Add(Dt.Rows[i]["State"].ToString());
                }
                sqlstring = "Select State From Booking Where ID="+RecordID;
                textBox1.Text = SQLDbHelper.ExecuteScalar(sqlstring).ToString();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}