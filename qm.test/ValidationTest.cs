using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.generator;
using qm.naive;

namespace qm.testW
{
    [TestClass]
    public class ValidationTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestRandomMatrix(int matrixSize)
        {
            var generator = new MatrixGenerator(0);
            TestMatrix(matrixSize, (size) => generator.GenerateRandomResultMatrix(size));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestLoserRandomMatrix(int matrixSize)
        {
            var generator = new MatrixGenerator(0);
            TestMatrix(matrixSize, (size) => generator.GenerateLoserResultMatrix(size, 70));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestPowerMatrix(int matrixSize)
        {
            var generator = new MatrixGenerator(0);
            TestMatrix(matrixSize, (size) => generator.GeneratePowerMatrix(size));
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMatrixSizes), DynamicDataSourceType.Method)]
        public void TestAutoPowerMatrix(int matrixSize)
        {
            var generator = new MatrixGenerator(0);
            TestMatrix(matrixSize, (size) => generator.GenerateAutoPowerMatrix(size));
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

            var naiveMultiplication = new NaiveMultiplication<int>();
            var strassenMultiplication = new StrassenMultiplication<int>(naiveMultiplication);
            var hybridMultiplication = new HybridMultiplication<int>(naiveMultiplication, strassenMultiplication);

            var naiveSolution = new NaiveAlgorithm().ConductAlgorithm(matrix);
            var naiveMultSolution = new QmAlgorithm<int>(naiveMultiplication).ConductAlgorithm(matrix);
            var strassenSolution = new QmAlgorithm<int>(strassenMultiplication).ConductAlgorithm(matrix);
            var hybridSolutioon = new QmAlgorithm<int>(hybridMultiplication).ConductAlgorithm(matrix);

            CollectionAssert.AreEqual(naiveSolution, naiveMultSolution);
            CollectionAssert.AreEqual(naiveSolution, strassenSolution);
            CollectionAssert.AreEqual(naiveSolution, hybridSolutioon);
        }
    }
}