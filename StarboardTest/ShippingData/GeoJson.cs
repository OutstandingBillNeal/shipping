namespace ShippingData;

// Although there are NuGet packages which
// include this type, using them would be
// overkill for such a simple requirement.

public class GeoJson
{
    public static string type { get; set; }
    public Geometry geometry { get; set; }
}

public class Geometry
{
    public string type => "Point";
    public double[] coordinates { get; set; }
}