using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmWorkDay : Form
    {
        public frmWorkDay()
        {
            InitializeComponent();
        }
        private int CurWorkerCode = -1;
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string sqlstring = @"Select Worker.*,WorkDay from Worker Inner join WorkerPlan on Worker.WorkerCode = WorkerPlan.Workercode
                                where Wyear=" + dateTimePicker1.Value.Year + " and Wmonth=" + dateTimePicker1.Value.Month + " and wday=" + dateTimePicker1.Value.Day + "";
            try
            {
                dataGridView1.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void È«ÌìToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurWorkerCode > -1)
            {
                string sqlstring = "Update WorkerPlan Set IsWork=1,WorkDay=1 Where WorkerCode=" + CurWorkerCode + " And Wyear=" + dateTimePicker1.Value.Year + " and Wmonth=" + dateTimePicker1.Value.Month + " and Wday=" + dateTimePicker1.Value.Day + "";
                SQLDbHelper.ExecuteSql(sqlstring);
                dataGridView1.SelectedRows[0].Cells["ColWorkDay"].Value = 1;
            }
        }

        private void °ëÌìToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurWorkerCode > -1)
            {
                string sqlstring = "Update WorkerPlan Set IsWork=1,WorkDay=0.5 Where WorkerCode=" + CurWorkerCode + " And Wyear=" + dateTimePicker1.Value.Year + " and Wmonth=" + dateTimePicker1.Value.Month + " and Wday=" + dateTimePicker1.Value.Day + "";
                SQLDbHelper.ExecuteSql(sqlstring);
                dataGridView1.SelectedRows[0].Cells["ColWorkDay"].Value = 0.5;
            }
        }

        private void ÐÝ¼ÙToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurWorkerCode > -1)
            {
                string sqlstring = "Update WorkerPlan Set IsWork=0,WorkDay=0 Where WorkerCode=" + CurWorkerCode + " And Wyear=" + dateTimePicker1.Value.Year + " and Wmonth=" + dateTimePicker1.Value.Month + " and Wday=" + dateTimePicker1.Value.Day + "";
                SQLDbHelper.ExecuteSql(sqlstring);
                dataGridView1.SelectedRows[0].Cells["ColWorkDay"].Value = 0;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CurWorkerCode = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["ColWorkerCode"].Value.ToString());
        }

        private void frmWorkDay_Load(object sender, EventArgs e)
        {
            dateTimePicker1_ValueChanged(null, null);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                dataGridView1.ReadOnly = false;
            }
            else
            {
                dataGridView1.ReadOnly = true;
            }
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }
    }
}