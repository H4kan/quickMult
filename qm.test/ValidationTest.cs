using qm.algorithm;
using qm.generator;
using qm.naive;

namespace qm.test
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var matrix = MatrixGenerator.GenerateRandomResultMatrix(1000, 0);

            var naiveSolution = new NaiveAlgorithm(1000, matrix).ConductAlgorithm();
            var qmSolution = new QmAlgorithm(1000, matrix).ConductAlgorithm();

            CollectionAssert.AreEqual(naiveSolution, qmSolution);
        }
    }
}