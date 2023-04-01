// See https://aka.ms/new-console-template for more information
using qm.generator;
using qm.naive;
using qm.reader;
using qm.utils;

Console.WriteLine("Hello, this is Ping-Pong problem solver!");
PrintOptionInfo();
ConsoleKeyInfo option;

do
{
    Console.Write("Enter your choice (1-Solve, 2-Generate or 3-Exit): ");
    option = Console.ReadKey();
    Console.WriteLine();

    if (option.KeyChar == '1')
    {
        var fileName = GetFileName(true);
        var resultMatrix = QmReader.LoadFromFile(fileName);
        while (resultMatrix == null)
        {
            Console.WriteLine($"The input data from the {fileName} has an invalid structure. Try again with diffrent file.");
            fileName = GetFileName(false);
            resultMatrix = QmReader.LoadFromFile(fileName);
        }

        var naiveAlg = new NaiveAlgorithm(resultMatrix.Length, resultMatrix);
        var solution = naiveAlg.ConductAlgorithm();

        var solutionFileName = Helpers.GetResultFileName(fileName);
        QmWriter.SaveSolutionToFile(solution, solutionFileName);

        Console.WriteLine($"Problem solved, the result is in the file {solutionFileName}");
        Console.WriteLine(Helpers.FormatResult(solution));
    }
    else if (option.KeyChar == '2')
    {
        int n = GetNumberOfPlayers();
        var fileName = GetFileName();

        var myArray = MatrixGenerator.GenerateRandomResultMatrix(n, 100);
        QmWriter.SaveMatrixToFile(myArray, fileName);

        Console.WriteLine($"A test instance for {n} players was generated and saved in a file {fileName}");
    }

    Console.WriteLine("\n----------------------------------------------------------------\n");
    PrintOptionInfo();
} while (option.KeyChar != '3');

Console.WriteLine("Bye!");
Console.WriteLine("Press any key to exit.");
Console.ReadKey();


static string GetFileName(bool fileShouldExist = false)
{

    Console.Write("Enter the file name: ");
    var fileName = Console.ReadLine();

    while (String.IsNullOrEmpty(fileName)
        || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
        || (fileShouldExist && !File.Exists(Helpers.GetPathForFile(fileName))))
    {
        Console.WriteLine("Invalid file name. Please enter an valid file name.");
        Console.WriteLine("Enter the file name: ");
        fileName = Console.ReadLine();
    }

    return fileName!;
}

static int GetNumberOfPlayers()
{
    int n;
    Console.Write("Enter the number of players: ");
    while (!Int32.TryParse(Console.ReadLine(), out n))
    {
        Console.WriteLine("Invalid input. Please enter an integer value.");
        Console.WriteLine("Enter the number of players: ");
    }

    return n;
}

static void PrintOptionInfo()
{
    Console.WriteLine("Please select an option:");
    Console.WriteLine("1. Solve ping-pong problem");
    Console.WriteLine("2. Generate test instance");
    Console.WriteLine("3. Exit");
}