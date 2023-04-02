namespace qm.utils.Interfaces
{
    public interface IQmWriter
    {
        void SaveMatrixToFile(byte[][] gameResultMatrix, string fileName);
        void SaveSolutionToFile(IEnumerable<int> solution, string fileName);
    }
}