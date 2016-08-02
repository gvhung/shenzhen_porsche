using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
namespace Workshop
{
    public partial class frmChart : Form
    {
        public frmChart()
        {
            InitializeComponent();
        }

        private void frmChart_Load(object sender, EventArgs e)
        {
            // Create an empty chart.
            ChartControl barChart = new ChartControl();

            // Create the first side-by-side bar series and add points to it.
            Series series1 = new Series("Side-by-Side Bar Series 1", ViewType.Bar);
            series1.Points.Add(new SeriesPoint("I", new double[] { 10 }));
            series1.Points.Add(new SeriesPoint("II", new double[] { 12 }));
            series1.Points.Add(new SeriesPoint("III", new double[] { 14 }));
            series1.Points.Add(new SeriesPoint("IV", new double[] { 17 }));

            // Create the second side-by-side bar series and add points to it.
            Series series2 = new Series("Side-by-Side Bar Series 2", ViewType.Bar);
            series2.Points.Add(new SeriesPoint("I", new double[] { 15 }));
            series2.Points.Add(new SeriesPoint("II", new double[] { 18 }));
            series2.Points.Add(new SeriesPoint("III", new double[] { 25 }));
            series2.Points.Add(new SeriesPoint("IV", new double[] { 33 }));

            // Add series to the chart.
            barChart.Series.Add(series1);
            barChart.Series.Add(series2);

            // Hide the legend (if necessary).
            //barChart.Legend.Visible = false;

            // Rotate the diagram (if necessary).
            ((XYDiagram)barChart.Diagram).Rotated = false;
            // Add a chart to the form.
            barChart.Size = new System.Drawing.Size(400, 250);
            this.Controls.Add(barChart);
        }
    }
}