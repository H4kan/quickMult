using qm.utils;

namespace qm.generator
{
    public class MatrixGenerator : IMatrixGenerator
    {
        private readonly Random _rand;

        public static int PowerScale = 400;

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

        // ELO system based method
        // greater power factor, closer game is to random
        public byte[][] GeneratePowerMatrix(int n, int playerRange = 1000)
        {
            return GeneratePowerMatrix_Internal(n, playerRange);
        }

        // bigger n, more percentage of player has prop X
        // this makes playerRange increase with n
        public byte[][] GenerateAutoPowerMatrix(int n)
        {
            var playerRange = n * 1000 / 64;
            return GeneratePowerMatrix_Internal(n, playerRange);
        }


        private byte[][] GeneratePowerMatrix_Internal(int n, int playerRange)
        {
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
                    // this is how it is in elo system
                    var iWonBoundary = 1 / (1 +
                        Math.Pow(10, (double)(playerPower[i] - playerPower[j]) / PowerScale));

                    var decider = _rand.NextDouble();

                    resultMatrix[i][j] = (byte)(decider <= iWonBoundary ? 1 : 0);
                    resultMatrix[j][i] = (byte)(1 - resultMatrix[i][j]);
                }
            }

            return resultMatrix;
        }

        public byte[][] GenerateDominationMatrix(int n)
        {
            var matrix = GenerateRandomResultMatrix(n);
            var dominator = _rand.Next(0, n);

            for (int i = 0; i < n; i++)
            {
                matrix[dominator][i] = 1;
                matrix[i][dominator] = 0;
            }
            matrix[dominator][dominator] = 0;

            return matrix;
        }
    }
}