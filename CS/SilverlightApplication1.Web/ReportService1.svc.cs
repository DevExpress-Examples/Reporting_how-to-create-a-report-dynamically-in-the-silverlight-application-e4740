using System;
using System.Data;
using System.Drawing;
using System.Linq;
using DevExpress.Data.Utils.ServiceModel;
using DevExpress.XtraReports.UI;

namespace SilverlightApplication1.Web {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService1" in code, svc and config file together.
    [SilverlightFaultBehavior]
    public class ReportService1 : DevExpress.XtraReports.Service.ReportService {
        protected override XtraReport CreateReportByName(string reportName) {
            if(reportName == "CustomReport") {
                return CreateXtraReport1();
            }

            return base.CreateReportByName(reportName);
        }

        private XtraReport CreateXtraReport1() {
            XtraReport report = new XtraReport();

            CreateAndFillDataSource(report);
            CreateReportStyles(report);
            CreateReportTable(report);

            return report;
        }

        private void CreateAndFillDataSource(XtraReport report) {
            DataSet1 dataSet = new DataSet1();
            DataSet1TableAdapters.ProductsTableAdapter adapt = new DataSet1TableAdapters.ProductsTableAdapter();
            adapt.Fill(dataSet.Products);

            report.DataSource = dataSet;
            report.DataMember = dataSet.Products.TableName;
        }
        private void CreateReportTable(XtraReport report) {
            DetailBand detail = new DetailBand();
            PageHeaderBand pageHeader = new PageHeaderBand();
            detail.Height = 20;
            pageHeader.Height = 30;

            // Place the bands onto a report
            report.Bands.AddRange(new Band[] { detail, pageHeader });

            DataSet ds = ((DataSet)report.DataSource);
            int colCount = ds.Tables[0].Columns.Count - 3; //-3 (2,3, 10)
            int colWidth = (report.PageWidth - (report.Margins.Left + report.Margins.Right)) / colCount;

            // Create a table to represent headers
            XRTable tableHeader = new XRTable();
            tableHeader.Height = 20;
            tableHeader.Width = (report.PageWidth - (report.Margins.Left + report.Margins.Right));
            XRTableRow headerRow = new XRTableRow();
            headerRow.Width = tableHeader.Width;
            tableHeader.Rows.Add(headerRow);

            tableHeader.BeginInit();

            // Create a table to display data
            XRTable tableDetail = new XRTable();
            tableDetail.Height = 20;
            tableDetail.Width = (report.PageWidth - (report.Margins.Left + report.Margins.Right));
            XRTableRow detailRow = new XRTableRow();
            detailRow.Width = tableDetail.Width;
            tableDetail.Rows.Add(detailRow);
            tableDetail.EvenStyleName = "EvenStyle";
            tableDetail.OddStyleName = "OddStyle";

            tableDetail.BeginInit();

            // Create table cells, fill the header cells with text, bind the cells to data
            for(int i = 0; i < colCount; i++) {
                //Skip some columns
                if(i == 2 || i == 3 || i == 10)
                    continue;
                XRTableCell headerCell = new XRTableCell();
                headerCell.Width = colWidth;
                headerCell.Text = ds.Tables[0].Columns[i].Caption;
                headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                headerCell.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                headerCell.BackColor = Color.DeepSkyBlue;
                headerCell.ForeColor = Color.White;

                XRTableCell detailCell = new XRTableCell();
                detailCell.Width = colWidth;
                detailCell.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                detailCell.DataBindings.Add("Text", null, ds.Tables[0].Columns[i].Caption);

                if(i == 0) {
                    headerCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                } else {
                    headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }

                // Place the cells into the corresponding tables
                headerRow.Cells.Add(headerCell);
                detailRow.Cells.Add(detailCell);
            }
            tableHeader.EndInit();
            tableDetail.EndInit();
            // Place the table onto a report's Detail band
            report.Bands[BandKind.PageHeader].Controls.Add(tableHeader);
            report.Bands[BandKind.Detail].Controls.Add(tableDetail);



        }
        private void CreateReportStyles(XtraReport report) {
            // Create different odd and even styles
            XRControlStyle oddStyle = new XRControlStyle();
            XRControlStyle evenStyle = new XRControlStyle();

            // Specify the odd style appearance
            oddStyle.BackColor = Color.SkyBlue;
            oddStyle.StyleUsing.UseBackColor = true;
            oddStyle.StyleUsing.UseBorders = false;
            oddStyle.Name = "OddStyle";

            // Specify the even style appearance
            evenStyle.BackColor = Color.Snow;
            evenStyle.StyleUsing.UseBackColor = true;
            evenStyle.StyleUsing.UseBorders = false;
            evenStyle.Name = "EvenStyle";

            // Add styles to report's style sheet
            report.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] { oddStyle, evenStyle });
        }

    }
}
