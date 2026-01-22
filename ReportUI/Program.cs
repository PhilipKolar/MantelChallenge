using ReportEngine.Data;
using ReportEngine.Reports;
using static System.Console;

namespace ReportUI;

class Program
{
    static void Main()
    {
        // TODO: Set up DI instead of newing up classes
        IDataProvider fileProvider = new FileDataProvider("./DataFiles/programming-task-example-data.log");
        IDataStream<ClfDataEntry> dataStream = new RegexParserStream(fileProvider);
        
        var logData = dataStream.FetchData();
        RunReports(logData);
    }

    private static void RunReports(IEnumerable<ClfDataEntry> data)
    {
        // We need to enumerate everything into memory here, this solution may not work for extremely large files. we
        // would need to use a different approach that manually tracks aggregate data as the iterator steps through the
        // enumerable (each new report would need to extend this), and we'd lose the convenience of our LINQ expressions.
        var dataList = data.ToList();
        
        var uniqueIpReport = new UniqueIpReport();
        WriteLine(uniqueIpReport.GetReport(dataList));
        
        var top3MostVisitedReport = new Top3MostVisitedReport();
        WriteLine(top3MostVisitedReport.GetReport(dataList));
        
        var top3IpReport = new Top3IpReport();
        WriteLine(top3IpReport.GetReport(dataList));
    }
}