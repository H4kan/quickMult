using System.Numerics;

namespace qm.algorithm
{
    public class QmAlgorithm<T> where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
    {
        private readonly T[][] edges;

        private readonly int playerNum;

        private IMatrixMultiplication<T> multiplication;

        private static T zeroTyped = (T) Convert.ChangeType(0, typeof(T));
        private static T oneTyped = (T)Convert.ChangeType(1, typeof(T));

        public QmAlgorithm(int playerNum, byte[][] edges, IMatrixMultiplication<T> matrixMultiplication)
        {
            this.edges = edges.Select(l => l.Select(e => (T)Convert.ChangeType(e, typeof(T))).ToArray()).ToArray();
            this.playerNum = playerNum;
            this.multiplication = matrixMultiplication;
        }

        public List<int> ConductAlgorithm()
        {

            var resultHandlingMatrix = this.multiplication.ConductSquareMultiplication(this.edges);

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
                        resultHandlingMatrix[i][j] += this.edges[i][j];
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