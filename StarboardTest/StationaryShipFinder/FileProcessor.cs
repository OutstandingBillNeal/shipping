using System.Text;
using Serilog;
using Analysis;
using ShippingData;
using System.Text.Json;

namespace StationaryShipFinder;

internal class FileProcessor(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public async Task Process(string inputFileName, string outputFileName)
    {
        const int BufferSize = 2048;
        const int stopAfterLines = int.MaxValue; // Temporary, to give me short run times if something such as a Console.Write is slowing it down.
        var linesRead = 0;
        var analyser = new CourseAnalyser();

        _logger.Information("Stationary ship finder starting.");

        using (var inputFileStream = File.OpenRead(inputFileName))
        {
            using var inputStreamReader = new StreamReader(inputFileStream, Encoding.UTF8, true, BufferSize);

            string line;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            while ((line = await inputStreamReader.ReadLineAsync()) != null && linesRead <= stopAfterLines)
            {
                linesRead++;
                var result = analyser.Read(line);

                if (result == null
                    || result.NextPositionReport == null
                    || result.NextPositionReport.Message == null
                    || result.NextPositionReport.Message.Latitude == null
                    || result.NextPositionReport.Message.Longitude == null)
                {
                    continue;
                }

                var latitude = result.NextPositionReport.Message.Latitude.Value;
                var longitude = result.NextPositionReport.Message.Longitude.Value;

                var output = new GeoJson
                {
                    geometry = new Geometry
                    {
                        coordinates = [latitude, longitude]
                    }
                };

                var fileLine = JsonSerializer.Serialize(output);
                File.AppendAllLines(outputFileName, [fileLine]);
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        _logger.Information("Done. {0} lines read.", linesRead);
    }
}
