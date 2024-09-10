using Ardalis.GuardClauses;
using ShippingData;
using System.Text.Json;

namespace Analysis;

public class CourseAnalyser(int maximumBatchSize)
{
    private readonly int _maximumBatchSize = maximumBatchSize;
    private readonly List<PositionReport> _positionReports = new();

    public CourseAnalyserReadResult Read(string jsonString)
    {
        Guard.Against.OutOfRange(_positionReports.Count(), "batch size", 0, _maximumBatchSize);

        var positionReport = JsonSerializer.Deserialize<PositionReport>(jsonString);

        return new CourseAnalyserReadResult();
    }
}
