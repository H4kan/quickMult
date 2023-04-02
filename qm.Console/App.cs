using qm.algorithm.QmAlgorithmFactory;
using qm.console.Options;
using qm.generator;
using qm.utils;
using qm.utils.Interfaces;
using qm.utils.Models;
using System.Diagnostics;

namespace qm.console
{
    public class App
    {
        private readonly IMatrixGenerator _matrixGenerator;
        private readonly IQmWriter _qmWriter;
        private readonly IQmReader _qmReader;
        private readonly IQmAlgorithmFactory<int> _qmAlgorithmFactory;

        public App(IMatrixGenerator matrixGenerator, IQmWriter qmWriter, IQmReader qmReader, IQmAlgorithmFactory<int> qmAlgorithmFactory)
        {
            _matrixGenerator = matrixGenerator;
            _qmWriter = qmWriter;
            _qmReader = qmReader;
            _qmAlgorithmFactory = qmAlgorithmFactory;
        }

        public Task Run(RunOptions options)
        {
            if (!LoadAndValidateInputFile(options.InputFileName, out var resultMatrix))
            {
                return Task.FromResult(-1);
            }

            var qmAlgorithm = _qmAlgorithmFactory.Create(options.Algorithm);
            var solution = qmAlgorithm.ConductAlgorithm(resultMatrix!);

            var solutionFileName = Helpers.GetResultFileName(options.InputFileName!);
            _qmWriter.SaveSolutionToFile(solution, solutionFileName);

            Console.WriteLine($"Problem successfully solved and the result has been saved to the file {solutionFileName}.\nThe calculated result is:");
            Console.WriteLine(Helpers.FormatResult(solution));
            return Task.CompletedTask;
        }

        public Task Generate(GenerateOptions options)
        {
            if (!CheckIfFileNameIsValid(options.OutputFileName))
            {
                Console.WriteLine($"The provided filename {options.OutputFileName} is invalid. Check the file name and try again.");
                return Task.FromResult(-1);
            }

            var randomMatrix = _matrixGenerator.GenerateRandomResultMatrix(options.Size!.Value);
            _qmWriter.SaveMatrixToFile(randomMatrix, options.OutputFileName!);

            Console.WriteLine($"A test instance for {options.Size.Value} players was generated and saved in a file {options.OutputFileName}");
            return Task.CompletedTask;
        }

        public Task Compare(CompareOptions options)
        {
            if (!LoadAndValidateInputFile(options.InputFileName, out var resultMatrix))
            {
                return Task.FromResult(-1);
            }

            var timeComparision = new Dictionary<MatrixAlgorithm, TimeSpan>();
            foreach (MatrixAlgorithm algorithm in Enum.GetValues(typeof(MatrixAlgorithm)))
            {
                var qmAlgorithm = _qmAlgorithmFactory.Create(algorithm);
                var elapsedTime = MeasureExecutionTime(() => qmAlgorithm.ConductAlgorithm(resultMatrix!));
                timeComparision.Add(algorithm, elapsedTime);
            }

            var timeComparisionFileName = Helpers.GetTimeComparisionFileName(options.InputFileName!);
            _qmWriter.SaveTimeComparisionResultsToFile(timeComparision, timeComparisionFileName);

            Console.WriteLine("\nComparison completed successfully. Results saved to: {0}", timeComparisionFileName);
            DisplayTimeResults(timeComparision);
            return Task.CompletedTask;
        }

        private static TimeSpan MeasureExecutionTime(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private bool LoadAndValidateInputFile(string? fileName, out byte[][]? resultsMatrix)
        {
            resultsMatrix = null;

            if (!CheckIfFileNameIsValid(fileName, true))
            {
                Console.WriteLine("The provided filename is invalid. Please ensure the file exists and has a valid format. Check the file name and try again.");
                return false;
            }

            resultsMatrix = _qmReader.LoadFromFile(fileName!);
            if (resultsMatrix == null)
            {
                Console.WriteLine($"The structure of the input file {fileName} is invalid. Please make sure the file follows the correct format and try again.");
                return false;
            }

            return true;
        }

        private static bool CheckIfFileNameIsValid(string? fileName, bool fileShouldExist = false)
        {
            if (string.IsNullOrEmpty(fileName)
            || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
            || fileShouldExist && !File.Exists(Helpers.GetPathForFile(fileName)))
            {
                return false;
            }

            return true;
        }

        private static void DisplayTimeResults(Dictionary<MatrixAlgorithm, TimeSpan> timeComparision)
        {
            Console.WriteLine("\nComparison results:");
            foreach (var entry in timeComparision)
            {
                Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
            }
        }
    }
}