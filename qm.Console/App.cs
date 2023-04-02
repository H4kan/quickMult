using qm.console.Options;
using qm.generator;
using qm.naive;
using qm.utils;
using qm.utils.Interfaces;
using qm.utils.Models;

namespace qm.console
{
    public class App
    {
        private readonly IMatrixGenerator _matrixGenerator;
        private readonly IQmWriter _qmWriter;
        private readonly IQmReader _qmReader;

        public App(IMatrixGenerator matrixGenerator, IQmWriter qmWriter, IQmReader qmReader)
        {
            _matrixGenerator = matrixGenerator;
            _qmWriter = qmWriter;
            _qmReader = qmReader;
        }

        public Task Run(RunOptions options)
        {
            if (CheckIfFileNameIsValid(options.InputFileName, true))
            {
                Console.WriteLine("The provided filename is invalid. Please ensure the file exists and has a valid format. Check the file name and try again.");
                return Task.FromResult(-1);
            }

            var resultsMatrix = _qmReader.LoadFromFile(options.InputFileName!);

            var naiveAlg = new NaiveAlgorithm(resultsMatrix!.Length, resultsMatrix);
            var solution = naiveAlg.ConductAlgorithm();

            var solutionFileName = Helpers.GetResultFileName(options.InputFileName!);
            _qmWriter.SaveSolutionToFile(solution, solutionFileName);

            Console.WriteLine($"Problem successfully solved and the result has been saved to the file {solutionFileName}.\n The calculated result is:");
            Console.WriteLine(Helpers.FormatResult(solution));
            return Task.CompletedTask;
        }

        public Task Generate(GenerateOptions options)
        {
            if (CheckIfFileNameIsValid(options.OutputFileName))
            {
                Console.WriteLine("The provided filename is invalid. Check the file name and try again.");
                return Task.FromResult(-1);
            }

            var randomMatrix = _matrixGenerator.GenerateRandomResultMatrix(options.Size!.Value);
            _qmWriter.SaveMatrixToFile(randomMatrix, options.OutputFileName!);

            Console.WriteLine($"A test instance for {options.Size.Value} players was generated and saved in a file {options.OutputFileName}");
            return Task.CompletedTask;
        }

        public Task Compare(CompareOptions options)
        {
            if (CheckIfFileNameIsValid(options.InputFileName, true))
            {
                Console.WriteLine("The provided filename is invalid. Please ensure the file exists and has a valid format. Check the file name and try again.");
                return Task.FromResult(-1);
            }

            var resultsMatrix = _qmReader.LoadFromFile(options.InputFileName!);

            var timeComparision = new Dictionary<MatrixAlgorithm, TimeSpan>();


            _qmWriter.SaveTimeComparisionResultsToFile(timeComparision, options.InputFileName!);

            return Task.CompletedTask;
        }

        private bool CheckIfFileNameIsValid(string? fileName, bool fileShouldExist = false)
        {
            if (string.IsNullOrEmpty(fileName)
            || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
            || fileShouldExist && !File.Exists(Helpers.GetPathForFile(fileName)))
            {
                return false;
            }

            return true;
        }
    }
}