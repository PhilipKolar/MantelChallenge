using System.IO;
using IISParser;
using ReportEngine.Extensions;

namespace ReportEngine.Data;

/*
 * Using the IISParser does NOT work - it returns 0 records from engine.ParseLog(). This is because it expects a header
 * row (see code in IISParser's ParserEngine.cs T? ProcessLine<T>(string line, Func<T> factory)). We could work around
 * this by either injecting a header row at the top of our file, but we may not want to mutate the log file if it's
 * being appended to live. We could alternatively make a temporary copy of the log file, inject the header row and clean
 * up afterward, which would be a fast hacky solution but may not be desirable particularly if the log can grow large.
 * 
 * Therefore, I'm abandoning IISParser and this class, just leaving it here for context
 */
public class IisParserStream : IDataStream<ClfDataEntry>
{
    private readonly string _path;

    public IisParserStream(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        _path = path;
    }

    public IEnumerable<ClfDataEntry> FetchData()
    {
        if (!File.Exists(_path))
            throw new FileNotFoundException(_path);
        
        var engine = new ParserEngine(_path);
        var records = engine.ParseLog();
        foreach (var r in records)
            yield return r.ToClfDataEntry();
    }
}