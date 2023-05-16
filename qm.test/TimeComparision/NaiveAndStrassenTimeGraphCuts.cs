using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.generator;
using qm.test.Helpers;
using qm.test.Models;
using System.Diagnostics;

namespace qm.test.TimeComparision
{
    [TestClass]
    public class NaiveAndStrassenTimeGraphCuts
    {
        [TestMethod]
        public void ConductComparisionOnSpecifiedRanges()
        {
            int step = 100;
            var generator = new MatrixGenerator(1000);
            var algorithmComparisionResult = InitializeAlgorithmResultsDictionary();
            var algorithms = InitializeAlgorithmDictionary();
            var playerNumberList = Enumerable.Range(49, 8).Select(x => x * step);

            foreach (var playerNumber in playerNumberList)
            {
                var matrices = GetGeneratedMatrix(playerNumber, generator);
                RunAlgorithm(matrices, algorithmComparisionResult[AlgorithmForTest.QmAlgorithmWithNaive], playerNumber, algorithms[AlgorithmForTest.QmAlgorithmWithNaive]);

                WriterHelper.UpdateFileContent(algorithmComparisionResult, "naive-strassen-time13-graph-cuts-results.txt");
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
                generator.GenerateAutoPowerMatrix(n),
            };

            return matrices;
        }

        private static Dictionary<AlgorithmForTest, List<string>> InitializeAlgorithmResultsDictionary()
        {
            return new Dictionary<AlgorithmForTest, List<string>>
            {
                [AlgorithmForTest.QmAlgorithmWithNaive] = new List<string> { "QM Algorithm with Naive" },
                //[AlgorithmForTest.QmAlgorithmWithStrassen] = new List<string> { "QM Algorithm with Strassen" }
            };
        }

        private static Dictionary<AlgorithmForTest, Func<byte[][], List<int>>> InitializeAlgorithmDictionary()
        {
            var naiveMultiplication = new NaiveMultiplication<int>();
            var strassenMultiplication = new StrassenMultiplication<int>(naiveMultiplication);

            return new Dictionary<AlgorithmForTest, Func<byte[][], List<int>>>
            {
                [AlgorithmForTest.QmAlgorithmWithNaive] = new QmAlgorithm<int>(naiveMultiplication).ConductAlgorithm,
                //[AlgorithmForTest.QmAlgorithmWithStrassen] = new QmAlgorithm<int>(strassenMultiplication).ConductAlgorithm,
            };
        }

    }
}
