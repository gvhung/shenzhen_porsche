using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Workshop
{
    public partial class frmWorkerSit : Form
    {
        public frmWorkerSit()
        {
            InitializeComponent();
        }
        //private SqlCommand Mycmd = new SqlCommand();
        private SqlDataAdapter Myda = new SqlDataAdapter();
        private DataTable DtPlan;
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nUDMonth_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = DateTime.Today.Year.ToString() + "年" + nUDMonth.Value.ToString() + "月";
            
            string sqlstring = "Select * from WorkerPlan where Wyear=" + DateTime.Today.Year + " and Wmonth=" + nUDMonth.Value;
            SqlCommand mycmd = SQLDbHelper.connection.CreateCommand();
            mycmd.CommandText = sqlstring;
            Myda.SelectCommand = mycmd;
            DtPlan = new DataTable("WorkerPlan");
            Myda.Fill(DtPlan);
            DtPlan = SQLDbHelper.Query(sqlstring).Tables[0];
            LoadDate();
        }
        private void LoadDate()
        {
            DateTime DateT = new DateTime(DateTime.Today.Year, int.Parse(nUDMonth.Value.ToString()), 1);
            TimeSpan ts = DateT.AddMonths(1).Subtract(DateT);
            if (ts.Days == 30)
            {
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = false;
            }
            else
            {
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = true;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string workercode = dataGridView1.Rows[i].Cells[0].Tag.ToString();
                DataRow[] Drs = DtPlan.Select("WorkerCode='" + workercode + "'");
                for (int j = 0; j < Drs.Length; j++)
                {
                    int day =int.Parse(Drs[j]["Wday"].ToString());
                    DataGridViewCellStyle dgvct1 = new DataGridViewCellStyle();
                    dgvct1.ForeColor = Color.White;
                    dgvct1.NullValue = "0";
                    dgvct1.Format = "N0";
                    dataGridView1.Rows[i].Cells[day].Style = dgvct1;
                    dataGridView1.Rows[i].Cells[day].Value = Drs[j]["WorkDay"].ToString();
                    if (int.Parse(Drs[j]["WorkDay"].ToString()) < 8)
                    {
                        DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                        dgvct.ForeColor = Color.Red;
                        dataGridView1.Rows[i].Cells[day].Style = dgvct;
                    }
                }
            }
        }
        private void frmWorkerSit_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            for (int i = 1; i < 32; i++)
            {
                DataGridViewTextBoxColumn dgcbc = new DataGridViewTextBoxColumn();
                dgcbc.HeaderText = i.ToString() + "日";
                dgcbc.Tag = i;
                dgcbc.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns.Add(dgcbc);
            }
            string sqlstring = "Select WorkerName,WorkerCode from Worker";
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                dataGridView1.Rows.Add(Dt.Rows.Count);
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = Dt.Rows[i]["WorkerName"].ToString();
                    dataGridView1.Rows[i].Cells[0].Tag = Dt.Rows[i]["WorkerCode"].ToString();
                    dataGridView1.Rows[i].Height = (dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / Dt.Rows.Count;
                }
                nUDMonth.Value = DateTime.Today.Month;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
            if (!ClsBLL.IsPower("新增工人"))
            {
                btnMonth.Enabled = false;
            }
        }
        private void btnToday_Click(object sender, EventArgs e)
        {
            try
            {
                frmWorkDay fwd = new frmWorkDay();
                fwd.ShowDialog();
                nUDMonth_ValueChanged(null,null);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            frmNewWorker fnw = new frmNewWorker();
            fnw.Show();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    int val = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                }
                catch
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                }
                string workercode = dataGridView1.Rows[e.RowIndex].Cells[0].Tag.ToString();
                string day = dataGridView1.Columns[e.ColumnIndex].Tag.ToString();
                string sqlstring = "Update WorkerPlan Set WorkDay=" + int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) + " where WorkerCode='"+ workercode +"' And Wyear=" + DateTime.Today.Year + " and Wmonth=" + nUDMonth.Value + " And Wday="+day;
                sqlstring += ";Update WorkerPlan Set IsWork=0 where WorkDay=0 And WorkerCode='" + workercode + "' And Wyear=" + DateTime.Today.Year + " and Wmonth=" + nUDMonth.Value + " And Wday=" + day;

                SQLDbHelper.ExecuteSql(sqlstring);

                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) < 8)
                {
                    DataGridViewCellStyle dgvct = new DataGridViewCellStyle();
                    dgvct.ForeColor = Color.Red;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = dgvct;
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}