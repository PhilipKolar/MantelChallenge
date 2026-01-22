using ReportEngine.Data;

namespace ReportEngine.Reports;

public class IpComparer: IEqualityComparer<ClfDataEntry>
{
    public bool Equals(ClfDataEntry? x, ClfDataEntry? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ClientIp == y.ClientIp;
    }

    public int GetHashCode(ClfDataEntry obj)
    {
        return (obj.ClientIp != null ? obj.ClientIp.GetHashCode() : 0);
    }
}