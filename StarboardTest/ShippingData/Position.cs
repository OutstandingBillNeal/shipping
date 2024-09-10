namespace ShippingData;

public class Position
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public static Position? FromPositionReport(PositionReport positionReport)
    {
        return positionReport == null
            || positionReport.Message == null
            || positionReport.Message.Latitude == null
            || positionReport.Message.Longitude == null
            ? null
            : new Position { Latitude = positionReport.Message.Latitude.Value, Longitude = positionReport.Message.Longitude.Value };
    }
}
