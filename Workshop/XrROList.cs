using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace Workshop
{
    public partial class XrROList : DevExpress.XtraReports.UI.XtraReport
    {
        private DataTable ReportData = new DataTable();
        private int PrintDirection = 0;

        /// <summary>
        /// 报表宽度
        /// </summary>
        private int ReportWidth
        {
            get
            {
                if (PrintDirection == 0)
                    return 1066;
                else
                    return 724;
            }
        }

        public XrROList(DataTable Dt, int printDirection,string title)
        {
            InitializeComponent();
            try
            {
                xlabTitle.Text = title;
                ReportData = Dt;
                this.DataSource = Dt;
                PrintDirection = printDirection;
                //
                //设置打印方向
                //
                if (printDirection == 0)
                {
                    this.PaperKind = System.Drawing.Printing.PaperKind.A4Rotated;
                }
                else
                {
                    this.PaperKind = System.Drawing.Printing.PaperKind.A4;
                }

                ShowReportHeader();
                ShowPageHeader();
                ShowDetail();
                if (Dt.Rows[Dt.Rows.Count - 1][0].ToString().Trim() != "合计")
                {
                    ShowGroupFooter();
                }
                ShowPageFooter();
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
            }
        }
        /// <summary>
        /// 设置ReportHeader
        /// </summary>
        private void ShowReportHeader()
        {
            xlabTitle.Width = ReportWidth;

            labPrintDate.Text = "打印时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            labPrintDate.Width = ReportWidth;
        }
        /// <summary>
        /// 设置PageHeader
        /// </summary>
        private void ShowPageHeader()
        {
            int ColumnCount = ReportData.Columns.Count;  //报表内容的列数

            //
            //表
            //
            XRTable PageHeaderTable1 = new XRTable();
            PageHeaderTable1.Width = ReportWidth;
            PageHeaderTable1.Location = new Point(1, 0);
            PageHeaderTable1.Borders = DevExpress.XtraPrinting.BorderSide.All;
            //
            //第一行
            //
            XRTableRow PageHeaderRow1 = new XRTableRow();
            PageHeaderRow1.Size = new Size(ReportWidth, 30);
            //
            //第一行的单元格
            //
            XRTableCell[] PageHeaderCells1 = new XRTableCell[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                string HeaderText = ReportData.Columns[i].ColumnName.ToString().Trim();
                PageHeaderCells1[i] = new XRTableCell();
                PageHeaderCells1[i].Name = "PageHeaderCells1" + i.ToString();
                PageHeaderCells1[i].Text = HeaderText;
                PageHeaderCells1[i].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                PageHeaderCells1[i].Font = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                PageHeaderRow1.Cells.Add(PageHeaderCells1[i]);
            }
            PageHeaderTable1.Rows.Add(PageHeaderRow1);

            PageHeader.Controls.Add(PageHeaderTable1);
        }
        /// <summary>
        /// 设置Detail
        /// </summary>
        private void ShowDetail()
        {
            int ColumnCount = ReportData.Columns.Count;  //报表内容的列数

            //
            //表
            //
            XRTable DetailTable1 = new XRTable();
            DetailTable1.Width = ReportWidth;
            DetailTable1.Location = new Point(1, 0);
            DetailTable1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            //
            //第一行
            //
            XRTableRow DetailRow1 = new XRTableRow();
            DetailRow1.Size = new Size(ReportWidth, 30);
            //
            //第一行的单元格
            //
            XRTableCell[] DetailCells1 = new XRTableCell[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                DetailCells1[i] = new XRTableCell();
                DetailCells1[i].Name = "DetailCells1" + i.ToString();
                DetailCells1[i].Text = ReportData.Columns[i].ColumnName;
                DetailCells1[i].DataBindings.Add("Text", ReportData, ReportData.Columns[i].ColumnName, "");
                if (i == 0)
                {
                    DetailCells1[i].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                }
                else
                {
                    DetailCells1[i].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                }
                DetailCells1[i].Font = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                DetailRow1.Cells.Add(DetailCells1[i]);
            }
            DetailTable1.Rows.Add(DetailRow1);

            Detail.Controls.Add(DetailTable1);
        }
        /// <summary>
        /// 设置GroupFooter
        /// </summary>
        private void ShowGroupFooter()
        {
            int ColumnCount = ReportData.Columns.Count;  //报表内容的列数

            //
            //表
            //
            XRTable GroupFooterTable1 = new XRTable();
            GroupFooterTable1.Width = ReportWidth;
            GroupFooterTable1.Location = new Point(1, 0);
            GroupFooterTable1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            //
            //第一行
            //
            XRTableRow GroupFooterRow1 = new XRTableRow();
            GroupFooterRow1.Size = new Size(ReportWidth, 30);
            //
            //第一行的单元格
            //
            XRTableCell[] GroupFooterCells1 = new XRTableCell[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                GroupFooterCells1[i] = new XRTableCell();
                GroupFooterCells1[i].Name = "GroupFooterCells1" + i.ToString();
                if (i == 0)
                {
                    GroupFooterCells1[i].Text = "合计";
                }
                else
                {
                    //GroupFooterCells1[i].Text = ReportData.Columns[i].DataType.ToString();
                    if (ReportData.Columns[i].DataType.ToString() == "System.Decimal" || ReportData.Columns[i].DataType.ToString() == "System.Int32")
                    {
                        GroupFooterCells1[i].Summary.Running = SummaryRunning.Page;
                        GroupFooterCells1[i].Summary.Func = SummaryFunc.Sum;
                        GroupFooterCells1[i].DataBindings.Add("Text", ReportData, ReportData.Columns[i].ColumnName, "");
                    }
                }
                GroupFooterCells1[i].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                GroupFooterCells1[i].Font = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                GroupFooterRow1.Cells.Add(GroupFooterCells1[i]);
            }
            GroupFooterTable1.Rows.Add(GroupFooterRow1);

            GroupFooter1.Controls.Add(GroupFooterTable1);
        }
        private void ShowPageFooter()
        {
            xrPageInfo1.Width = ReportWidth;
        }
    }
}
