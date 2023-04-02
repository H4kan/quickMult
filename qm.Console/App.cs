using qm.console.Options;
using qm.generator;
using qm.utils.Interfaces;

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
        return Task.CompletedTask;
    }

    public Task Generate(GenerateOptions options)
    {
        if(String.IsNullOrEmpty(options.OutputFileName)
            || options.OutputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            Console.WriteLine("The provided filename is invalid. Please ensure the file has a valid format. Check the file name and try again.");
            return Task.FromResult(-1);
        }

        var randomMatrix = _matrixGenerator.GenerateRandomResultMatrix(options.Size!.Value);
        _qmWriter.SaveMatrixToFile(randomMatrix, options.OutputFileName);

        Console.WriteLine($"A test instance for {options.Size.Value} players was generated and saved in a file {options.OutputFileName}");
        return Task.CompletedTask;
    }

    public Task Compare(CompareOptions options)
    {
        return Task.CompletedTask;
    }
}