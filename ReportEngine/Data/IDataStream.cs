namespace ReportEngine.Data;

public interface IDataStream<T>
{
    IEnumerable<T> FetchData();
}