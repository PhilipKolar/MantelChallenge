using ReportEngine.Data;
using ReportEngine.Reports;
using ReportEngineTests.Data;
using Xunit;

namespace ReportEngineTests.Reports;

public class UniqueIpReportTests
{
    [Fact]
    public void GetReport_FullFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new FileDataProvider("./DataFiles/programming-task-example-data.log");
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new UniqueIpReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal("Number of unique IP addresses: 11", results);
    }
    
    [Fact]
    public void GetReport_EmptyFile_ReturnsCorrectResult()
    {
        // Arrange
        var provider = new MockDataProvider([]);
        var parserStream = new RegexParserStream(provider);
        var dataEntries = parserStream.FetchData();
        var sut = new UniqueIpReport();
        
        // Act
        var results = sut.GetReport(dataEntries);
        
        // Assert
        Assert.Equal("Number of unique IP addresses: 0", results);
    }
}