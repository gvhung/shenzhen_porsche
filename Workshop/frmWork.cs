using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Workshop
{
    public partial class frmWork : Form
    {
        public frmWork()
        {
            InitializeComponent();
        }
        private Point mouse_offset;

        private DateTime starttime = DateTime.Today;
        private DateTime endtime = DateTime.Today;
        private int Colindex = -1;
        private int Rowindex = -1;
        private int PcStartTop = 0;
        private int PcStartLeft = 0;
        private bool IsMove = false;
        private int sizewidth = 80;
        private int[,] Cards;
        private int index = 0;
        string[] States = new string[9] {"正式","等待开工","已派工", "维修进行中", "中断", "完工", "洗车","过时","延时到明天" };
        private int WaiteCars = 0;
        private Control CureentCt;     
        private void frmWork_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            label1.Text = DateTime.Now.ToString("yyyy年MM月dd日");
            starttime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));
            endtime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet2"));
            TimeSpan ts = endtime.Subtract(starttime);
            int cols = ts.Hours;
            if (ts.Minutes > 0) cols++;
            for (int i = 0; i <= cols; i++)
            {
                DateTime NewDt = starttime.AddHours(i);
                dataGridView1.Columns.Add("Col" + NewDt.Hour.ToString(), NewDt.Hour.ToString() +":00");
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ShowGrid();
            int Interval = int.Parse(ClsBLL.GetSet("txtSet4"));
            timer1.Interval = Interval * 1000 * 60;
            timer1.Enabled = true;
            CheckPower();
        }
        private void wbt_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                picUp_Click(null, null);
            }
            else
            {
                picDown_Click(null, null);
            }
        }
        private void CheckPower()
        {
            if (!ClsBLL.IsPower(btnBooking.Text))
            {
                btnBooking.Enabled = false;
            }
            if (!ClsBLL.IsPower(btnCarStae.Text))
            {
                btnCarStae.Enabled = false;
            }
            if (!ClsBLL.IsPower(btnClearCar.Text))
            {
                btnClearCar.Enabled = false;
                洗车ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower(btnStart.Text))
            {
                btnStart.Enabled = false;
                开工ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower(btnComplete.Text))
            {
                btnComplete.Enabled = false;
                完工ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower(btnPause.Text))
            {
                btnPause.Enabled = false;
                中断ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower(btnDelay.Text))
            {
                btnDelay.Enabled = false;
                延时ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("分单"))
            {
                分单ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("返修"))
            {
                返修ToolStripMenuItem.Visible = false;
            }
        }
        private void ShowGrid()
        {
            pn.Left = 0;
            pn.Height = 25;
            pn.Width = dataGridView1.Columns[0].Width;
            pn.Top = dataGridView1.Height - pn.Height + panel1.Height + panel4.Height + 5;
            //选择今天当班的维修工人
            string sqlstring = "Select WorkerName,Worker.WorkerCode from WorkerPlan inner join Worker on WorkerPlan.WorkerCode=Worker.WorkerCode Where Wyear=" + DateTime.Today.Year + " and Wmonth=" + DateTime.Today.Month + " and Wday=" + DateTime.Today.Day + " and IsWork=1 and Workergroup in ('"+ ClsBLL.UserGroup +"') order by Workergroup desc";
            dataGridView1.Rows.Clear();
            WaiteCars = 0;
            DataTable Worker = SQLDbHelper.Query(sqlstring).Tables[0];
            if (Worker.Rows.Count == 0) return;
            dataGridView1.Rows.Add(Worker.Rows.Count);
            Cards = new int[dataGridView1.Rows.Count, 23];
            //当天单的状态，以及前7天未处理的单
            //2012.05.12 修改 StartServiceTime 为 AssignTime
            sqlstring = "Select * from Booking Where State<>'洗车' And State<>'正式' and servicetype='机电维修' And  AssignTime between '" + DateTime.Today.ToShortDateString() + "' and '" + DateTime.Today.AddDays(1).ToShortDateString() + "'";
            sqlstring += " Union all Select * from Booking Where servicetype='机电维修' And (State='维修进行中' Or State='中断' Or State='过时') And StartServiceTime between '" + DateTime.Today.AddDays(-7).ToShortDateString() + "' and '" + DateTime.Today.ToShortDateString() + "'";
            DataTable WorkerRO = SQLDbHelper.Query(sqlstring).Tables[0];
            int i = 0;
            //已经派工的预约
            foreach (DataRow DrWorker in Worker.Rows)
            {
                try
                {
                    dataGridView1.Rows[i].Height = (dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.Rows.Count;
                    if(dataGridView1.Rows[i].Height>70)
                    {
                        dataGridView1.Rows[i].Height=70;
                    }
                    dataGridView1.Rows[i].Cells[1].Value = DrWorker["WorkerName"].ToString();
                    dataGridView1.Rows[i].Cells[1].Tag = DrWorker["WorkerCode"].ToString();
                    foreach (DataRow DrRo in WorkerRO.Select("Worker='" + DrWorker["WorkerName"].ToString() + "'"))
                    {
                        string starttime = string.Empty;
                        if (DrRo["StartServiceTime"] != null)
                        {
                            starttime = DrRo["StartServiceTime"].ToString();
                        }
                        //2012.05.12 修改
                        string info = DrRo["State"].ToString() + "," + DrRo["CarNo"].ToString() + "," + DrRo["ServiceType"].ToString() + "," + DrRo["ServiceHour"].ToString() + "," + starttime + "," + DrRo["PlanCompleteTime"].ToString() + "," + DrRo["EndServiceTime"].ToString() + "," + DrRo["PlanOutTime"].ToString() + "," + DrRo["Remark"].ToString() + "," + DrRo["AssignTime"].ToString();

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("车    牌:" + DrRo["CarNo"].ToString());
                        sb.AppendLine("车    型:" + DrRo["CarType"].ToString());
                        sb.AppendLine("维修项目:" + DrRo["ServiceItem"].ToString());
                        sb.AppendLine("接车人:" + DrRo["Receiver"].ToString());
                        sb.AppendLine("计划完成时间:" + DrRo["PlanOutTime"].ToString());
                        sb.AppendLine("预计维修工时时间:" + Convert.ToString(100 * decimal.Parse(DrRo["ServiceHour"].ToString())));
                        info = info + "," + sb.ToString();
                        AddCarCard(DrRo["ID"].ToString(), info, i);
                    }
                    i++;
                }
                catch(Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            ChangeWidth();
            //当前等待派工的预约
            sqlstring = "Select * from Booking Where (servicetype='机电维修' And State='正式' and ComeTime <= '" + DateTime.Today.AddDays(1).ToShortDateString() + "')";
            sqlstring += " OR (servicetype='机电维修' And State='延时到明天' and StartServiceTime<'" + DateTime.Today.ToString() + "') Order by ComeTime Desc";
            DataTable DtRo = SQLDbHelper.Query(sqlstring).Tables[0];
            WaiteCars = 0;
            ComboBox cmb = new ComboBox();
            foreach (DataRow DrRo in DtRo.Rows)
            {
                if (!cmb.Items.Contains(DrRo["CarNo"].ToString()))
                {
                    cmb.Items.Add(DrRo["CarNo"].ToString());
                    string info = DrRo["BookIndex"].ToString() + "," + DrRo["State"].ToString() + "," + DrRo["CarNo"].ToString() + "," + DrRo["ServiceType"].ToString() + "," + DrRo["ServiceHour"].ToString() + "," + DrRo["ComeTime"].ToString() + "," + DrRo["PlanOutTime"].ToString() + "," + DrRo["IsBook"].ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("车    牌:" + DrRo["CarNo"].ToString());
                    sb.AppendLine("车    型:" + DrRo["CarType"].ToString());
                    sb.AppendLine("维修项目:" + DrRo["ServiceItem"].ToString());
                    info = info + "," + sb.ToString();
                    string delaybook = string.Empty;
                    if (DrRo["DelayBook"] != null)
                    {
                        delaybook = DrRo["DelayBook"].ToString();
                    }
                    info = info + "," + delaybook;
                    AddWaiteCar(DrRo["ID"].ToString(), info);
                }
            }
            dataGridView1.Columns[0].HeaderText = "等待[" + cmb.Items.Count.ToString() + "]";
            dataGridView1.ClearSelection();
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
        private void AddWaiteCar(string ID, string info)
        {
            DoubleClickButton wbt = new DoubleClickButton();
            string[] strinfo = info.Split(new Char[] { ',' });
            wbt.Name = ID.ToString();
            wbt.Top = dataGridView1.Rows[0].Height * WaiteCars + dataGridView1.ColumnHeadersHeight;
            wbt.Left = 1;  //已经存在cards的数量
            wbt.TextAlign = ContentAlignment.MiddleLeft;
            wbt.Height = dataGridView1.Rows[0].Height;
            wbt.Width = dataGridView1.Columns[0].Width - 2;
            wbt.ImageKey = info;
            wbt.BackColor = Color.Yellow;
            if (DateTime.Parse(strinfo[5]).CompareTo(DateTime.Today) < 0)
            {
                wbt.BackColor = Color.Wheat;
            }
            wbt.Tag = strinfo[2].ToString();
            ToolTip tt = new ToolTip();
            tt.SetToolTip(wbt, strinfo[8]);

            wbt.MouseDown += new MouseEventHandler(wbt_MouseDown);
            wbt.MouseMove += new MouseEventHandler(wbt_MouseMove);
            wbt.MouseUp += new MouseEventHandler(wbt_MouseUp);
            wbt.DoubleClick+=new EventHandler(wbt_DoubleClick);
            wbt.Paint+=new PaintEventHandler(wbt_Paint);
            wbt.MouseWheel+=new MouseEventHandler(wbt_MouseWheel);
            dataGridView1.Controls.Add(wbt);
            WaiteCars = WaiteCars+ 1;
        }
        //添加车辆预约卡片
        private void AddCarCard(string ID, string info,int rowindex)
        {
            DoubleClickButton bt = new DoubleClickButton();
            string[] strs = info.Split(new Char[] { ',' });
            string startdate = strs[4];
            decimal hours =decimal.Parse(strs[3]);
            string plancomdate = strs[5];//计划完成时间
            DateTime PlanEndDate = DateTime.Now;
            if (startdate == string.Empty) startdate = strs[9];
            if (plancomdate != string.Empty)
            {
                PlanEndDate = DateTime.Parse(plancomdate);
            }
            else
            {
                PlanEndDate = DateTime.Parse(startdate).AddMinutes(double.Parse(Convert.ToString(hours * 60 + ClsBLL.Pausemins(int.Parse(ID), DateTime.Parse(startdate)))));
            }
            if (strs[0] == "中断")     //如果是中断，则停在做中断操作时的时间点，否则停在按计算计划完成的时间点上
            {
                PlanEndDate = ClsBLL.PauseTime(int.Parse(ID));
            }
            TimeSpan DtSp = PlanEndDate.Subtract(starttime);   //计划完成时间和开始时间或计划开始时间的差
            int hour = DtSp.Hours;             //如果超出了时间范围，显示在最后一列
            if (hour > dataGridView1.ColumnCount - 3)
            {
                hour = dataGridView1.ColumnCount - 3;
            }
            if (hour < 0)
            {
                hour = 0;
            }
            bt.AccessibleDescription = hour.ToString();
            bt.TabIndex = rowindex;
            Cards[rowindex, hour] = Cards[rowindex, hour] + 1;

            decimal left = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width + dataGridView1.Columns[2].Width * hour;
            bt.Left = int.Parse(Convert.ToString(Math.Round(left, 0)));
            int top = rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight + 3;

            bt.Height = dataGridView1.Rows[0].Height - 6;
            bt.Width = dataGridView1.Columns[2].Width ;
            bt.Top = top;
            bt.Tag = strs[0]; //状态
            bt.Name = ID;
            bt.ImageKey = info;

            dataGridView1.Controls.Add(bt);
            string planout = strs[7];
            if (planout != string.Empty)
            {
                planout = DateTime.Parse(planout).ToString("MM-dd HH:mm");
            }
            bt.Text = strs[1] + "\n" + planout;
            if (strs[8].IndexOf("分单") > -1)
            {
                string fendan = string.Empty;
                if(strs[8].Length> strs[8].IndexOf("分单")+2)
                {
                    fendan = strs[8].Substring(strs[8].IndexOf("分单") + 2, 1);
                    if (!ClsBLL.IsNumber(fendan))
                    {
                        fendan = string.Empty;
                    }
                }
                if (fendan == string.Empty)
                {
                    bt.Text = strs[1] + "  合\n" + planout;
                }
                else
                {
                    bt.Text = strs[1] + "  合"+ fendan +"\n" + planout;
                }
            }
            bt.Font = new Font("宋体", 11, FontStyle.Regular);
            ToolTip tt = new ToolTip();
            tt.SetToolTip(bt, strs[strs.Length - 1]);
            Color cl = Color.PowderBlue;
            //"正式","等待开工","已派工", "维修进行中", "中断", "完工", "洗车","过时","延时到明天"
            if (strs[0] == States[1])
            {
                cl = Color.Aqua;
            }
            if (strs[0] == States[3])
            {
                cl = Color.Green;
                if (strs[8].IndexOf("返修") > -1)
                {
                    cl = Color.MediumSeaGreen;
                }
                if (strs[8].IndexOf("追加项目") > -1)
                {
                    cl = Color.OliveDrab;
                }
            }
            if (strs[0] == States[4])
            {
                cl = Color.LightGray;
            }
            if (strs[0] == States[5])
            {
                cl = Color.Blue;
            }
            if (strs[0] == States[6])
            {
                cl = Color.MediumOrchid;
            }
            if (strs[0] == States[7])
            {
                cl = Color.Red;
            }
            if (strs[0] == States[8])
            {
                cl = Color.Orange;
            }
            bt.BackColor = cl;
            bt.ContextMenuStrip = contextMenuStrip2;
            bt.MouseDown+=new MouseEventHandler(bt_MouseDown);
            bt.MouseMove+=new MouseEventHandler(bt_MouseMove);
            bt.MouseUp+=new MouseEventHandler(bt_MouseUp);
            bt.DoubleClick+=new EventHandler(bt_DoubleClick);
            //ChangeWidth(rowindex,hour,bt);
        }

        private void ChangeWidth()
        {
            //int rowindex,int colindex, DoubleClickButton bt
            //int m = 0;
            //foreach (Control ct in dataGridView1.Controls)
            //{   //当一个单元格有多个卡片时，调整卡片宽度
            //    if (ct.AccessibleDescription == bt.AccessibleDescription && ct.Top == bt.Top && ct.Name != bt.Name)
            //    {
            //        m++;
            //        ct.Width = dataGridView1.Columns[2].Width / Cards[rowindex, colindex];
            //        bt.Width = dataGridView1.Columns[2].Width / Cards[rowindex, colindex];
            //        if (m == 2)
            //        {
            //            ct.Left = ct.Left - dataGridView1.Columns[2].Width / 6;
            //        }
            //        bt.Left = ct.Left + ct.Width;
            //    }
            //}
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    if (Cards[i, j] > 1)
                    {
                        int m = 0;
                        foreach (Control ct in dataGridView1.Controls)
                        {
                            if (ct.AccessibleDescription != null)
                            {
                                DoubleClickButton dcb = (DoubleClickButton)ct;
                                if (dcb.TabIndex == i && ct.AccessibleDescription == j.ToString())
                                {
                                    int left = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width + dataGridView1.Columns[2].Width * j;

                                    ct.Width = ct.Width / Cards[i, j];
                                    ct.Left = left + m * ct.Width;
                                    m++;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void bt_DoubleClick(object sender, EventArgs e)
        {
            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
            frmBookLook fbn = new frmBookLook(int.Parse(bt.Name));
            fbn.ShowDialog();
        }
        private void bt_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                if (((Control)sender).Tag.ToString() != "维修进行中" && ((Control)sender).Tag.ToString() != "中断")
                {
                    ((Control)sender).Top = PcStartTop;
                    ((Control)sender).Left = PcStartLeft;
                    return;
                }
                decimal top = decimal.Parse(Convert.ToString(((Control)sender).Top - dataGridView1.ColumnHeadersHeight));
                decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
                Rowindex = int.Parse(decimal.Round(top / height, 0).ToString());

                if (Math.Abs(((Control)sender).Top - PcStartTop) < dataGridView1.Rows[2].Height / 2)
                {
                    ((Control)sender).Top = PcStartTop;
                    ((Control)sender).Left = PcStartLeft;
                    return;
                }
                if (top == 0)
                {
                    ((Control)sender).Top = PcStartTop;
                    ((Control)sender).Left = PcStartLeft;
                    return;
                }
                if (Rowindex == -1)
                {
                    Rowindex = 0;
                }
                ((Control)sender).Top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
                try
                {
                    if (Rowindex > -1)
                    {
                        if (((Control)sender).Name != string.Empty)
                        {
                            //记录ID
                            int recordid = int.Parse(((Control)sender).Name);
                            //维修工时
                            decimal servicehours = ClsBLL.GetServiceHour(recordid);
                            //维修工人
                            string serviceworker = dataGridView1.Rows[Rowindex].Cells[1].Value.ToString();
                            if (MessageBox.Show("你确定要把该单转给<" + serviceworker + ">做吗？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                ((Control)sender).Top = PcStartTop;
                                ((Control)sender).Left = PcStartLeft;
                                return;
                            }
                            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
                            string[] strs = bt.ImageKey.Split(new Char[] { ',' });
                            string startdate = strs[4];  //计划完成工作时间 = 开始工作时间 + 维修工时                            
                            string plancomplete = strs[5];  //预计完成时间
                            if (((Control)sender).Tag.ToString() == "中断")
                            {
                                ClsBLL.ServicePauseStart(recordid);
                                plancomplete = DateTime.Parse(plancomplete).AddMinutes(ClsBLL.Pausemins(recordid, DateTime.Parse(startdate))).ToShortDateString();
                            }
                            string sqlstring = "Insert Into BookingAdd(BookID,OldWorker,StartServiceTime)Select ID,Worker,StartServiceTime from Booking where ID=" + recordid;
                            sqlstring += ";Update Booking Set StartServiceTime='" + DateTime.Now.ToString() + "',PlanCompleteTime='" + plancomplete + "', Worker='" + serviceworker + "',State='维修进行中' where ID=" + recordid;
                            SQLDbHelper.ExecuteSql(sqlstring);
                            ClsBLL.AddMsg(recordid, "车牌号码:" + strs[1] + ",该单转给<" + serviceworker + ">做");
                            刷新ToolStripMenuItem_Click(null, null);
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
            mouse_offset = new Point(-e.X, -e.Y);
            PcStartTop = ((Control)sender).Top;
            PcStartLeft = ((Control)sender).Left;

            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
            开工ToolStripMenuItem.Enabled = true;
            中断ToolStripMenuItem.Enabled = true;
            洗车ToolStripMenuItem.Enabled = true;
            完工ToolStripMenuItem.Enabled = true;
            延时ToolStripMenuItem.Enabled = true;
            返修ToolStripMenuItem.Enabled = true;
            追加工时ToolStripMenuItem.Enabled = true;
            洗车与送车时间ToolStripMenuItem.Enabled = true;
            分单ToolStripMenuItem.Enabled = true;
            if (bt.Tag.ToString() == "等待开工")
            {
                中断ToolStripMenuItem.Enabled = false;
                洗车ToolStripMenuItem.Enabled = false;
                完工ToolStripMenuItem.Enabled = false;
                延时ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                追加工时ToolStripMenuItem.Enabled = false;
                洗车与送车时间ToolStripMenuItem.Enabled = false;
            }
            if (bt.Tag.ToString() == "维修进行中")
            {
                开工ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                洗车ToolStripMenuItem.Enabled = false;
                洗车与送车时间ToolStripMenuItem.Enabled = false;
                中断ToolStripMenuItem.Text = "中断";
            }
            if (bt.Tag.ToString() == "中断")
            {
                开工ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                中断ToolStripMenuItem.Text = "继续";
                洗车ToolStripMenuItem.Enabled = false;
                洗车与送车时间ToolStripMenuItem.Enabled = false;
            }
            if (bt.Tag.ToString() == "完工")
            {
                开工ToolStripMenuItem.Enabled = false;
                中断ToolStripMenuItem.Enabled = false;
                完工ToolStripMenuItem.Enabled = false;
                延时ToolStripMenuItem.Enabled = false;
                追加工时ToolStripMenuItem.Enabled = false;
                分单ToolStripMenuItem.Enabled = false;
            }
            if (bt.Tag.ToString() == "洗车")
            {
                开工ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                中断ToolStripMenuItem.Enabled = false;
                完工ToolStripMenuItem.Enabled = false;
                延时ToolStripMenuItem.Enabled = false;
                洗车ToolStripMenuItem.Enabled = false;
                追加工时ToolStripMenuItem.Enabled = false;
                分单ToolStripMenuItem.Enabled = false;
            }
            if (bt.Tag.ToString() == "延时到明天")
            {
                开工ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                中断ToolStripMenuItem.Enabled = false;
                洗车ToolStripMenuItem.Enabled = false;
                洗车与送车时间ToolStripMenuItem.Enabled = false;
                分单ToolStripMenuItem.Enabled = false;
                追加工时ToolStripMenuItem.Enabled = false;
                延时ToolStripMenuItem.Enabled = false;
            }
            if (bt.Tag.ToString() == "过时")
            {
                开工ToolStripMenuItem.Enabled = false;
                洗车ToolStripMenuItem.Enabled = false;
                返修ToolStripMenuItem.Enabled = false;
                洗车与送车时间ToolStripMenuItem.Enabled = false;
                分单ToolStripMenuItem.Enabled = false;
            }
        }
        private void bt_MouseMove(object sender, MouseEventArgs e)
        {
            CureentCt = ((Control)sender);
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
            if (CureentCt.Text.IndexOf("合") > 0)
            {
                分单ToolStripMenuItem.Text = "追加分单";
            }
            else
            {
                分单ToolStripMenuItem.Text = "分单";
            }
            IsMove = true;
        }
        private void wbt_Paint(object sender, PaintEventArgs e)
        {
            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
            if (bt.Tag != null)
            {
                if (txtCarNo.Text != string.Empty)
                {
                    if (bt.Tag.ToString().ToLower().EndsWith(txtCarNo.Text.ToLower()) && bt.Left == 1)
                    {
                        int borderWidth = 3;
                        Color borderColor = Color.Red;
                        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
                                            borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid);

                    }
                    else if(bt.Left == 1)
                    {
                        int borderWidth = 3;
                        Color borderColor = Color.Transparent;
                        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
                                            borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid);

                    }
                }
            }
            string info = bt.ImageKey;
            string[] Strs = info.Split(new Char[] { ',' });

            Font ft1 = new Font("宋体", 11, FontStyle.Regular);
            Font ft2 = new Font("宋体", 10, FontStyle.Regular);

            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString(Strs[2], ft1, 1000, sf);

            int xpoint = int.Parse(Convert.ToString(Math.Floor(bt.Width * 0.5)));
            int ypoint = int.Parse(Convert.ToString(Math.Floor(bt.Height * 0.5)));
            string isbook = "自";
            if (Convert.ToBoolean(Strs[7]))
            {
                isbook = "预";
            }
            if (Strs[9].IndexOf("迟到") > -1)
            {
                isbook = "迟";
            }
            e.Graphics.DrawString(Strs[2] +"  "+ isbook, ft1, Brushes.Black, 10, 5);  //车牌
            e.Graphics.DrawString("进场时间:" + DateTime.Parse(Strs[5]).ToString("MM-dd HH:mm"), ft2, Brushes.Black, 5, size.Height + 5);  //时间差
            if (Strs[1] == "延时到明天")
            {
                string delayreason = ClsBLL.DelayReason(int.Parse(bt.Name));
                e.Graphics.DrawString("中断:" + delayreason, ft2, Brushes.Black, 5, size.Height + 20); //维修类型
            }
            else
            {
                if (Strs[6] != string.Empty)
                {
                    e.Graphics.DrawString("计划完成:" + DateTime.Parse(Strs[6]).ToString("MM-dd HH:mm"), ft2, Brushes.Black, 5, size.Height + 20);  //时间差
                }
            }
        }
        private void wbt_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ClsBLL.IsPower("派工"))
            {
                MessageBox.Show("你没有权限派工！");
                ((Control)sender).Top = PcStartTop;
                ((Control)sender).Left = PcStartLeft;
                return;
            }
            if (IsMove)
            {
                decimal top = decimal.Parse(Convert.ToString(((Control)sender).Top - dataGridView1.ColumnHeadersHeight));
                decimal height = decimal.Parse(dataGridView1.Rows[0].Height.ToString());
                Rowindex = int.Parse(decimal.Round(top / height, 0).ToString());
                int left = ((Control)sender).Left;
                int startleft = dataGridView1.Columns[0].Width + dataGridView1.Columns[1].Width;
                Colindex = (left - startleft) / dataGridView1.Columns[2].Width;
                Colindex = Colindex + 2;
                if (top == 0 || left < dataGridView1.Columns[0].Width)
                {
                    ((Control)sender).Top = PcStartTop;
                    ((Control)sender).Left = PcStartLeft;
                    return;
                }
                if (Rowindex == -1)
                {
                    Rowindex = 0;
                }
                ((Control)sender).Top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
                ((Control)sender).Left = (Colindex - 2) * dataGridView1.Columns[2].Width + startleft;
                try
                {
                    if (Rowindex > -1)
                    {
                        if (((Control)sender).Name != string.Empty)
                        {
                            string sqlstring = string.Empty;
                            DoubleClickButton bt =(DoubleClickButton)((Control)sender);
                            //记录ID
                            int recordid = int.Parse(bt.Name);
                            string[] strinfo = bt.ImageKey.Split(new Char[] { ',' });
                            if (strinfo[1] == "延时到明天")
                            {
                                //延时到明天的单，第二天派工时确认追加工时，预计完成时间是当前时间加上追加工时
                                if (ClsBLL.GetFendanNum(recordid) > 1)
                                {    //如果有两张同时延时到明天的单，则进入分单界面。
                                    frmServiceItem fsi = new frmServiceItem(recordid);
                                    if (fsi.ShowDialog() == DialogResult.OK)
                                    {
                                        刷新ToolStripMenuItem_Click(null, null);
                                    }
                                    else
                                    {
                                        ((Control)sender).Top = PcStartTop;
                                        ((Control)sender).Left = PcStartLeft;
                                        return;
                                    }
                                }
                                else
                                {
                                    frmAddHour2Day frmaddhour = new frmAddHour2Day(recordid);
                                    if (frmaddhour.ShowDialog() == DialogResult.OK)
                                    {
                                        ClsBLL.AddMsg(recordid, "车牌号码:" + strinfo[2] + ",派工给" + ClsBLL.GetWorker(recordid));
                                        刷新ToolStripMenuItem_Click(null, null);
                                    }
                                    else
                                    {
                                        ((Control)sender).Top = PcStartTop;
                                        ((Control)sender).Left = PcStartLeft;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                //维修工人
                                string serviceworker = dataGridView1.Rows[Rowindex].Cells[1].Value.ToString();
                                sqlstring = "Update Booking Set AssignTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', Worker='" + serviceworker + "',State='等待开工' where ID=" + recordid;
                                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                                {
                                    ClsBLL.AddMsg(recordid, "车牌号码:" + strinfo[2] + ",派工给" + serviceworker);
                                    ClsBLL.ServicePauseStart(recordid);
                                    刷新ToolStripMenuItem_Click(null, null);
                                    bt.Tag = "等待开工";
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
            IsMove = false;
        }
        private void wbt_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);//
            PcStartTop = ((Control)sender).Top;
            PcStartLeft = ((Control)sender).Left;
        }
        private void wbt_MouseMove(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;//设置拖动时鼠标箭头
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
            IsMove = true;
        }
        private void wbt_DoubleClick(object sender, EventArgs e)
        {
            if (!ClsBLL.IsPower("分单")) return;
            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
            frmServiceItem fsi = new frmServiceItem(int.Parse(bt.Name));
            if (fsi.ShowDialog() == DialogResult.OK)
            {
                刷新ToolStripMenuItem_Click(null, null);
            }
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            index = -1;
            ClearContrl();
            ShowGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1[e.ColumnIndex, e.RowIndex].Selected = false;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            Button bt = new Button();
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString("车牌:粤ABCDEF  12°20' ", bt.Font, 1000, sf);
            sizewidth = int.Parse(size.Width.ToString());

            //画背景色
            e.Graphics.FillRectangle(new SolidBrush(dataGridView1.BackgroundColor), 2, dataGridView1.ColumnHeadersHeight+2, dataGridView1.Columns[0].Width-3, dataGridView1.Height - dataGridView1.ColumnHeadersHeight);

            for (int i = 1; i <= dataGridView1.RowCount; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(dataGridView1.Columns[0].Width, dataGridView1.ColumnHeadersHeight + dataGridView1.Rows[0].Height * i), new Point(dataGridView1.Width, dataGridView1.ColumnHeadersHeight + dataGridView1.Rows[0].Height * i));
            }
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(dataGridView1.Columns[0].Width, dataGridView1.ColumnHeadersHeight), new Point(dataGridView1.Columns[0].Width, dataGridView1.Height));

        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            index = -1;
            ClearContrl();
            ShowGrid();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 开工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (CureentCt == null) return;
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                string sqlstring = string.Empty;
                //记录ID
                int recordid = int.Parse(bt.Name);
                string[] strinfo = bt.ImageKey.Split(new Char[] { ',' });
                //维修工时
                decimal servicehours = ClsBLL.GetServiceHour(recordid);
                DateTime DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                sqlstring = "Update Booking Set StartServiceTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',PlanCompleteTime='" + DtPlanComplete + "',State='维修进行中' where ID=" + recordid;
                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    ClsBLL.AddMsg(recordid, "车牌号码:" + strinfo[2] + ",开始维修--" + ClsBLL.UserName);
                    刷新ToolStripMenuItem_Click(null, null);
                    bt.Tag = "维修进行中";
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void 中断ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            if (中断ToolStripMenuItem.Text == "中断")
            {
                frmPause fpr = new frmPause(recordid);
                if (fpr.ShowDialog() == DialogResult.OK)
                {
                    刷新ToolStripMenuItem_Click(null, null);
                }
            }
            else
            {
                //设置中断并开始的记录
                ClsBLL.ServicePauseStart(recordid);
                string[] strs = bt.ImageKey.Split(new Char[] { ',' });
                string startdate = strs[4];  //中断之前的开始时间
                string plancomplete = strs[5];  //预计完成时间
                DateTime PlanEndDate = DateTime.Parse(plancomplete).AddMinutes(ClsBLL.Pausemins(recordid, DateTime.Parse(startdate)));
                //新的预计完成时间
                string sqlstring = "Update Booking Set PlanCompleteTime='" + PlanEndDate + "',State='维修进行中' where ID=" + recordid;
                SQLDbHelper.ExecuteSql(sqlstring);
                ClsBLL.AddMsg(recordid, "车牌号码:" + strs[1] + ",中断后继续维修");
                刷新ToolStripMenuItem_Click(null, null);
            }
        }

        private void 完工ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            string[] strs = bt.ImageKey.Split(new Char[] { ',' });
            int recordid = int.Parse(bt.Name);
            string sqlstring = "Update booking set State='完工', EndServiceTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' where ID=" + recordid;
            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
            {
                ClsBLL.AddMsg(recordid, "车牌号码:" + strs[1] + ",完工");
                if (bt.Tag.ToString() == "中断" || bt.Tag.ToString() == "过时")
                {
                    ClsBLL.ServicePauseStart(recordid);//结束中断
                }
                sqlstring = @"Select Count(*) from Booking A,(Select CarNo,VIN,CreateDate From Booking Where ID="+ recordid +")B";
                sqlstring +=" Where A.CarNo=B.CarNo And A.VIN=B.VIN And A.CreateDate=B.CreateDate and A.State<>'完工'";
                int r = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                if(r>0)
                {
                    MessageBox.Show("还有"+ r.ToString() + "张分单未完工！");
                    ClsBLL.AddMsg(recordid, "车牌号码:" + bt.Tag.ToString() + "还有" + r.ToString() + "张分单未完工！" + ClsBLL.UserName);
                }
                bt.BackColor = Color.Blue;
                bt.Tag = "完工";
            }
        }

        private void 洗车ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            string[] strs = bt.ImageKey.Split(new Char[] { ',' });
            string sqlstring = @"Select Count(*) from Booking A,(Select CarNo,VIN,CreateDate From Booking Where ID=" + recordid + ")B";
            sqlstring += " Where A.CarNo=B.CarNo And A.VIN=B.VIN And A.CreateDate=B.CreateDate and A.State<>'完工'";
            int r = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
            if (r > 0)
            {
                MessageBox.Show("还有" + r.ToString() + "张分单未完工，不能洗车。");
                return;
            }
            sqlstring = "Update A set A.State='洗车'";
            sqlstring += " From Booking A,(Select CarNo,BookTime From Booking Where ID=" + recordid + ") B";
            sqlstring += " Where A.CarNo=B.CarNo And Substring(convert(nvarchar(50),A.booktime,120),1,10)=substring(convert(nvarchar(50),B.booktime,120),1,10)";
            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
            {
                ClsBLL.AddMsg(recordid, "车牌号码:" + strs[1] + ",洗车");
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Text == bt.Text)
                    {
                        dataGridView1.Controls.Remove(ct);
                    }
                }
            }
        }
        private void 洗车与送车时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            string sqlstring = @"Select Count(*) from Booking A,(Select CarNo,VIN,CreateDate From Booking Where ID=" + recordid + ")B";
            sqlstring += " Where A.CarNo=B.CarNo And A.VIN=B.VIN And A.CreateDate=B.CreateDate and A.State<>'完工'";
            int r = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
            if (r > 0)
            {
                MessageBox.Show("还有" + r.ToString() + "张分单未完工，不能洗车。");
                return;
            }
            frmParkSite fps = new frmParkSite(recordid, 1, bt.Text);
            fps.ShowDialog();
            bt.BackColor = Color.MediumOrchid;
            bt.Tag = "洗车";
            foreach (Control ct in dataGridView1.Controls)
            {
                if (ct.Name == bt.Name)
                {
                    dataGridView1.Controls.Remove(ct);
                }
            }
        }

        private void 延时到明天ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            frmDelayService fdt = new frmDelayService(recordid,bt);
            fdt.ShowDialog();
        }
        private void 追加工时ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            frmAddHourAddItem fdt = new frmAddHourAddItem(recordid);
            if (fdt.ShowDialog() == DialogResult.OK)
            {
                刷新ToolStripMenuItem_Click(null, null);
            }
        }

        private void 返修ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            frmAddHourReser fdt = new frmAddHourReser(recordid);
            if (fdt.ShowDialog() == DialogResult.OK)
            {
                刷新ToolStripMenuItem_Click(null, null);
            }
        }
        private void 分单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt == null) return;
            DoubleClickButton bt = (DoubleClickButton)CureentCt;
            int recordid = int.Parse(bt.Name);
            frmServiceItem fsi = new frmServiceItem(recordid);
            if (fsi.ShowDialog() == DialogResult.OK)
            {
                刷新ToolStripMenuItem_Click(null, null);
            }
        }
        private void 开工ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnStart.Enabled = 开工ToolStripMenuItem.Enabled;
        }
        private void 中断ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnPause.Enabled = 中断ToolStripMenuItem.Enabled;
        }

        private void 完工ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnComplete.Enabled = 完工ToolStripMenuItem.Enabled;
        }

        private void 洗车ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnClearCar.Enabled = 洗车ToolStripMenuItem.Enabled;
        }

        private void 中断ToolStripMenuItem_TextChanged(object sender, EventArgs e)
        {
            btnPause.Text = 中断ToolStripMenuItem.Text;
        }
        private void 追加工时ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnAddhour.Enabled = 追加工时ToolStripMenuItem.Enabled;
        }

        private void 延时ToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            btnDelay.Enabled = 延时ToolStripMenuItem.Enabled;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ClsBLL.CheckFormIsOpen("frmBooking"))
            {
                frmBooking fcsb = new frmBooking();
                fcsb.Show();
            }
            else
            {
                Form frm = Application.OpenForms["frmBooking"];
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
        private void picUp_Click(object sender, EventArgs e)
        {
            int sumheight = 0;
            foreach (Control ct in dataGridView1.Controls)
            {
                if (ct.Left == 1 && ct.Top>0)
                {
                    sumheight += ct.Height;
                }
            }
            if (sumheight > dataGridView1.Height - dataGridView1.Rows[1].Height)
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Left == 1)
                    {
                        ct.Top = ct.Top - ct.Height;
                        if (ct.Top < dataGridView1.ColumnHeadersHeight)
                        {
                            ct.Visible = false;
                        }
                        if (ct.Top < dataGridView1.Height && ct.Top > dataGridView1.ColumnHeadersHeight)
                        {
                            ct.Visible = true;
                        }
                    }
                }
            }
        }

        private void picDown_Click(object sender, EventArgs e)
        {
            int sumheight = 0;
            foreach (Control ct in dataGridView1.Controls)
            {
                if (ct.Left == 1 && ct.Top < dataGridView1.Height)
                {
                    sumheight += ct.Height;
                }
            }
            if (sumheight > dataGridView1.Height + dataGridView1.ColumnHeadersHeight)
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Left == 1)
                    {
                        ct.Top = ct.Top + ct.Height;
                        if (ct.Top > dataGridView1.Height)
                        {
                            ct.Visible = false;
                        }
                        if (ct.Top < dataGridView1.Height && ct.Top > dataGridView1.ColumnHeadersHeight - 10)
                        {
                            ct.Visible = true;
                        }
                    }
                }
            }
        }

        private void txtCarNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                FindCar();
            }
        }
        private void MoveCard(Control ct)
        {
            if (ct.Top > dataGridView1.ColumnHeadersHeight)
            {
                ct.Top = ct.Top - ct.Height;
                ct.Visible = true;
                MoveCard(ct);
            }
        }
        private void FindCar()
        {
            if (txtCarNo.Text != string.Empty)
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Tag != null)
                    {
                        if (ct.Tag.ToString().ToLower().IndexOf(txtCarNo.Text.ToLower())>-1 && ct.Left == 1)
                        {
                            if (ct.Top > dataGridView1.Height)
                            {
                                foreach (Control cts in dataGridView1.Controls)
                                {
                                    if (cts.Left == 1 && cts.Name != ct.Name)
                                    {
                                        cts.Top = cts.Top + cts.Height;
                                    }
                                }
                                MoveCard(ct);
                            }
                            ct.Focus();
                            dataGridView1.Refresh();
                        }
                    }
                }
            }
        }
        private void txtCarNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCarNo.Text.Length == 5)
            {
                FindCar();
            }
        }

        private void picUp_MouseDown(object sender, MouseEventArgs e)
        {
            timer2.Enabled = true;
        }

        private void picUp_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Enabled = false;
        }

        private void picDown_MouseDown(object sender, MouseEventArgs e)
        {
            timer3.Enabled = true;
        }

        private void picDown_MouseUp(object sender, MouseEventArgs e)
        {
            timer3.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


    }
}