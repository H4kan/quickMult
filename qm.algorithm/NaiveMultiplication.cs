using qm.utils;

namespace qm.algorithm
{
    public class NaiveMultiplication : IMatrixMultiplication
    {
        public byte[][] ConductSquareMultiplication(byte[][] input)
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    for (int k = 0; k < input.Length; k++)
                    {
                        resultHandlingMatrix[i][j] |= (byte)(input[i][k] & input[k][j]);
                    }
                }
            }

            return resultHandlingMatrix;
        }

        public byte[][] ConductMultiplication(byte[][] inputA, byte[][] inputB)
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix(inputA.Length);
            for (int i = 0; i < inputA.Length; i++)
            {
                for (int j = 0; j < inputA.Length; j++)
                {
                    for (int k = 0; k < inputB.Length; k++)
                    {
                        resultHandlingMatrix[i][j] |= (byte)(inputA[i][k] & inputB[k][j]);
                    }
                }
            }
            
            return resultHandlingMatrix;
        }
    }
}
