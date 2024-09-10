using ShippingData;

namespace AnalysisTests;

public class PositionTests
{
    [Fact]
    public void When_latitude_is_null_then_Position_FromPositionReport_returns_null()
    {
        var message = new Message { Latitude = null, Longitude = 1 };
        var positionReport = new PositionReport { Message = message };
        var position = Position.FromPositionReport(positionReport);
        Assert.Null(position);
    }

    [Fact]
    public void When_longitude_is_null_then_Position_FromPositionReport_returns_null()
    {
        var message = new Message { Latitude = 1, Longitude = null };
        var positionReport = new PositionReport { Message = message };
        var position = Position.FromPositionReport(positionReport);
        Assert.Null(position);
    }

    [Fact]
    public void When_neither_coordinate_is_null_then_Position_FromPositionReport_returns_non_null()
    {
        var message = new Message { Latitude = 1, Longitude = 2 };
        var positionReport = new PositionReport { Message = message };
        var position = Position.FromPositionReport(positionReport);
        Assert.NotNull(position);
        Assert.Equal(message.Latitude, position.Latitude);
        Assert.Equal(message.Longitude, position.Longitude);
    }
}