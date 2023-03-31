using qm.utils;

namespace qm.generator
{
    public class MatrixGenerator
    {
        public static byte[][] GenerateRandomResultMatrix(int n, int seed = 0)
        {
            var rand = new Random(seed);
            var resultMatrix = Helpers.InitializeMatrix(n);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    resultMatrix[i][j] = (byte)(rand.Next(2));
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }


        public static byte[][] GenerateLoserResultMatrix(int n, int seed = 0, float loserPerc = 80)
        {
            var rand = new Random(seed);
            var resultMatrix = Helpers.InitializeMatrix(n);

            for (int i = 0; i < n - 1; i++)
            {
                var isNormal = rand.Next(100) > loserPerc;

                for (int j = i + 1; j < n; j++)
                {
                    resultMatrix[i][j] = isNormal ? (byte)(rand.Next(2)) : (byte)0;
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }
    }
}