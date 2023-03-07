using System.Text;

namespace qm.reader
{
    public static class QmWriter
    {
        public static void SaveSolutionToFile(IEnumerable<int> solution, string fileName)
        {
            var solutionAsString = string.Join(", ", solution);
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            File.WriteAllText(filePath, solutionAsString);
        }

        public static void SaveMatrixToFile(byte[][] gameResultMatrix, string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var sb = new StringBuilder(gameResultMatrix.Length.ToString() + '\n');

            Array.ForEach(gameResultMatrix, row => sb.Append(string.Join(' ', row) + '\n'));
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
