using System.Numerics;
using qm.algorithm.MatrixMultiplication;

namespace qm.algorithm
{
    public interface IQmAlgorithm<T, Algorithm>
        where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
        where Algorithm : IMatrixMultiplication<T>
    {
        List<int> ConductAlgorithm(byte[][] results);
    }
}