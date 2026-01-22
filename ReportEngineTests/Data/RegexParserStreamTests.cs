using ReportEngine.Data;
using Xunit;

namespace ReportEngineTests.Data;

public class RegexParserStreamTests
{
    [Fact]
    public void FetchData_1ValidRow_ReturnsOK()
    {
        // Arrange
        var mockProvider = new MockDataProvider([@"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /intranet-analytics/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7"""]);
        var sut = new RegexParserStream(mockProvider);

        // Act
        var results = sut.FetchData().ToList();

        // Assert
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equivalent(new ClfDataEntry
        {
            ClientIp =  "177.71.128.21",
            UserAgent = "Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7",
            HttpStatusCode = 200,
            Identity = null,
            RequestLineHttpMethod = "GET",
            RequestLineUri = "/intranet-analytics/",
            Referer = null,
            RequestLine = "GET /intranet-analytics/ HTTP/1.1",
            RequestLineHttpVersion =  "HTTP/1.1",
            RequestTime = new DateTimeOffset(2018,07,10,22,21,28,new TimeSpan(2,0,0)),
            ResponseSize = 3574,
            UserId = null
        }, results[0]);
    }
    
    [Fact]
    public void FetchData_2ValidRows_ReturnsOK()
    {
        // Arrange
        var mockProvider = new MockDataProvider([
            @"177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] ""GET /intranet-analytics/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7""",
            @"168.41.191.40 - - [09/Jul/2018:10:11:30 +0200] ""GET http://example.net/faq/ HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (Linux; U; Android 2.3.5; en-us; HTC Vision Build/GRI40) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"""]);
        var sut = new RegexParserStream(mockProvider);

        // Act
        var results = sut.FetchData().ToList();

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
        Assert.Equivalent(new ClfDataEntry
        {
            ClientIp =  "177.71.128.21",
            UserAgent = "Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7",
            HttpStatusCode = 200,
            Identity = null,
            RequestLineHttpMethod = "GET",
            RequestLineUri = "/intranet-analytics/",
            Referer = null,
            RequestLine = "GET /intranet-analytics/ HTTP/1.1",
            RequestLineHttpVersion =  "HTTP/1.1",
            RequestTime = new DateTimeOffset(2018,07,10,22,21,28,new TimeSpan(2,0,0)),
            ResponseSize = 3574,
            UserId = null
        }, results[0]);
        Assert.Equivalent(new ClfDataEntry
        {
            ClientIp =  "168.41.191.40",
            UserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.5; en-us; HTC Vision Build/GRI40) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
            HttpStatusCode = 200,
            Identity = null,
            RequestLineHttpMethod = "GET",
            RequestLineUri = "http://example.net/faq/",
            Referer = null,
            RequestLine = "GET http://example.net/faq/ HTTP/1.1",
            RequestLineHttpVersion =  "HTTP/1.1",
            RequestTime = new DateTimeOffset(2018,07,09,10,11,30,new TimeSpan(2,0,0)),
            ResponseSize = 3574,
            UserId = null
        }, results[1]);
    }

    [Fact]
    public void FetchData_InvalidRow_ReturnsEmpty()
    {
        // Arrange
        var mockProvider = new MockDataProvider([@"72.44.32.10 - - [09/Jul/2018:15:48:07 +0200] ""GET / HTTP/1.1"" 200 3574 ""-"" ""Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0"" junk extra"]);
        var sut = new RegexParserStream(mockProvider);

        // Act
        var results = sut.FetchData().ToList();

        // Assert
        Assert.NotNull(results);
        Assert.Empty(results);
    }
}