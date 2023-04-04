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

        public async Task Run(RunOptions options)
        {
            if (!LoadAndValidateInputFile(options.InputFileName, out var resultMatrix))
            {
                return;
            }

            Console.Write("Solving problem... ");
            var solution = await ExecuteWithProgressIndicator(
                () => SolveProblemUsingAlgorithm(options.Algorithm, resultMatrix!));

            var solutionFileName = Helpers.GetResultFileName(options.InputFileName!);
            _qmWriter.SaveSolutionToFile(solution, solutionFileName);

            Console.WriteLine($"\nProblem successfully solved and the result has been saved to the file {solutionFileName}.\nThe calculated result is:");
            Console.WriteLine($"{Helpers.FormatResult(solution)}\n");
        }

        public async Task GenerateRandom(GenerateRandomOptions options)
        {
            await GenerateMatrix(options, opts => _matrixGenerator.GenerateRandomResultMatrix(opts.Size!.Value), "random");
        }

        public async Task GeneratePower(GeneratePowerOptions options)
        {
            await GenerateMatrix(options, opts => _matrixGenerator.GeneratePowerMatrix(opts.Size!.Value, opts.Range), "power");
        }

        public async Task GenerateLoser(GenerateLoserOptions options)
        {
            await GenerateMatrix(options, opts => _matrixGenerator.GenerateLoserResultMatrix(opts.Size!.Value, opts.PercentageOfLosers), "loser");
        }

        public async Task Compare(CompareOptions options)
        {
            if (!LoadAndValidateInputFile(options.InputFileName, out var resultMatrix))
            {
                return;
            }

            Console.Write("Performing comparisons... ");
            var timeComparision = await ExecuteWithProgressIndicator(
                () => PerformComparison(resultMatrix!));

            var timeComparisionFileName = Helpers.GetTimeComparisionFileName(options.InputFileName!);
            _qmWriter.SaveTimeComparisionResultsToFile(timeComparision, timeComparisionFileName);

            Console.WriteLine("\nComparison completed successfully. Results saved to: {0}", timeComparisionFileName);
            DisplayTimeResults(timeComparision);
        }

        private async Task GenerateMatrix<T>(T options, Func<T, byte[][]> generateMatrixFunc, string generatorType) where T : BaseGenerateOptions
        {
            if (!CheckIfFileNameIsValid(options.OutputFileName))
            {
                Console.WriteLine($"The provided filename {options.OutputFileName} is invalid. Check the file name and try again.\n");
                return;
            }

            Console.Write($"Generating {generatorType} matrix... ");
            var randomMatrix = await ExecuteWithProgressIndicator(
                    () => generateMatrixFunc(options));
            _qmWriter.SaveMatrixToFile(randomMatrix, options.OutputFileName!);

            Console.WriteLine($"\nA test instance for {options.Size!.Value} players was generated using {generatorType} matrix generator and saved in a file {options.OutputFileName}\n");
        }

        private List<int> SolveProblemUsingAlgorithm(MatrixAlgorithm algorithm, byte[][] resultMatrix)
        {
            var qmAlgorithm = _qmAlgorithmFactory.Create(algorithm);
            return qmAlgorithm.ConductAlgorithm(resultMatrix);
        }

        private IDictionary<MatrixAlgorithm, TimeSpan> PerformComparison(byte[][] resultMatrix)
        {
            var timeComparision = new Dictionary<MatrixAlgorithm, TimeSpan>();

            foreach (MatrixAlgorithm algorithm in Enum.GetValues(typeof(MatrixAlgorithm)))
            {
                var qmAlgorithm = _qmAlgorithmFactory.Create(algorithm);
                var elapsedTime = MeasureExecutionTime(() => qmAlgorithm.ConductAlgorithm(resultMatrix));
                timeComparision.Add(algorithm, elapsedTime);
            }

            return timeComparision;
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

        private static void DisplayTimeResults(IDictionary<MatrixAlgorithm, TimeSpan> timeComparision)
        {
            Console.WriteLine("Comparison results:");
            foreach (var entry in timeComparision)
            {
                Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
            }
            Console.WriteLine();
        }

        private static async Task ShowProgressIndicator(CancellationToken cancellationToken)
        {
            var spinner = new[] { '|', '/', '-', '\\' };
            int counter = 0;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.Write(spinner[counter++ % spinner.Length]);
                    await Task.Delay(100, cancellationToken);
                    Console.Write("\b");
                }
            }
            catch (OperationCanceledException) { }
        }

        private static async Task<T> ExecuteWithProgressIndicator<T>(Func<T> taskToExecute)
        {
            using var cts = new CancellationTokenSource();
            var progressTask = ShowProgressIndicator(cts.Token);
            var result = await Task.Run(taskToExecute);

            cts.Cancel();
            await progressTask;

            return result;
        }
    }
}