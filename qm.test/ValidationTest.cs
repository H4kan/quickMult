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
            for (int i = 0; i < 10; i++)
            {
                var matrix = MatrixGenerator.GenerateRandomResultMatrix(8, 0);

                var naiveSolution = new NaiveAlgorithm(8, matrix).ConductAlgorithm();
                var qmSolution = new QmAlgorithm(8, matrix).ConductAlgorithm();

                CollectionAssert.AreEqual(naiveSolution, qmSolution);
            }
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