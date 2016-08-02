using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Workshop
{
    public partial class frmBookLook : Form
    {
        public frmBookLook(int id)
        {
            InitializeComponent();
            RID = id;
        }
        public frmBookLook(string carno)
        {
            InitializeComponent();
            CarNo = carno;
        }
        private string CarNo = string.Empty;
        private int RID = -1;
        private void frmBookNew_Load(object sender, EventArgs e)
        {

            try
            {
                string sqlstring = "Select * from Booking where CarNo like '%" + CarNo + "%' Order by BookTime desc";
                if (RID > 0)
                {
                    sqlstring = "Select * from Booking where ID=" + RID;
                }
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (RID == -1)
                    {
                        RID = int.Parse(Dt.Rows[0]["ID"].ToString());
                    }
                    txtCarNo.Text = Dt.Rows[0]["CarNo"].ToString();
                    this.Text = "查看车牌号码“" + txtCarNo.Text + "”最近一次预约信息";
                    cmbCarType.Text = Dt.Rows[0]["CarType"].ToString();
                    txtServiceItem.Text = Dt.Rows[0]["ServiceItem"].ToString();
                    txtLinkMan.Text = Dt.Rows[0]["LinkMan"].ToString();
                    txtTel.Text = Dt.Rows[0]["Tel"].ToString();
                    txtBookHour.Text = Convert.ToString(decimal.Floor(decimal.Parse(Dt.Rows[0]["BookHour"].ToString()) * 100));
                    string ServiceType = Dt.Rows[0]["ServiceType"].ToString();
                    if (ServiceType.StartsWith("机电"))
                    {
                        chkJD.Checked = true;
                        chkBJ.Checked = false;
                    }
                    else
                    {
                        chkBJ.Checked = true;
                        chkJD.Checked = false;
                    }
                    DateTime DateT = DateTime.Parse(Dt.Rows[0]["BookTime"].ToString());
                    dateTimePicker1.Value = DateT;
                    numericUpDown1.Value = DateT.Hour;
                    numericUpDown2.Value = DateT.Minute;
                    txtRemark.Text = Dt.Rows[0]["Remark"].ToString();

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

                    txtVIN.Text = Dt.Rows[0]["VIN"].ToString();
                    txtRunKM.Text = Dt.Rows[0]["RunKM"].ToString();
                    txtEmail.Text = Dt.Rows[0]["Email"].ToString();
                    cmbPreSA.Text = Dt.Rows[0]["PreSA"].ToString();
                    txtParts.Text = Dt.Rows[0]["Parts"].ToString();

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
                    if (txtState.Text != "预约" && txtState.Text != "失约")
                    {
                        chkJD.Enabled = false;
                        chkBJ.Enabled = false;
                        txtBookHour.ReadOnly = true;
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
            this.Close();
        }
    }
}