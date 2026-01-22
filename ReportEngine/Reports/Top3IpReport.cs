using System.Text;
using ReportEngine.Data;

namespace ReportEngine.Reports;

public class Top3IpReport : IReportProcessor
{
    public string GetReport(IEnumerable<ClfDataEntry> data)
    {
        var topIps = data
            .Where(x => x is { ClientIp: not null })
            .GroupBy(x => x.ClientIp)
            .Select(x => new { ClientIp = x.Key, Requests = x.Count() })
            .OrderByDescending(x => x.Requests)
            .Take(3);
        
        var sb = new StringBuilder("The top 3 most active IP addresses are:");

        foreach (var item in topIps)
            sb.AppendLine().Append('\t').Append(item.ClientIp).Append(" : ").Append(item.Requests).Append(" request(s)");

        return sb.ToString();
    }
}