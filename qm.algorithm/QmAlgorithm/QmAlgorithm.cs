using qm.algorithm.MatrixMultiplication;
using System.Numerics;

namespace qm.algorithm.QmAlgorithm
{
    public class QmAlgorithm<T, Algorithm> : IQmAlgorithm<T, Algorithm>
        where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
        where Algorithm : IMatrixMultiplication<T>
    {
        private readonly Algorithm _multiplicationAlgorithm;

        private static readonly T zeroTyped = (T)Convert.ChangeType(0, typeof(T));
        private static readonly T oneTyped = (T)Convert.ChangeType(1, typeof(T));


        public QmAlgorithm(Algorithm matrixMultiplication)
        {
            _multiplicationAlgorithm = matrixMultiplication;
        }

        public List<int> ConductAlgorithm(byte[][] results)
        {
            var edges = results.Select(l => l.Select(e => (T)Convert.ChangeType(e, typeof(T))).ToArray()).ToArray();
            int playerNum = edges.Length;
            var resultHandlingMatrix = _multiplicationAlgorithm.ConductSquareMultiplication(edges);

            for (int i = 0; i < playerNum; i++)
            {
                for (int j = 0; j < playerNum; j++)
                {
                    if (i == j)
                    {
                        resultHandlingMatrix[i][j] = oneTyped;
                    }
                    else
                    {
                        resultHandlingMatrix[i][j] += edges[i][j];
                    }
                }
            }

            var result = new List<int>();
            bool haveXProp;
            for (int i = 0; i < playerNum; i++)
            {
                haveXProp = true;
                for (int j = 0; j < playerNum; j++)
                {
                    if (resultHandlingMatrix[i][j] == zeroTyped)
                    {
                        haveXProp = false;
                        break;
                    }
                }
                if (haveXProp)
                {
                    result.Add(i);
                }
            }

            return result;
        }
    }
}