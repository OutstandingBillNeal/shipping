namespace ShippingData;

public class PositionReport
{
    public required Message Message { get; set; }
    public int UTCTimeStamp { get; set; }
    public DateTime UTCTime =>
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            + TimeSpan.FromMilliseconds(UTCTimeStamp);
}
