using System.Numerics;

namespace qm.algorithm.QmAlgorithm
{
    public interface IQmAlgorithm<T>
          where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
    {
        List<int> ConductAlgorithm(byte[][] results);
    }
}