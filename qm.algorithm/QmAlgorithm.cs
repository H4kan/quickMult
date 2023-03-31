namespace qm.algorithm
{
    public class QmAlgorithm
    {
        private readonly byte[][] edges;

        private readonly int playerNum;

        public QmAlgorithm(int playerNum, byte[][] edges)
        {
            this.edges = edges;
            this.playerNum = playerNum;
        }

        public List<int> ConductAlgorithm()
        {

            IMatrixMultiplication matrixMultiplication = new StrassenMultiplication();

            var resultHandlingMatrix = matrixMultiplication.ConductSquareMultiplication(this.edges);

            for (int i = 0; i < playerNum; i++)
            {
                for (int j = 0; j < playerNum; j++)
                {
                    if (i == j)
                    {
                        resultHandlingMatrix[i][j] = 1;
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
                    if (resultHandlingMatrix[i][j] == 0)
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