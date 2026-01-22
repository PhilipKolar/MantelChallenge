using ReportEngine.Data;
using ReportEngine.Reports;
using ReportEngineTests.Data;
using Xunit;

namespace ReportEngineTests.Reports;

public class Top3IpReportTests
{
    [Fact]
    public void GetReport_FullFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new FileDataProvider("./DataFiles/programming-task-example-data.log");
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new Top3IpReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal($"The top 3 most active IP addresses are:{Environment.NewLine}\t168.41.191.40 : 4 request(s){Environment.NewLine}\t177.71.128.21 : 3 request(s){Environment.NewLine}\t50.112.00.11 : 3 request(s)", results);
    }
    
    [Fact]
    public void GetReport_EmptyFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new MockDataProvider([]);
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new Top3IpReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal("The top 3 most active IP addresses are:", results);
    }
}