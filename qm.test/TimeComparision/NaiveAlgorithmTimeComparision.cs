using qm.generator;
using qm.naive;
using qm.test.Helpers;
using System.Diagnostics;

namespace qm.test.TimeComparision
{
    [TestClass]
    public class NaiveAlgorithmTimeComparision
    {
        [TestMethod]
        public void CompareNaiveExecutionWithMatrixTypeAndSize()
        {
            int step = 50;
            var generator = new MatrixGenerator(1000);
            var matricesComparisionResult = InitializeMatricesResultsDictionary();
            var algorithm = new NaiveAlgorithm();
            var playerNumberList = Enumerable.Range(1, 200).Select(x => x * step);

            foreach (var playerNumber in playerNumberList)
            {
                var matrices = GetMatricesDictionary(playerNumber, generator);

                foreach (MatricesForTest matrix in Enum.GetValues(typeof(MatricesForTest)))
                {
                    RunAlgorithm(matrices[matrix], matricesComparisionResult[matrix], playerNumber, algorithm.ConductAlgorithm);
                }

                WriterHelper.UpdateFileContent(matricesComparisionResult, "naive-time-comparision-results.txt");
            }
        }

        private static void RunAlgorithm(byte[][] matrix, List<string> list, int n, Func<byte[][], List<int>> runAlgorithm)
        {
            var timer = new Stopwatch();

            timer.Start();
            runAlgorithm(matrix);
            timer.Stop();

            list.Add($"{n} {timer.Elapsed.TotalNanoseconds}");
        }

        public enum MatricesForTest
        {
            AutoPowerMatrix,
            RandomResultMatrix,
            DominationMatrix
        }

        private static Dictionary<MatricesForTest, List<string>> InitializeMatricesResultsDictionary()
        {
            return new Dictionary<MatricesForTest, List<string>>
            {
                [MatricesForTest.AutoPowerMatrix] = new List<string> { "Auto Power Matrix" },
                [MatricesForTest.RandomResultMatrix] = new List<string> { "Random Matrix" },
                [MatricesForTest.DominationMatrix] = new List<string> { "Domination Matrix" },
            };
        }

        private static Dictionary<MatricesForTest, byte[][]> GetMatricesDictionary(int n, MatrixGenerator generator)
        {
            return new Dictionary<MatricesForTest, byte[][]>
            {
                [MatricesForTest.AutoPowerMatrix] = generator.GenerateAutoPowerMatrix(n),
                [MatricesForTest.RandomResultMatrix] = generator.GenerateRandomResultMatrix(n),
                [MatricesForTest.DominationMatrix] = generator.GenerateDominationMatrix(n),
            };
        }
    }
}
