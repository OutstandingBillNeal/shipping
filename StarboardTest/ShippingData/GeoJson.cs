namespace ShippingData;

// Although there are NuGet packages which
// include this type, using them would be
// overkill for such a simple requirement.

public class GeoJson
{
    public required string type { get; set; }
    public required Geometry geometry { get; set; }
}

public class Geometry
{
    public required string type { get; set; }
    public required double[] coordinates { get; set; }
}