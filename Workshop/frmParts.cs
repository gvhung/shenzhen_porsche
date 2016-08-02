using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmParts : Form
    {
        public frmParts()
        {
            InitializeComponent();
            initialStyle = this.FormBorderStyle;
        }
        private FormBorderStyle initialStyle;
        private int Rowindex = -1;

        private DateTime starttime = DateTime.Today;
        private DateTime endtime = DateTime.Today;
        private DataTable WorkHours = new DataTable();
        private int sizewidth = 100;
        private int[,] Cards;//
        private string UpdateDetail = string.Empty;
        private int index = 0;
        private void frmParts_Load(object sender, EventArgs e)
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
                dataGridView1.Rows.Add(cols + 1);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DateTime NewDt = starttime.AddMinutes(i*30);
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
            if (!ClsBLL.IsPower(btnWork.Text))
            {
                btnWork.Enabled = false;
            }
            if (!ClsBLL.IsPower(btnCarState.Text))
            {
                btnCarState.Enabled = false;
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
                frmBookNew fb = new frmBookNew(ID,true);
                if (fb.ShowDialog() == DialogResult.OK)
                {
                    刷新ToolStripMenuItem_Click(null, null);
                }
            }
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
            sb.AppendLine("零件准备:" + dr["PreParts"].ToString());

            if (DateTime.Parse(label1.Text).ToString("yyyy-MM-dd") != booktime.ToString("yyyy-MM-dd"))
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
            bt.Width = sizewidth+10;
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
            string Sa = dr["PreSA"].ToString();
            if (bt.BackColor == Color.Blue)
            {
                Sa = dr["Receiver"].ToString(); 
            }
            string bookstr=string.Empty;
            if (dr["PreParts"].ToString() == "零件已备")
            {
                bookstr = "备";
            }
            bt.Text = Sa + "/" + dr["LinkMan"].ToString() + "-" + dr["CarNo"].ToString() + "  " + bookstr;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(bt, sb.ToString());

            bt.DoubleClick += new EventHandler(bt_Click);
            dataGridView1.Controls.Add(bt);
            Cards[Rowindex, tempcol] = Cards[Rowindex, tempcol] + 1;
            dataGridView1.ClearSelection();
        }
        private void ShowGrid1(DateTime Dt)
        {
            label1.Text = Dt.ToString("yyyy年MM月dd日");
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
                    CheckRowindex(TempT);
                    AddCarCard(dr);
                }
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
            Rowindex = (hour - starttime.Hour) * 2 ;
            if (min == 30)
            {
                Rowindex++;
                if (hour == endtime.Hour && endtime.Minute == 0)
                {
                    Rowindex--;
                }
            }
        }
        private void ShowGrid2(int month)
        {
            dataGridView2.Rows.Clear();
            //dataGridView2.Rows.Add(5);
            dataGridView2.Rows.Add(6);
            dataGridView2.RowTemplate.Height = 140;
            DateTime DateM = new DateTime(int.Parse(nUDYear.Value.ToString()), month, 1);

            string sqlstring = "Select * from WorkHours where DateIndex between '" + DateM + "' and '" + DateM.AddMonths(1) + "'";
            WorkHours = SQLDbHelper.Query(sqlstring).Tables[0];

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
                if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
                {
                    dataGridView1.Visible = true;
                    dataGridView2.Visible = false;
                    panDate.Visible = false;
                    panCar.Visible = true;
                    dataGridView1.BringToFront();
                    dataGridView1.Top = panel1.Height + panel4.Height;
                    dataGridView1.Height = this.Height - panel1.Height - panel4.Height;

                    ShowGrid1(DateTime.Parse(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString()));
                }
            }
        }
        private void btnToday_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            panDate.Visible = false;
            panCar.Visible = true;
            dataGridView1.BringToFront();
            dataGridView1.Top = panel1.Height + panel4.Height;
            dataGridView1.Height = this.Height - panel1.Height - panel4.Height;
            ShowGrid1(DateTime.Today);
        }
        private void btnMonth_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            panDate.Visible = true;
            panCar.Visible = false;
            panDate.Left = panCar.Left;
            dataGridView2.Top = panel1.Height + panel4.Height;
            dataGridView2.Height = this.Height - panel1.Height - panel4.Height;
            nUDYear.Value = DateTime.Today.Year;
            nUDMonth.Value = DateTime.Today.Month;
            ShowGrid2(DateTime.Today.Month);
        }
      
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void nUDMonth_ValueChanged(object sender, EventArgs e)
        {
            if (nUDMonth.Visible)
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;
                dataGridView2.Top = panel1.Height + panel4.Height;
                dataGridView2.Height = this.Height - panel1.Height - panel4.Height;
                ShowGrid2(int.Parse(nUDMonth.Value.ToString()));
            }
        }
        private void txtCarNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (ClsBLL.CarBookRecord(txtCarNo.Text))
                {
                    frmBookLook fbn = new frmBookLook(txtCarNo.Text);
                    fbn.Show();
                }
                else
                {
                    MessageBox.Show("没有车牌号为“"+ txtCarNo.Text + "”预约信息！");
                }
            }
        }

        private void frmParts_Paint(object sender, PaintEventArgs e)
        {
            Button bt = new Button();
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString("张柏芝/陈冠希-粤A.BCDEF 备", bt.Font, 8000, sf);
            sizewidth =int.Parse(size.Width.ToString());
        }

        private void btnCarNo_Click(object sender, EventArgs e)
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

        private void btnCarState_Click(object sender, EventArgs e)
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
            if (dataGridView1.Visible)
            {
                ShowGrid1(DateTime.Parse(label1.Text));
            }
            else
            {
                ShowGrid2(DateTime.Today.Month);
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

        private void button1_Click(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            frmSelectBook fsb = new frmSelectBook(dateTimePicker3.Value, sizewidth);
            fsb.Show();
        }

        private void panCar_VisibleChanged(object sender, EventArgs e)
        {
            panLook.Visible = panCar.Visible;
            panLook.Top = panCar.Top;
        }
    }
}
