using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmServiceItem : Form
    {
        public frmServiceItem(int rid)
        {
            InitializeComponent();
            RID = rid;
        }
        private int RID = -1;
        string ServiceType = string.Empty;
        int selectrow = -1;
        string remark = string.Empty;
        DataTable Dt = new DataTable();         
        private void frmServiceItem_Load(object sender, EventArgs e)
        {
            string sqlstring = string.Empty;
            try
            {
                Dt = SQLDbHelper.Query("Select * from Booking where ID=" + RID).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    txtCarNo.Text = Dt.Rows[0]["CarNo"].ToString();
                    txtCarType.Text = Dt.Rows[0]["CarType"].ToString();
                    txtServiceItem.Text = Dt.Rows[0]["ServiceItem"].ToString();
                    txtPlanOutTime.Text = Dt.Rows[0]["PlanOutTime"].ToString();
                    remark = Dt.Rows[0]["Remark"].ToString();
                    DateTime DateT = DateTime.Parse(Dt.Rows[0]["BookTime"].ToString());
                    dateTimePicker1.Value = DateT;
                    numericUpDown1.Value = DateT.Hour;
                    numericUpDown2.Value = DateT.Minute;

                    txtServiceHour.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));
                    ServiceType = Dt.Rows[0]["ServiceType"].ToString();
                    string workerold = Dt.Rows[0]["Worker"].ToString();
                    sqlstring = "Select WorkerName,Worker.WorkerCode from WorkerPlan inner join Worker on WorkerPlan.WorkerCode=Worker.WorkerCode Where WorkerGroup='" + ServiceType + "' And Wyear=" + DateTime.Today.Year + " and Wmonth=" + DateTime.Today.Month + " and Wday=" + DateTime.Today.Day + " and IsWork=1 and Position like '%工人%' and WorkerName <>'" + workerold + "' order by Workergroup desc";
                    DataTable WorkerDt = SQLDbHelper.Query(sqlstring).Tables[0];
                    foreach (DataRow dr in WorkerDt.Rows)
                    {
                        comboBox1.Items.Add(dr["WorkerName"].ToString());
                    }
                    if (ServiceType == "机电维修")
                    {
                        chkJD.Checked = true;
                        chkBJ.Checked = false;
                    }
                    else
                    {
                        chkBJ.Checked = true;
                        chkJD.Checked = false;
                    }
                    if (Dt.Rows[0]["Worker"].ToString() != string.Empty)  //派工之后再分单
                    {
                        if (remark.IndexOf("分单") == -1)
                        {
                            dataGridView1.Rows.Add(1);
                            dataGridView1.Rows[0].Cells[0].Value = Dt.Rows[0]["Worker"].ToString();
                            dataGridView1.Rows[0].Cells[1].Value = int.Parse(decimal.Floor(100 * decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString())).ToString());
                            dataGridView1.Rows[0].Cells[2].Value = Dt.Rows[0]["ServiceItem"].ToString();
                            dataGridView1.Rows[0].Cells[3].Value = Dt.Rows[0]["ID"].ToString();
                        }
                        else  //分单之后再分单
                        {
                            sqlstring = "Select * from booking Where CreateDate='" + Dt.Rows[0]["CreateDate"].ToString() + "' and ServiceType='" + Dt.Rows[0]["ServiceType"].ToString() + "' And CarNo='" + txtCarNo.Text + "' And Remark like '%分单%'";
                            Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                dataGridView1.Rows.Add(1);
                                dataGridView1.Rows[i].Cells[0].Value = Dt.Rows[i]["Worker"].ToString();
                                dataGridView1.Rows[i].Cells[1].Value = int.Parse(decimal.Floor(100 * decimal.Parse(Dt.Rows[i]["ServiceHour"].ToString())).ToString());
                                dataGridView1.Rows[i].Cells[2].Value = Dt.Rows[i]["ServiceItem"].ToString();
                                dataGridView1.Rows[i].Cells[3].Value = Dt.Rows[i]["ID"].ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            //维修工时
            if (dataGridView1.Rows.Count==0) return;
            if (dataGridView1.Rows[0].Cells[1].Value == null)
            {
                MessageBox.Show("请填写分单派工！");
                return;
            }
            DateTime starttime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));
            try
            {
                string worker = string.Empty;
                decimal servicehours = 0;
                servicehours = servicehours / 100;
                string serviceitem = string.Empty;
                int id = -1;
                DateTime StartServiceTime = DateTime.Now;
                DateTime DtPlanComplete = DateTime.Now;

                string insertstring = string.Empty;
                string sqlstring = "Select count(Distinct CreateDate) from Booking where AssignTime between '" + DateTime.Today.ToString() + "' and '" + DateTime.Today.AddDays(1).ToString() + "' And Remark like '%分单%'";
                int fendan = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                fendan++;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    id = -1;
                    if (dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        worker = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        servicehours = decimal.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        servicehours = servicehours / 100;
                        serviceitem = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[3].Value != null)
                        {
                            id = int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }
                        DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                        if (DateTime.Now.CompareTo(starttime) < 0)
                        {
                            DtPlanComplete = starttime;
                        }
                        if (id == -1)  //未派工，首次分单
                        {
                            if (i == 0)
                            {
                                sqlstring = "Update Booking Set AssignTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',PlanCompleteTime='" + DtPlanComplete + "', Worker='" + worker + "',ServiceHour=" + servicehours + ",ServiceItem='" + serviceitem + "',State='等待开工',Remark=isnull(Remark,'')+'，分单" + fendan + "' where ID=" + RID;
                                SQLDbHelper.ExecuteSql(sqlstring);
                            }
                            else
                            {
                                insertstring += ";Insert Into Booking(BookIndex,booktime,carno,vin,cartype,linkman,tel,worker,bookhour,servicehour,serviceitem,";
                                insertstring += "servicetype,state,CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,AssignTime,PlanOutTime,PlanCompleteTime,StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                                insertstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts)";
                                insertstring += "Select BookIndex,booktime,carno,vin,cartype,linkman,tel,'" + worker + "' as worker,bookhour," + servicehours + " as servicehour,'" + serviceitem + "' as serviceitem,";
                                insertstring += "Servicetype,'等待开工',CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',PlanOutTime,'" + DtPlanComplete + "' as PlanCompleteTime,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' as StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                                insertstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts from booking where ID=" + RID;
                            }
                        }
                        else //已派工，分单
                        {
                            DataRow[] Drs = Dt.Select("ID=" + id);
                            if (remark.IndexOf("分单") > -1)
                            {
                                if (Drs.Length > 0)
                                {
                                    if (Drs[0]["State"].ToString() == "延时到明天")
                                    {
                                        decimal facthour = ClsBLL.GetFactHours(id);
                                        if (servicehours > facthour) servicehours = servicehours - facthour;
                                        DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                                        sqlstring = "Update Booking Set ServiceHour=" + servicehours + ",PlanCompleteTime='" + DtPlanComplete + "',ServiceItem='" + serviceitem + "',State='等待开工' where ID=" + id;
                                    }
                                    else
                                    {
                                        sqlstring = "Update Booking Set ServiceHour=" + servicehours + ",ServiceItem='" + serviceitem + "',State='等待开工' where ID=" + id;
                                    }
                                }
                            }
                            else
                            {
                                if (Drs[0]["State"].ToString() == "延时到明天")
                                {
                                    decimal facthour = ClsBLL.GetFactHours(id);
                                    if (servicehours > facthour) servicehours = servicehours - facthour;
                                    DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                                    sqlstring = "Update Booking Set ServiceHour=" + servicehours + ",PlanCompleteTime='" + DtPlanComplete + "',ServiceItem='" + serviceitem + "',State='等待开工',Remark=isnull(Remark,'')+'，分单" + fendan + "' where ID=" + id;
                                }
                                else
                                {
                                    sqlstring = "Update Booking Set ServiceHour=" + servicehours + ",PlanCompleteTime='" + DtPlanComplete + "',ServiceItem='" + serviceitem + "',State='等待开工',Remark=isnull(Remark,'')+'，分单" + fendan + "' where ID=" + id;
                                }
                            }
                            SQLDbHelper.ExecuteSql(sqlstring);
                        }
                    }
                }
                if (insertstring.StartsWith(";"))
                {
                    SQLDbHelper.ExecuteSql(insertstring.Substring(1));
                }
                MessageBox.Show("保存成功！");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectrow > -1)
            {
                if (dataGridView1.Rows[selectrow].Cells[3].Value == null)
                {
                    dataGridView1.Rows.RemoveAt(selectrow);
                }
                else
                {
                    MessageBox.Show("已经派工，不能删除");
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            selectrow = e.RowIndex;
            if (dataGridView1.Rows[selectrow].Cells[3].Value == null)
            {
                dataGridView1.ReadOnly = false;
            }
            else
            {
                if (e.ColumnIndex == 0)
                {
                    dataGridView1.ReadOnly = true;
                }
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectrow > -1)
            {
                dataGridView1.Rows[selectrow].Cells[0].Value = comboBox1.Text;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 0)
            {
                comboBox1.Visible = true;
                comboBox1.Width = dataGridView1.Columns[0].Width;
                comboBox1.Left = dataGridView1.Left + 2;
                comboBox1.Top = dataGridView1.Top + dataGridView1.ColumnHeadersHeight + dataGridView1.Rows[0].Height * e.RowIndex;
            }
            else
            {
                comboBox1.Visible = false;
            }
            if (dataGridView1.Rows[selectrow].Cells[0].Value != null)
            {
                comboBox1.Visible = false;
            }
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows[selectrow].Cells[3].Value == null)
                {
                    dataGridView1.Rows.RemoveAt(selectrow);
                }
                else
                {
                    MessageBox.Show("已经派工，不能删除");
                }
            }
        }
        /*
        private void button1_Click(object sender, EventArgs e)
        {
            //if (remark.IndexOf("分单") > -1)
            //{
            //    MessageBox.Show("改单已经是分单，不能再分单！");
            //    return;
            //}
            //维修工时
            if (dataGridView1.Rows.Count == 0) return;
            if (dataGridView1.Rows[0].Cells[1].Value == null)
            {
                MessageBox.Show("请填写分单派工！");
                return;
            }
            DateTime starttime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));
            try
            {
                string worker = dataGridView1.Rows[0].Cells[0].Value.ToString();
                decimal servicehours = decimal.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString());
                servicehours = servicehours / 100;
                string serviceitem = dataGridView1.Rows[0].Cells[2].Value.ToString();
                DateTime DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                if (DateTime.Now.CompareTo(starttime) < 0)
                {
                    DtPlanComplete = starttime;
                }
                string sqlstring = "Select count(Distinct CreateDate) from Booking where AssignTime between '" + DateTime.Today.ToString() + "' and '" + DateTime.Today.AddDays(1).ToString() + "' And Remark like '%分单%' ";
                int fendan = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                fendan++;
                if (workerold == string.Empty)
                {
                    sqlstring = "Update Booking Set AssignTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',StartServiceTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "',PlanCompleteTime='" + DtPlanComplete + "', Worker='" + worker + "',ServiceHour=" + servicehours + ",ServiceItem='" + serviceitem + "',State='维修进行中',Remark=isnull(Remark,'')+'，分单" + fendan + "' where ID=" + RID;
                }
                else
                {
                    DtPlanComplete = DateTime.Parse(startdate).AddMinutes(double.Parse(Convert.ToString(servicehours * 60 + ClsBLL.Pausemins(RID, DateTime.Parse(startdate)))));
                    sqlstring = "Update Booking Set PlanCompleteTime='" + DtPlanComplete + "',ServiceHour=" + servicehours + ",ServiceItem='" + serviceitem + "',State='维修进行中',Remark=isnull(Remark,'')+'，分单" + fendan + "' where ID=" + RID;
                }
                for (int i = 1; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value != null)
                    {
                        worker = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        servicehours = decimal.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        serviceitem = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        servicehours = servicehours / 100;
                        DtPlanComplete = DateTime.Now.AddMinutes(double.Parse(Convert.ToString(servicehours * 60)));
                        if (DateTime.Now.CompareTo(starttime) < 0)
                        {
                            DtPlanComplete = starttime;
                        }
                        sqlstring += ";Insert Into Booking(BookIndex,booktime,carno,vin,cartype,linkman,tel,worker,bookhour,servicehour,serviceitem,";
                        sqlstring += "servicetype,state,CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,AssignTime,PlanOutTime,PlanCompleteTime,StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                        sqlstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts)";
                        sqlstring += "Select BookIndex,booktime,carno,vin,cartype,linkman,tel,'" + worker + "' as worker,bookhour," + servicehours + " as servicehour,'" + serviceitem + "' as serviceitem,";
                        sqlstring += "Servicetype,'维修进行中',CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,AssignTime,PlanOutTime,'" + DtPlanComplete + "' as PlanCompleteTime,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' as StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                        sqlstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts from booking where ID=" + RID;
                    }
                }

                if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                {
                    MessageBox.Show("保存成功！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
         */
    }
}