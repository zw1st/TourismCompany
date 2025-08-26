using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Text;

namespace IvanSusaninProject_BusinessLogic.OfficePackage;

public class MigraDocPdfBuilder : BasePdfBuilder
{
    private readonly Document _document;

    public MigraDocPdfBuilder()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _document = new Document();
        DefineStyles();
    }

    public override BasePdfBuilder AddHeader(string header)
    {
        _document.AddSection().AddParagraph(header, "NormalBold");
        return this;
    }

    public override BasePdfBuilder AddParagraph(string text)
    {
        _document.LastSection.AddParagraph(text, "Normal");
        return this;
    }

    public override BasePdfBuilder AddTable(int[] columnsWidths, List<string[]> data)
    {
        if (columnsWidths == null || columnsWidths.Length == 0)
            throw new ArgumentNullException(nameof(columnsWidths));

        if (data == null || data.Count == 0)
            throw new ArgumentNullException(nameof(data));

        if (data.Any(row => row.Length != columnsWidths.Length))
            throw new InvalidOperationException("Количество столбцов не соответствует ширинам");

        var table = new Table();
        table.Borders.Width = 0.75;
        table.Borders.Color = Colors.Black;

        for (int i = 0; i < columnsWidths.Length; i++)
        {
            var column = table.AddColumn(Unit.FromCentimeter(columnsWidths[i]));
            column.Format.Alignment = ParagraphAlignment.Center;
        }

        var headerRow = table.AddRow();
        headerRow.HeadingFormat = true;
        headerRow.Shading.Color = Colors.LightGray;
        headerRow.VerticalAlignment = VerticalAlignment.Center;

        for (int i = 0; i < data[0].Length; i++)
        {
            var cell = headerRow.Cells[i];
            cell.AddParagraph(data[0][i]);
            cell.Format.Font.Bold = true;
            cell.Format.Alignment = ParagraphAlignment.Center;
        }

        for (int i = 1; i < data.Count; i++)
        {
            var dataRow = table.AddRow();
            dataRow.VerticalAlignment = VerticalAlignment.Center;

            for (int j = 0; j < data[i].Length; j++)
            {
                var cell = dataRow.Cells[j];
                cell.AddParagraph(data[i][j]);
                cell.Format.Alignment = ParagraphAlignment.Left;
            }
        }

        _document.LastSection.Add(table);
        _document.LastSection.AddParagraph(); 

        return this;
    }

    public override BasePdfBuilder AddPieChart(string title, List<(string Caption, double Value)> data)
    {
        if (data == null || data.Count == 0)
        {
            return this;
        }

        var chart = new Chart(ChartType.Pie2D);
        var series = chart.SeriesCollection.AddSeries();
        series.Add(data.Select(x => x.Value).ToArray());

        var xseries = chart.XValues.AddXSeries();
        xseries.Add(data.Select(x => x.Caption).ToArray());

        chart.DataLabel.Type = DataLabelType.Percent;
        chart.DataLabel.Position = DataLabelPosition.OutsideEnd;

        chart.Width = Unit.FromCentimeter(16);
        chart.Height = Unit.FromCentimeter(12);

        chart.TopArea.AddParagraph(title);

        chart.XAxis.MajorTickMark = TickMarkType.Outside;

        chart.YAxis.MajorTickMark = TickMarkType.Outside;
        chart.YAxis.HasMajorGridlines = true;

        chart.PlotArea.LineFormat.Width = 1;
        chart.PlotArea.LineFormat.Visible = true;

        chart.TopArea.AddLegend();

        _document.LastSection.Add(chart);

        return this;
    }

    public override Stream Build()
    {
        var stream = new MemoryStream();
        var renderer = new PdfDocumentRenderer(true)
        {
            Document = _document
        };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(stream);
        return stream;
    }

    private void DefineStyles()
    {
        var style = _document.Styles.AddStyle("NormalBold", "Normal");
        style.Font.Bold = true;
    }
}