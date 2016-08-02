using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmBooking : Form
    {
        public frmBooking()
        {
            InitializeComponent();
            initialStyle = this.FormBorderStyle;
        }
        private FormBorderStyle initialStyle;
        private Point mouse_offset;

        private int Colindex = -1;
        private int Rowindex = -1;
        private int PcStartTop = 0;
        private int PcStartLeft = 0;

        private int PcStartRow = 0;
        private int PcStartCol = 0;
        private bool IsMove = false;

        private DateTime starttime = DateTime.Today;
        private DateTime endtime = DateTime.Today;
        private DataTable WorkHours = new DataTable();
        private decimal DayUseHoursJD = 0; //当天的剩余工时
        private decimal DayUseHoursCS = 0; //当天的剩余工时
        private int sizewidth = 190;
        private int[,] Cards;//
        private string UpdateDetail = string.Empty;
        private int index = 0;
        private Control CureentCt;
        private ComboBox CmbCars = new ComboBox();
        private int SelectBookID = -1;
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            try
            {
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
                nUDYear.Value = DateTime.Today.Year;
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
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //纵向合并
            if (e.ColumnIndex==0 && e.RowIndex >= 0)
            {
                using (
                    Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // 擦除原单元格背景
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        /**/////绘制线条,这些线条是单元格相互间隔的区分线条,
                        ////因为我们只对列name做处理,所以datagridview自己会处理左侧和上边缘的线条
                        if (e.RowIndex != this.dataGridView1.RowCount - 1)
                        {
                            if (this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                            {
                                if (e.Value.ToString() != this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                                {

                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);//下边缘的线
                                    //绘制值
                                    if (e.Value != null && e.Value.ToString() != string.Empty)
                                    {
                                        if (e.RowIndex > 2)
                                        {
                                            e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                                Brushes.Black, e.CellBounds.X + 2,
                                                e.CellBounds.Y - 12, StringFormat.GenericDefault);
                                        }
                                        else
                                        {
                                            e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                                Brushes.Black, e.CellBounds.X + 2,
                                                e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);//下边缘的线
                            //绘制值
                            if (e.Value != null && e.Value.ToString()!=string.Empty)
                            {
                                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                    Brushes.Black, e.CellBounds.X + 2,
                                    e.CellBounds.Y - 8, StringFormat.GenericDefault);
                            }
                        }
                        //右侧的线
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);

                        e.Handled = true;
                    }
                }
            }
        }
        //双击新增预约
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 1 || e.ColumnIndex==0)
            {
                return;
            }
            if (!ClsBLL.IsPower("新增预约"))
            {
                MessageBox.Show("你没有权限新增预约！");
                return;
            }
            Colindex = e.ColumnIndex;
            Rowindex = e.RowIndex;
            if (e.RowIndex < 1) return;
            DateTime Date1 = DateTime.Parse(labDateTime.Text);
            DateTime Date2;
            string cellval = dataGridView1.Rows[Rowindex].Cells[0].Value.ToString();
            Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval);
            if (!ClsBLL.CheckFormIsOpen("frmBookNew"))
            {
                string servicetype = "机电维修";
                if (Colindex == 2)
                {
                    servicetype = "车身维修";
                }
                frmBookNew fm3 = new frmBookNew(Date2, servicetype);
                fm3.AddCarHandle += new frmBookNew.AddCar(AddCarCardNew);
                if (fm3.ShowDialog() == DialogResult.OK)
                {
                    刷新ToolStripMenuItem_Click(null, null);
                }
            }
            else
            {
                Form frm = Application.OpenForms["frmBookNew"];
                frm.Focus();
            }
        }
        private void AddCarCardNew(int recordid)
        {
            string sqlstring = "Select * from Booking where ID="+ recordid;
            try
            {
                DataTable DtBook = SQLDbHelper.Query(sqlstring).Tables[0];
                foreach (DataRow dr in DtBook.Rows)
                {
                    DateTime TempT = DateTime.Parse(dr["BookTime"].ToString());
                    CheckRowindex(TempT);
                    AddCarCard(dr);
                }
                SumWorkHours(0, 0);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //双击卡片修改预约
        private void bt_Click(object sender, EventArgs e)
        {

            DoubleClickButton ct = (DoubleClickButton)sender;
            int ID = -1;
            if (ct.Name != string.Empty)
            {
                ID = int.Parse(ct.Name);
            }
            if (ID > -1)
            {
                frmBookNew fb = new frmBookNew(ID,false);
                if (fb.ShowDialog() == DialogResult.OK)
                {
                    刷新ToolStripMenuItem_Click(null, null);
                }
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
                ((Control)sender).Top = PcStartTop;
                ((Control)sender).Left = PcStartLeft;
                return;
            }
            if (IsMove)
            {
                decimal top = decimal.Parse(Convert.ToString(((Control)sender).Top - dataGridView1.ColumnHeadersHeight));
                decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
                Rowindex = int.Parse(decimal.Round(top / height, 0).ToString());
                if (Rowindex == -1)
                {
                    Rowindex = 0;
                }
                ((Control)sender).Top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
                try
                {
                    if (Rowindex > -1)
                    {
                        //拖动完成之后更改数据库
                        int cols = 0;
                        string servicetype = string.Empty;
                        if (((Control)sender).Left >= dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width)
                        {
                            cols = 2;
                            servicetype = "车身维修";
                            ((Control)sender).Left = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width + Cards[Rowindex, cols] * ((Control)sender).Width;
                        }
                        else
                        {
                            cols = 1;
                            servicetype = "机电维修";
                            ((Control)sender).Left = dataGridView1.Columns[0].Width + Cards[Rowindex, cols] * ((Control)sender).Width;
                        }
                        if (PcStartCol == cols && PcStartRow == Rowindex)  //如果移动有效
                        {
                            ((Control)sender).Top = PcStartTop;
                            ((Control)sender).Left = PcStartLeft;
                        }
                        else
                        {
                            DateTime Date1 = DateTime.Parse(labDateTime.Text);
                            DateTime Date2;
                            string cellval = dataGridView1.Rows[Rowindex].Cells[0].Value.ToString();
                            Date2 = DateTime.Parse(Date1.ToString("yyyy-MM-dd") + " " + cellval);
                            if (Date2.CompareTo(DateTime.Now) < 0)  //拖动之后的时间小于当前时间，不能拖动
                            {
                                ((Control)sender).Top = PcStartTop;
                                ((Control)sender).Left = PcStartLeft;
                                return;
                            }
                            Cards[Rowindex, cols] = Cards[Rowindex, cols] + 1;
                            Cards[PcStartRow, PcStartCol] = Cards[PcStartRow, PcStartCol] - 1;

                            if (((Control)sender).Name != string.Empty)
                            {
                                int recordid = int.Parse(((Control)sender).Name);
                                string sqlstring = "Update Booking Set BookTime='" + Date2.ToString() + "', ServiceType='" + servicetype + "',State='预约' where ID=" + recordid;   //,DelayBook='延迟预约'
                                SQLDbHelper.ExecuteSql(sqlstring);
                                ((Control)sender).BackColor = Color.Yellow;
                                ((Control)sender).Tag = "预约";
                            }
                            Button bt = (Button)((Control)sender);
                            ToolTip tt = new ToolTip();
                            tt.SetToolTip(bt, bt.ImageKey);
                        }
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

            decimal top = decimal.Parse(Convert.ToString(PcStartTop - dataGridView1.ColumnHeadersHeight));
            decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
            PcStartRow = int.Parse(decimal.Round(top / height, 0).ToString());
            if (PcStartLeft >= dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width)
            {
                PcStartCol = 2;
            }
            else
            {
                PcStartCol = 1;
            }
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
        //添加车辆预约卡片
        private void AddCarCard(DataRow dr)
        {
            DateTime booktime = DateTime.Parse(dr["BookTime"].ToString());
            string recordid = dr["ID"].ToString();
            string state = dr["State"].ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("车    牌:" + dr["CarNo"].ToString());
            sb.AppendLine("车    型:" + dr["CarType"].ToString());
            sb.AppendLine("维修类型:" + dr["ServiceType"].ToString());
            sb.AppendLine("维修项目:" + dr["ServiceItem"].ToString());
            sb.AppendLine("状    态:" + dr["State"].ToString());
            string carnotype = dr["CarNo"].ToString().Trim() + dr["ServiceType"].ToString().Trim();
            if (!CmbCars.Items.Contains(carnotype))
            {
                CmbCars.Items.Add(carnotype);
            }
            else
            {
                //MessageBox.Show(dr["CarNo"].ToString().Trim() + dr["ServiceType"].ToString().Trim());
                return;
            }
            if (DateTime.Parse(labDateTime.Text).ToString("yyyy-MM-dd") != booktime.ToString("yyyy-MM-dd"))
            {
                return;
            }
            DoubleClickButton bt = new DoubleClickButton();
            CheckRowindex(booktime);
            bt.Tag = state;
            bt.ImageKey = sb.ToString();
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
                bt.ForeColor = Color.White;
            }
            bt.Name = recordid;
            bt.Width = sizewidth+5;
            bt.Height = dataGridView1.Rows[0].Height;
            int tempcol = 1;
            if (dr["ServiceType"].ToString() == "车身维修")
            {
                tempcol = 2;
            }
            
            int left = 0;
            int top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < tempcol; i++)
            {
                left += dataGridView1.Columns[i].Width;
            }
            bt.Top = top;
            bt.Left = left + Cards[Rowindex, tempcol] * bt.Width;  //已经存在cards的数量
            bt.TextAlign = ContentAlignment.MiddleLeft;
            string remindstr = string.Empty;
            if (dr["IsRemind"].ToString().ToLower() == "true")
            {
                if (dr["IsRemindSuc"].ToString().ToLower() == "true")
                {
                    remindstr = " √";  //×√
                }
                else
                {
                    remindstr = " ×";
                }
            }
            string Sa = dr["PreSA"].ToString();
            if (bt.BackColor == Color.Blue)
            {
                Sa = dr["Receiver"].ToString(); 
            }
            string bookstr="自";
            if (Convert.ToBoolean(dr["IsBook"].ToString())) bookstr = "预";
            if (dr["DelayBook"] != null && dr["DelayBook"].ToString().IndexOf("迟到") > -1) bookstr = "迟";
            bt.Text = dr["CarNo"].ToString() + "/" + dr["LinkMan"].ToString() + "―" + Sa + " " + bookstr + remindstr;

            bt.ContextMenuStrip = contextMenuStrip1;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(bt, sb.ToString());

            bt.MouseDown += new MouseEventHandler(bt_MouseDown);
            bt.MouseMove += new MouseEventHandler(bt_MouseMove);
            bt.MouseUp += new MouseEventHandler(bt_MouseUp);
            bt.DoubleClick += new EventHandler(bt_Click);
            dataGridView1.Controls.Add(bt);
            Cards[Rowindex, tempcol] = Cards[Rowindex, tempcol] + 1;
            dataGridView1.ClearSelection();
        }
        private void ShowGrid1(DateTime Dt,string wherestr)
        {
            labDateTime.Text = Dt.ToString("yyyy年MM月dd日");
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
                if (wherestr!=string.Empty && btnPreview.Text != "全部")
                {
                    sqlstring += " And (PreSA='" + ClsBLL.UserName + "' Or Receiver='" + ClsBLL.UserName + "')";
                }
                sqlstring += " Order by BookTime";
                DataTable DataBook = SQLDbHelper.Query(sqlstring).Tables[0];
                //显示所有预约列表
                CmbCars.Items.Clear();
                foreach (DataRow dr in DataBook.Rows)
                {
                    DateTime TempT = DateTime.Parse(dr["BookTime"].ToString());
                    CheckRowindex(TempT);
                    AddCarCard(dr);
                }
                SumWorkHours(0, 0);
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void CheckRowindex(DateTime TempT)
        {
            int hour = TempT.Hour;
            int min = TempT.Minute;
            if (hour < starttime.Hour)
            {
                hour = starttime.Hour;
            }
            if (hour > endtime.Hour)
            {
                hour = endtime.Hour;
            }
            Rowindex = (hour - starttime.Hour) * 2 + 1;
            if (min == 30)
            {
                Rowindex++;
                if (hour == endtime.Hour && endtime.Minute == 0)
                {
                    Rowindex--;
                }
            }
        }
        private void SumWorkHours(decimal jd,decimal cs)
        {
            DataRow[] Drs = WorkHours.Select("DateIndex='" + DateTime.Parse(labDateTime.Text).ToString("yyyy-MM-dd") + "'");
            if (Drs.Length > 0)
            {
                DayUseHoursJD = decimal.Parse(Drs[0]["UseJDHours"].ToString());
                DayUseHoursCS = decimal.Parse(Drs[0]["UseCSHours"].ToString());

                DayUseHoursJD = DayUseHoursJD - jd;
                DayUseHoursCS = DayUseHoursCS - cs;

                string jdstr = "总工作TU:" + Drs[0]["JDHours"].ToString() + " 占用TU:" + DayUseHoursJD.ToString() + " 剩余TU:" + Convert.ToString(decimal.Parse(Drs[0]["JDHours"].ToString()) - DayUseHoursJD);
                string csstr = "总工作TU:" + Drs[0]["CSHours"].ToString() + " 占用TU:" + DayUseHoursCS.ToString() + " 剩余TU:" + Convert.ToString(decimal.Parse(Drs[0]["CSHours"].ToString()) - DayUseHoursCS);
                dataGridView1.Rows[0].Cells[1].Value = jdstr;
                dataGridView1.Rows[0].Cells[2].Value = csstr;
                dataGridView1.Columns[1].HeaderText = "机电维修 [预约来店：" + Drs[0]["SumCarsBook"].ToString() + " 自行来店：" + Drs[0]["SumCarsNoBook"].ToString() + "]";
            }
            else
            {
                string jdstr = "总工作TU:0  占用TU:0 剩余TU:0";
                string csstr = "总工作TU:0  占用TU:0 剩余TU:0";
                dataGridView1.Rows[0].Cells[1].Value = jdstr;
                dataGridView1.Rows[0].Cells[2].Value = csstr;
                dataGridView1.Columns[1].HeaderText = "机电维修 [预约来店：0 自行来店：0";

            }
        }
        //获取每个月有多少个周
        private int WeekOfMonth(DateTime day)
        {
            DateTime firstofmonth;
            firstofmonth = Convert.ToDateTime(day.Date.Year + "-" + day.Date.Month + "-" + 1);
            int months = 1;
            int days = DateTime.DaysInMonth(day.Year, day.Month);
            int wkday = (int)firstofmonth.DayOfWeek;
            int subdays = 0;
            if (wkday == 0)
            {
                subdays = days - 1;
            }
            else
            {
                subdays = days - ((7 - wkday) + 1);
            }
            months += subdays / 7;
            if (subdays % 7 > 0)
            {
                months += 1;
            }
            return months;
        }
        private void ShowGrid2(int month)
        {
            DateTime DateM = new DateTime(int.Parse(nUDYear.Value.ToString()), month, 1);
            int weeks = WeekOfMonth(DateM);
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(weeks);
            dataGridView2.RowTemplate.Height = dataGridView2.Height / weeks;

            string sqlstring = "Select * from WorkHours where DateIndex between '" + DateM + "' and '" + DateM.AddMonths(1) + "'";
            WorkHours = SQLDbHelper.Query(sqlstring).Tables[0];

            labDateTime.Text = DateM.ToString("yyyy年MM月");
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
                //for (int i = 1; i < 5; i++)
                for (int i = 1; i < dataGridView2.Rows.Count; i++)
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
                                if (sumhours > 0)
                                {
                                    Sb.AppendLine("总共工时: " + sumhours.ToString());
                                    Sb.AppendLine("剩余工时: " + Convert.ToString(sumhours - usehours));
                                    Sb.AppendLine("总车辆数: " + Drs[0]["SumCars"].ToString());
                                    decimal subrate = 100 * (sumhours - usehours) / sumhours;
                                    subrate = decimal.Round(subrate, 1);
                                    Sb.AppendLine("剩余比例: " + subrate.ToString() + "%");
                                }
                            }

                            if (usehours > sumhours * 8 / 10)
                            {
                                DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                                dgvcs.ForeColor = Color.Red;
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
        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null && dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                {
                    panel5.Visible = true;
                    dataGridView2.Visible = false;
                    panDate.Visible = false;
                    panCar.Visible = true;
                    panel2.Visible = false;
                    panel5.BringToFront();
                    panel5.Top = panel1.Height + panel4.Height;
                    panel5.Height = this.Height - panel1.Height - panel4.Height;

                    ShowGrid1(DateTime.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString()),string.Empty);
                    btnPreview.Text = "我接车";
                }
            }
        }

        private void 更改预约时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (CureentCt.Tag.ToString() != "预约" && CureentCt.Tag.ToString() != "失约" && CureentCt.Tag.ToString() != "取消")
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
                if (CureentCt.Tag.ToString() == "正式")
                {
                    MessageBox.Show("预约状态是“正式”,不能转为正式！");
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
                            if (fct.ShowDialog() == DialogResult.OK)
                            {
                                int recordid = int.Parse(CureentCt.Name);
                                //判断是否有两个类型预约
                                int doubleid = ClsBLL.GetDoubleID(recordid, DateTime.Parse(labDateTime.Text));
                                if (doubleid > -1)
                                {
                                    foreach (Control ct in dataGridView1.Controls)
                                    {
                                        if (ct.Name == doubleid.ToString())
                                        {
                                            ct.Tag = "正式";
                                            ct.BackColor = Color.Blue;
                                            ct.ForeColor = Color.White;
                                        }
                                    }
                                }
                                CureentCt.Tag = "正式";
                                CureentCt.BackColor = Color.Blue;
                                CureentCt.ForeColor = Color.White;
                                try
                                {
                                    DataTable DtTime = SQLDbHelper.Query("Select DelayBook,IsBook from Booking where ID=" + recordid).Tables[0];
                                    if (DtTime.Rows.Count > 0)
                                    {
                                        if (DtTime.Rows[0]["IsBook"].ToString() == "0")
                                        {
                                            string txt = CureentCt.Text;
                                            CureentCt.Text = txt.Replace("预", "自");
                                            CureentCt.Refresh();
                                        }
                                        if (DtTime.Rows[0]["DelayBook"] != null && DtTime.Rows[0]["DelayBook"].ToString().IndexOf("迟到") > -1)
                                        {
                                            string txt = CureentCt.Text;
                                            CureentCt.Text = txt.Replace("预", "迟");
                                            CureentCt.Text = txt.Replace("自", "迟");
                                            CureentCt.Refresh();
                                        }
                                    }
                                }
                                catch (Exception Err)
                                {
                                    MessageBox.Show(Err.Message);
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
        }
        private void 取消预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (CureentCt.Tag.ToString() == "正式")
                {
                    MessageBox.Show("预约状态是“正式”,不能转为正式！");
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
                            int doubleid = ClsBLL.GetDoubleID(recordid, DateTime.Parse(labDateTime.Text));
                            if (doubleid > -1)
                            {
                                foreach (Control ct in dataGridView1.Controls)
                                {
                                    if (ct.Name==doubleid.ToString())
                                    {
                                        sqlstring = "Update Booking set State='取消',Success=0 where ID=" + doubleid;
                                        SQLDbHelper.ExecuteSql(sqlstring);
                                        ct.Tag = "取消";
                                        ct.BackColor = Color.Gray;
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
        }
        private void 删除预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (CureentCt.Tag.ToString() == "正式")
                {
                    MessageBox.Show("预约状态是“正式”,不能转为正式！");
                    return;
                }
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
                            int doubleid = ClsBLL.GetDoubleID(recordid, DateTime.Parse(labDateTime.Text));
                            if (doubleid > -1)
                            {
                                foreach (Control ct in dataGridView1.Controls)
                                {
                                    if (ct.Name == doubleid.ToString())
                                    {
                                        sqlstring = "Delete From Booking where ID=" + doubleid;
                                        dataGridView1.Controls.Remove(ct);
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
        }
        private void btnToday_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            dataGridView2.Visible = false;
            panel2.Visible = false;
            panel5.BringToFront();
            panel5.Top = panel1.Height + panel4.Height;
            panel5.Height = this.Height - panel1.Height - panel4.Height;
            panCar.Visible = true;
            panDate.Visible = false;
            ShowGrid1(DateTime.Today,string.Empty);
            btnPreview.Text = "我接车";
        }
        private void btnMonth_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            dataGridView2.Visible = true;
            panel2.Visible = false;
            dataGridView2.Top = panel1.Height + panel4.Height;
            dataGridView2.Height = this.Height - panel1.Height - panel4.Height;

            panCar.Visible = false;
            panDate.Visible = true;
            panDate.Left = panCar.Left;
            nUDYear.Value = DateTime.Today.Year;
            nUDMonth.Value = DateTime.Today.Month;
            ShowGrid2(DateTime.Today.Month);
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            dataGridView2.Visible = false;
            panDate.Visible = false;
            panel2.Visible = true;
            panel2.Top = panel1.Height + panel4.Height;
            panel2.Height = this.Height - panel1.Height - panel4.Height-3;
            try
            {
                if (comboBox1.Items.Count == 0)
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

                    sqlstring = "Select Items from SysDictionary where ItemName='SA'";
                    DataTable Dt2 = SQLDbHelper.Query(sqlstring).Tables[0];
                    comboBox3.Items.Add("--全部--");
                    foreach (DataRow dr in Dt2.Rows)
                    {
                        comboBox3.Items.Add(dr[0].ToString());
                    }
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
                if (comboBox3.Text != string.Empty && comboBox3.Text.IndexOf("全部") == -1)
                {
                    strWhere += " And Receiver ='" + comboBox3.Text + "'";
                }
                if (txtCarNo.Text != string.Empty)
                {
                    strWhere = " Where CarNo like '%" + txtCarNo.Text + "%'";
                }
                dataGridView3.DataSource = SQLDbHelper.Query(sqlstring + strWhere + "  Order by Booktime desc").Tables[0];
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
        private void nUDMonth_ValueChanged(object sender, EventArgs e)
        {
            if (nUDMonth.Visible)
            {
                panel5.Visible = false;
                dataGridView2.Visible = true;
                panel2.Visible = false;
                dataGridView2.Top = panel1.Height + panel4.Height;
                dataGridView2.Height = this.Height - panel1.Height - panel4.Height;
                ShowGrid2(int.Parse(nUDMonth.Value.ToString()));
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (ClsBLL.CarBookRecord(textBox1.Text))
                {
                    frmBookLook fbn = new frmBookLook(textBox1.Text);
                    fbn.Show();
                }
                else
                {
                    MessageBox.Show("没有车牌号为“"+ textBox1.Text + "”预约信息！");
                }
            }
        }
        private void frmBooking_Paint(object sender, PaintEventArgs e)
        {
            Button bt = new Button();
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString("粤A.BCDEF/李自然/王子龙 自 ×", bt.Font, 8000, sf);
            sizewidth =int.Parse(size.Width.ToString());
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel5.Visible)
            {
                ShowGrid1(DateTime.Parse(labDateTime.Text),string.Empty);
            }
            else
            {
                ShowGrid2(int.Parse(nUDMonth.Value.ToString()));
            }
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panCar_VisibleChanged(object sender, EventArgs e)
        {
            panLook.Visible = panCar.Visible;
            panLook.Top = panCar.Top;
        }
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            frmSelectBook fsb = new frmSelectBook(dateTimePicker3.Value, sizewidth);
            fsb.Show();
        }
        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                int bookid = int.Parse(dataGridView3.Rows[e.RowIndex].Cells["ColID"].Value.ToString());
                SelectBookID = bookid;
                frmBookLook fbn = new frmBookLook(bookid);
                fbn.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (SelectBookID >-1)
            {
                frmBookLook fbn = new frmBookLook(SelectBookID);
                fbn.Show();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (panel5.Visible)
            {
                ShowGrid1(DateTime.Parse(labDateTime.Text),btnPreview.Text);
            }
            if (btnPreview.Text == "全部")
            {
                btnPreview.Text = "我接车";
            }
            else 
            {
                btnPreview.Text = "全部";
            }
        }
    }
    public class DoubleClickButton : Button
    {
        public DoubleClickButton(): base()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }
    }
}
