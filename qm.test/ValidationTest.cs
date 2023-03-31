using qm.algorithm;
using qm.generator;
using qm.naive;

namespace qm.test
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void TestRandomMatrix()
        {
            var matrix = MatrixGenerator.GenerateRandomResultMatrix(1000, 0);

            var naiveSolution = new NaiveAlgorithm(1000, matrix).ConductAlgorithm();
            var qmSolution = new QmAlgorithm(1000, matrix).ConductAlgorithm();

            CollectionAssert.AreEqual(naiveSolution, qmSolution);
        }

        [TestMethod]
        public void TestLoserRandomMatrix()
        {
            int n = 1000;
            var matrix = MatrixGenerator.GenerateLoserResultMatrix(n, 0, 90);

            var naiveSolution = new NaiveAlgorithm(n, matrix).ConductAlgorithm();
            var qmSolution = new QmAlgorithm(n, matrix).ConductAlgorithm();

            CollectionAssert.AreEqual(naiveSolution, qmSolution);
        }
    }
}