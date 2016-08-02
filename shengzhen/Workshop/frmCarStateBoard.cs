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
        string[] States = new string[5] { "���ɹ�", "ά�޽�����", "�ж�", "�깤", "ϴ��" };
        DataTable RoDt = new DataTable();
        private Control CureentCt;
        private void frmCarStateBoad_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            labDateTime.Text = DateTime.Today.ToString("yyyy��MM��dd��");
            ClsBLL.IniCombox(comboBox1, "�ж�ԭ��");
            dataGridView1.Rows.Add((dataGridView1.Height - dataGridView1.ColumnHeadersHeight) / dataGridView1.RowTemplate.Height);

            string sqlstring = "Select Items from SysDictionary where ItemName='SA'";
            DataTable Dt2 = SQLDbHelper.Query(sqlstring).Tables[0];
            cmbReceiver.Items.Add("--ȫ��--");
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

            if (!ClsBLL.IsPower("׷����Ŀ"))
            {
                ׷����ĿToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("����"))
            {
                ����ToolStripMenuItem.Visible = false;
            }
            if (!ClsBLL.IsPower("�޸�״̬"))
            {
                �޸�״̬ToolStripMenuItem.Visible = false;
            }
        }
        private void ShowGrid(string PauseReason)
        {
            string wherestrr = "1=1";
            if (cmbReceiver.Text != "--ȫ��--" && cmbReceiver.Text != string.Empty)
            {
                wherestrr = " Receiver='" + cmbReceiver.Text + "'";
            }
            string sqlstring = "Select * from Booking Where "+ wherestrr +" And (State='��ʽ' Or State='��ʱ������' OR StartServiceTime between '" + DateTime.Today.ToString() + "' and '" + DateTime.Today.AddDays(1).ToShortDateString() + "')";
            sqlstring += " Union all Select * from Booking Where " + wherestrr + " And (State='ά�޽�����' Or State='�ж�' Or State='��ʱ') And StartServiceTime between '" + DateTime.Today.AddDays(-7).ToShortDateString() + "' and '" + DateTime.Today.ToShortDateString() + "'";
            sqlstring += " Union all Select * from Booking Where " + wherestrr + " And State='�깤' And EndServiceTime between '" + DateTime.Today.AddDays(-1).ToShortDateString() + "' and '" + DateTime.Today.ToShortDateString() + "'";

            DataTable RoDt1 = SQLDbHelper.Query(sqlstring).Tables[0];
            RoDt1.DefaultView.Sort = "StartServiceTime Desc";
            RoDt = RoDt1.DefaultView.Table;
            foreach (DataRow Dr in RoDt.Rows)
            {
                string time = string.Empty;
                switch (Dr["State"].ToString())
                {
                    case "��ʽ":
                    case "��ʱ������":
                        Colindex = 0;
                        time = Dr["ComeTime"].ToString();
                        break;
                    case "ά�޽�����":
                    case "��ʱ":
                        Colindex = 1;
                        time = Dr["StartServiceTime"].ToString();
                        break;
                    case "�ж�":
                        Colindex = 2;
                        time = ClsBLL.PauseTime(int.Parse(Dr["ID"].ToString())).ToString();
                        break;
                    case "�깤":
                        Colindex = 3;
                        time = Dr["EndServiceTime"].ToString();
                        break;
                    case "ϴ��":
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
                        Spantime = ts.Hours.ToString() + "��" + ts.Minutes.ToString() + "'";
                        Time = DateTime.Parse(time).ToString("MM-dd HH:mm");
                    }
                    //�ڵȴ����ж��ж�ԭ�򣬲���ʾ�ȴ�ʱ��
                    if (Dr["State"].ToString() == "��ʽ" || Dr["State"].ToString() == "��ʱ������")
                    {
                        string delayreason = ClsBLL.DelayReason(int.Parse(Dr["ID"].ToString()));
                        if (delayreason != string.Empty)
                        {
                            Spantime = delayreason;
                        }
                    }
                    //����ţ�״̬�����ƣ�ά�����ͣ�������ʱ��/��ʼ����ʱ��/�ж�ʱ��/�깤ʱ��/ϴ��ʱ�䣩,ʱ��ͣ��λ���ƻ����ʱ��
                    string info = Colindex.ToString() + "," + Dr["State"].ToString() + "," + Dr["CarNo"].ToString() + "," + Dr["ServiceType"].ToString() + "," + Time + "," + Spantime + "," + Dr["ParkSite"].ToString() + "," + Dr["PlanOutTime"].ToString() + "," + Dr["Receiver"].ToString() + "," + Dr["Worker"].ToString() + "," + Dr["Remark"].ToString();
                    if (Dr["State"].ToString() == "��ʽ" || Dr["State"].ToString() == "��ʱ������")
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
                        if (Dr["State"].ToString() == "�ж�" && PauseReason != string.Empty)
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
        //��ӳ���ԤԼ��Ƭ
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
            {   //ͬ���� ����״̬��ͬ���ڴ˽���ֻ��ʾһ��
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
            bt.Left = left + 5; //+ 50;  //�Ѿ�����cards������

            Color cl = Color.PowderBlue;
            if (Colindex == 0)
            {
                cl = Color.Yellow;
                bt.MouseWheel += new MouseEventHandler(bt_MouseWheel);
            }
            if (Colindex == 1)
            {
                cl = Color.Green;
                if (strs[10].IndexOf("����") > -1)
                {
                    cl = Color.MediumSeaGreen;
                }
                if (strs[10].IndexOf("׷����Ŀ") > -1)
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
            if(!ClsBLL.IsPower("�鿴ά����Ϣ"))
            {
                MessageBox.Show("��û��Ȩ�޲鿴ά����Ϣ��");
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

            Font ft1 = new Font("����", 14, FontStyle.Regular);
            Font ft2 = new Font("����", 11, FontStyle.Regular);

            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            SizeF size = e.Graphics.MeasureString(Strs[2], ft1, 1000, sf);

            int xpoint = int.Parse(Convert.ToString(Math.Floor(bt.Width * 0.5)));
            int ypoint = int.Parse(Convert.ToString(Math.Floor(bt.Height * 0.6)));

            Color cl = Color.PowderBlue;
            //����ţ�״̬�����ƣ�ά�����ͣ�������ʱ��/��ʼ����ʱ��/�ж�ʱ��/�깤ʱ��/ϴ��ʱ�䣩,ʱ��ͣ��λ���ƻ����ʱ�䣬�Ӵ��ˣ�ά��ʦ��
            //���ַ�
            e.Graphics.DrawString(Strs[2], ft1, Brushes.Black, 2, 2);  //����
            int xleft = bt.Width * 4 / 10 + 5;
            string Time = string.Empty;
            if (Strs[7] != string.Empty)
            {
                Time = DateTime.Parse(Strs[7]).ToString("MM-dd HH:mm");
            }
            if (NewColindex == 0)
            {
                e.Graphics.DrawString("SA:" + Strs[8], ft2, Brushes.Black, 5, size.Height + 3); //ά������
                e.Graphics.DrawString("����ʱ��:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
                if (Strs[1] == "��ʱ������")  //��ʱ��������ж�ԭ��
                {
                    string delayreason = ClsBLL.DelayReason(int.Parse(bt.Name));
                    e.Graphics.DrawString("�ж�:" + delayreason, ft2, Brushes.Black, xleft, 5);  //�ƻ����ʱ��
                }
                else
                {
                    e.Graphics.DrawString("�ƻ����:" + Time, ft2, Brushes.Black, xleft, 5);  //�ƻ����ʱ��
                }
            }
            if (NewColindex == 1)
            {
                e.Graphics.DrawString("ά��:"+Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //ά������
                e.Graphics.DrawString("��ʼ����:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
                e.Graphics.DrawString("�ƻ����:" + Time, ft2, Brushes.Black, xleft, 5);  //ʱ��
            }
            if(NewColindex == 2)
            {
                e.Graphics.DrawString("ά��:" + Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //ά������
                e.Graphics.DrawString("�ж�:" + ClsBLL.PauseReason(int.Parse(bt.Name)), ft2, Brushes.Black, xleft, 5);
                e.Graphics.DrawString("�ж�ʱ��:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
            }
            if (NewColindex == 3)
            {
                e.Graphics.DrawString("ά��:" + Strs[9], ft2, Brushes.Black, 5, size.Height + 3); //ά������
                e.Graphics.DrawString("�깤ʱ��:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
                e.Graphics.DrawString("�ƻ����:" + Time, ft2, Brushes.Black, xleft, 5);  //ʱ��
            }
            if (NewColindex == 4)
            {
                e.Graphics.DrawString("SA:"+Strs[8], ft2, Brushes.Black, 5, size.Height + 3); //ά������
                e.Graphics.DrawString("ͣ��λ:" + Strs[6], ft2, Brushes.Black, xleft, 5); //ͣ��λ
                if (Strs[4] !=string.Empty)
                {
                    e.Graphics.DrawString("�ͳ�ʱ��:" + Strs[4], ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
                }
                else
                {
                    e.Graphics.DrawString("�ͳ�ʱ��:", ft2, Brushes.Black, xleft, size.Height + 3);  //ʱ��
                }
            }
            //����
            e.Graphics.DrawLine(new Pen(Color.SkyBlue, 3), new Point(xleft-5, bt.Height - 5), new Point(xleft-5, 5));
         }
        #region ��ʱ����
        private void bt_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);//
            PcStartTop = ((Control)sender).Top;
            PcStartLeft = ((Control)sender).Left;
        }
        private void bt_MouseMove(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;//�����϶�ʱ����ͷ
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//����ƫ��
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
        }
        #endregion
        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ˢ��ToolStripMenuItem_Click(null, null);
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

        private void ˢ��ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ׷����ĿToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                int recordid = int.Parse(CureentCt.Name);
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                string[] strs =bt.ImageKey.Split(new Char[] { ',' });
                if (strs.Length > 1)
                {
                    if (strs[1] == "�깤" || strs[1] == "ϴ��")
                    {
                        frmAddHourAddItem fdt = new frmAddHourAddItem(recordid);
                        if (fdt.ShowDialog() == DialogResult.OK)
                        {
                            ˢ��ToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("ֻ���ڡ��깤���͡�ϴ����״̬����׷����Ŀ��");
                    }
                }
            }
        }
        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                int recordid = int.Parse(CureentCt.Name);
                DoubleClickButton bt = (DoubleClickButton)CureentCt;
                string[] strs = bt.ImageKey.Split(new Char[] { ',' });
                if (strs.Length > 1)
                {
                    if (strs[1] == "�깤" || strs[1] == "ϴ��")
                    {
                        frmAddHourReser fdt = new frmAddHourReser(recordid);
                        if (fdt.ShowDialog() == DialogResult.OK)
                        {
                            ˢ��ToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("ֻ���ڡ��깤���͡�ϴ����״̬���ܷ��ޣ�");
                    }
                }
            }
        }
        private void �޸�״̬ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CureentCt != null)
            {
                if (MessageBox.Show("��ȷ��Ҫ�޸�״̬��", "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                int recordid = int.Parse(CureentCt.Name);
                frmChangeState fcs = new frmChangeState(recordid);
                if (fcs.ShowDialog() == DialogResult.OK)
                {
                    ˢ��ToolStripMenuItem_Click(null, null);
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
                {   //ͬ���� ����״̬��ͬ���ڴ˽���ֻ��ʾһ��
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
                ˢ��ToolStripMenuItem_Click(null,null);
            }
        }
    }
}