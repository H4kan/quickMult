using qm.utils;

namespace qm.generator
{
    public class MatrixGenerator : IMatrixGenerator
    {
        private readonly Random _rand;

        public MatrixGenerator(int? seed = null)
        {
            _rand = seed == null ? new Random((int)DateTime.Now.Ticks) : new Random(seed.Value);
        }

        public byte[][] GenerateRandomResultMatrix(int n)
        {
            var resultMatrix = Helpers.InitializeMatrix<byte>(n);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    resultMatrix[i][j] = (byte)(_rand.Next(2));
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }


        public byte[][] GenerateLoserResultMatrix(int n, float loserPerc = 80)
        {
            var resultMatrix = Helpers.InitializeMatrix<byte>(n);

            for (int i = 0; i < n - 1; i++)
            {
                var isNormal = _rand.Next(100) > loserPerc;

                for (int j = i + 1; j < n; j++)
                {
                    resultMatrix[i][j] = isNormal ? (byte)(_rand.Next(2)) : (byte)0;
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }
    }
}