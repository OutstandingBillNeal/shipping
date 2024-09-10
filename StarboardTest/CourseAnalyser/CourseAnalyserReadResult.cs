using ShippingData;

namespace Analysis;

public class CourseAnalyserReadResult
{
    public PositionReport FirstPositionReport { get; set; }
    public PositionReport NextPositionReport { get; set; }
    private double SpeedInKnots()
    {
        return 1;
    }
}
