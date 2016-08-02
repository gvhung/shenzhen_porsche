using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmNewWorker : Form
    {
        public frmNewWorker()
        {
            InitializeComponent();
        }

        private void frmNewWorker_Load(object sender, EventArgs e)
        {
            ShowGrid();
        }
        private void ShowGrid()
        {
            string sqlstring = "Select * from Worker";
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                dataGridView1.DataSource = Dt;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == string.Empty || textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty)
            {
                MessageBox.Show("请填写完整！");
                return;
            }
            if (textBox1.Enabled)
            {
                string sqlstring = "Select count(*) from Worker where WorkerCode='" + textBox1.Text + "'";
                int rs = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                if (rs > 0)
                {
                    MessageBox.Show("工号：" + textBox1.Text + "已经存在！");
                    return;
                }
                sqlstring = "Insert Into Worker(WorkerGroup,WorkerCode,WorkerName,Position,HourRate) values('" + comboBox1.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','"+ comboBox2.Text +"'," + textBox3.Text + ")";
                try
                {
                    SQLDbHelper.ExecuteSql(sqlstring);
                    MessageBox.Show("保存成功！");
                    ShowGrid();
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    AddWorkerPlan();

                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            else   //修改
            {
                string sqlstring = "Update Worker Set WorkerGroup='"+ comboBox1.Text +"',WorkerName='"+ textBox2.Text +"',HourRate="+ textBox3.Text + ",Position='"+ comboBox2.Text +"' Where WorkerCode='"+ textBox1.Text +"'" ;
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    ShowGrid();
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    textBox3.Text = "1";
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string workercode = dataGridView1.SelectedRows[0].Cells["Column3"].Value.ToString();
            try
            {
                if (SQLDbHelper.ExecuteSql("Delete from Worker where Workercode='" + workercode + "'") > 0)
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string workercode = dataGridView1.SelectedRows[0].Cells["Column3"].Value.ToString();
                DataTable Dt = SQLDbHelper.Query("Select * from Worker where Workercode='" + workercode + "'").Tables[0];
                comboBox1.Text = Dt.Rows[0]["WorkerGroup"].ToString();
                comboBox2.Text = Dt.Rows[0]["Position"].ToString();
                textBox1.Text = workercode;
                textBox2.Text = Dt.Rows[0]["WorkerName"].ToString();
                textBox3.Text = Dt.Rows[0]["HourRate"].ToString();
                textBox1.Enabled = false;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void AddWorkerPlan()
        {
            try
            {
                DataTable Dt = SQLDbHelper.Query("Select * from Worker Where WorkerCode not in (Select WorkerCode from WorkerPlan)").Tables[0];
                for (int i = 2003; i < 2015; i++)
                {
                    for (int j = 1; j < 13; j++)
                    {
                        for (int d = 1; d < 32; d++)
                        {
                            string sqlstring = "";
                            for (int m = 0; m < Dt.Rows.Count; m++)
                            {
                                sqlstring += ";Insert into WorkerPlan(Wyear,Wmonth,Wday,workercode)values(" + i + "," + j + "," + d + ",'" + Dt.Rows[m]["WorkerCode"].ToString() + "')";
                            }
                            if (sqlstring.Length > 1)
                            {
                                SQLDbHelper.ExecuteSql(sqlstring.Substring(1));
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewWorker_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != char.Parse("."))
            {
                ClsBLL.Key_Number(e);
            }
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = "1";
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            修改ToolStripMenuItem_Click(null, null);
        }
    }
}