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
    [Ignore]
    public class AlgorithmsTimeComparisionForMatrices
    {
        [TestMethod]
        public void AutoPowerMatrixTimeComparision()
        {
            int step = 50;
            var generator = new MatrixGenerator(1000);
            var algorithmComparisionResult = InitializeAlgorithmResultsDictionary();
            var algorithms = InitializeAlgorithmDictionary();
            var playerNumberList = Enumerable.Range(1, 60).Select(x => x * step);

            foreach (var playerNumber in playerNumberList)
            {
                var matrix = generator.GenerateAutoPowerMatrix(playerNumber);

                foreach (AlgorithmForTest algorithm in Enum.GetValues(typeof(AlgorithmForTest)))
                {
                    RunAlgorithm(matrix, algorithmComparisionResult[algorithm], playerNumber, algorithms[algorithm]);
                }

                WriterHelper.UpdateFileContent(algorithmComparisionResult, "algorithm-time-comparision-auto-power-matrix-results.txt");
            }
        }

        [TestMethod]
        public void DominationMatrixTimeComparision()
        {
            int step = 50;
            var generator = new MatrixGenerator(1000);
            var algorithmComparisionResult = InitializeAlgorithmResultsDictionary();
            var algorithms = InitializeAlgorithmDictionary();
            var playerNumberList = Enumerable.Range(1, 60).Select(x => x * step);

            foreach (var playerNumber in playerNumberList)
            {
                var matrix = generator.GenerateDominationMatrix(playerNumber);

                foreach (AlgorithmForTest algorithm in Enum.GetValues(typeof(AlgorithmForTest)))
                {
                    RunAlgorithm(matrix, algorithmComparisionResult[algorithm], playerNumber, algorithms[algorithm]);
                }

                WriterHelper.UpdateFileContent(algorithmComparisionResult, "algorithm-time-comparision-domination-matrix-results.txt");
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
