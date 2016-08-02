using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Workshop
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.XtraEditors.Controls.Localizer.Active = new DevExpress.LocalizationCHS.XtraEditorsLocalizer();
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new DevExpress.LocalizationCHS.XtraGridLocalizer();
            try
            {
                //if (!System.IO.File.Exists(System.Environment.SystemDirectory + @"\NetRockey4NDCom.dll"))
                //{
                //    System.IO.File.Copy(Application.StartupPath + @"\NetRockey4NDCom.dll", System.Environment.SystemDirectory + @"\NetRockey4NDCom.dll");
                //    //注册COM组件
                //    string DllPath = System.Environment.SystemDirectory + @"\NetRockey4NDCom.dll";
                //    System.Diagnostics.Process.Start("CMD.exe", "/c regsvr32/s " + DllPath);
                //}
                Application.Run(new frmMain());
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Source + "/" + Err.TargetSite.Attributes.ToString() + "/" + Err.Message);
            }
        }
    }
}