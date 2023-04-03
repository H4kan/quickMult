using qm.utils.Models;

namespace qm.utils.Interfaces
{
    public interface IQmWriter
    {
        void SaveMatrixToFile(byte[][] gameResultMatrix, string fileName);
        void SaveSolutionToFile(IEnumerable<int> solution, string fileName);
        void SaveTimeComparisionResultsToFile(IDictionary<MatrixAlgorithm, TimeSpan> results, string fileName);
    }
}