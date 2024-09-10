﻿using System.Text;
using Serilog;
using Analysis;
using ShippingData;
using System.Text.Json;
using System.Xml.Schema;

namespace StationaryShipFinder;

internal class FileProcessor(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public async Task Process(string inputFileName, string outputFileName)
    {
        const int BufferSize = 2048;
        const int stopAfterLines = int.MaxValue; // Temporary, to give me short run times if something such as a Console.Write is slowing it down.
        const double deDupingDistanceKm = 10.0;
        var linesRead = 0;
        var linesWritten = 0;
        var analyser = new CourseAnalyser();
        var stationaryPositions = new List<Position>();

        _logger.Information("Stationary ship finder starting.");

        using (var inputFileStream = File.OpenRead(inputFileName))
        {
            using var inputStreamReader = new StreamReader(inputFileStream, Encoding.UTF8, true, BufferSize);

            string line;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            while ((line = await inputStreamReader.ReadLineAsync()) != null && linesRead <= stopAfterLines)
            {
                linesRead++;
                var stationaryPosition = analyser.Read(line);

                if (stationaryPosition == null)
                {
                    continue;
                }

                // Define a square around this new point...
                var oneCorner = stationaryPosition.Transpose(-deDupingDistanceKm, -deDupingDistanceKm);
                var otherCorner = stationaryPosition.Transpose(deDupingDistanceKm, deDupingDistanceKm);
                // ... and find out if we've already recorded a point within that square.
                var alreadyRecorded = stationaryPositions
                    .Any(sp => sp.Latitude >= oneCorner.Latitude && sp.Latitude <= otherCorner.Latitude
                        && sp.Longitude >= oneCorner.Longitude && sp.Longitude <= otherCorner.Longitude);

                if (alreadyRecorded)
                {
                    continue;
                }

                var output = new GeoJson
                {
                    geometry = new Geometry
                    {
                        coordinates = [stationaryPosition.Latitude, stationaryPosition.Longitude]
                    }
                };

                var fileLine = JsonSerializer.Serialize(output);
                await File.AppendAllLinesAsync(outputFileName, [fileLine]);
                linesWritten++;
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        _logger.Information("Done. {0} lines read, {1} lines written.", linesRead, linesWritten);
    }
}
