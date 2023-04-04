using qm.utils;

namespace qm.generator
{
    public class MatrixGenerator : IMatrixGenerator
    {
        private readonly Random _rand;

        public static int PowerAmplificator = 4;

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

        // range is [0, 1000] - 0 is random
        public byte[][] GeneratePowerMatrix(int n, int playerRange)
        {
            // this is equivalent
            if (playerRange == 0)
            {
                return GenerateRandomResultMatrix(n);
            }

            int[] playerPower = new int[n];
            for (int i = 0; i < n; i++)
            {
                playerPower[i] = _rand.Next(playerRange + 1);
            }

            var resultMatrix = Helpers.InitializeMatrix<byte>(n);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    // i has prob power[i]^ampl/(power[i]^ampl + power[j]^ampl)
                    // => 1 / (1 + (power[j]/power[i])^ampl)
                    // logarithms for better precision
                    var iWonBoundary = 1 / (1 +
                        Math.Pow(Math.E,
                        PowerAmplificator * (Math.Log(playerPower[j]) - Math.Log(playerPower[i]))));

                    var decider = _rand.NextDouble();

                    resultMatrix[i][j] = (byte)(decider <= iWonBoundary ? 1 : 0);
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }
    }
}