using qm.utils;

namespace qm.algorithm
{
    public class NaiveMultiplication : IMatrixMultiplication
    {
        public byte[][] ConductSquareMultiplication(byte[][] input)
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix(input.Length);
            var a = resultHandlingMatrix[0][0] & resultHandlingMatrix[0][0];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    for (int k = 0; k < input.Length; k++)
                    {
                        resultHandlingMatrix[i][j] += (byte)(input[i][k] & input[k][j]);
                    }
                }
            }

            return resultHandlingMatrix;
        }
    }
}
