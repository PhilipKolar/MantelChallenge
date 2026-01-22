using System.Globalization;
using System.Text.RegularExpressions;

namespace ReportEngine.Data;

public class RegexParserStream(IDataProvider dataProvider) : IDataStream<ClfDataEntry>
{
    private static readonly Regex ClfRegex = new Regex(
        @"^(?<ip>\S+)\s+" +
        @"(?<ident>\S+)\s+" +
        @"(?<user>\S+)\s+" +
        @"\[(?<time>[^\]]+)\]\s+" +
        @"""(?<request>[^""]*)""\s+" +
        @"(?<status>\d{3})\s+" +
        @"(?<bytes>\S+)\s+" +
        @"""(?<referer>[^""]*)""\s+" +
        @"""(?<agent>[^""]*)""$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public IEnumerable<ClfDataEntry> FetchData()
    {
        string? nextRow;
        while ((nextRow = dataProvider.NextRow()) != null)
            if (TryParseClfLine(nextRow, out var parsedData))
                yield return parsedData;
    }

    private static bool TryParseClfLine(string line, out ClfDataEntry entry)
    {
        entry = null!;

        var m = ClfRegex.Match(line);
        if (!m.Success)
        {
            Console.WriteLine($"Invalid data entry detected, skipping row with value: {line}"); // TODO: Convert to an ILogger.Warning() call
            return false;
        }

        var ip = m.Groups["ip"].Value;
        var ident = m.Groups["ident"].Value;
        var user = m.Groups["user"].Value;
        var timeRaw = m.Groups["time"].Value;
        var request = m.Groups["request"].Value;
        var statusRaw = m.Groups["status"].Value;
        var bytesRaw = m.Groups["bytes"].Value;
        var refererRaw = m.Groups["referer"].Value;
        var agentRaw = m.Groups["agent"].Value;

        var timestamp = DateTimeOffset.MinValue;
        if (timeRaw != "-" && !TryParseApacheTime(timeRaw, out timestamp))
            return false;

        var status = int.MinValue;
        if (statusRaw != "-" && !int.TryParse(statusRaw, NumberStyles.None, CultureInfo.InvariantCulture, out status))
            return false;

        var bytes = long.MinValue;
        if (bytesRaw != "-" && !long.TryParse(bytesRaw, NumberStyles.None, CultureInfo.InvariantCulture, out bytes))
            return false;
        
        TryParseRequestLine(request, out var httpMethod, out var uri, out var httpVersion);

        entry = new ClfDataEntry
        {
            ClientIp = ip == "-" ? null : ip,
            Identity = ident == "-" ? null : ident,
            UserId = user == "-" ? null : user,
            RequestTime = timestamp == DateTimeOffset.MinValue ?  null : timestamp,
            RequestLine = request == "-" ? null :  request,
            HttpStatusCode = status == int.MinValue ? null : status,
            ResponseSize = bytes == long.MinValue ? null : bytes,
            Referer = refererRaw == "-" ? null : refererRaw,
            UserAgent = agentRaw == "-" ? null : agentRaw,
            RequestLineHttpMethod = httpMethod?.ToUpperInvariant(),
            RequestLineUri = uri?.ToLowerInvariant(),
            RequestLineHttpVersion = httpVersion.ToUpperInvariant(),
        };

        return true;
    }
    
    private static bool TryParseApacheTime(string raw, out DateTimeOffset dto)
    {
        // Normalize e.g. "+0200" → "+02:00"
        var normalized = Regex.Replace(raw, @"([+-]\d{2})(\d{2})$", "$1:$2");

        return DateTimeOffset.TryParseExact(
            normalized,
            "dd/MMM/yyyy:HH:mm:ss zzz",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces,
            out dto);
    }
    
    private static bool TryParseRequestLine(
        string requestLine,
        out string httpMethod,
        out string uri,
        out string httpVersion)
    {
        httpMethod = null!;
        uri = null!;
        httpVersion = null!;

        if (string.IsNullOrWhiteSpace(requestLine))
            return false;

        // Split on spaces, but only into 3 parts max
        // This preserves URIs that might contain spaces if they are ever logged quoted.
        var parts = requestLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
            return false;

        httpMethod = parts[0];
        uri = parts[1];
        httpVersion = parts[2];

        return true;
    }
}