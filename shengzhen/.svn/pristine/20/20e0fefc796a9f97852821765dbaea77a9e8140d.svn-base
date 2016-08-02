using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
namespace Workshop
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        #region 加密锁相关定义
        enum Ry4Cmd : ushort
        {
            RY_FIND = 1,			//Find NetRockey4ND
            RY_FIND_NEXT,		//Find next
            RY_OPEN,			//Open NetRockey4ND
            RY_CLOSE,			//Close NetRockey4ND
            RY_READ,			//Read NetRockey4ND
            RY_WRITE,			//Write NetRockey4ND
            RY_RANDOM,			//Generate random
            RY_SEED,			//Generate seed
            RY_READ_USERID = 10,	//Read UID
            RY_CHECK_MODULE = 12,	//Check Module
            RY_CALCULATE1 = 14,	//Calculate1
            RY_CALCULATE2,		//Calculate1
            RY_CALCULATE3,		//Calculate1
        };

        enum Ry4ErrCode : uint
        {
            ERR_SUCCESS = 0,							//No error
            ERR_NO_PARALLEL_PORT = 0x80300001,		//(0x80300001)No parallel port
            ERR_NO_DRIVER,							//(0x80300002)No drive
            ERR_NO_ROCKEY,							//(0x80300003)No NetRockey4ND
            ERR_INVALID_PASSWORD,					//(0x80300004)Invalid password
            ERR_INVALID_PASSWORD_OR_ID,				//(0x80300005)Invalid password or ID
            ERR_SETID,								//(0x80300006)Set id error
            ERR_INVALID_ADDR_OR_SIZE,				//(0x80300007)Invalid address or size
            ERR_UNKNOWN_COMMAND,					//(0x80300008)Unkown command
            ERR_NOTBELEVEL3,						//(0x80300009)Inner error
            ERR_READ,								//(0x8030000A)Read error
            ERR_WRITE,								//(0x8030000B)Write error
            ERR_RANDOM,								//(0x8030000C)Generate random error
            ERR_SEED,								//(0x8030000D)Generate seed error
            ERR_CALCULATE,							//(0x8030000E)Calculate error
            ERR_NO_OPEN,							//(0x8030000F)The NetRockey4ND is not opened
            ERR_OPEN_OVERFLOW,						//(0x80300010)Open NetRockey4ND too more(>16)
            ERR_NOMORE,								//(0x80300011)No more NetRockey4ND
            ERR_NEED_FIND,							//(0x80300012)Need Find before FindNext
            ERR_DECREASE,							//(0x80300013)Dcrease error
            ERR_AR_BADCOMMAND,						//(0x80300014)Band command
            ERR_AR_UNKNOWN_OPCODE,					//(0x80300015)Unkown op code
            ERR_AR_WRONGBEGIN,						//(0x80300016)There could not be constant in first instruction in arithmetic 
            ERR_AR_WRONG_END,						//(0x80300017)There could not be constant in last instruction in arithmetic 
            ERR_AR_VALUEOVERFLOW,					//(0x80300018)The constant in arithmetic overflow
            ERR_UNKNOWN = 0x8030ffff,					//(0x8030FFFF)Unkown error

            ERR_RECEIVE_NULL = 0x80300100,			//(0x80300100)Receive null
            ERR_PRNPORT_BUSY = 0x80300101				//(0x80300101)Parallel port busy

        };
        #endregion
        private DateTime LoginTime = DateTime.Now;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ClsBLL.CheckFormIsOpen("frmBooking"))
                {
                    frmBooking fw = new frmBooking();
                    fw.Show();
                }
                else
                {
                    Form frm = Application.OpenForms["frmBooking"];
                    frm.Focus();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Source + "/" + Err.TargetSite.Attributes.ToString() +"/" + Err.Message);
            }
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
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
        /// <summary>
        /// 打开VansCardMemberDB SQL连接源
        /// </summary>
        /// <param name="type">类型</param>
        private void OpenSqlConnection()
        {
            try
            {
                ConnectionSets connectionsets = new ConnectionSets();

                XmlSerializer mySerializer = new XmlSerializer(typeof(ConnectionSets));
                FileStream myFileStream = new FileStream(Application.StartupPath + @"\DBConnectSet.xml", FileMode.Open);
                connectionsets = (ConnectionSets)mySerializer.Deserialize(myFileStream);
                myFileStream.Close();

                int datasourcetype = connectionsets.Type;
                string server = connectionsets.Server;
                string database = connectionsets.DataBase;
                string user = connectionsets.User;
                string password = ClassTripleDES.DecryptWithKey(connectionsets.Password, ClassTripleDES.passswordkey);

                SqlConnection Mycns = new SqlConnection();
                string connectstring = "Data Source=" + server + ";Initial Catalog=" + database + ";Password=" + password + ";User ID=" + user;
                Mycns.ConnectionString = connectstring;
                try
                {
                    Mycns.Open();
                    if (Mycns.State == ConnectionState.Open)
                    {
                        SQLDbHelper.connectionString = connectstring;
                        AppConfig.ConfigSetValue(Application.ExecutablePath, "Connectstring", connectstring);
                    }
                }
                catch
                {
                }
                finally
                {
                    Mycns.Close();
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show("连接错误!\n\n请设置连接配置后,再重新运行!\n\n" + Err.Message, "连接错误!", MessageBoxButtons.OK);
                bool result = false;
                Process[] myProcesses = Process.GetProcesses();
                foreach (Process myProcess in myProcesses)
                {
                    if (myProcess.ProcessName == "DBConnectSet")
                    {
                        result = true;
                        break;
                    }
                }
                if (!result)
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + @"\DBConnectSet.exe");
                }
                Application.Exit();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            //Rectangle clientRectangle = ClientRectangle;
            //Point clientPoint = PointToScreen(new Point(0, 0));
            //clientRectangle.Offset(clientPoint.X - Left, clientPoint.Y - Top);
            //Region = new Region(clientRectangle); 
            
            panel1.Left = (this.Width - panel1.Width) / 2;
            panel2.Left = (this.Width - panel2.Width) / 2;
            OpenSqlConnection();
            LoginTime = DateTime.Now;
            timer3.Interval = 1000 * 60 * 3;
            CheckRegister();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmWork fw = new frmWork();
            fw.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmCarStateBoad fcsb = new frmCarStateBoad();
            fcsb.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmReport fr = new frmReport();
            fr.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmParts fpts = new frmParts();
            fpts.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            frmSysSet fss = new frmSysSet();
            fss.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            frmLogin fl = new frmLogin();
            fl.Left = button1.Left + panel1.Left;
            fl.Width = button4.Left + button4.Width - button1.Left;
            fl.Height = button5.Top + button5.Height - button1.Top;
            fl.Top = button1.Top + panel1.Top;
            if (fl.ShowDialog() == DialogResult.Cancel)
            {
                this.Close();
            }
            foreach (Control ct in panel1.Controls)
            {
                if (ct.Name.StartsWith("but"))
                {
                    if (!ClsBLL.IsPower(ct.Text))
                    {
                        ct.Enabled = false;
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要关闭系统吗？", "关闭系统", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (DateTime.Now.Hour > 16)
                {
                    object obj = SQLDbHelper.ExecuteScalar("Select Position from Worker Where WorkerName='" + ClsBLL.UserName + "'");
                    if (obj != null)
                    {
                        string usergroup = obj.ToString();
                        if (usergroup == "主管")
                        {
                            string sqlstring = "Select count(*) from Booking Where StartServiceTime between '" + DateTime.Today.ToShortDateString() + "' and '" + DateTime.Now.ToString() + "' and ServiceType='" + ClsBLL.UserGroup + "' and State in ('中断','维修进行中','过时')";
                            int bills = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
                            if (bills > 0)
                            {
                                string message="有" + bills.ToString() + "张单没有完成，请处理完之后再关闭系统！！！" ;
                                if (MessageBox.Show(message, "系统提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    ClsBLL.AddMsg(-1, message + "--" + ClsBLL.UserName);
                                    return;
                                }
                            }
                        }
                    }
                }
                this.Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string sqlstring = "Select * from UpdateMsg where Createdate >'" +LoginTime +"'";
            sqlstring += " And Creator <>'"+ ClsBLL.UserName +"' And ID not in(Select MsgID from UserMsg Where UserName='" + ClsBLL.UserName + "')";
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (!ClsBLL.CheckFormIsOpen("frmMsg") && this.WindowState!=FormWindowState.Minimized)
                    {
                        frmMsg fm = new frmMsg(Dt.Rows[0]["Message"].ToString(), int.Parse(Dt.Rows[0]["BookID"].ToString()), int.Parse(Dt.Rows[0]["ID"].ToString()));
                        fm.Show();
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
                timer2.Enabled = false;
            }
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            frmUpdate fu = new frmUpdate();
            fu.Show();
        }
        private void CheckRegister()
        {
            ClsBLL.IsRegist = false;
            object obj = SQLDbHelper.ExecuteScalar("Select Keyword_value from Setting where keyword='IP'");
            string ip = string.Empty;
            string mac = string.Empty;
            if (obj != null)
            {
                ip = obj.ToString();
            }
            if (ip != string.Empty)
            {
                mac = ClassTripleDES.GetMacAddress(ip);
            }
            object obj2 = SQLDbHelper.ExecuteScalar("Select Keyword_value from Setting where keyword='key'");
            string key =string.Empty;
            if(obj2 != null)
            {
                key = obj2.ToString();
            }
            if (key == ClassTripleDES.EncryptWithKey(mac, ClassTripleDES.passswordkey))
            {
                ClsBLL.IsRegist = true;
            }
        }
    }
}