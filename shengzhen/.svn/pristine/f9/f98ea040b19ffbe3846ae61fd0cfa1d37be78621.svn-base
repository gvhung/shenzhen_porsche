using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmBookNew : Form
    {
        public frmBookNew(DateTime dt,string servicetype)
        {
            InitializeComponent();
            CurrentDt = dt;
            ServiceType = servicetype;
        }
        public frmBookNew(int id,bool isUpdateParts)
        {
            InitializeComponent();
            RID = id;
            IsUpdateParts = isUpdateParts;
        }
        public delegate void AddCar(int recordid);
        public event AddCar AddCarHandle;

        DateTime DateT = DateTime.Now;
        DateTime CurrentDt = DateTime.Today;
        int RID = -1;
        string ServiceType = string.Empty;
        string State = string.Empty;
        decimal bookhoursold = 0;
        string servicetypeold = string.Empty;
        private bool IsUpdateParts = false;

        private void frmBookNew_Load(object sender, EventArgs e)
        {
            string sqlstring = string.Empty;
            ClsBLL.IniCombox(cmbCarType, "车型");
            ClsBLL.IniCombox(cmbRemindResult, "提醒失败");
            ClsBLL.IniCombox(cmbPreSA, "SA");

            if (ClsBLL.GetSet("ComeType") == "0")
            {
                checkBox1.Visible = true;
                checkBox2.Visible = true;
            }
            if (IsUpdateParts == false)
            {
                tabControl1.TabPages.RemoveAt(1);
            }
            if (RID == -1) //新增
            {
                dateTimePicker1.Value = DateTime.Parse(CurrentDt.ToString("yyyy-MM-dd"));
                numericUpDown1.Value = CurrentDt.Hour;
                numericUpDown2.Value = CurrentDt.Minute;
                if (ServiceType.StartsWith("机电"))
                {
                    chkJD.Checked = true;
                    chkBJ.Checked = false;
                }
                else
                {
                    chkJD.Checked = false;
                    chkBJ.Checked = true;
                }
            }
            else  //修改
            {
                LoadBooking();
            }
        }
        private void LoadBooking()
        {
            try
            {
                string sqlstring = "Select * from Booking where ID=" + RID;
                this.Text = "修改预约信息";
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    txtCarNo.Text = Dt.Rows[0]["CarNo"].ToString();
                    this.Text = "修改车牌号码“" + txtCarNo.Text + "”预约信息";
                    cmbCarType.Text = Dt.Rows[0]["CarType"].ToString();
                    txtServiceItem.Text = Dt.Rows[0]["ServiceItem"].ToString();
                    txtLinkMan.Text = Dt.Rows[0]["LinkMan"].ToString();
                    txtTel.Text = Dt.Rows[0]["Tel"].ToString();
                    txtBookHour.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["BookHour"].ToString()) * 100));
                    bookhoursold = decimal.Parse(Dt.Rows[0]["BookHour"].ToString());
                    servicetypeold = Dt.Rows[0]["ServiceType"].ToString();
                    if (servicetypeold.StartsWith("机电"))
                    {
                        chkJD.Checked = true;
                        chkBJ.Checked = false;
                    }
                    else
                    {
                        chkBJ.Checked = true;
                        chkJD.Checked = false;
                    }
                    DateT = DateTime.Parse(Dt.Rows[0]["BookTime"].ToString());
                    dateTimePicker1.Value = DateT;
                    numericUpDown1.Value = DateT.Hour;
                    numericUpDown2.Value = DateT.Minute;

                    txtRemark.Text = Dt.Rows[0]["Remark"].ToString();
                    txtVIN.Text = Dt.Rows[0]["VIN"].ToString();
                    txtRunKM.Text = Dt.Rows[0]["RunKM"].ToString();
                    txtEmail.Text = Dt.Rows[0]["Email"].ToString();
                    cmbPreSA.Text = Dt.Rows[0]["PreSA"].ToString();
                    txtParts.Text = Dt.Rows[0]["Parts"].ToString();
                    State = Dt.Rows[0]["State"].ToString();

                    txtLastMan.Text = Dt.Rows[0]["LastUpdate"].ToString();
                    txtComeTime.Text = Dt.Rows[0]["ComeTime"].ToString();
                    txtState.Text = Dt.Rows[0]["State"].ToString();
                    txtParksite.Text = Dt.Rows[0]["Parksite"].ToString();
                    txtCarTopNo.Text = Dt.Rows[0]["CarTopNo"].ToString();
                    txtCreator.Text = Dt.Rows[0]["Creator"].ToString();
                    txtPlanCompleteTime.Text = Dt.Rows[0]["PlanOutTime"].ToString();

                    txtWoker.Text = Dt.Rows[0]["Worker"].ToString();
                    txtStartServiceTime.Text = Dt.Rows[0]["StartServiceTime"].ToString();
                    txtEndServiceTime.Text = Dt.Rows[0]["EndServiceTime"].ToString();
                    txtClearTime.Text = Dt.Rows[0]["ClearCarTime"].ToString();
                    txtCreateDate.Text = Dt.Rows[0]["CreateDate"].ToString();
                    txtReceiver.Text = Dt.Rows[0]["Receiver"].ToString();
                    txtServiceTU.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["ServiceHour"].ToString()) * 100));

                    string preparts = Dt.Rows[0]["PreParts"].ToString();
                    if (preparts == chkParts1.Text)
                    {
                        chkParts1.Checked = true;
                    }
                    if (preparts == chkParts2.Text)
                    {
                        chkParts2.Checked = true;
                    }
                    if (preparts == chkParts3.Text)
                    {
                        chkParts3.Checked = true;
                    }
                    string sendservice = Dt.Rows[0]["SendService"].ToString();
                    if (sendservice.IndexOf("代步车") > -1)
                    {
                        chkReCar.Checked = true;
                    }
                    if (sendservice.IndexOf("出租车") > -1)
                    {
                        chkRentCar.Checked = true;
                    }
                    if (sendservice.IndexOf("接送") > -1)
                    {
                        chkSend.Checked = true;
                    }
                    if (Dt.Rows[0]["IsBook"].ToString().ToLower() == "false")
                    {
                        checkBox2.Checked = true;
                    }
                    if (Dt.Rows[0]["IsRemind"].ToString().ToLower() == "true")
                    {
                        chkIsRemind.Checked = true;
                        if (Dt.Rows[0]["IsRemindSuc"].ToString().ToLower() == "false")
                        {
                            radioButton2.Checked = true;
                            cmbRemindResult.Text = Dt.Rows[0]["RemindResult"].ToString();
                        }
                    }
                    if (IsUpdateParts)  //零件
                    {
                        dateTimePicker1.Enabled = false;
                        numericUpDown1.Enabled = false;
                        numericUpDown2.Enabled = false;
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        foreach (Control ct in tabPage1.Controls)
                        {
                            if (ct.Name.StartsWith("txt") || ct.Name.StartsWith("ch") || ct.Name.StartsWith("cmb"))
                            {
                                ct.Enabled = false;
                            }
                        }
                        //foreach (Control ct in tabPage2.Controls)
                        //{
                        //    if (ct.Name.StartsWith("txt"))
                        //    {
                        //        ct.Enabled = false;
                        //    }
                        //}
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        numericUpDown1.Enabled = false;
                        numericUpDown2.Enabled = false;
                        dateTimePicker1.Enabled = false;
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckReg()) return;
            string sqlstring = string.Empty;
            if (IsUpdateParts)  //配件部修改零件
            {
                SaveParts();
                return;
            }
            if (!CheckText()) return;
            DateTime BookTime = DateTime.Parse(dateTimePicker1.Value.ToShortDateString() +" " + numericUpDown1.Value.ToString() + ":" + numericUpDown2.Value.ToString());
            int IsBook = 1;
            //if (!checkBox1.Visible)  //按照设置的规则确定是预约来店还是自行来店
            //{
            //    string hou = ClsBLL.GetSet("txtSet8");
            //    int hourset = 3;
            //    if (hou != string.Empty) hourset = int.Parse(hou);
            //    TimeSpan ts = BookTime.Subtract(DateTime.Now);
            //    if (ts.Hours >= hourset)
            //    {
            //        checkBox1.Checked = true;
            //    }
            //    else
            //    {
            //        checkBox2.Checked = true;
            //    }
            //}
            //if (checkBox2.Checked) IsBook = 0;
            int IsRemind = 0;
            int IsRemindSuc = 1;
            string RemindResult = string.Empty;
            if (chkIsRemind.Checked)
            {
                IsRemind = 1;
                if (radioButton2.Checked)
                {
                    IsRemindSuc = 0;
                    RemindResult = cmbRemindResult.Text;
                    if (RemindResult == string.Empty)
                    {
                        MessageBox.Show("请选择提醒结果！");
                        return;
                    }
                }
            }
            string sendservice = string.Empty;
            if (chkReCar.Checked) sendservice = "代步车";
            if (chkRentCar.Checked) sendservice += ",出租车";
            if (chkSend.Checked) sendservice += ",接送";
            if (sendservice.StartsWith(",")) sendservice = sendservice.Substring(1);
            decimal RunKM = 0;
            if (txtRunKM.Text != string.Empty) RunKM = decimal.Parse(txtRunKM.Text);
            decimal servicehours =decimal.Parse(txtBookHour.Text)/100;

            string sertype1 = string.Empty;
            string sertype2 = string.Empty;
            if (chkJD.Checked)
            {
                sertype1 = "机电维修";
                if (RID == -1 && CheckExsit(sertype1)) return;
            }
            if (chkBJ.Checked)
            {
                sertype2 = "车身维修";
                if (RID == -1 && CheckExsit(sertype2)) return;
            }
            int ID = -1;
            try
            {
                if (RID == -1 ) //新增
                {
                    int BookIndex = 0;
                    decimal jdhour = servicehours;
                    decimal cshour = servicehours;
                    if (panWorkHours.Visible)
                    {
                        jdhour = decimal.Parse(txtJDHours.Text) / 100;
                        cshour = decimal.Parse(txtCSHours.Text) / 100;
                    }
                    if (sertype1 != string.Empty)
                    {
                        sqlstring = "Insert Into Booking(BookIndex,BookTime,CarNo,CarType,ServiceItem,ServiceType,LinkMan,Tel,BookHour,ServiceHour,Creator,Remark,VIN,IsBook,IsRemind,IsRemindSuc,RemindResult,PreSA,RunKM,Email,SendService)";
                        sqlstring += "Values('" + BookIndex + "','" + BookTime + "','" + txtCarNo.Text + "','" + cmbCarType.Text + "'";
                        sqlstring += ",'" + txtServiceItem.Text + "','" + sertype1 + "','" + txtLinkMan.Text + "','" + txtTel.Text + "'," + jdhour + "," + jdhour + ",'" + ClsBLL.UserName + "','" + txtRemark.Text + "'";
                        sqlstring += ",'" + txtVIN.Text + "'," + IsBook + "," + IsRemind + "," + IsRemindSuc + ",'" + RemindResult + "'";
                        sqlstring += ",'" + cmbPreSA.Text + "'," + RunKM + ",'" + txtEmail.Text + "','" + sendservice + "')";
                        if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                        {
                            ID = int.Parse(SQLDbHelper.ExecuteScalar("Select Max(ID) as MaxID from Booking").ToString());
                            ClsBLL.AddSysLog(ID, "新增预约，车牌号码:" + txtCarNo.Text + ",维修类型:" + sertype1);
                            AddCarHandle(ID);
                        }
                    }
                    if (sertype2 != string.Empty)
                    {
                        sqlstring = "Insert Into Booking(BookIndex,BookTime,CarNo,CarType,ServiceItem,ServiceType,LinkMan,Tel,BookHour,ServiceHour,Creator,Remark,VIN,IsBook,IsRemind,IsRemindSuc,RemindResult,PreSA,RunKM,Email,SendService)";
                        sqlstring += "Values('" + BookIndex + "','" + BookTime + "','" + txtCarNo.Text + "','" + cmbCarType.Text + "'";
                        sqlstring += ",'" + txtServiceItem.Text + "','" + sertype2 + "','" + txtLinkMan.Text + "','" + txtTel.Text + "'," + cshour + "," + cshour + ",'" + ClsBLL.UserName + "','" + txtRemark.Text + "'";
                        sqlstring += ",'" + txtVIN.Text + "'," + IsBook + "," + IsRemind + "," + IsRemindSuc + ",'" + RemindResult + "'";
                        sqlstring += ",'" + cmbPreSA.Text + "'," + RunKM + ",'" + txtEmail.Text + "','" + sendservice + "')";
                        if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                        {
                            ID = int.Parse(SQLDbHelper.ExecuteScalar("Select Max(ID) as MaxID from Booking").ToString());
                            ClsBLL.AddSysLog(ID, "新增预约，车牌号码:" + txtCarNo.Text + ",维修类型:" + sertype2);
                            AddCarHandle(ID);
                        }
                    }
                    MessageBox.Show("新增成功！");
                    this.Close();
                }
                else
                {    //修改
                    string delay = string.Empty;
                    //if (DateT.CompareTo(BookTime) != 0) delay = "延迟预约";
                    string updatesertype = sertype1;
                    if (updatesertype == string.Empty) updatesertype = sertype2;
                    //if (sertype1 != string.Empty && sertype2 != string.Empty) updatesertype = servicetypeold;
                    if(updatesertype != servicetypeold) 
                    {
                        if (CheckExsit(updatesertype)) return;
                    }
                    sqlstring = "Update Booking Set BookTime='" + BookTime + "',CarNo='" + txtCarNo.Text + "'";
                    sqlstring += ",CarType='" + cmbCarType.Text + "',ServiceItem='" + txtServiceItem.Text + "',ServiceType='" + updatesertype + "'";
                    sqlstring += ",LinkMan='" + txtLinkMan.Text + "',Tel='" + txtTel.Text + "',UpdateDate='" + DateTime.Now + "'";
                    sqlstring += ",DelayBook='" + delay + "',BookHour=" + servicehours + ",ServiceHour=" + servicehours + ",Remark=isnull(Remark,'')+'，" + txtRemark.Text + "'";
                    sqlstring += ",VIN='" + txtVIN.Text + "',IsRemind=" + IsRemind + ",IsBook="+ IsBook;
                    sqlstring += ",IsRemindSuc=" + IsRemindSuc + ",RemindResult='" + RemindResult + "',LastUpdate='" + ClsBLL.UserName + "'";
                    sqlstring += ",PreSA='" + cmbPreSA.Text + "',RunKM=" + RunKM + ",Email='" + txtEmail.Text + "',SendService='" + sendservice + "'";
                    sqlstring += " Where ID=" + RID;

                    if (State == "失约" || State == "取消")
                    {
                        sqlstring += ";Update A Set A.State='预约' From Booking A,(Select CarNo,booktime From Booking Where ID=" + RID + ") B";
                        sqlstring += " Where A.CarNo=B.CarNo And substring(convert(nvarchar(50),A.booktime,120),1,10)=substring(convert(nvarchar(50),B.booktime,120),1,10)";
                    }
                    if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
                    {
                        int doubleid = ClsBLL.GetDoubleID(RID, BookTime);
                        if (doubleid > -1)
                        {
                            //修改公共部分
                            sqlstring = "Update A Set A.BookTime='" + BookTime + "',A.CarNo='" + txtCarNo.Text + "'";
                            sqlstring += ",A.CarType='" + cmbCarType.Text + "',A.LinkMan='" + txtLinkMan.Text + "',A.Tel='" + txtTel.Text + "'";
                            sqlstring += ",A.DelayBook='" + delay + "',A.VIN='" + txtVIN.Text + "',A.IsRemind=" + IsRemind + ",A.IsBook=" + IsBook;
                            sqlstring += ",A.IsRemindSuc=" + IsRemindSuc + ",A.RemindResult='" + RemindResult + "',A.LastUpdate='" + ClsBLL.UserName + "'";
                            sqlstring += ",A.PreSA='" + cmbPreSA.Text + "',A.RunKM=" + RunKM + ",A.Email='" + txtEmail.Text + "',A.SendService='" + sendservice + "'";
                            sqlstring += " From Booking A,(Select ID,CarNo,BookTime From Booking Where ID=" + RID + ") B";
                            sqlstring += " Where A.CarNo=B.CarNo And substring(convert(nvarchar(50),A.booktime,120),1,10)=substring(convert(nvarchar(50),B.booktime,120),1,10) And A.ID<>B.ID";
                            SQLDbHelper.ExecuteSql(sqlstring);
                        }
                        ClsBLL.AddSysLog(RID, "修改预约，车牌号码:" + txtCarNo.Text);
                        //if (sertype1 != string.Empty && sertype2!=string.Empty)
                        //{    //修改时增加维修类型
                        //    string temptype = "机电维修";
                        //    if (servicetypeold == "机电维修")
                        //    {
                        //        temptype = "车身维修";
                        //    }
                        //    if (CheckExsit(temptype)) return;
                        //    sqlstring = "Insert Into Booking(BookIndex,booktime,carno,vin,cartype,linkman,tel,bookhour,servicehour,serviceitem,";
                        //    sqlstring += "servicetype,state,CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,AssignTime,PlanOutTime,PlanCompleteTime,StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                        //    sqlstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts)";
                        //    sqlstring +=            "Select BookIndex,booktime,carno,vin,cartype,linkman,tel,bookhour,servicehour, serviceitem,";
                        //    sqlstring += "'"+ temptype +"' as Servicetype,state,CarTopNo,Creator,Createdate,Updatedate,Success,ComeTime,AssignTime,PlanOutTime,PlanCompleteTime,StartServiceTime,Remark,IsBook,IsRemind,IsRemindSuc,";
                        //    sqlstring += "RemindResult,Receiver,LastUpdate,PreSA,RunKM,Email,SendService,PreParts,Parts from booking where ID=" + RID;
                        //    SQLDbHelper.ExecuteSql(sqlstring);
                        //}
                        MessageBox.Show("修改成功！");
                        this.DialogResult = DialogResult.OK;
                    }
                    if (decimal.Parse(txtBookHour.Text) != bookhoursold * 100)
                    {
                        ClsBLL.AddMsg(RID, "车牌号码:" + txtCarNo.Text + "修改了预约维修工时。--" + ClsBLL.UserName);
                    }
                }
                this.Close();
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        //修改配件
        private void SaveParts()
        {
            string preparts = string.Empty;
            if (chkParts1.Checked)
            {
                preparts = chkParts1.Text;
            }
            if (chkParts2.Checked)
            {
                preparts = chkParts2.Text;
            }
            if (chkParts3.Checked)
            {
                preparts = chkParts3.Text;
            }
            if (preparts == string.Empty)
            {
                MessageBox.Show("请选择零件准备情况！");
                return;
            }
            string sqlstring = "Update A set A.Parts='" + txtParts.Text + "',A.PreParts='" + preparts + "'";
            sqlstring += " From Booking A,(Select CarNo,BookTime from Booking Where ID=" + RID + ") B";
            sqlstring += " Where A.CarNo=B.CarNo And substring(convert(nvarchar(50),A.booktime,120),1,10)=substring(convert(nvarchar(50),B.booktime,120),1,10)";
            if (SQLDbHelper.ExecuteSql(sqlstring) > 0)
            {
                ClsBLL.AddSysLog(RID, "修改预约零件准备情况，车牌号码:" + txtCarNo.Text);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        //检查
        private bool CheckText()
        {
            if (txtCarNo.Text == string.Empty || txtBookHour.Text == string.Empty || txtServiceItem.Text == string.Empty || cmbPreSA.Text == string.Empty)
            {
                MessageBox.Show("请按要求完整填写必选项！");
                return false;
            }
            if (txtBookHour.Text != string.Empty)
            {
                decimal servicehours = decimal.Parse(txtBookHour.Text);
                if (servicehours < 10)
                {
                    MessageBox.Show("维修工时错误！");
                    txtBookHour.Focus();
                    return false;
                }
            }
            if (!chkBJ.Checked && !chkJD.Checked)
            {
                MessageBox.Show("请选择一个维修类型！");
                return false;
            }
            if (panWorkHours.Visible)
            {
                if (txtJDHours.Text == string.Empty || txtCSHours.Text == string.Empty)
                {
                    MessageBox.Show("请输入机电工时和车身工时！");
                    return false;
                }
            }
            return true;
        }
        //检查是否已经存在
        private bool CheckExsit()
        {
            DateTime BookTime = DateTime.Parse(dateTimePicker1.Value.ToShortDateString() + " " + numericUpDown1.Value.ToString() + ":" + numericUpDown2.Value.ToString());
            string sqlstring = "Select count(*) from Booking where (VIN='" + txtVIN.Text + "' Or CarNo='" + txtCarNo.Text + "') and Booktime between '" + BookTime.ToShortDateString() + "' and '" + BookTime.AddDays(1).ToShortDateString() + "'";
            try
            {
                if (int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString()) > 0)
                {
                    if (MessageBox.Show("车牌号：" + txtCarNo.Text + "或者车架号: " + txtVIN.Text + " 在" + BookTime.ToShortDateString() + "已经预约过！你还要增加吗？", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return true;
                    }
                    else
                    {
                        string stype = "车身维修";
                        if (chkJD.Checked)
                        {
                            stype = "机电维修";
                        }
                        sqlstring = "Select count(*) from Booking where (VIN='" + txtVIN.Text + "' Or CarNo='" + txtCarNo.Text + "') and Booktime between '" + BookTime.ToShortDateString() + "' and '" + BookTime.AddDays(1).ToShortDateString() + "' And Servicetype='" + stype + "'";
                        if (int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString()) > 0)
                        {
                            MessageBox.Show("车牌号：" + txtCarNo.Text + "或者车架号: " + txtVIN.Text + " 在" + BookTime.ToShortDateString() + "已经预约过同类型的维修,不能预约");
                            return true;
                        }
                    }
                }
                sqlstring = "Select Count(*) from Booking where booktime='" + BookTime + "' and ServiceType='" + ServiceType + "'";
                int rs = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                int maxnum = int.Parse(ClsBLL.GetSet("txtSet6"));
                if (rs > maxnum)
                {
                    MessageBox.Show("在同一时间点只能预约"+ maxnum + "辆车！");
                    return true;
                }
                return false;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
                return false;
            }
        }
        private bool CheckExsit(string svrtype)
        {
            DateTime BookTime = DateTime.Parse(dateTimePicker1.Value.ToShortDateString() + " " + numericUpDown1.Value.ToString() + ":" + numericUpDown2.Value.ToString());
            string sqlstring = "Select count(*) from Booking where (VIN='" + txtVIN.Text + "' Or CarNo='" + txtCarNo.Text + "') and Booktime between '" + BookTime.ToShortDateString() + "' and '" + BookTime.AddDays(1).ToShortDateString() + "' And Servicetype='" + svrtype + "'";
            try
            {
                if (int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString()) > 0)
                {
                    MessageBox.Show("车牌号：" + txtCarNo.Text + "或者车架号: " + txtVIN.Text + " 在" + BookTime.ToShortDateString() + "已经预约过同类型的维修,不能预约");
                    return true;
                }
                return false;
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCarNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(","))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.End)
            {
                txtCarNo.GetNextControl(this, true).Focus();
            }
        }
        private bool CheckReg()
        {
            string sqlstring = "Select Count(*) as cunt from Booking where Booktime between '" + DateT.ToShortDateString() + "' and '" + DateT.AddDays(1).ToShortDateString() + "' ";
            try
            {
                object obj = SQLDbHelper.ExecuteScalar(sqlstring);
                int bi = int.Parse(obj.ToString());
                if (!ClsBLL.IsRegist)
                {
                    if (bi > 10)
                    {
                        MessageBox.Show("软件未注册，只能新增10张单！");
                        return false;
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
            return true;
        }

        private void txtServiceHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != char.Parse("."))
            {
                ClsBLL.Key_Number(e);
            }
        }

        private void txtServiceItem_MouseLeave(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = !radioButton1.Checked;
            if (radioButton1.Checked)
            {
                label24.Visible = false;
                cmbRemindResult.Visible = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label24.Visible = true;
                cmbRemindResult.Visible = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }

        private void chkJD_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJD.Checked)
            {
                if (RID > 0)
                {
                    chkBJ.Checked = false;
                }
                else
                {
                    if (chkBJ.Checked)
                    {
                        panWorkHours.Visible = true;
                    }
                }
            }
            else
            {
                panWorkHours.Visible = false;
            }
        }

        private void chkBJ_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBJ.Checked)
            {
                if (RID > 0)
                {
                    chkJD.Checked = false;
                }
                else
                {
                    if (chkJD.Checked)
                    {
                        panWorkHours.Visible = true;
                    }
                }
            }
            else
            {
                panWorkHours.Visible = false;
            }
        }

        private void btnRadom_Click(object sender, EventArgs e)
        {
            if (cmbPreSA.Items.Count > 0)
            {
                Random rnd = new Random();
                int rndNum = rnd.Next(cmbPreSA.Items.Count);
                cmbPreSA.SelectedIndex = rndNum;
            }
        }

        private void chkParts1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParts1.Checked)
            {
                chkParts2.Checked = false;
                chkParts3.Checked = false;
            }
        }

        private void chkParts2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParts2.Checked)
            {
                chkParts1.Checked = false;
                chkParts3.Checked = false;
            }
        }

        private void chkParts3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParts3.Checked)
            {
                chkParts2.Checked = false;
                chkParts1.Checked = false;
            }
        }

        private void txtCarNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCarNo.Text.Length == 8)
            {
                string sqlstring = "Select * from Booking where CarNo='"+ txtCarNo.Text +"'";
                DataTable DtCar = SQLDbHelper.Query(sqlstring).Tables[0];
                if (DtCar.Rows.Count > 0)
                {
                    txtVIN.Text = DtCar.Rows[0]["VIN"].ToString();
                    cmbCarType.Text = DtCar.Rows[0]["CarType"].ToString();
                    txtLinkMan.Text = DtCar.Rows[0]["LinkMan"].ToString();
                    txtTel.Text = DtCar.Rows[0]["Tel"].ToString();
                    txtEmail.Text = DtCar.Rows[0]["Email"].ToString();
                }
            }
        }
    }
}