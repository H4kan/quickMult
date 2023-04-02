using System.Numerics;

namespace qm.algorithm.MatrixMultiplication
{
    public interface IMatrixMultiplication<T> where T : IBitwiseOperators<T, T, T>
    {
        public T[][] ConductSquareMultiplication(T[][] input);
    }
}
