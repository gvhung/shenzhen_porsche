using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorkerPlan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            object obj = SQLDbHelper.ExecuteScalar("Select count(*) from WorkerPlan");
            if (obj != null)
            {
                MessageBox.Show(obj.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Dt = SQLDbHelper.Query("Select * from Worker ").Tables[0];
                for (int i = 2013; i < 2015; i++)
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
    }
}
