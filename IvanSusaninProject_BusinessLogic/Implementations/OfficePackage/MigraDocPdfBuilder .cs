using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
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