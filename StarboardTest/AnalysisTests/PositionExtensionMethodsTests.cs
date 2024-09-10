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
        double latitude, double expectedKilometers)
    {
        var start = new Position { Latitude = latitude, Longitude = 0 };
        var end = new Position { Latitude = latitude, Longitude = 1 };
        var distance = start.KilometersTo(end);

        Assert.Equal(expectedKilometers, distance, 1);
    }

    [Theory]
    [InlineData(0.0, 111.1)]
    [InlineData(15.0, 107.3)]
    [InlineData(30.0, 96.2)]
    [InlineData(45.0, 78.6)]
    [InlineData(60.0, 55.6)]
    [InlineData(75.0, 28.8)]
    //[InlineData(90.0, 0.0)] can't go a kilometer east at this latitude
    public void Transposing_longitude_by_one_kilometer_gives_correct_longitude(
        double latitude, double kilometersPerDegreeLongitude)
    {
        var expectedLongitude = 1 / kilometersPerDegreeLongitude;
        var start = new Position { Latitude = latitude, Longitude = 0 };
        var end = start.Transpose(0, 1);

        Assert.Equal(expectedLongitude, end.Longitude, 1);
    }
}
