Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports DevExpress.Data.Utils.ServiceModel
Imports DevExpress.XtraReports.UI

Namespace SilverlightApplication1.Web
	' NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService1" in code, svc and config file together.
	<SilverlightFaultBehavior> _
	Public Class ReportService1
		Inherits DevExpress.XtraReports.Service.ReportService
		Protected Overrides Function CreateReportByName(ByVal reportName As String) As XtraReport
			If reportName = "CustomReport" Then
				Return CreateXtraReport1()
			End If

			Return MyBase.CreateReportByName(reportName)
		End Function

		Private Function CreateXtraReport1() As XtraReport
			Dim report As New XtraReport()

			CreateAndFillDataSource(report)
			CreateReportStyles(report)
			CreateReportTable(report)

			Return report
		End Function

		Private Sub CreateAndFillDataSource(ByVal report As XtraReport)
			Dim dataSet As New DataSet1()
			Dim adapt As New DataSet1TableAdapters.ProductsTableAdapter()
			adapt.Fill(dataSet.Products)

			report.DataSource = dataSet
			report.DataMember = dataSet.Products.TableName
		End Sub
		Private Sub CreateReportTable(ByVal report As XtraReport)
			Dim detail As New DetailBand()
			Dim pageHeader As New PageHeaderBand()
			detail.Height = 20
			pageHeader.Height = 30

			' Place the bands onto a report
			report.Bands.AddRange(New Band() { detail, pageHeader })

			Dim ds As DataSet = (CType(report.DataSource, DataSet))
			Dim colCount As Integer = ds.Tables(0).Columns.Count - 3 '-3 (2,3, 10)
			Dim colWidth As Integer = (report.PageWidth - (report.Margins.Left + report.Margins.Right)) / colCount

			' Create a table to represent headers
			Dim tableHeader As New XRTable()
			tableHeader.Height = 20
			tableHeader.Width = (report.PageWidth - (report.Margins.Left + report.Margins.Right))
			Dim headerRow As New XRTableRow()
			headerRow.Width = tableHeader.Width
			tableHeader.Rows.Add(headerRow)

			tableHeader.BeginInit()

			' Create a table to display data
			Dim tableDetail As New XRTable()
			tableDetail.Height = 20
			tableDetail.Width = (report.PageWidth - (report.Margins.Left + report.Margins.Right))
			Dim detailRow As New XRTableRow()
			detailRow.Width = tableDetail.Width
			tableDetail.Rows.Add(detailRow)
			tableDetail.EvenStyleName = "EvenStyle"
			tableDetail.OddStyleName = "OddStyle"

			tableDetail.BeginInit()

			' Create table cells, fill the header cells with text, bind the cells to data
			For i As Integer = 0 To colCount - 1
				'Skip some columns
				If i = 2 OrElse i = 3 OrElse i = 10 Then
					Continue For
				End If
				Dim headerCell As New XRTableCell()
				headerCell.Width = colWidth
				headerCell.Text = ds.Tables(0).Columns(i).Caption
				headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
				headerCell.Font = New Font("Segoe UI", 10, FontStyle.Bold)
				headerCell.BackColor = Color.DeepSkyBlue
				headerCell.ForeColor = Color.White

				Dim detailCell As New XRTableCell()
				detailCell.Width = colWidth
				detailCell.Font = New Font("Segoe UI", 10, FontStyle.Regular)
				detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
				detailCell.DataBindings.Add("Text", Nothing, ds.Tables(0).Columns(i).Caption)

				If i = 0 Then
					headerCell.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
					detailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom
				Else
					headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All
					detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All
				End If

				' Place the cells into the corresponding tables
				headerRow.Cells.Add(headerCell)
				detailRow.Cells.Add(detailCell)
			Next i
			tableHeader.EndInit()
			tableDetail.EndInit()
			' Place the table onto a report's Detail band
			report.Bands(BandKind.PageHeader).Controls.Add(tableHeader)
			report.Bands(BandKind.Detail).Controls.Add(tableDetail)



		End Sub
		Private Sub CreateReportStyles(ByVal report As XtraReport)
			' Create different odd and even styles
			Dim oddStyle As New XRControlStyle()
			Dim evenStyle As New XRControlStyle()

			' Specify the odd style appearance
			oddStyle.BackColor = Color.SkyBlue
			oddStyle.StyleUsing.UseBackColor = True
			oddStyle.StyleUsing.UseBorders = False
			oddStyle.Name = "OddStyle"

			' Specify the even style appearance
			evenStyle.BackColor = Color.Snow
			evenStyle.StyleUsing.UseBackColor = True
			evenStyle.StyleUsing.UseBorders = False
			evenStyle.Name = "EvenStyle"

			' Add styles to report's style sheet
			report.StyleSheet.AddRange(New DevExpress.XtraReports.UI.XRControlStyle() { oddStyle, evenStyle })
		End Sub

	End Class
End Namespace
