using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraCharts;
namespace Workshop
{
    public partial class XrCars : DevExpress.XtraReports.UI.XtraReport
    {
        public XrCars(string combox)
        {
            InitializeComponent();
            this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
            int years = int.Parse(combox.Substring(0, 4));
            int months = int.Parse(combox.Substring(combox.IndexOf("年") + 1, combox.IndexOf("月") - combox.IndexOf("年") - 1));
            try
            {
                string sqlstring = "select *,datepart(d,BookTime) as bookday,datepart(d,ComeTime) as comeday from Booking Where datepart(yyyy,CreateDate)=" + years + " and datepart(MM,CreateDate)=" + months;
                DataTable Dt = SQLDbHelper.Query(sqlstring).Tables[0];
                //ChartControl barChart = new ChartControl();
                barChart.Titles[0].Text = combox + "预约情况统计表";
                barChart.Series.Clear();
                Series series1 = new Series("预约车辆", ViewType.Bar);
                Series series2 = new Series("到场车辆", ViewType.Bar);

                for (int i = 1; i < 32; i++)
                {
                    string datewhere = years.ToString() + "-" + months.ToString().PadLeft(2, char.Parse("0")) + "-" + i.ToString().PadLeft(2, char.Parse("0"));

                    DataRow[] Dr1 = Dt.Select("bookday=" + i);
                    double db1 = 0;
                    if (Dr1.Length > 0)
                    {
                        db1 = double.Parse(Dr1.Length.ToString());
                    }

                    DataRow[] Dr2 = Dt.Select("comeday=" + i);
                    double db2 = 0;
                    if (Dr2.Length > 0)
                    {
                        db2 = double.Parse(Dr2.Length.ToString());
                    }
                    series1.Points.Add(new SeriesPoint(i.ToString() + "日", new double[] { db1 }));
                    series2.Points.Add(new SeriesPoint(i.ToString() + "日", new double[] { db2 }));
                }
                barChart.Width = PageWidth-20;
                barChart.Height = 500;
                barChart.Series.Add(series1);
                barChart.Series.Add(series2);
            }
            catch (Exception Err)
            {
                throw Err;
            }
        }

    }
}
