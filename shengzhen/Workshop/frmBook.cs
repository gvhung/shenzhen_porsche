using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmBook : Form
    {
        public frmBook()
        {
            InitializeComponent();
        }
        private Point mouse_offset;

        private int Colindex = -1;
        private int Rowindex = -1;
        private int PcStartTop = 0;
        private int PcStartLeft = 0;
        private bool IsMove = false;

        private DateTime starttime = DateTime.Today;
        private DateTime endtime = DateTime.Today;
        private DataTable WorkHours = new DataTable();
        private decimal DayUseHoursJD = 0; //当天的剩余工时
        private decimal DayUseHoursCS = 0; //当天的剩余工时
        private int sizewidth = 100;
        private int[,] Cards;//
        private string UpdateDetail = string.Empty;
        private int index = 0;
        private Control CureentCt;
        public class DoubleClickButton : Button
        {
            public DoubleClickButton(): base()
            {
                SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
            }
        }

        private void frmBook_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            try
            {
                string sqlstring = "Select * from WorkHours where DateIndex between '" + DateTime.Today.AddDays(1 - DateTime.Today.Day) + "' and '" + DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day) + "'";

                WorkHours = SQLDbHelper.Query(sqlstring).Tables[0];

                starttime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));
                endtime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet2"));

                TimeSpan ts = endtime.Subtract(starttime);
                int cols = ts.Hours * 2;
                if (ts.Minutes > 0) cols++;
                dataGridView1.Rows.Add(cols + 1 + 1);
                for (int i = 1; i < dataGridView1.Rows.Count; i++)
                {
                    DateTime NewDt = starttime.AddMinutes((i - 1) * 30);
                    dataGridView1.Rows[i].Cells[0].Value = NewDt.Hour.ToString() + ":" + NewDt.Minute.ToString().PadRight(2, char.Parse("0"));
                }
                nUDMonth.Value = DateTime.Today.Month;
                btnMonth_Click(null, null);

                int Interval = int.Parse(ClsBLL.GetSet("txtSet3"));
                timer1.Interval = Interval * 1000 * 60;
                timer1.Enabled = true;
                CheckPower();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void CheckPower()
        {
            if (!ClsBLL.IsPower(button1.Text))
            {
                button1.Enabled = false;
            }
            if (!ClsBLL.IsPower(button2.Text))
            {
                button2.Enabled = false;
            }
            if (!ClsBLL.IsPower("删除预约"))
            {
                删除预约ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("取消预约"))
            {
                取消预约ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("预约转正式"))
            {
                转为正式预约ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("更改预约时间"))
            {
                更改预约时间ToolStripMenuItem.Visible = false;
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            nUDMonth.Visible = false;
            panel2.Visible = false;
            dataGridView1.BringToFront();
            dataGridView1.Top = panel1.Height + panel4.Height;
            dataGridView1.Height = this.Height - panel1.Height - panel4.Height;
            ShowGrid1(DateTime.Today);
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            nUDMonth.Visible = true;
            panel2.Visible = false;
            dataGridView2.Top = panel1.Height + panel4.Height;
            dataGridView2.Height = this.Height - panel1.Height - panel4.Height;
            ShowGrid2(DateTime.Today.Month);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ClsBLL.CheckFormIsOpen("frmWork"))
            {
                frmWork fw = new frmWork();
                fw.Show();
            }
            else
            {
                Form frm = Application.OpenForms["frmWork"];
                frm.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ClsBLL.CheckFormIsOpen("frmCarStateBoad"))
            {
                frmCarStateBoad fcsb = new frmCarStateBoad();
                fcsb.Show();
            }
            else
            {
                Form frm = Application.OpenForms["frmCarStateBoad"];
                frm.Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            nUDMonth.Visible = false;
            panel2.Visible = true;
            panel2.Top = panel1.Height + panel4.Height;
            panel2.Height = this.Height - panel1.Height - panel4.Height;
            try
            {
                string sqlstring = "Select Distinct Creator from Booking";
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                comboBox1.Items.Add("--全部--");
                foreach (DataRow dr in Dt.Rows)
                {
                    comboBox1.Items.Add(dr[0].ToString());
                }

                sqlstring = "Select Distinct State from Booking";
                DataTable Dt1 = SQLDbHelper.Query(sqlstring).Tables[0];
                comboBox2.Items.Add("--全部--");
                foreach (DataRow dr in Dt1.Rows)
                {
                    comboBox2.Items.Add(dr[0].ToString());
                }
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
        private void ShowGrid1(DateTime Dt)
        {
            label1.Text = Dt.ToString("yyyy年MM月dd日");
            if (label1.Text != DateTime.Today.ToString("yyyy年MM月dd日"))
            {
                转为正式预约ToolStripMenuItem.Enabled = false;
            }
            index = 0;
            ClearContrl();
            Cards = new int[dataGridView1.Rows.Count, 3];

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Height = (dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.Rows.Count;
            }
            try
            {
                string sqlstring = "Select * from Booking where Booktime between '" + Dt.ToShortDateString() + "' and '" + Dt.AddDays(1).ToShortDateString() + "'";
                DataTable DataBook = SQLDbHelper.Query(sqlstring).Tables[0];
                //显示所有预约列表
                foreach (DataRow dr in DataBook.Rows)
                {
                    DateTime TempT = DateTime.Parse(dr["BookTime"].ToString());
                    int hour = TempT.Hour;
                    int min = TempT.Minute;
                    Rowindex = (hour - starttime.Hour) * 2 + 1;
                    if (min == 30)
                    {
                        Rowindex++;
                    }
                    for (int i = 1; i < dataGridView1.Columns.Count; i++)
                    {
                        if (dr["ServiceType"].ToString() == dataGridView1.Columns[i].HeaderText)
                        {
                            Colindex = i;
                        }
                    }
                    string recordid = dr["ID"].ToString();
                    string bookindex = dr["BookIndex"].ToString();
                    //if (dr["State"].ToString() != "失约" && dr["State"].ToString() != "取消")
                    //{
                    //    if (Colindex == 1)
                    //    {
                    //        jdhour += decimal.Parse(dr["ServiceHour"].ToString());
                    //    }
                    //    if (Colindex == 2)
                    //    {
                    //        cshour += decimal.Parse(dr["ServiceHour"].ToString());
                    //    }
                    //}
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("车    牌:" + dr["CarNo"].ToString());
                    sb.AppendLine("车    型:" + dr["CarType"].ToString());
                    sb.AppendLine("维修项目:" + dr["ServiceItem"].ToString());
                    sb.AppendLine("状    态:" + dr["State"].ToString());
                    if (hour >= starttime.Hour)
                    {
                        AddCarCard(recordid, bookindex, dr["State"].ToString(), sb.ToString(), dr["CarNo"].ToString(), DateTime.Parse(dr["BookTime"].ToString()));
                    }
                }
                SumWorkHours(0, 0);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //添加车辆预约卡片
        private void AddCarCard(string ID, string bookindex, string state, string detail, string carno, DateTime booktime)
        {
            if (DateTime.Parse(label1.Text).ToString("yyyy-MM-dd") != booktime.ToString("yyyy-MM-dd"))
            {
                return;
            }
            DoubleClickButton bt = new DoubleClickButton();

            bt.Tag = state;
            bt.ImageKey = detail;
            if (state == "失约")
            {
                bt.BackColor = Color.Red;
            }
            else if (state == "取消")
            {
                bt.BackColor = Color.Gray;
            }
            else if (state == "预约")
            {
                bt.BackColor = Color.Yellow;
            }
            else
            {
                bt.BackColor = Color.Blue;
            }
            bt.Name = ID;
            bt.Width = sizewidth + 15;
            bt.Height = dataGridView1.Rows[0].Height;
            int left = 0;
            int top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < Colindex; i++)
            {
                left += dataGridView1.Columns[i].Width;
            }
            bt.Top = top;
            bt.Left = left + Cards[Rowindex, Colindex] * bt.Width;  //已经存在cards的数量
            bt.TextAlign = ContentAlignment.MiddleLeft;
            bt.Text = "NO. " + bookindex + "\n" + "车牌:" + carno.Trim();

            bt.ContextMenuStrip = contextMenuStrip1;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(bt, detail);

            bt.MouseDown += new MouseEventHandler(bt_MouseDown);
            bt.MouseMove += new MouseEventHandler(bt_MouseMove);
            bt.MouseUp += new MouseEventHandler(bt_MouseUp);
            bt.DoubleClick += new EventHandler(bt_Click);
            dataGridView1.Controls.Add(bt);
            Cards[Rowindex, Colindex] = Cards[Rowindex, Colindex] + 1;
            dataGridView1.ClearSelection();
        }

        //双击卡片修改预约
        private void bt_Click(object sender, EventArgs e)
        {

            Control ct = (Control)sender;
            if (ct.Tag.ToString() != "预约" && ct.Tag.ToString() != "失约")
            {
                MessageBox.Show("改预约状态为“" + ct.Tag.ToString() + "”，不能修改！");
                return;
            }
            int ID = -1;
            if (ct.Name != string.Empty)
            {
                ID = int.Parse(ct.Name);
            }
            frmBookNew fb = new frmBookNew(ID);
            fb.UpdateCardHandle += new frmBookNew.UpdateCard(fb_UpdateCardHandle);
            fb.UpdateCardHoursdHandle += new frmBookNew.UpdateCardHours(SumWorkHours);
            if (fb.ShowDialog() == DialogResult.OK)
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(ct, UpdateDetail);
            }
        }
        private void fb_UpdateCardHandle(string ID, DateTime booktime, string detail)
        {
            //如果修改之后的日期不是当天，则删除卡片
            if (DateTime.Parse(label1.Text).ToString("yyyy-MM-dd") != booktime.ToString("yyyy-MM-dd"))
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Name == ID)
                    {
                        dataGridView1.Controls.Remove(ct);
                    }
                }
            }
            else
            {
                UpdateDetail = detail;
                ToolTip tt = new ToolTip();
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                tt.SetToolTip(bt, detail);
            }
        }
        private void bt_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ClsBLL.IsPower("更改预约时间"))
            {
                MessageBox.Show("你没有权限更改预约时间！");
                ((Control)sender).Top = PcStartTop;
                ((Control)sender).Left = PcStartLeft;
                return;
            }
            if (((Control)sender).Tag.ToString() != "预约" && ((Control)sender).Tag.ToString() != "失约")
            {
                //MessageBox.Show("改预约状态为“" + ((Control)sender).Tag.ToString() + "”,不能移动。");
                ((Control)sender).Top = PcStartTop;
                ((Control)sender).Left = PcStartLeft;
                return;
            }
            if (IsMove)
            {
                //decimal top = decimal.Parse(Convert.ToString(((Control)sender).Top - PcStartTop));
                //decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
                //Rowindex += int.Parse(decimal.Round(top / height, 0).ToString());
                decimal top = decimal.Parse(Convert.ToString(((Control)sender).Top - dataGridView1.ColumnHeadersHeight));
                decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
                Rowindex = int.Parse(decimal.Round(top / height, 0).ToString());
                if (top == 0)//Math.Abs(top - height) <(height * 3/4) ||
                {
                    return;
                }
                if (Rowindex == -1)
                {
                    Rowindex = 0;
                }
                ((Control)sender).Top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
                try
                {
                    if (Rowindex > 0)
                    {
                        //拖动完成之后更改数据库
                        int cols = (((Control)sender).Left - dataGridView1.Columns[0].Width) / dataGridView1.Columns[1].Width + 1;
                        DateTime Date1 = DateTime.Parse(label1.Text);
                        DateTime Date2;
                        string cellval = dataGridView1.Rows[Rowindex].Cells[0].Value.ToString();
                        Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval);
                        //if (Rowindex == dataGridView1.Rows.Count)
                        //{
                        //    Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval).AddMinutes(30);
                        //}
                        //else
                        //{
                        //    if (dataGridView1.Rows[Rowindex + 1].Cells[0].Value.ToString() == cellval)
                        //    {
                        //        Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval);
                        //    }
                        //    else
                        //    {
                        //        Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval).AddMinutes(30);
                        //    }
                        //}
                        if (Date2.CompareTo(DateTime.Now) < 0)  //拖动之后的时间小于当前时间，不能拖动
                        {
                            ((Control)sender).Top = PcStartTop;
                            ((Control)sender).Left = PcStartLeft;
                        }
                        string servicetype = dataGridView1.Columns[cols].HeaderText;
                        if (((Control)sender).Name != string.Empty)
                        {
                            int recordid = int.Parse(((Control)sender).Name);
                            string sqlstring = "Update Booking Set BookTime='" + Date2.ToString() + "', ServiceType='" + servicetype + "',State='预约',DelayBook='延迟预约' where ID=" + recordid;
                            SQLDbHelper.ExecuteSql(sqlstring);
                            ((Control)sender).BackColor = Color.Yellow;
                            ((Control)sender).Tag = "预约";
                        }
                        Button bt = (Button)((Control)sender);
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(bt, bt.ImageKey);
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            IsMove = false;
        }
        private void bt_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);//
            PcStartTop = ((Control)sender).Top;
            PcStartLeft = ((Control)sender).Left;
        }
        private void bt_MouseMove(object sender, MouseEventArgs e)
        {
            CureentCt = ((Control)sender);
            ((Control)sender).Cursor = Cursors.Arrow;//设置拖动时鼠标箭头
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
            IsMove = true;
        }
        private void ClearContrl()
        {
            if (index < dataGridView1.Controls.Count)
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    dataGridView1.Controls.Remove(ct);
                }
                dataGridView1.Refresh();
                index++;
                ClearContrl();
            }
        }
        private void SumWorkHours(decimal jd, decimal cs)
        {
            DataRow[] Drs = WorkHours.Select("DateIndex='" + DateTime.Parse(label1.Text).ToString("yyyy-MM-dd") + "'");
            if (Drs.Length > 0)
            {
                DayUseHoursJD = decimal.Parse(Drs[0]["UseJDHours"].ToString());
                DayUseHoursCS = decimal.Parse(Drs[0]["UseCSHours"].ToString());

                DayUseHoursJD = DayUseHoursJD - jd;
                DayUseHoursCS = DayUseHoursCS - cs;

                string jdstr = "总工作时间:" + Drs[0]["JDHours"].ToString() + "  占用时间:" + DayUseHoursJD.ToString() + "  剩余时间:" + Convert.ToString(decimal.Parse(Drs[0]["JDHours"].ToString()) - DayUseHoursJD);
                string csstr = "总工作时间:" + Drs[0]["CSHours"].ToString() + "  占用时间:" + DayUseHoursCS.ToString() + "  剩余时间:" + Convert.ToString(decimal.Parse(Drs[0]["CSHours"].ToString()) - DayUseHoursCS);
                dataGridView1.Rows[0].Cells[1].Value = jdstr;
                dataGridView1.Rows[0].Cells[2].Value = csstr;
            }
        }
        private void ShowGrid2(int month)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(5);
            dataGridView2.RowTemplate.Height = 140;
            DateTime DateM = new DateTime(DateTime.Today.Year, month, 1);
            label1.Text = DateM.ToString("yyyy年MM月");
            TimeSpan ts = DateM.AddMonths(1).Subtract(DateM);
            int tmp = Convert.ToInt32(DateM.DayOfWeek);
            if (tmp == 0) tmp = 7;
            int d = 1;
            try
            {
                for (int i = tmp - 1; i < 7; i++)
                {
                    dataGridView2.Rows[0].Cells[i].Value = Convert.ToString(d) + "日";
                    d++;
                }
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        if (d <= ts.Days)
                        {
                            dataGridView2.Rows[i].Cells[j].Value = Convert.ToString(d) + "日";
                            d++;
                        }
                    }

                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        StringBuilder Sb = new StringBuilder();
                        if (dataGridView2.Rows[i].Cells[j].Value != null)
                        {
                            int day = int.Parse(dataGridView2.Rows[i].Cells[j].Value.ToString().Substring(0, dataGridView2.Rows[i].Cells[j].Value.ToString().Length - 1));
                            Sb.AppendLine("       " + day.ToString() + "日");
                            Sb.AppendLine(string.Empty);
                            decimal sumhours = 0;
                            decimal usehours = 0;
                            dataGridView2.Rows[i].Cells[j].Tag = DateM.AddDays(day - 1).ToString("yyyy-MM-dd");

                            DataRow[] Drs = WorkHours.Select("DateIndex='" + DateM.AddDays(day - 1).ToString("yyyy-MM-dd") + "'");
                            if (Drs.Length > 0)
                            {
                                sumhours = decimal.Parse(Drs[0]["JDHours"].ToString()) + decimal.Parse(Drs[0]["CSHours"].ToString());
                                usehours = decimal.Parse(Drs[0]["UseJDHours"].ToString()) + decimal.Parse(Drs[0]["UseCSHours"].ToString());
                                if (sumhours > usehours)
                                {
                                    Sb.AppendLine("总共工时: " + sumhours.ToString());
                                    Sb.AppendLine("剩余工时: " + Convert.ToString(sumhours - usehours));
                                }
                            }

                            if (usehours > sumhours * 8 / 10)
                            {
                                DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                                dgvcs.ForeColor = Color.Red;
                                //dataGridView2.Rows[i].Cells[j].ErrorText = "1";
                                dataGridView2.Rows[i].Cells[j].Style = dgvcs;
                            }
                            dataGridView2.Rows[i].Cells[j].Value = Sb.ToString();
                        }
                    }
                    dataGridView2.Rows[i].Height = (dataGridView2.Height - dataGridView2.ColumnHeadersHeight) / dataGridView2.Rows.Count;
                }
                dataGridView2.Rows[0].Selected = false;
                dataGridView2.ClearSelection();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        //双击新增预约
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!ClsBLL.IsPower("新增预约"))
            {
                MessageBox.Show("你没有权限新增预约！");
                return;
            }
            Colindex = e.ColumnIndex;
            Rowindex = e.RowIndex;
            if (e.RowIndex < 1) return;
            DateTime Date1 = DateTime.Parse(label1.Text);
            DateTime Date2;
            string cellval = dataGridView1.Rows[Rowindex].Cells[0].Value.ToString();
            //if (Rowindex == dataGridView1.Rows.Count)
            //{
            //    Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval).AddMinutes(30);
            //}
            //else
            //{
            //    if (dataGridView1.Rows[Rowindex - 1].Cells[0].Value.ToString() == cellval)
            //    {
            //        Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval).AddMinutes(30);
            //    }
            //    else
            //    {
            Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval);
            //    }
            //}
            if (Date2.CompareTo(DateTime.Now) > 0)
            {
                frmBookNew fm3 = new frmBookNew(Date2, dataGridView1.Columns[e.ColumnIndex].HeaderText);
                fm3.AddCarHandle += new frmBookNew.AddCar(AddCarCard);
                fm3.UpdateCardHoursdHandle += new frmBookNew.UpdateCardHours(SumWorkHours);
                fm3.Show();
            }
            else
            {
                MessageBox.Show("选择的预约时间不能小于当前时间！");
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            nUDMonth.Visible = false;
            panel2.Visible = false;
            dataGridView1.BringToFront();
            dataGridView1.Top = panel1.Height + panel4.Height;
            dataGridView1.Height = this.Height - panel1.Height - panel4.Height;
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
            {
                ShowGrid1(DateTime.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString()));
            }
        }

        private void 更改预约时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (CureentCt.Tag.ToString() != "预约" && CureentCt.Tag.ToString() != "失约")
                {
                    MessageBox.Show("改预约状态是“" + CureentCt.Tag.ToString() + "”,不能更改预约时间！");
                    return;
                }
                bt_Click(CureentCt, e);
            }
        }

        private void 转为正式预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {

                if (CureentCt.Tag.ToString() != "预约")
                {
                    MessageBox.Show("改预约状态是“" + CureentCt.Tag.ToString() + "”,不能转为正式！");
                    return;
                }
                if (MessageBox.Show("你确定执行提交预约吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CureentCt.Name != string.Empty)
                    {
                        try
                        {
                            frmCarTop fct = new frmCarTop(int.Parse(CureentCt.Name));
                            fct.Left = CureentCt.Left + 100;
                            fct.Top = CureentCt.Top + panel1.Height + panel4.Height - 100;
                            fct.ShowDialog();
                            int recordid = int.Parse(CureentCt.Name);
                            string sqlstring = "Update Booking set State='正式',Success=1,ComeTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' where ID=" + recordid;
                            SQLDbHelper.ExecuteSql(sqlstring);
                            CureentCt.Tag = "正式";
                            CureentCt.BackColor = Color.Blue;
                        }
                        catch (Exception Err)
                        {
                            MessageBox.Show(Err.Message);
                        }
                    }
                }
            }
        }
        private void 取消预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (CureentCt.Tag.ToString() != "预约")
                {
                    MessageBox.Show("改预约状态是“" + CureentCt.Tag.ToString() + "”,不能取消！");
                    return;
                }
                if (MessageBox.Show("你确定取消该预约吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CureentCt.Name != string.Empty)
                    {
                        try
                        {
                            int recordid = int.Parse(CureentCt.Name);
                            string sqlstring = "Update Booking set State='取消',Success=0 where ID=" + recordid;
                            SQLDbHelper.ExecuteSql(sqlstring);
                            CureentCt.Tag = "取消";
                            CureentCt.BackColor = Color.Gray;
                        }
                        catch (Exception Err)
                        {
                            MessageBox.Show(Err.Message);
                        }
                    }
                }
            }
        }
        private void 删除预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (MessageBox.Show("你确定删除该预约吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (CureentCt.Name != string.Empty)
                    {
                        try
                        {
                            int recordid = int.Parse(CureentCt.Name);
                            string sqlstring = "Delete from Booking where ID=" + recordid;
                            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                            {
                                dataGridView1.Controls.Remove(CureentCt);
                            }
                        }
                        catch (Exception Err)
                        {
                            MessageBox.Show(Err.Message);
                        }
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlstring = "Select * from View_Search";
                string strWhere = " Where BookTime between '" + dateTimePicker1.Value.ToShortDateString() + "' and '" + dateTimePicker2.Value.AddDays(1).ToShortDateString() + "'";
                if (comboBox1.Text != string.Empty && comboBox1.Text.IndexOf("全部") == -1)
                {
                    strWhere += " And Creator ='" + comboBox1.Text + "'";
                }
                if (comboBox2.Text != string.Empty && comboBox2.Text.IndexOf("全部") == -1)
                {
                    strWhere += " And State ='" + comboBox2.Text + "'";
                }
                if (txtCarNo.Text != string.Empty)
                {
                    strWhere = " Where CarNo like '%" + txtCarNo.Text + "%'";
                }
                dataGridView3.DataSource = SQLDbHelper.Query(sqlstring + strWhere).Tables[0];
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void txtCarNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }
        private void nUDMonth_VisibleChanged(object sender, EventArgs e)
        {
            label7.Visible = nUDMonth.Visible;
        }

        private void nUDMonth_ValueChanged(object sender, EventArgs e)
        {
            if (nUDMonth.Visible)
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;
                panel2.Visible = false;
                dataGridView2.Top = panel1.Height + panel4.Height;
                dataGridView2.Height = this.Height - panel1.Height - panel4.Height;
                //ShowGrid2(int.Parse(nUDMonth.Value.ToString()));
            }
        }

        private void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Visible = dataGridView1.Visible;
            dateTimePicker3.Top = nUDMonth.Top;
            label8.Visible = dataGridView1.Visible;
            label9.Visible = dataGridView1.Visible;
            label10.Visible = dataGridView1.Visible;
            textBox1.Visible = dataGridView1.Visible;
            comboBox3.Visible = dataGridView1.Visible;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (ClsBLL.CarBookRecord(textBox1.Text))
                {
                    frmBookNew fbn = new frmBookNew(textBox1.Text);
                    fbn.Show();
                }
                else
                {
                    MessageBox.Show("没有车牌号为“" + textBox1.Text + "”预约信息！");
                }
            }
        }
    }
}