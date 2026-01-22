namespace ReportEngine.Data;

public class FileDataProvider : IDataProvider, IDisposable
{
    private readonly StreamReader _reader;

    public FileDataProvider(string path)
    {   
        if (!File.Exists(path))
            throw new FileNotFoundException(path);
        
        _reader = new StreamReader(path);
    }

    public string? NextRow() => !_reader.EndOfStream ? _reader.ReadLine() : null; //TODO: Convert to async

    public void Dispose() => _reader.Dispose();
}