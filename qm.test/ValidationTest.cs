using qm.algorithm;
using qm.generator;
using qm.naive;

namespace qm.test
{
    [TestClass]
    public class ValidationTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestRandomMatrix(int matrixSize)
        {
            TestMatrix(matrixSize, (size) => MatrixGenerator.GenerateRandomResultMatrix(size, 0));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestLoserRandomMatrix(int matrixSize)
        {
            TestMatrix(matrixSize, (size) => MatrixGenerator.GenerateLoserResultMatrix(size, 0, 70));
        }

        public static IEnumerable<object[]> GetMatrixSizes()
        {
            return new List<object[]>
            {
                new object[] { 4 },
                new object[] { 8 },
                new object[] { 16 },
                new object[] { 32 },
                new object[] { 64 },
                new object[] { 128 },
                new object[] { 256 },
                new object[] { 512 },
                new object[] { 657 },
                new object[] { 43 },
                new object[] { 765 },
                new object[] { 122 },
            };
        }

        private static void TestMatrix(int matrixSize, Func<int, byte[][]> matrixGenerator)
        {
            var matrix = matrixGenerator(matrixSize);

            var naiveSolution = new NaiveAlgorithm(matrixSize, matrix).ConductAlgorithm();
            var naiveMultSolution = new QmAlgorithm<int>(matrixSize, matrix, new NaiveMultiplication<int>()).ConductAlgorithm();
            var strassenSolution = new QmAlgorithm<int>(matrixSize, matrix, new StrassenMultiplication<int>(4)).ConductAlgorithm();
            var hybridSolutioon = new QmAlgorithm<int>(matrixSize, matrix, new HybridMultiplication<int>()).ConductAlgorithm();

            CollectionAssert.AreEqual(naiveSolution, naiveMultSolution);
            CollectionAssert.AreEqual(naiveSolution, strassenSolution);
            CollectionAssert.AreEqual(naiveSolution, hybridSolutioon);
        }
    }
}