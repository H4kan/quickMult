using qm.algorithm.MatrixMultiplication;
using System.Numerics;

namespace qm.algorithm.QmAlgorithm
{
    public interface IQmAlgorithm<T, Algorithm>
        where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
        where Algorithm : IMatrixMultiplication<T>
    {
        List<int> ConductAlgorithm(byte[][] results);
    }
}