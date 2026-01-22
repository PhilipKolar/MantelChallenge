using ReportEngine.Data;

namespace ReportEngine.Reports;

public class UniqueIpReport : IReportProcessor
{
    public string GetReport(IEnumerable<ClfDataEntry> data)
    {
        var numberOfIps = data.Distinct(new IpComparer()).Count();
        return $"Number of unique IP addresses: {numberOfIps}"; // TODO: pull string from a resource file for multilingual support
    }
}