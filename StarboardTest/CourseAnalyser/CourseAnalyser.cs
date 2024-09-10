using ShippingData;
using System.Text.Json;

namespace Analysis;

public class CourseAnalyser()
{
    private readonly Dictionary<int, PositionReport> _positionReports = new();
    private const int SecondsPerHour = 3600;
    private int[] _validMessageTypes = [1, 2, 3, 18, 19, 27];
    private const int _minimumTimeForSpeedCalculation = 60;
    private const double _maximumSpeedToTriggerReporting = 1.0;

    public Position? Read(string jsonString)
    {
        var positionReport = JsonSerializer.Deserialize<PositionReport>(jsonString);

        if (positionReport == null
            || !_validMessageTypes.Contains(positionReport.Message.MessageID))
        {
            // Couldn't get a valid or useful position report from the JSON.
            return null;
        }

        if (!_positionReports.ContainsKey(positionReport.Message.UserID))
        {
            // This vessel has no previously recorded position reports.
            _positionReports.Add(positionReport.Message.UserID, positionReport);
            return null;
        }

        var startPositionReport = _positionReports[positionReport.Message.UserID];
        var endPositionReport = positionReport;
        var secondsTaken = endPositionReport.UTCTimeStamp - startPositionReport.UTCTimeStamp;

        if (secondsTaken < _minimumTimeForSpeedCalculation)
        {
            // Not enough has elapsed since the last report for us to calculate a speed for this vessel.
            _positionReports[positionReport.Message.UserID] = endPositionReport;
            return null;
        }

        var startPosition = Position.FromPositionReport(startPositionReport);
        var endPosition = Position.FromPositionReport(endPositionReport);

        if (startPosition == null || endPosition == null)
        {
            // Both are required to determine speed.
            return null;
        }

        var distanceTravelled = startPosition.NauticalMilesTo(endPosition);
        var knots = distanceTravelled * SecondsPerHour / secondsTaken;

        if (knots > _maximumSpeedToTriggerReporting)
        {
            // This vessel is clipping along nicely.
            // Record its latest position.
            _positionReports[positionReport.Message.UserID] = endPositionReport;
            return null;
        }

        // This vessel has stopped, or is moving slowly.
        // Return its position.
        return endPosition;
    }
}
