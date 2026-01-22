namespace ReportEngine.Data;

/// <summary>
/// Following the fields as defined here: https://en.wikipedia.org/wiki/Common_Log_Format
/// </summary>
public record ClfDataEntry
{
    public string? ClientIp { get; set; }
    /// <summary>
    /// RFC 1413 identity https://en.wikipedia.org/wiki/Ident_protocol
    /// </summary>
    public string? Identity { get; set; }
    public string? UserId { get; set; }
    public DateTimeOffset? RequestTime { get; set; }
    public string? RequestLine { get; set; }
    public int? HttpStatusCode { get; set; }
    /// <summary>
    /// Response size in bytes
    /// </summary>
    public long? ResponseSize { get; set; }
    public string? Referer { get; set; }
    public string? UserAgent { get; set; }
    public string? RequestLineHttpMethod { get; set; }
    public string? RequestLineUri { get; set; }
    public string? RequestLineHttpVersion { get; set; }
    
}