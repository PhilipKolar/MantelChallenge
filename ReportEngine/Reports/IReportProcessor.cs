using ReportEngine.Data;

namespace ReportEngine.Reports;

public interface IReportProcessor
{
    string GetReport(IEnumerable<ClfDataEntry> data);
}