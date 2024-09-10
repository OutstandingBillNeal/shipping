using ShippingData;

namespace Analysis;

public static class PositionExtensionMethods
{
    private const double KilometersPerDegreeAtEquator = 111.1111;
    private const double NauticalMilesPerKilometer = 0.539957;

    public static double KilometersTo(this Position start, Position end)
    {
        // TODO: implement haversine
        var degreesLatitude = start.Latitude - end.Latitude;
        var degreesLongitude = start.Longitude - end.Longitude;
        var kmLatitude = degreesLatitude * KilometersPerDegreeAtEquator;
        var kmLongitude = degreesLongitude * KilometersPerDegreeAtEquator * Math.Cos(start.Latitude * Math.PI / 180);
        var totalKm = Math.Sqrt(kmLatitude * kmLatitude + kmLongitude * kmLongitude);
        return totalKm;
    }

    public static double NauticalMilesTo(this Position start, Position end)
    {
        return start.KilometersTo(end) * NauticalMilesPerKilometer;
    }
}
