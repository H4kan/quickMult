using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.generator;
using qm.naive;
using qm.test.Helpers;
using qm.test.Models;
using System.Diagnostics;

namespace qm.test.TimeComparision
{
    [TestClass]
    public class AlgorithmAverageTimeComparision
    {
        [TestMethod]
        public void TestAverageTimeComparision()
        {
            int step = 5;
            var generator = new MatrixGenerator(1000);
            var algorithmComparisionResult = InitializeAlgorithmResultsDictionary();
            var algorithms = InitializeAlgorithmDictionary();
            var playerNumberList = Enumerable.Range(1, 300).Select(x => x * step);

            foreach (var playerNumber in playerNumberList)
            {
                var matrices = GetGeneratedMatrix(playerNumber, generator);

                foreach (AlgorithmForTest algorithm in Enum.GetValues(typeof(AlgorithmForTest)))
                {
                    RunAlgorithm(matrices, algorithmComparisionResult[algorithm], playerNumber, algorithms[algorithm]);
                }

                WriterHelper.UpdateFileContent(algorithmComparisionResult, "algorithm-average-time-results.txt");
            }
        }

        private static void RunAlgorithm(List<byte[][]> matrices, List<string> list, int n, Func<byte[][], List<int>> runAlgorithm)
        {
            var timer = new Stopwatch();
            foreach (var matrix in matrices)
            {
                timer.Start();
                runAlgorithm(matrix);
                timer.Stop();
            }

            list.Add($"{n} {timer.Elapsed.TotalNanoseconds / matrices.Count}");
        }

        private static List<byte[][]> GetGeneratedMatrix(int n, MatrixGenerator generator)
        {
            var matrices = new List<byte[][]>()
            {
                generator.GenerateRandomResultMatrix(n),
                generator.GenerateDominationMatrix(n),
                generator.GenerateAutoPowerMatrix(n),
                generator.GeneratePowerMatrix(n),

                generator.GenerateRandomResultMatrix(n),
                generator.GenerateDominationMatrix(n),
                generator.GenerateAutoPowerMatrix(n),
                generator.GeneratePowerMatrix(n),
            };

            return matrices;
        }

        private static Dictionary<AlgorithmForTest, List<string>> InitializeAlgorithmResultsDictionary()
        {
            return new Dictionary<AlgorithmForTest, List<string>>
            {
                [AlgorithmForTest.Naive] = new List<string> { "Naive Algorithm" },
                [AlgorithmForTest.QmAlgorithmWithNaive] = new List<string> { "QM Algorithm with Naive" },
                [AlgorithmForTest.QmAlgorithmWithHybrid] = new List<string> { "QM Algorithm with Hybrid" },
                [AlgorithmForTest.QmAlgorithmWithStrassen] = new List<string> { "QM Algorithm with Strassen" }
            };
        }

        private static Dictionary<AlgorithmForTest, Func<byte[][], List<int>>> InitializeAlgorithmDictionary()
        {
            var naiveMultiplication = new NaiveMultiplication<int>();
            var strassenMultiplication = new StrassenMultiplication<int>(naiveMultiplication);
            var hybridMultiplication = new HybridMultiplication<int>(naiveMultiplication, strassenMultiplication);

            return new Dictionary<AlgorithmForTest, Func<byte[][], List<int>>>
            {
                [AlgorithmForTest.Naive] = new NaiveAlgorithm().ConductAlgorithm,
                [AlgorithmForTest.QmAlgorithmWithNaive] = new QmAlgorithm<int>(naiveMultiplication).ConductAlgorithm,
                [AlgorithmForTest.QmAlgorithmWithHybrid] = new QmAlgorithm<int>(hybridMultiplication).ConductAlgorithm,
                [AlgorithmForTest.QmAlgorithmWithStrassen] = new QmAlgorithm<int>(strassenMultiplication).ConductAlgorithm,
            };
        }
    }
}
