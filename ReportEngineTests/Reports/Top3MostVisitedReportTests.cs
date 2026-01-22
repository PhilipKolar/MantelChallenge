using ReportEngine.Data;
using ReportEngine.Reports;
using ReportEngineTests.Data;
using Xunit;

namespace ReportEngineTests.Reports;

public class Top3MostVisitedReportTests
{
    [Fact]
    public void GetReport_FullFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new FileDataProvider("./DataFiles/programming-task-example-data.log");
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new Top3MostVisitedReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal($"The top 3 most visited URIs are:{Environment.NewLine}\t/docs/manage-websites/ : 2 hit(s){Environment.NewLine}\t/intranet-analytics/ : 1 hit(s){Environment.NewLine}\thttp://example.net/faq/ : 1 hit(s)", results);
    }
    
    [Fact]
    public void GetReport_EmptyFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new MockDataProvider([]);
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new Top3MostVisitedReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal("The top 3 most visited URIs are:", results);
    }
}