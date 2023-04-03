using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.utils.Models;
using System.Numerics;

namespace qm.algorithm.QmAlgorithmFactory
{
    public interface IQmAlgorithmFactory<T> where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
    {
        IQmAlgorithm<T, IMatrixMultiplication<T>> Create(MatrixAlgorithm matrixAlgorithm);
    }
}
