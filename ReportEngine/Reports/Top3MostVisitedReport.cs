using System.Text;
using ReportEngine.Data;

namespace ReportEngine.Reports;

public class Top3MostVisitedReport : IReportProcessor
{
    public string GetReport(IEnumerable<ClfDataEntry> data)
    {
        var topUris = data
            .Where(x => x is { RequestLineHttpMethod: "GET", RequestLineUri: not null })
            .GroupBy(x => x.RequestLineUri)
            .Select(x => new { Uri = x.Key, Hits = x.Count() })
            .OrderByDescending(x => x.Hits)
            .Take(3);
        var sb = new StringBuilder("The top 3 most visited URIs are:");

        foreach (var item in topUris)
            sb.AppendLine().Append('\t').Append(item.Uri).Append(" : ").Append(item.Hits).Append(" hit(s)");

        return sb.ToString();
    }
}