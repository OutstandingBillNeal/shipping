using System.Text;
using Serilog;

namespace StationaryShipFinder;

internal class FileProcessor(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public void Process(string inputFileName, string outputFileName)
    {
        const int BufferSize = 2048;
        const int numberOfCoursesPerBatch = 1000;
        const int stopAfterLines = int.MaxValue; // Temporary, to give me short run times if something such as a Console.Write is slowing it down.
        var linesRead = 0;

        _logger.Information("Stationary ship finder starting, batch size: {0}.", numberOfCoursesPerBatch);

        using (var inputFileStream = File.OpenRead(inputFileName))
        {
            using var inputStreamReader = new StreamReader(inputFileStream, Encoding.UTF8, true, BufferSize);
            string line;

            while ((line = inputStreamReader.ReadLine()) != null && linesRead <= stopAfterLines)
            {
                linesRead++;
                // Process line
                //Console.WriteLine(line);
            }
        }

        _logger.Information("Done. {0} lines read.", linesRead);
    }
}
