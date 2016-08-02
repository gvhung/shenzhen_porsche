using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;
namespace Workshop
{
    public partial class XrPause : DevExpress.XtraReports.UI.XtraReport
    {
        public XrPause(DataTable Dt,string rttitle)
        {
            InitializeComponent();
            xrLabTitle.Text = rttitle;
            try
            {
                xrChart1.Series[0].Points.Clear();
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string reason = Dt.Rows[i][0].ToString();
                    xrChart1.Series[0].Points.Add(new SeriesPoint(reason, new double[] { double.Parse(Dt.Rows[i][1].ToString()) }));
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
        }

    }
}
