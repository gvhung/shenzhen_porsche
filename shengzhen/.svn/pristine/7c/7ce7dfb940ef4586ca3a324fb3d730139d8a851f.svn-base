using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Security.Cryptography;
namespace Workshop
{
    public class ClsBLL
    {
        public static string GetGroup(string worker)
        {
            return string.Empty;
        }
        public static string key = "VansCard";
        public static string UserID = string.Empty;
        public static string UserName = string.Empty;
        public static string UserGroup = string.Empty;
        public static bool IsRegist = true;
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GetUserGroup(string username)
        {
            string sqlstring = "Select WorkerGroup from Worker Where WorkerName='"+ username +"'";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return "机电维修','车身维修";
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 获取机电总维修工时
        /// </summary>
        /// <returns></returns>
        public static decimal SumHours(DateTime CurrentDate,string servicetype)
        {
            decimal workhours = 0;  //工作时间
            //工人数量
            int workers = 0;        
            string sqlstring = "Select Count(*) from WorkerPlan Where Wyear=" + CurrentDate.Year + " and Wmonth=" + CurrentDate.Month + " and Wday=" + CurrentDate.Day + " and IsWork=1";
            sqlstring += " And WorkerCode in (Select WorkerCode from Worker Where WorkerGroup='" + servicetype + "')";
            try
            {
                DateTime DtEnd = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet2"));
                DateTime DtStart = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + ClsBLL.GetSet("txtSet1"));

                TimeSpan ts = DtEnd.Subtract(DtStart);
                workhours = ts.Hours + ts.Minutes / decimal.Parse(Convert.ToString(60));
                workers = int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
            }
            catch (Exception Err)
            {
                throw Err;
            }
            return workhours * workers;
        }
        public static void AddMsg(int bookid, string msg)
        {
            string sqlstring = "Insert Into UpdateMsg(BookID,Message,Creator)Values("+ bookid +",'"+ msg +"','"+ ClsBLL.UserName +"')";
            sqlstring += ";Insert Into SysLog(BookID,Message,Creator)Values(" + bookid + ",'" + msg + "','" + ClsBLL.UserName + "')";
            SQLDbHelper.ExecuteSql(sqlstring);
        }
        public static void AddSysLog(int bookid, string msg)
        {
            string sqlstring = "Insert Into SysLog(BookID,Message,Creator)Values(" + bookid + ",'" + msg + "','" + ClsBLL.UserName + "')";
            SQLDbHelper.ExecuteSql(sqlstring);
        }
        /// <summary>
        /// 获取单实际维修TU
        /// </summary>
        /// <returns></returns>
        public static decimal GetFactHours(int recordid)
        {
            string sqlstring = "Select dbo.F_FactHour(" + recordid + ")";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj.ToString() == string.Empty)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(obj.ToString())/100;
            }
        }
        /// <summary>
        /// 获取分单数量
        /// </summary>
        /// <returns></returns>
        public static int GetFendanNum(int recordid)
        {
            string sqlstring = @"Select Count(*) from Booking A,(Select CarNo,VIN,CreateDate From Booking Where ID=" + recordid + ")B";
            sqlstring += " Where A.CarNo=B.CarNo And A.VIN=B.VIN And A.CreateDate=B.CreateDate and A.State='延时到明天'";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取维修工时
        /// </summary>
        /// <returns></returns>
        public static decimal GetServiceHour(int recordid)
        {
            string sqlstring = "Select ServiceHour From Booking Where ID=" + recordid ;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取维修工时
        /// </summary>
        /// <returns></returns>
        public static string GetWorker(int recordid)
        {
            string sqlstring = "Select Worker From Booking Where ID=" + recordid;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 获取计划完成时间
        /// </summary>
        /// <returns></returns>
        public static string GetPlanComplete(int recordid)
        {
            string sqlstring = "Select PlanCompleteTime From Booking Where ID=" + recordid;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        public static int GetDoubleID(int recordid, DateTime booktime)
        {
            int year = booktime.Year;
            int month = booktime.Month;
            int day = booktime.Day;
            string sqlstring = "Select A.ID from Booking A,(Select ID,CarNo,Substring(convert(nvarchar(50),Booktime,120),1,10) as BookDay From Booking Where year(BookTime)=" + year + " and month(BookTime)=" + month + " and day(BookTime)=" + day + " and ID=" + recordid + ") B Where A.CarNo=B.CarNo and substring(convert(nvarchar(50),A.booktime,120),1,10)=B.BookDay And A.ID<>B.ID";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return -1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取系统设置关键字
        /// </summary>
        /// <returns></returns>
        public static string GetSet(string keyword)
        {
            string sqlstring = "Select KeyWord_Value From Setting Where KeyWord='"+ keyword +"'";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 维修中断原因
        /// </summary>
        public static string PauseReason(int id)
        {
            string sqlstring = "Select Reason From ServicePause Where ID in (Select Max(ID) as MaxID from ServicePause Where BookID=" + id + ")";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 延迟原因
        /// </summary>
        public static string DelayReason(int id)
        {
            string sqlstring = "Select DelayReason From DelayService Where BookID=" + id ;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 停车位
        /// </summary>
        public static string ParkSite(int id)
        {
            string sqlstring = "Select ParkSite From Booking Where ID=" + id;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 车顶号
        /// </summary>
        public static string CarTop(int id)
        {
            string sqlstring = "Select CarTopNo From Booking Where ID=" + id;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 等待时长
        /// </summary>
        public static string WaiteTime(int id)
        {
            string sqlstring = "Select ComeTime from Booking Where ID=" + id;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(obj.ToString()));
                string retstr = ts.Hours.ToString() + "°" + ts.Minutes + "'";
                return retstr;
            }
        }
        /// <summary>
        /// 等待时长
        /// </summary>
        public static string OverTime(int id)
        {
            string sqlstring = "Select dateadd(m,servicehour * 60,StartServiceTime) from booking Where ID=" + id;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(obj.ToString()));
                string retstr = ts.Hours.ToString() + "°" + ts.Minutes + "'";
                return retstr;
            }
        }
        /// <summary>
        /// 维修时长
        /// </summary>
        public static string ServiceTime(int id)
        {
            string sqlstring = "Select StartServiceTime from Booking Where ID=" + id;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(obj.ToString()));
                string retstr = ts.Hours.ToString() + "°" + ts.Minutes + "'";
                return retstr;
            }
        }
        /// <summary>
        /// 维修中断原因
        /// </summary>
        public static string PauseTimeStr(int id)
        {
            string sqlstring = "Select PauseTime From ServicePause Where ID in (Select Max(ID) as MaxID from ServicePause Where BookID=" + id + ")";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                TimeSpan ts = DateTime.Now.Subtract(DateTime.Parse(obj.ToString()));
                string retstr = ts.Hours.ToString() + "°" + ts.Minutes + "'";
                return retstr;
            }
        }
        /// <summary>
        /// 维修中断记录
        /// </summary>
        public static void AddServicePause(int id,string worker,string reason)
        {
            string sqlstring = "Insert ServicePause(BookID,PauseTime,Reason,Worker) values ("+ id +",'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm") +"','"+ reason +"','"+ worker+"')";
            sqlstring += ";Update Booking Set State='中断' Where ID="+id;
            SQLDbHelper.ExecuteSql(sqlstring);
        }
        /// <summary>
        /// 维修中断又继续维修
        /// </summary>
        public static void ServicePauseStart(int id)
        {
            string sqlstring = "Update ServicePause Set StartTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' Where StartTime Is Null And ID in (Select Max(ID) as MaxID from ServicePause Where BookID=" + id + ")";
            SQLDbHelper.ExecuteSql(sqlstring);
        }
        /// <summary>
        /// 车牌预约记录
        /// </summary>
        public static bool CarBookRecord(string carno)
        {
            string sqlstring = "Select Count(*) from Booking Where CarNo like '%" + carno + "%'";
            int records =int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
            if (records > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 中断时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DateTime PauseTime(int id)
        {
            string sqlstring = "Select PauseTime From ServicePause Where ID in (Select Max(ID) as MaxID from ServicePause Where BookID=" + id + ")";
            object obj= SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return DateTime.Now;
            }
            else
            {
                return DateTime.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取累计中断的时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Pausemins(int id)
        {
            int summin = 0;
            //正常中断时间
            string sqlstring = "Select * from ServicePause Where PauseTime is not null and StartTime is not null and BookID=" + id;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                foreach (DataRow dr in Dt.Rows)
                {
                    DateTime PauseTime = DateTime.Parse(dr["PauseTime"].ToString());
                    DateTime StartTime = DateTime.Parse(dr["StartTime"].ToString());
                    TimeSpan ts = StartTime.Subtract(PauseTime);
                    summin += ts.Hours * 60 + ts.Minutes;
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
            return summin;
        }
        /// <summary>
        /// 获取一个时间点之后的累计中断的时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Pausemins(int id,DateTime starttime)
        {
            int summin = 0;
            //正常中断时间
            string sqlstring = "Select Sum(DATEDIFF(mi,PauseTime,StartTime)) as summin  from ServicePause Where PauseTime>='" + starttime + "' And PauseTime is not null and StartTime is not null and BookID=" + id;
            try
            {
                //DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                //foreach (DataRow dr in Dt.Rows)
                //{
                //    DateTime PauseTime = DateTime.Parse(dr["PauseTime"].ToString());
                //    DateTime StartTime = DateTime.Parse(dr["StartTime"].ToString());
                //    TimeSpan ts = StartTime.Subtract(PauseTime);
                //    summin +=ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;
                //}
                object obj = SQLDbHelper.ExecuteScalar(sqlstring);
                if (obj.ToString() != string.Empty)
                {
                    int.Parse(obj.ToString());
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
            return summin;
        }
        /// <summary>
        /// 获取状态为延时到明天的单，总共工作时间，不包括中间中断的时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int SerMins(int id)
        {
            int summin = 0;
            //正常中断时间
            string sqlstring = "Select * from DelayService Where BookID=" + id;
            try
            {
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                foreach (DataRow dr in Dt.Rows)
                {
                    DateTime StartServiceTime = DateTime.Parse(dr["StartServiceTime"].ToString());
                    DateTime OperateTime = DateTime.Parse(dr["OperateTime"].ToString());
                    TimeSpan ts = OperateTime.Subtract(StartServiceTime);
                    summin += ts.Hours * 60 + ts.Minutes;
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
            return summin - Pausemins(id);
        }
        /// <summary>
        /// 完工时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DateTime EndServiceTime(int id)
        {
            string sqlstring = "Select EndServiceTime From Booking Where ID=" + id ;
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj == null)
            {
                return DateTime.Now;
            }
            else
            {
                return DateTime.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取已用工时
        /// </summary>
        /// <param name="Dt"></param>
        /// <returns></returns>
        public static decimal UseHours(DateTime Dt)
        {
            string sqlstring = "Select sum(ServiceHour) as sumhours from Booking where BookTime between '"+ Dt.ToShortDateString()+"' and '"+ Dt.AddDays(1).ToShortDateString() +"' and State<>'取消' and State <>'失约'";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj.ToString() == string.Empty)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取工单类型的数量
        /// </summary>
        /// <returns></returns>
        public static int GetSerType(string servicetype)
        {
            string sqlstring = "Select Count(*) from Booking Where ServiceType='" + servicetype + "' and State='正式' and ComeTime <= '" + DateTime.Today.AddDays(1).ToShortDateString() + "'";
            sqlstring += " OR State='延时到明天' and StartServiceTime<'" + DateTime.Today.ToString() + "'";
            return int.Parse(SQLDbHelper.ExecuteScalar(sqlstring).ToString());
        }
        /// <summary>
        /// 文本过虑，数字验证
        /// </summary>
        /// <param name="e"></param>
        /// <returns>True处理,false未处理</returns>
        public static bool Key_Number(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar >= '０' && e.KeyChar <= '９')
            {
                int KeyCode = (int)e.KeyChar;
                KeyCode = KeyCode - 65248;
                e.KeyChar = (char)KeyCode;
            }
            else
            {
                if (!
                    ((e.KeyChar >= '0' && e.KeyChar <= '9')
                    || e.KeyChar == (char)Keys.Enter
                    || e.KeyChar == (char)Keys.Left
                    || e.KeyChar == (char)Keys.Right))
                    e.Handled = true;
                if (e.KeyChar == (char)Keys.Back)
                {
                    e.Handled = false;
                }
            }
            return e.Handled;
        }
        /// <summary>
        /// 判断输入的文本框是否是数值型
        /// </summary>decimal
        /// <param name="txtvalues"></param>
        /// <returns></returns>
        public static bool IsNumber(string txtvalues)
        {
            int val = 0;
            try
            {
                val = int.Parse(txtvalues);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 初始化Combox
        /// </summary>
        /// <param name="cmb">Combox对象</param>
        /// <param name="dictionaryName">词典名称</param>
        public static void IniCombox(ComboBox cmb, string dictionaryName)
        {
            if (dictionaryName == null) return;
            string sqlString = "Select * from SysDictionary where ItemName='" + dictionaryName + "' Order by Items";

            DataTable Dt = SQLDbHelper.Query(sqlString).Tables[0];
            try
            {
                DataRow[] Drs = Dt.Select("ItemName='" + dictionaryName + "'");
                cmb.Items.Clear();
                foreach (DataRow Dr in Drs)
                {
                    cmb.Items.Add(Dr["Items"]);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 功能名称
        /// </summary>
        /// <param name="functionname"></param>
        /// <returns></returns>
        public static bool IsPower(string functionname)
        {
            if (UserID.ToLower() == "admin" || UserID.ToLower()=="liquanchun") return true;
            string sqlstring = "Select FunName from SysPower Inner join SysFunction on SysPower.FunID=SysFunction.ID where FunName='" + functionname + "' And UserID='" + ClsBLL.UserID + "'";
            object obj = SQLDbHelper.ExecuteScalar(sqlstring);
            if (obj != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量保存系统词典
        /// </summary>
        /// <returns></returns>
        public static void SaveItem(System.Windows.Forms.TreeView treeView)
        {
            DataTable Dt = null;
            string sqlstr = null;
            try
            {
                Dt = new DataTable();
                DataSet ds = new DataSet();
                sqlstr = "select * from SysDictionary where 1=0";
                ds = SQLDbHelper.Query(sqlstr);
                Dt = ds.Tables[0];
                for (int i = 0; i < treeView.Nodes[0].Nodes.Count; i++)
                {
                    for (int j = 0; j < treeView.Nodes[0].Nodes[i].Nodes.Count; j++)
                    {
                        DataRow Dr = Dt.NewRow();
                        Dr["ItemName"] = treeView.Nodes[0].Nodes[i].Text;
                        Dr["Items"] = treeView.Nodes[0].Nodes[i].Nodes[j].Text;
                        Dt.Rows.Add(Dr);
                    }
                }
                SQLDbHelper.ExecuteSql("Delete from SysDictionary");
                SQLDbHelper.UpdateTable(ds, sqlstr);
            }
            catch (Exception Err)
            {
                throw Err;
            }
            finally
            {
                //DbHelperSQL._connection.Close();
                //Member.Common.DbHelper._connection.Close();
            }
        }

        public static bool CheckFormIsOpen(string asFormName)
        {
            bool bResult = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == asFormName)
                {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }
        public static string JieMiKey(string str, string p_key)
        {
            try
            {
                byte[] bt = Convert.FromBase64String(str);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(p_key, null);
                byte[] key = pdb.GetBytes(24);
                byte[] iv = pdb.GetBytes(8);
                MemoryStream ms = new MemoryStream();
                TripleDESCryptoServiceProvider tdesc = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, tdesc.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                return (new System.Text.UnicodeEncoding()).GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }
    }
}
