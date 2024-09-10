// The plan:
// 1. Read file from start to finish, building a list of N courses.
//    N is configurable, and can be adjusted based on observed memory consumption.
// 2. Output the results for the first N courses, and save a list of vessel IDs included.
// 3. Repeat 1 & 2, ignoreing any vessels already reported.
using Serilog;
using StationaryShipFinder;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting");

// Validate command line arguments
var commandLineArguments = Environment.GetCommandLineArgs();
if (commandLineArguments.Length != 3) // DLL name is provided automatically as first arg.
{
    throw new ArgumentException("Two command line arguments must be supplied: <input file name> and <output file name>");
}
var inputFileName = commandLineArguments[1];
var outputFileName = commandLineArguments[2];

var processor = new FileProcessor(Log.Logger);
processor.Process(inputFileName, outputFileName);
