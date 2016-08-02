using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmSelectBook : Form
    {
        public frmSelectBook(DateTime dt,int sizewidth)
        {
            InitializeComponent();
            SelectDt = dt;
            Sizewidth = sizewidth;
        }
        private int Colindex = -1;
        private int Rowindex = -1;
        private int[,] Cards = new int[23, 4];
        private DateTime SelectDt = DateTime.Today;
        private DateTime starttime = DateTime.Today;
        private DateTime endtime = DateTime.Today;
        private int Sizewidth = 50;
        private void frmSelectBook_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.WorkingArea.Width * 8/10;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height * 8/10;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width / 10;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height * 17 / 100;
            this.Text = SelectDt.ToString("yyyy年MM月dd日") + "预约情况";

            starttime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));
            endtime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet2"));
            TimeSpan ts = endtime.Subtract(starttime);
            int cols = ts.Hours * 2;
            if (ts.Minutes > 0) cols++;
            dataGridView1.Rows.Add(cols + 1);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DateTime NewDt = starttime.AddMinutes(i * 30);
                dataGridView1.Rows[i].Cells[0].Value = NewDt.Hour.ToString() + ":" + NewDt.Minute.ToString().PadRight(2, char.Parse("0"));
                dataGridView1.Rows[i].Height = dataGridView1.Height / dataGridView1.Rows.Count;
            }
            dataGridView1.Columns[1].Width = dataGridView1.Width * 6 / 10;
            ShowGrid1(SelectDt);
        }
        private void DataGridView_Paint(object sender, PaintEventArgs e)
        {
            //DataGridView dgv = (DataGridView)sender;
            //e.Graphics.FillRectangle(new SolidBrush(Color.Gray), 1, 1, dgv.Width - 2, dgv.ColumnHeadersHeight - 1);
            //for (int i = 0; i < dgv.Columns.Count; i++)
            //{
            //    string headertxt = dgv.Columns[i].HeaderText;
            //    Font ft = new Font("幼圆", 15, FontStyle.Regular);

            //    StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            //    SizeF size = e.Graphics.MeasureString(headertxt, ft, 1000, sf);
            //    int left = 0;
            //    for (int j = 0; j < i; j++)
            //    {
            //        left += dgv.Columns[j].Width;
            //    }
            //    e.Graphics.DrawString(headertxt, ft, Brushes.White, left + (dgv.Columns[i].Width - size.Width) / 2, 2);
            //}
        }
        private void ShowGrid1(DateTime Dt)
        {
            Cards = new int[dataGridView1.Rows.Count, 3];
            string sqlstring = "Select * from Booking where Booktime between '" + Dt.ToShortDateString() + "' and '" + Dt.AddDays(1).ToShortDateString() + "'";
            DataTable DataBook = SQLDbHelper.Query(sqlstring).Tables[0];
            //显示所有预约列表
            foreach (DataRow dr in DataBook.Rows)
            {
                DateTime TempT = DateTime.Parse(dr["BookTime"].ToString());
                int hour = TempT.Hour;
                int min = TempT.Minute;
                Rowindex = (hour - starttime.Hour) * 2;
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
        }
        private void AddCarCard(string ID, string bookindex, string state, string detail, string carno, DateTime booktime)
        {
            DoubleClickButton bt = new DoubleClickButton();

            bt.Height = dataGridView1.Rows[0].Height;
            bt.Width = Sizewidth + 20;
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

            int left = 0;
            int top = Rowindex * dataGridView1.Rows[0].Height + dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < Colindex; i++)
            {
                left += dataGridView1.Columns[i].Width;
            }

            bt.Top = top;
            bt.Left = left + Cards[Rowindex, Colindex] * bt.Width;  //已经存在cards的数量
            bt.TextAlign = ContentAlignment.MiddleLeft;
            bt.Text = "NO. " + bookindex + "\n" + "车牌：" + carno;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(bt, detail);

            dataGridView1.Controls.Add(bt);
            Cards[Rowindex, Colindex] = Cards[Rowindex, Colindex] + 1;
            dataGridView1.ClearSelection();
        }

        private void frmSelectBook_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}