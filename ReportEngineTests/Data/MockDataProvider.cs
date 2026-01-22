using ReportEngine.Data;

namespace ReportEngineTests.Data;

public class MockDataProvider(IList<string> mockData) : IDataProvider
{
    private readonly IList<string> _mockData = mockData;
    private int _mockDataIndex = 0;

    public string? NextRow()
    {
        return _mockDataIndex >= _mockData.Count ? null : _mockData[_mockDataIndex++];
    }
}