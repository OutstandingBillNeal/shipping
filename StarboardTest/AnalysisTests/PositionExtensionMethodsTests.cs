using Analysis;
using ShippingData;

namespace AnalysisTests;

public class PositionExtensionMethodsTests
{
    [Theory]
    [InlineData(0.0, 111.1)]
    [InlineData(15.0, 107.3)]
    [InlineData(30.0, 96.2)]
    [InlineData(45.0, 78.6)]
    [InlineData(60.0, 55.6)]
    [InlineData(75.0, 28.8)]
    [InlineData(90.0, 0.0)]
    public void Degrees_of_longitude_are_smaller_at_the_poles(
        double degrees, double expectedKilometers)
    {
        var start = new Position { Latitude = degrees, Longitude = 0 };
        var end = new Position { Latitude = degrees, Longitude = 1 };
        var distance = start.KilometersTo(end);

        Assert.Equal(expectedKilometers, distance, 1);
    }
}
