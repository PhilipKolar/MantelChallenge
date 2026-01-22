using ReportEngine.Data;
using IISParser;

namespace ReportEngine.Extensions;

// Note: No longer used, see explanation in IisParserStream.cs
public static class IisParserExtensions
{
    public static ClfDataEntry ToClfDataEntry(this IISLogRecord record)
    {
        return new ClfDataEntry
        {
            ClientIp =  record.ClientIp,
            Identity = record.UserAgent,
            UserId = record.UserAgent,
            RequestTime = record.Timestamp,
            RequestLine = $"{record.HttpMethod} {record.UriPath} ${record.HttpVersion}",
            HttpStatusCode = record.StatusCode,
            ResponseSize = record.BytesSent
        };
    }
}