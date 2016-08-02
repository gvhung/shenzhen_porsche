using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmCarStateBoad : Form
    {
        public frmCarStateBoad()
        {
            InitializeComponent();
        }
        private Point mouse_offset;

        private int Colindex = -1;
        private int PcStartTop = 0;
        private int PcStartLeft = 0;
        private int[] Cards = new int[5];
        private int index = 0;
        string[] States = new string[5] { "已派工", "维修进行中", "中断", "完工", "洗车" };
        DataTable RoDt = new DataTable();
        private Control CureentCt;
        private void frmCarStateBoad_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            labDateTime.Text = DateTime.Today.ToString("yyyy年MM月dd日");
            ClsBLL.IniCombox(comboBox1, "中断原因");
            dataGridView1.Rows.Add((dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.RowTemplate.Height);

            string sqlstring = "Select Items from SysDictionary where ItemName='SA'";
            DataTable Dt2 = SQLDbHelper.Query(sqlstring).Tables[0];
            cmbReceiver.Items.Add("--全部--");
            foreach (DataRow dr in Dt2.Rows)
            {
                cmbReceiver.Items.Add(dr[0].ToString());
            }
            ShowGrid(string.Empty);
            //dataGridView1.BackgroundColor = Color.PaleTurquoise;
            dataGridView1.ClearSelection();

            int Interval = int.Parse(ClsBLL.GetSet("txtSet5"));
            timer1.Interval = Interval * 1000 * 60;
            timer1.Enabled = true;

            if (!ClsBLL.IsPower(btnToday.Text))
            {
                btnToday.Enabled = false;
            }
            if (!ClsBLL.IsPower(btnMonth.Text))
            {
                btnMonth.Enabled = false;
            }
            pn0.Left = (dataGridView1.Columns[0].Width - pn0.Width) / 2;
            pn1.Left = (dataGridView1.Columns[1].Width - pn1.Width) / 2 + dataGridView1.Columns[0].Width * 1;
            pn2.Left = (dataGridView1.Columns[2].Width - pn2.Width) / 2 + dataGridView1.Columns[0].Width * 2;
            pn3.Left = (dataGridView1.Columns[3].Width - pn3.Width) / 2 + dataGridView1.Columns[0].Width * 3;
            pn4.Left = (dataGridView1.Columns[4].Width - pn4.Width) / 2 + dataGridView1.Columns[0].Width * 4;
            pn0.Controls.Add(pic0Down);
            pic0Down.Top=pic0Up.Top;

            if (!ClsBLL.IsPower("追加项目"))
            {
                追加项目ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("返修"))
            {
                返修ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("修改状态"))
            {
                修改状态ToolStripMenuItem.Visible = false;
            }
        }
        private void ShowGrid(string PauseReason)
        {
            string wherestrr = "1=1";
            if (cmbReceiver.Text != "--全部--" && cmbReceiver.Text != string.Empty)
            {
                wherestrr = " Receiver='" + cmbReceiver.Text + "'";
            }
            string sqlstring = "Select * from Booking Where "+ wherestrr +" And (State='正式' Or State='延时到明天' OR StartServiceTime between '" + DateTime.Today.ToString() + "' and '" + DateTime.Today.AddDays(1).ToShortDateString() + "')";
            sqlstring += " Union all Select * from Booking Where " + wherestrr + " And (State='维修进行中' Or State='中断' Or State='过时') And StartServiceTime between '" + DateTime.Today.AddDays(-7).ToShortDateString() + "' and '" + DateTime.Today.ToShortDateString() + "'";
            sqlstring += " Union all Select * from Booking Where " + wherestrr + " And State='完工' And EndServiceTime between '" + DateTime.Today.AddDays(-1).ToShortDateString() + "' and '" + DateTime.Today.ToShortDateString() + "'";

            DataTable RoDt1 = SQLDbHelper.Query(sqlstring).Tables[0];
            RoDt1.DefaultView.Sort = "StartServiceTime Desc";
            RoDt = RoDt1.DefaultView.Table;
            foreach (DataRow Dr in RoDt.Rows)
            {
                string time = string.Empty;
                switch (Dr["State"].ToString())
                {
                    case "正式":
                    case "延时到明天":
                        Colindex = 0;
                        time = Dr["ComeTime"].ToString();
                        break;
                    case "维修进行中":
                    case "过时":
                        Colindex = 1;
                        time = Dr["StartServiceTime"].ToString();
                        break;
                    case "中断":
                        Colindex = 2;
                        time = ClsBLL.PauseTime(int.Parse(Dr["ID"].ToString())).ToString();
                        break;
                    case "完工":
                        Colindex = 3;
                        time = Dr["EndServiceTime"].ToString();
                        break;
                    case "洗车":
                        Colindex = 4;
                        time = Dr["ClearCarTime"].ToString();
                        break;
                    default:
                        break;
                }
                try
                {
                    string Time = string.Empty;
                    string Spantime = string.Empty;
                    if (time == string.Empty && Colindex != 4)
                    {
                        time = DateTime.Now.ToString();
                    }
                    if (time != string.Empty)
                    {
                        TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(time));
                        Spantime = ts.Hours.ToString() + "°" + ts.Minutes.ToString() + "'";
                        Time = DateTime.Parse(time).ToString("MM-dd HH:mm");
                    }
                    //在等待列判断中断原因，不显示等待时间
                    if (Dr["State"].ToString() == "正式" || Dr["State"].ToString() == "延时到明天")
                    {
                        string delayreason = ClsBLL.DelayReason(int.Parse(Dr["ID"].ToString()));
                        if (delayreason != string.Empty)
                        {
                            Spantime = delayreason;
                        }
                    }
                    //列序号，状态，车牌，维修类型，（进场时间/开始工作时间/中断时间/完工时间/洗车时间）,时间差，停车位，计划完成时间
                    string info = Colindex.ToString() + "," + Dr["State"].ToString() + "," + Dr["CarNo"].ToString() + "," + Dr["ServiceType"].ToString() + "," + Time + "," + Spantime + "," + Dr["ParkSite"].ToString() + "," + Dr["PlanOutTime"].ToString() + "," + Dr["Receiver"].ToString() + "," + Dr["Worker"].ToString() + "," + Dr["Remark"].ToString();
                    if (Dr["State"].ToString() == "正式" || Dr["State"].ToString() == "延时到明天")
                    {
                        if (PauseReason != string.Empty && PauseReason != Spantime)
                        {

                        }
                        else
                        {
                            AddCarCard(Dr["ID"].ToString(), info);
                        }
                    }
                    else
                    {
                        if (Dr["State"].ToString() == "中断" && PauseReason != string.Empty)
                        {
                            if (ClsBLL.PauseReason(int.Parse(Dr["ID"].ToString())) == PauseReason)
                            {
                                AddCarCard(Dr["ID"].ToString(), info);
                            }
                        }
                        else
                        {
                            AddCarCard(Dr["ID"].ToString(), info);
                        }
                    }
                }
                catch (Exception Err)
                {
                    MessageBox.Show(Err.Message);
                }
            }
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].HeaderText.IndexOf("[") > 0)
                {
                    string header = dataGridView1.Columns[i].HeaderText;
                    header = header.Substring(0, header.IndexOf("["));
                    dataGridView1.Columns[i].HeaderText = header + "[" + Cards[i].ToString() + "]";
                }
                else
                {
                    dataGridView1.Columns[i].HeaderText = dataGridView1.Columns[i].HeaderText + "[" + Cards[i].ToString() + "]";
                }
            }
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
        //添加车辆预约卡片
        private void AddCarCard(string ID, string info)
        {
            DoubleClickButton bt = new DoubleClickButton();
            string[] strs = info.Split(new Char[] { ',' });
            bt.Height = dataGridView1.RowTemplate.Height;
            bt.Width = dataGridView1.Columns[0].Width-10;
            bt.Tag = strs[2];

            bt.Name = ID;
            bt.ImageKey = info;
            int left = 0;
            int top = Cards[Colindex] * dataGridView1.RowTemplate.Height + dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < Colindex; i++)
            {
                left += dataGridView1.Columns[i].Width;
            }

            foreach (Control ct in dataGridView1.Controls)
            {   //同车牌 并且状态相同，在此界面只显示一次
                if (ct.Tag != null)
                {
                    if (ct.Tag.ToString() == bt.Tag.ToString() && ct.Name != bt.Name)
                    {
                        DoubleClickButton cbt = (DoubleClickButton)((Control)ct);
                        string[] tempstrs = cbt.ImageKey.Split(new Char[] { ',' });
                        if (tempstrs[1] == strs[1])
                        {
                            return;
                        }
                    }
                }
            }
            bt.ContextMenuStrip = contextMenuStrip2;
            dataGridView1.Controls.Add(bt);
            bt.Top = top;
            bt.Left = left + 5; //+ 50;  //已经存在cards的数量

            Color cl = Color.PowderBlue;
            if (Colindex == 0)
            {
                cl = Color.Yellow;
                bt.MouseWheel += new MouseEventHandler(bt_MouseWheel);
            }
            if (Colindex == 1)
            {
                cl = Color.Green;
                if (strs[10].IndexOf("返修") > -1)
                {
                    cl = Color.MediumSeaGreen;
                }
                if (strs[10].IndexOf("追加项目") > -1)
                {
                    cl = Color.OliveDrab;
                }
            }
            if (Colindex == 2)
            {
                cl = Color.LightGray;
            }
            if (Colindex == 3)
            {
                cl = Color.Blue;
            }
            if (Colindex == 4)
            {
                cl = Color.Orange;
                if (strs[4]==string.Empty)
                {
                    cl = Color.MediumOrchid;
                }
            }
            if (Colindex == 1)
            {
                if (DateTime.Parse(DateTime.Today.Year.ToString() + "-" + strs[4]).CompareTo(DateTime.Today) < 0)
                {
                    cl = Color.Wheat;
                }
            }
            bt.BackColor = cl;
            bt.Paint += new PaintEventHandler(bt_Paint); 
            bt.DoubleClick +=new EventHandler(bt_DoubleClick);
            bt.MouseEnter +=new EventHandler(bt_MouseEnter);
            
            Cards[Colindex] = Cards[Colindex] + 1;
        }
        private void bt_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                pic0Up_Click(pic0Up, null);
            }
            else
            {
                pic0Down_Click(pic0Down, null);
            }
        }
        private void bt_MouseEnter(object sender, EventArgs e)
        {
            CureentCt = ((Control)sender);
        }
        private void bt_DoubleClick(object sender, EventArgs e)
        {
            if(!ClsBLL.IsPower("查看维修信息"))
            {
                MessageBox.Show("你没有权限查看维修信息！");
                return;
            }
            DoubleClickButton bt = (DoubleClickButton)((Control)sender);
            frmBookLook fbn = new frmBookLook(int.Parse(bt.Name));
            fbn.ShowDialog();
        }
        private void bt_Paint(object sender, PaintEventArgs e)
        {
            Button bt = (Button)((Control)sender);
            if (bt.Tag != null)
            {
                if (txtCarNo.Text != string.Empty)
                {
                    if (bt.Tag.ToString().ToLower().EndsWith(txtCarNo.Text.ToLower()))
                    {
                        int borderWidth = 3;
                        Color borderColor = Color.Red;
                        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
                                            borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid);

                    }
                }
            }

            string info = bt.ImageKey;
            string[] Strs = info.Split(new Char[] { ',' });
            int NewColindex = int.Parse(Strs[0]);

            Font ft1 = new Font("宋体", 14, FontStyle.Regular);
            Font ft2 = new Font("宋体", 11, FontStyle.Regular);

            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString(Strs[2], ft1, 1000, sf);

            int xpoint = int.Parse(Convert.ToString(Math.Floor(bt.Width * 0.5)));
            int ypoint = int.Parse(Convert.ToString(Math.Floor(bt.Height * 0.6)));

            Color cl = Color.PowderBlue;
            //列序号，状态，车牌，维修类型，（进场时间/开始工作时间/中断时间/完工时间/洗车时间）,时间差，停车位，计划完成时间，接待人，维修师傅
            //画字符
            e.Graphics.DrawString(Strs[2], ft1, Brushes.Black, 2, 2);  //车牌
            int xleft = bt.Width * 4 / 10 + 5;
            string Time = string.Empty;
            if (Strs[7] != string.Empty)
            {
                Time = DateTime.Parse(Strs[7]).ToString("MM-dd HH:mm");
            }
            if (NewColindex == 0)
            {
                e.Graphics.DrawString("SA:" + Strs[8], ft2, Brushes.Black, 5, size.Height + 3); //维修类型
                e.Graphics.DrawString("进场时间:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //时间
                if (Strs[1] == "延时到明天")  //延时到明天的中断原因
                {
                    string delayreason = ClsBLL.DelayReason(int.Parse(bt.Name));
                    e.Graphics.DrawString("中断:" + delayreason, ft2, Brushes.Black, xleft, 5);  //计划完成时间
                }
                else
                {
                    e.Graphics.DrawString("计划完成:" + Time, ft2, Brushes.Black, xleft, 5);  //计划完成时间
                }
            }
            if (NewColindex == 1)
            {
                e.Graphics.DrawString("维修:"+Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //维修类型
                e.Graphics.DrawString("开始工作:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //时间
                e.Graphics.DrawString("计划完成:" + Time, ft2, Brushes.Black, xleft, 5);  //时间
            }
            if(NewColindex == 2)
            {
                e.Graphics.DrawString("维修:" + Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //维修类型
                e.Graphics.DrawString("中断:" + ClsBLL.PauseReason(int.Parse(bt.Name)), ft2, Brushes.Black, xleft, 5);
                e.Graphics.DrawString("中断时间:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //时间
            }
            if (NewColindex == 3)
            {
                e.Graphics.DrawString("维修:" + Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //维修类型
                e.Graphics.DrawString("完工时间:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //时间
                e.Graphics.DrawString("计划完成:" + Time, ft2, Brushes.Black, xleft, 5);  //时间
            }
            if (NewColindex == 4)
            {
                e.Graphics.DrawString("SA:"+Strs[8], ft2, Brushes.Black, 5, size.Height + 3); //维修类型
                e.Graphics.DrawString("停车位:" + Strs[6], ft2, Brushes.Black, xleft, 5); //停车位
                if (Strs[4] !=string.Empty)
                {
                    e.Graphics.DrawString("送车时间:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //时间
                }
                else
                {
                    e.Graphics.DrawString("送车时间:", ft2, Brushes.Black, xleft, size.Height + 3);  //时间
                }
            }
            //画线
            e.Graphics.DrawLine(new Pen(Color.SkyBlue, 3), new Point(xleft-5, bt.Height - 5), new Point(xleft-5, 5));
         }
        #region 暂时不用
        private void bt_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);//
            PcStartTop = ((Control)sender).Top;
            PcStartLeft = ((Control)sender).Left;
        }
        private void bt_MouseMove(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;//设置拖动时鼠标箭头
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
        }
        #endregion
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            刷新ToolStripMenuItem_Click(null, null);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1[e.ColumnIndex, e.RowIndex].Selected = false;
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {            
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(dataGridView1.Columns[0].Width * i+2, dataGridView1.ColumnHeadersHeight), new Point(dataGridView1.Columns[0].Width * i+2, dataGridView1.Height));
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            index = 0;
            Cards = new int[5];
            ClearContrl();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add((dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.RowTemplate.Height);
            ShowGrid(string.Empty);
        }

        private void btnToday_Click(object sender, EventArgs e)
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

        private void btnMonth_Click(object sender, EventArgs e)
        {
            if (!ClsBLL.CheckFormIsOpen("frmWork"))
            {
                frmWork fcsb = new frmWork();
                fcsb.Show();
            }
            else
            {
                Form frm = Application.OpenForms["frmWork"];
                frm.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pic0Up_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            string pbname = pb.Name;

            int sumheight = 0;
            if (pbname.StartsWith("pic"))
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Left / dataGridView1.Columns[0].Width == int.Parse(pbname.Substring(0, 4).Substring(3, 1)))
                    {
                        if (ct.Top > 0)
                        {
                            sumheight += ct.Height;
                        }
                    }
                }
            }
            if (sumheight > dataGridView1.Height)
            {
                if (pbname.StartsWith("pic"))
                {
                    foreach (Control ct in dataGridView1.Controls)
                    {
                        if (ct.Left / dataGridView1.Columns[0].Width == int.Parse(pbname.Substring(0, 4).Substring(3, 1)))
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
        }

        private void pic0Down_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            string pbname = pb.Name;
            int sumheight = 0;
            if (pbname.StartsWith("pic"))
            {
                foreach (Control ct in dataGridView1.Controls)
                {
                    if (ct.Left / dataGridView1.Columns[0].Width == int.Parse(pbname.Substring(0, 4).Substring(3, 1)))
                    {
                        if (ct.Top < dataGridView1.ColumnHeadersHeight)
                        {
                            sumheight += ct.Height;
                        }
                    }
                }
            }
            if (sumheight > dataGridView1.ColumnHeadersHeight+dataGridView1.Rows[1].Height)
            {
                if (pbname.StartsWith("pic"))
                {
                    foreach (Control ct in dataGridView1.Controls)
                    {
                        if (ct.Left / dataGridView1.Columns[0].Width == int.Parse(pbname.Substring(0, 4).Substring(3, 1)))
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
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pic0Down_Click(pic0Up, null);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pic0Up_Click(pic0Down, null);
        }

        private void pic0Up_MouseDown(object sender, MouseEventArgs e)
        {
            timer2.Enabled = true;
        }

        private void pic0Up_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Enabled = false;
        }

        private void pic0Down_MouseDown(object sender, MouseEventArgs e)
        {
            timer3.Enabled = true;
        }

        private void pic0Down_MouseUp(object sender, MouseEventArgs e)
        {
            timer3.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pn0.Controls.Count > 0)
            {
                if (!comboBox1.Text.StartsWith("System"))
                {
                    index = -1;
                    Cards = new int[5];
                    ClearContrl();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add((dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.RowTemplate.Height);
                    ShowGrid(comboBox1.Text);
                }
            }
        }

        private void 追加项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                int recordid = int.Parse(CureentCt.Name);
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                string[] strs =bt.ImageKey.Split(new Char[] { ',' });
                if (strs.Length > 1)
                {
                    if (strs[1] == "完工" || strs[1] == "洗车")
                    {
                        frmAddHourAddItem fdt = new frmAddHourAddItem(recordid);
                        if (fdt.ShowDialog() == DialogResult.OK)
                        {
                            刷新ToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("只能在“完工”和“洗车”状态才能追加项目！");
                    }
                }
            }
        }
        private void 返修ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                int recordid = int.Parse(CureentCt.Name);
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                string[] strs = bt.ImageKey.Split(new Char[] { ',' });
                if (strs.Length > 1)
                {
                    if (strs[1] == "完工" || strs[1] == "洗车")
                    {
                        frmAddHourReser fdt = new frmAddHourReser(recordid);
                        if (fdt.ShowDialog() == DialogResult.OK)
                        {
                            刷新ToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("只能在“完工”和“洗车”状态才能返修！");
                    }
                }
            }
        }
        private void 修改状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (MessageBox.Show("你确定要修改状态吗？", "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                int recordid = int.Parse(CureentCt.Name);
                frmChangeState fcs = new frmChangeState(recordid);
                if (fcs.ShowDialog() == DialogResult.OK)
                {
                    刷新ToolStripMenuItem_Click(null, null);
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
        private void FindCar()
        {
            if (txtCarNo.Text != string.Empty)
            {
                foreach (Control ct in dataGridView1.Controls)
                {   //同车牌 并且状态相同，在此界面只显示一次
                    if (ct.Tag != null)
                    {
                        if (ct.Tag.ToString().IndexOf(txtCarNo.Text)>-1)
                        {
                            MoveCard(ct);
                            ct.Focus();
                            dataGridView1.Refresh();
                        }
                    }
                }
            }
        }
        private void MoveCard(Control ct)
        {
            if (ct.Top > dataGridView1.Height)
            {
                ct.Top = ct.Top - ct.Height;
                ct.Visible = true;
                MoveCard(ct);
            }
        }

        private void BtPaint()
        {

        }

        private void cmbReceiver_TextChanged(object sender, EventArgs e)
        {
            if (cmbReceiver.Text != string.Empty)
            {
                刷新ToolStripMenuItem_Click(null,null);
            }
        }
    }
}