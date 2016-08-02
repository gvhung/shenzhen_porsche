using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraCharts;

namespace Workshop
{
    public partial class frmReport : Form
    {
        public frmReport()
        {
            InitializeComponent();
        }
        string sqlstring = string.Empty;
        int selectrow = 0;
        int recordid = -1;
        string carno = string.Empty;
        DataTable Dt;
        DataTable CmbDt = new DataTable();
        DevExpress.XtraGrid.GridControl XGridC ;
        private void frmReport_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;

            sqlstring = @"select distinct cast(datepart(yy,EndServiceTime) as nvarchar(50)) + '年'+ cast(datepart(MM,EndServiceTime) as nvarchar(50)) + '月' as 年月 from Booking
                        where EndServiceTime is not null order by 年月 desc";
            CmbDt = SQLDbHelper.Query(sqlstring).Tables[0];
            foreach (Control ct in this.panel2.Controls)
            {
                if (ct.Name.StartsWith("btnRt"))
                {
                    if (!ClsBLL.IsPower(ct.Text))
                    {
                        ct.Enabled = false;
                    }
                }
            }
            if (!ClsBLL.IsPower("工单明细"))
            {
                btnQuery.Enabled = false;
                button10.Enabled = false;
                button20.Enabled = false;
                button21.Enabled = false;
                button22.Enabled = false;
                button23.Enabled = false;
            }
            if (!ClsBLL.IsPower("异常单查询"))
            {
                button20.Enabled = false;
            }
            if (!ClsBLL.IsPower("修改状态"))
            {
                button21.Enabled = false;
            }
            if (!ClsBLL.IsPower("操作日志"))
            {
                button22.Enabled = false;
            }
            if (!ClsBLL.IsPower("删除工单"))
            {
                button23.Enabled = false;
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            XGridC = null;
            string tempcmb = DateTime.Today.Year.ToString() + "年" + DateTime.Today.Month + "月";
            if (tabControl1.SelectedIndex == 0)
            {
                btnQuery_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                comboBox4.DataSource = CmbDt;
                comboBox4.DisplayMember = "年月";
                comboBox4.Text = tempcmb;
                button12_Click(null, null);
                XGridC = gridControl5;
            }
            if (tabControl1.SelectedIndex == 2)
            {
                comboBox5.DataSource = CmbDt;
                comboBox5.DisplayMember = "年月";
                comboBox5.Text = tempcmb;
                button9_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 3)
            {
                comboBox1.DataSource = CmbDt;
                comboBox1.DisplayMember = "年月";
                comboBox1.Text = tempcmb;
                btnShowGrid_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 4)
            {
                comboBox2.DataSource = CmbDt;
                comboBox2.DisplayMember = "年月";
                comboBox2.Text = tempcmb;
                button4_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 5)
            {
                comboBox3.DataSource = CmbDt;
                comboBox3.DisplayMember = "年月";
                comboBox3.Text = tempcmb;
                button7_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 6)
            {
                comboBox6.DataSource = CmbDt;
                comboBox6.DisplayMember = "年月";
                comboBox6.Text = tempcmb;
                button1_Click(null, null);
            }
            if (tabControl1.SelectedIndex == 7)
            {
                comboBox7.DataSource = CmbDt;
                comboBox7.DisplayMember = "年月";
                comboBox7.Text = tempcmb;
                button8_Click(null, null);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
            {
                selectrow = e.FocusedRowHandle;
            }
        }

        #region 工单明细
        //工单明细
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            string sqlwhere = string.Empty;
            gridControl20.Visible = false;
            gcRo.Visible = true;
            sqlwhere = "预约时间>'" + dateTimePicker1.Value.ToString("yy-MM-dd 00:00") + "' and 预约时间<'" + dateTimePicker2.Value.AddDays(1).ToString("yy-MM-dd 00:00") + "' And CreateDate>'" + dateTimePicker1.Value.ToShortDateString() + "'";
            string sqlstring = "Select * from View_SelectBooking Where " + sqlwhere;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                Dt.Columns.Remove("CreateDate");
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    Dt.Rows[i]["序号"] = Convert.ToString(i + 1);
                }
                gcRo.DataSource = Dt;
                if (gridView1.Columns["单号"] != null)
                {
                    gridView1.Columns["单号"].Visible = false;
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string sqlwhere = string.Empty;
            sqlwhere = "预约时间>'" + dateTimePicker1.Value.ToString("yy-MM-dd 00:00") + "' and 预约时间<'" + dateTimePicker2.Value.AddDays(1).ToString("yy-MM-dd 00:00") + "' And CreateDate>'" + dateTimePicker1.Value.ToShortDateString() + "'";
            string where = gridView1.FilterPanelText;
            if (where.IndexOf("与") > -1)
            {
                where = where.Replace("与", "And");
            }
            if (where.IndexOf("Like") > -1)
            {
                where = where.Replace("匹配(Like)", "like");
            }
            if (where != string.Empty)
            {
                sqlwhere += "And " + where;
            }
            if (gcRo.Visible)
            {
                string sqlstring = "Select * from View_SelectBooking Where " + sqlwhere;
                Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                Dt.Columns.Remove("CreateDate");
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    Dt.Rows[i]["序号"] = Convert.ToString(i + 1);
                }
                for (int i = 0; i<gridView1.Columns.Count - 1; i++)
                {
                    if (!gridView1.Columns[i].Visible)
                    {
                        Dt.Columns.Remove(gridView1.Columns[i].Caption);
                    }
                }
                Dt.Columns.Remove("单号");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 0, "预约工单明细表");
                    xr1.ShowPreview();
                }
            }
            else
            {
                Dt = (DataTable)gridControl20.DataSource;
                Dt.Columns.Remove("单号");
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 0, "异常工单明细表");
                    xr1.ShowPreview();
                }
            }
        }
        //异常查询
        private void button20_Click(object sender, EventArgs e)
        {
            string sqlstring = "Exec BookingAbort '" + dateTimePicker1.Value.ToShortDateString() + "','" + dateTimePicker2.Value.ToShortDateString() + "'";
            try
            {
                gridControl20.Visible = true;
                gcRo.Visible = false;

                gridControl20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                gridControl20.Location = new System.Drawing.Point(0, 51);
                gridControl20.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
                gridControl20.Size = gcRo.Size;

                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    Dt.Rows[i]["序号"] = Convert.ToString(i + 1);
                }
                gridControl20.DataSource = Dt;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //修改状态
        private void button21_Click(object sender, EventArgs e)
        {
            if (recordid > -1)
            {
                frmChangeState fcs = new frmChangeState(recordid);
                fcs.Show();
            }
        }
        //操作日志
        private void button22_Click(object sender, EventArgs e)
        {
            frmSysLog syslog = new frmSysLog(carno,recordid);
            syslog.Show();
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
            {
                selectrow = e.FocusedRowHandle;
                recordid = int.Parse(gridView1.GetRowCellValue(e.FocusedRowHandle, "单号").ToString());
                if (gridControl20.Visible)
                {
                    carno = gridView22.GetRowCellValue(e.FocusedRowHandle, "车牌号码").ToString();
                }
                else
                {
                    carno = gridView1.GetRowCellValue(e.FocusedRowHandle, "车牌号码").ToString();
                }
            }
        }
        private void gridView22_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
            {
                selectrow = e.FocusedRowHandle;
                recordid = int.Parse(gridView22.GetRowCellValue(e.FocusedRowHandle, "单号").ToString());
                carno = gridView22.GetRowCellValue(e.FocusedRowHandle, "车牌号码").ToString();
            }
        }
        private void button23_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (gcRo.Visible)
                {
                    if (gridView1.GetRowCellValue(selectrow, "完工时间") != null && gridView1.GetRowCellValue(selectrow, "完工时间").ToString()!=string.Empty)
                    {
                        MessageBox.Show("该单已经完工不能删除！");
                        return;
                    }
                }
                string sqlstring = "Delete from Booking Where ID="+ recordid + ";Delete from BookingAdd Where BookID="+ recordid + ";Delete from DelayService Where BookID="+recordid;
                sqlstring += ";Delete from ServiceAddHours Where BookID="+recordid+";Delete from ServicePause Where BookID="+recordid;
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    if (gcRo.Visible)
                    {
                        gridView1.DeleteRow(selectrow);
                    }
                    else
                    {
                        gridView22.DeleteRow(selectrow);
                    }
                }
            }
        }
        #endregion
        #region 预约情况统计
        //客人预约情况
        private void gridControl9_Enter(object sender, EventArgs e)
        {
            XGridC = (DevExpress.XtraGrid.GridControl)sender;
        }
        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == string.Empty || comboBox4.Text.StartsWith("System")) return;
            ShowGrid();
        }
        private void ShowGrid()
        {
            int years = int.Parse(comboBox4.Text.Substring(0, 4));
            int months = int.Parse(comboBox4.Text.Substring(comboBox4.Text.IndexOf("年") + 1, comboBox4.Text.IndexOf("月") - comboBox4.Text.IndexOf("年") - 1));
            try
            {
                string sqlstring = string.Empty;  
                sqlstring = "Exec Sp_BookRemindResult " + years + "," + months + "";
                gridControl5.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];

                sqlstring = "Exec Sp_BookRemindFaild " + years + "," + months + "";
                gridControl6.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];

                sqlstring = "Exec Sp_BookComeType " + years + "," + months + "";
                gridControl7.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];

                sqlstring = "Exec Sp_BookState " + years + "," + months + "";
                gridControl8.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            ShowGrid();
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectrow = 0;
            pieChart.Visible = false;
            if (tabControl3.SelectedIndex == 0)
            {
                gridControl9.Visible = true;
                gridControl13.Visible = false;
                gridControl14.Visible = false;
                XGridC = gridControl5;
            }
            if (tabControl3.SelectedIndex == 1)
            {
                gridControl9.Visible = false;
                gridControl13.Visible = true;
                gridControl14.Visible = false;
                XGridC = gridControl7;
            }
            if (tabControl3.SelectedIndex == 2)
            {
                gridControl9.Visible = false;
                gridControl13.Visible = false;
                gridControl14.Visible = true;
                XGridC = gridControl8;
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl3.SelectedIndex == 0)
                {
                    Dt = (DataTable)gridControl5.DataSource;
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        XrROList xr1 = new XrROList(Dt, 1, comboBox4.Text + "电话提醒");
                        xr1.ShowPreview();
                    }

                    Dt = (DataTable)gridControl6.DataSource;
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        XrROList xr1 = new XrROList(Dt, 1, comboBox4.Text + "电话提醒失败原因");
                        xr1.ShowPreview();
                    }
                }
                if (tabControl3.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl7.DataSource;
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        XrROList xr1 = new XrROList(Dt, 1, comboBox4.Text + "来店方式");
                        xr1.ShowPreview();
                    }
                }
                if (tabControl3.SelectedIndex == 2)
                {
                    Dt = (DataTable)gridControl8.DataSource;
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        XrROList xr1 = new XrROList(Dt, 1, comboBox4.Text + "预约结果");
                        xr1.ShowPreview();
                    }
                }

            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void btnDetail_Click(object sender, EventArgs e)
        {
            int years = int.Parse(comboBox4.Text.Substring(0, 4));
            int months = int.Parse(comboBox4.Text.Substring(comboBox4.Text.IndexOf("年") + 1, comboBox4.Text.IndexOf("月") - comboBox4.Text.IndexOf("年") - 1));
            string sqlstring = string.Empty;
            pieChart.Visible = false;
            try
            {
                //电话提醒
                if (tabControl3.SelectedIndex == 0)
                {
                    gridControl9.Visible = true;
                    if (XGridC != null)
                    {
                        string remindres = string.Empty;
                        if (XGridC.Name == "gridControl5")
                        {
                            if (gridView7.GetRowCellValue(selectrow, "提醒结果") == null) return;
                            remindres = gridView7.GetRowCellValue(selectrow, "提醒结果").ToString();
                            sqlstring = "Exec Sp_BookRemindDetail2 '" + remindres + "'," + years + "," + months + "";
                        }
                        else
                        {
                            if (gridView8.GetRowCellValue(selectrow, "失败原因") == null) return;
                            string reason = gridView8.GetRowCellValue(selectrow, "失败原因").ToString();
                            sqlstring = "Exec Sp_BookRemindDetail '" + reason + "'," + years + "," + months + "";
                        }
                        gridControl9.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];

                        if (XGridC.Name == "gridControl5")
                        {
                            if (remindres == "成功")
                            {
                                gridView11.Columns[0].Visible = false;
                            }
                            else
                            {
                                gridView11.Columns[0].Visible = true;
                            }
                        }
                    }
                }
                //来店方式
                if (tabControl3.SelectedIndex == 1)
                {
                    gridControl13.Visible = true;
                    if (gridView9.GetRowCellValue(selectrow, "来店方式") == null) return;
                    string cometype = gridView9.GetRowCellValue(selectrow, "来店方式").ToString();
                    int ct = 0;
                    if (cometype == "预约来店")
                    {
                        ct = 1;
                    }
                    sqlstring = "Exec Sp_BookComeTypeDetail " + ct + "," + years + "," + months + "";
                    gridControl13.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
                //预约状态
                if (tabControl3.SelectedIndex == 2)
                {
                    gridControl14.Visible = true;
                    if (gridView10.GetRowCellValue(selectrow, "预约结果") == null) return;
                    string bookstate = gridView10.GetRowCellValue(selectrow, "预约结果").ToString();
                    sqlstring = "Exec Sp_BookStateDetail '" + bookstate + "'," + years + "," + months + "";
                    gridControl14.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void btnGrphi_Click(object sender, EventArgs e)
        {
            if (XGridC == null)   //明细表
            {
                return;
            }
            try
            {
                if (tabControl3.SelectedIndex == 0)
                {
                    if (XGridC != null)
                    {
                        if (XGridC.Name == "gridControl5" || XGridC.Name == "gridControl6" || XGridC.Name == "gridControl9")
                        {
                            Dt = (DataTable)XGridC.DataSource;
                        }
                    }
                }
                if (tabControl3.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl7.DataSource;
                }
                if (tabControl3.SelectedIndex == 2)
                {
                    Dt = (DataTable)gridControl8.DataSource;
                }
                if (Dt.Rows.Count == 0) return;
                pieChart.Titles[0].Text = comboBox4.Text + tabControl3.SelectedTab.Text + "统计";
                pieChart.Series[0].Points.Clear();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string reason = Dt.Rows[i][0].ToString();
                    pieChart.Series[0].Points.Add(new SeriesPoint(reason, new double[] { double.Parse(Dt.Rows[i][1].ToString()) }));
                }
                pieChart.Visible = true;
                gridControl9.Visible = false;
                gridControl13.Visible = false;
                gridControl14.Visible = false;
                pieChart.Size = new System.Drawing.Size(500, 250);
                pieChart.Left = gridControl9.Left;
                pieChart.Top = gridControl9.Top;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (pieChart.Visible)
            {
                //pieChart.ShowPrintPreview();
                DataTable RtDt = new DataTable();
                if (tabControl3.SelectedIndex == 0)
                {
                    if (XGridC != null)
                    {
                        if (XGridC.Name == "gridControl5" || XGridC.Name == "gridControl6" || XGridC.Name == "gridControl9")
                        {
                            RtDt = (DataTable)XGridC.DataSource;
                        }
                    }
                }
                if (tabControl3.SelectedIndex == 1)
                {
                    RtDt = (DataTable)gridControl7.DataSource;
                }
                if (tabControl3.SelectedIndex == 2)
                {
                    RtDt = (DataTable)gridControl8.DataSource;
                }
                XrPause xrp = new XrPause(RtDt, pieChart.Titles[0].Text);
                xrp.ShowPreview();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridControl9.Visible)
                {
                    Dt = (DataTable)gridControl9.DataSource;
                }
                if (gridControl13.Visible)
                {
                    Dt = (DataTable)gridControl13.DataSource;
                }
                if (gridControl14.Visible)
                {
                    Dt = (DataTable)gridControl14.DataSource;
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 1, comboBox4.Text + tabControl3.SelectedTab.Text + "明细表");
                    xr1.ShowPreview();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        #endregion
        #region 预约工时统计
        private void button9_Click(object sender, EventArgs e)
        {
            comboBox5_SelectedValueChanged(null, null);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                Dt = (DataTable)gridControl10.DataSource;
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 1, comboBox5.Text + "预约工时统计");
                    xr1.ShowPreview();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text == string.Empty || comboBox5.Text.StartsWith("System")) return;
            int years = int.Parse(comboBox5.Text.Substring(0, 4));
            int months = int.Parse(comboBox5.Text.Substring(comboBox5.Text.IndexOf("年") + 1, comboBox5.Text.IndexOf("月") - comboBox5.Text.IndexOf("年") - 1));
            string sqlstring = "exec BookHourStat " + years + "," + months;
            try
            {
                gridControl10.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        #endregion
        #region SA工作统计
        //预约情况统计表
        private void btnShowGrid_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel3.Width = tabControl1.Width - 10;
            barChart.Visible = false;
            panel3.Left = 5;
            if (comboBox1.Text == string.Empty) return;
            int years = int.Parse(comboBox1.Text.Substring(0, 4));
            int months = int.Parse(comboBox1.Text.Substring(comboBox1.Text.IndexOf("年") + 1, comboBox1.Text.IndexOf("月") - comboBox1.Text.IndexOf("年") - 1));
            string sqlstring = "exec sp_sacars " + years + "," + months;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                gcSA.DataSource = Dt;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //预约情况统计表
        private void btnShowGraphi_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            barChart.Visible = true;
            barChart.Left = panel3.Left;
            barChart.Top = panel3.Top;
            barChart.Width = panel3.Width;
            barChart.Height = panel3.Height;
            barChart.Left = tabControl2.Left;
            if (comboBox1.Text == string.Empty) return;
            int years = int.Parse(comboBox1.Text.Substring(0, 4));
            int months = int.Parse(comboBox1.Text.Substring(comboBox1.Text.IndexOf("年") + 1, comboBox1.Text.IndexOf("月") - comboBox1.Text.IndexOf("年") - 1));
            try
            {
                sqlstring = "exec Sp_BookGraphi " + years + "," + months;
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                barChart.Titles[0].Text = comboBox1.Text + "预约情况统计表";
                barChart.Series.Clear();
                Series series1 = new Series("预约车辆", ViewType.Bar);
                Series series2 = new Series("到场车辆", ViewType.Bar);

                for (int i = 1; i < 32; i++)
                {
                    string datewhere = years.ToString() + "-" + months.ToString().PadLeft(2, char.Parse("0")) + "-" + i.ToString().PadLeft(2, char.Parse("0"));

                    DataRow[] Dr1 = Dt.Select("bookday=" + i);
                    double db1 = 0;
                    if (Dr1.Length > 0)
                    {
                        db1 = double.Parse(Dr1.Length.ToString());
                    }

                    DataRow[] Dr2 = Dt.Select("comeday=" + i);
                    double db2 = 0;
                    if (Dr2.Length > 0)
                    {
                        db2 = double.Parse(Dr2.Length.ToString());
                    }
                    series1.Points.Add(new SeriesPoint(i.ToString() + "日", new double[] { db1 }));
                    series2.Points.Add(new SeriesPoint(i.ToString() + "日", new double[] { db2 }));
                }

                barChart.Series.Add(series1);
                barChart.Series.Add(series2);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //sa工作统计表
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 1 && selectrow > -1)
            {
                if (comboBox1.Text == string.Empty) return;
                int years = int.Parse(comboBox1.Text.Substring(0, 4));
                int months = int.Parse(comboBox1.Text.Substring(comboBox1.Text.IndexOf("年") + 1, comboBox1.Text.IndexOf("月") - comboBox1.Text.IndexOf("年") - 1));

                string saname = gridView2.GetRowCellValue(selectrow, "SA姓名").ToString();
                string sqlstring = "exec Sp_SAWorkDetail '" + saname + "'," + years + "," + months;
                try
                {
                    gridControl4.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (barChart.Visible)
                {
                    XrCars xrc = new XrCars(comboBox1.Text);
                    xrc.ShowPreview();
                }
                else
                {
                    if (tabControl2.SelectedIndex == 0)
                    {
                        Dt = (DataTable)gcSA.DataSource;
                    }
                    if (tabControl2.SelectedIndex == 1)
                    {
                        Dt = (DataTable)gridControl4.DataSource;
                    }
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        XrROList xr1 = new XrROList(Dt, 1, comboBox1.Text + tabControl2.SelectedTab.Text);
                        xr1.ShowPreview();
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        #endregion
        #region 维修工业绩统计
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (tabControl4.SelectedIndex == 0)
            {
                button4_Click(null, null);
            }

        }
        //业绩汇总
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.StartsWith("System")) return;
            int years = int.Parse(comboBox2.Text.Substring(0, 4));
            int months = int.Parse(comboBox2.Text.Substring(comboBox2.Text.IndexOf("年") + 1, comboBox2.Text.IndexOf("月") - comboBox2.Text.IndexOf("年") - 1));
            try
            {
                string sqlstring = "Exec WorkerStat " + years + "," + months + "";
                gridControl2.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                int dir = 1;
                if (tabControl4.SelectedIndex == 0)
                {
                    Dt = (DataTable)gridControl2.DataSource;
                }
                if (tabControl4.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl15.DataSource;
                    dir = 0;
                }
                if (tabControl4.SelectedIndex == 2)
                {
                    Dt = (DataTable)gridControl3.DataSource;
                }
                if (tabControl4.SelectedIndex == 3)
                {
                    Dt = (DataTable)gridControl19.DataSource;
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, dir, comboBox2.Text + tabControl4.SelectedTab.Text);
                    xr1.ShowPreview();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == string.Empty) return;
            button18.Enabled = false;
            try
            {
                int years = int.Parse(comboBox2.Text.Substring(0, 4));
                int months = int.Parse(comboBox2.Text.Substring(comboBox2.Text.IndexOf("年") + 1, comboBox2.Text.IndexOf("月") - comboBox2.Text.IndexOf("年") - 1));
                string servicename = gridView3.GetRowCellValue(selectrow, "维修工人").ToString();
                if (tabControl4.SelectedIndex == 1 && selectrow > -1)
                {
                    sqlstring = "Exec WorkerList "+ years +","+ months +",'"+ servicename +"'";
                    DataTable WorkerListDt = SQLDbHelper.Query(sqlstring).Tables[0];
                    WorkerListDt.DefaultView.Sort = " 开始维修时间 Desc"; 
                    gridControl15.DataSource = WorkerListDt.DefaultView.Table;
                }
                if (tabControl4.SelectedIndex == 2 && selectrow > -1)
                {
                    sqlstring = "Exec WorkerReService " + years + "," + months + ",'" + servicename + "'";
                    gridControl3.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
                if (tabControl4.SelectedIndex == 3 && selectrow > -1)
                {
                    sqlstring = "Exec WorkerAddItem " + years + "," + months + ",'" + servicename + "'";
                    gridControl19.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //查询
        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                string servicename = gridView3.GetRowCellValue(selectrow, "维修工人").ToString();
                string sqlstring = "Exec WorkerList2 '" + dateTimePicker3.Value.ToShortDateString() + "','" + dateTimePicker4.Value.AddDays(1).ToShortDateString() + "','" + servicename + "'";
                DataTable WorkerListDt = SQLDbHelper.Query(sqlstring).Tables[0];
                WorkerListDt.DefaultView.Sort = " 开始维修时间 Desc";
                gridControl15.DataSource = WorkerListDt.DefaultView.Table;
                button18.Enabled = true;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //预览
        private void button18_Click(object sender, EventArgs e)
        {
            Dt = (DataTable)gridControl15.DataSource;
            if (Dt != null && Dt.Rows.Count > 0)
            {
                XrROList xr1 = new XrROList(Dt, 0, dateTimePicker3.Value.ToString("MM月dd日")  + "至" + dateTimePicker4.Value.ToString("MM月dd日") + tabControl4.SelectedTab.Text);
                xr1.ShowPreview();
            }
        }
        #endregion
        #region 维修中断统计
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == string.Empty || comboBox3.Text.StartsWith("System")) return;
            button7_Click(null, null);
        }
        //异常情况统计
        private void button7_Click(object sender, EventArgs e)
        {
            int years = int.Parse(comboBox3.Text.Substring(0, 4));
            int months = int.Parse(comboBox3.Text.Substring(comboBox3.Text.IndexOf("年") + 1, comboBox3.Text.IndexOf("月") - comboBox3.Text.IndexOf("年") - 1));
            try
            {
                sqlstring = "Exec Sp_BookPauseStat " + years + "," + months + "";
                Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                gridControl11.DataSource = Dt;
                chartControl2.Titles[0].Text = comboBox3.Text + "异常情况统计";
                chartControl2.Series[0].Points.Clear();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string reason = Dt.Rows[i][0].ToString();
                    chartControl2.Series[0].Points.Add(new SeriesPoint(reason, new double[] { double.Parse(Dt.Rows[i][1].ToString()) }));
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl6.SelectedIndex == 0)
                {
                    Dt = (DataTable)gridControl11.DataSource;
                }
                if (tabControl6.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl12.DataSource;
                }
                if (tabControl6.SelectedIndex == 2)
                {
                    XrPause xrp = new XrPause((DataTable)gridControl11.DataSource, comboBox3.Text + "异常情况统计");
                    xrp.ShowPreview();
                    return;
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 1, comboBox3.Text + tabControl6.SelectedTab.Text);
                    xr1.ShowPreview();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void tabControl6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl6.SelectedIndex == 1 && selectrow > -1)
                {
                    if (comboBox3.Text == string.Empty) return;
                    int years = int.Parse(comboBox3.Text.Substring(0, 4));
                    int months = int.Parse(comboBox3.Text.Substring(comboBox3.Text.IndexOf("年") + 1, comboBox3.Text.IndexOf("月") - comboBox3.Text.IndexOf("年") - 1));
                    if (gridView13.RowCount > 0)
                    {
                        string reason = gridView13.GetRowCellValue(selectrow, "中断原因").ToString();
                        sqlstring = "Exec Sp_BookPauseReason " + years + "," + months + ",'"+ reason +"'"; 
                        try
                        {
                            gridControl12.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                        }
                        catch (Exception Err)
                        {
                            MessageBox.Show(Err.Message);
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        #endregion
        #region 车辆完工统计
        private void button1_Click(object sender, EventArgs e)
        {
            int years = int.Parse(comboBox6.Text.Substring(0, 4));
            int months = int.Parse(comboBox6.Text.Substring(comboBox6.Text.IndexOf("年") + 1, comboBox6.Text.IndexOf("月") - comboBox6.Text.IndexOf("年") - 1));
            try
            {
                string sqlstring = "Exec Sp_ServiceType " + years + "," + months + "";
                gridControl1.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];

                sqlstring = "Exec Sp_ServiceEndType " + years + "," + months + "";
                gridControl16.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                Dt = (DataTable)gridControl1.DataSource;
                chartControl1.Titles[0].Text = comboBox6.Text + tabControl5.SelectedTab.Text;
                chartControl1.Series[0].Points.Clear();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string reason = Dt.Rows[i][0].ToString();
                    chartControl1.Series[0].Points.Add(new SeriesPoint(reason, new double[] { double.Parse(Dt.Rows[i][1].ToString()) }));
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }

        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (tabControl5.SelectedIndex == 0)
            {
                Dt = (DataTable)gridControl1.DataSource;
            }
            if (tabControl5.SelectedIndex == 1)
            {
                Dt = (DataTable)gridControl16.DataSource;
            }
            if (Dt != null && Dt.Rows.Count > 0)
            {
                XrROList xr1 = new XrROList(Dt, 1, comboBox6.Text + tabControl5.SelectedTab.Text);
                xr1.ShowPreview();
            }
        }
        private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chartControl1.Titles[0].Text = comboBox6.Text + tabControl5.SelectedTab.Text;
                chartControl1.Series[0].Points.Clear();

                if (tabControl5.SelectedIndex == 0)
                {
                    Dt = (DataTable)gridControl1.DataSource;
                }
                if (tabControl5.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl16.DataSource;
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        string reason = Dt.Rows[i][0].ToString();
                        chartControl1.Series[0].Points.Add(new SeriesPoint(reason, new double[] { double.Parse(Dt.Rows[i][1].ToString()) }));
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.Text == string.Empty || comboBox6.Text.StartsWith("System")) return;
            button1_Click(null, null);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            XrPause xrp = new XrPause((DataTable)gridControl1.DataSource, comboBox6.Text + tabControl5.SelectedTab.Text);
            xrp.ShowPreview();
        }
        #endregion
        #region 预约响应时间统计
        //预约等待响应统计
        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox7.Text == string.Empty) return;
            int years = int.Parse(comboBox7.Text.Substring(0, 4));
            int months = int.Parse(comboBox7.Text.Substring(comboBox7.Text.IndexOf("年") + 1, comboBox7.Text.IndexOf("月") - comboBox7.Text.IndexOf("年") - 1));
            string sqlstring = "exec Sp_SABookDays " + years + "," + months;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                gridControl17.DataSource = Dt;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //预约等待响应明细
        private void tabControl7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl7.SelectedIndex == 1 && selectrow > -1)
            {
                if (comboBox7.Text == string.Empty) return;
                int years = int.Parse(comboBox7.Text.Substring(0, 4));
                int months = int.Parse(comboBox7.Text.Substring(comboBox7.Text.IndexOf("年") + 1, comboBox7.Text.IndexOf("月") - comboBox7.Text.IndexOf("年") - 1));

                string saname = gridView19.GetRowCellValue(selectrow, "SA姓名").ToString();
                string sqlstring = "exec Sp_SABookDaysDetail " + years + "," + months + ",'" + saname + "'";
                try
                {
                    gridControl18.DataSource = SQLDbHelper.Query(sqlstring).Tables[0];
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl7.SelectedIndex == 0)
                {
                    Dt = (DataTable)gridControl7.DataSource;
                }
                if (tabControl7.SelectedIndex == 1)
                {
                    Dt = (DataTable)gridControl8.DataSource;
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    XrROList xr1 = new XrROList(Dt, 1, comboBox7.Text + tabControl7.SelectedTab.Text);
                    xr1.ShowPreview();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Text == string.Empty || comboBox7.Text.StartsWith("System")) return;
            button8_Click(null, null);
        }
        #endregion
        private void btnRt0_Click(object sender, EventArgs e)
        {
            Button Bt = (Button)sender;
            int selectindex = int.Parse(Bt.Name.Substring(5,1));
            tabControl1.SelectedIndex = selectindex;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
