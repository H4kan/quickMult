using qm.utils;
using System.Numerics;

namespace qm.algorithm
{
    public class NaiveMultiplication<T> : IMatrixMultiplication<T> where T: IBitwiseOperators<T, T, T>
    {
        public T[][] ConductSquareMultiplication(T[][] input)
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix<T>(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    for (int k = 0; k < input.Length; k++)
                    {
                        resultHandlingMatrix[i][j] |= (T)(input[i][k] & input[k][j]);
                    }
                }
            }

            return resultHandlingMatrix;
        }

        public T[][] ConductMultiplication(T[][] inputA, T[][] inputB)
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix<T>(inputA.Length);
            for (int i = 0; i < inputA.Length; i++)
            {
                for (int j = 0; j < inputA.Length; j++)
                {
                    for (int k = 0; k < inputB.Length; k++)
                    {
                        resultHandlingMatrix[i][j] |= (T)(inputA[i][k] & inputB[k][j]);
                    }
                }
            }
            
            return resultHandlingMatrix;
        }
    }
}
