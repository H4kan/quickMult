﻿using qm.utils;

namespace qm.naive
{
    public class NaiveAlgorithm
    {

        private readonly byte[][] edges;

        private readonly int playerNum;

        public NaiveAlgorithm(int playerNum, byte[][] edges)
        {
            this.edges = edges;
            this.playerNum = playerNum;
        }

        public List<int> ConductAlgorithm()
        {
            var resultHandlingMatrix = Helpers.InitializeMatrix(this.playerNum);
            Helpers.CopyMatrix(resultHandlingMatrix, this.edges);

            // iterate for each player
            for (int i = 0; i < playerNum; i++)
            {
                for (int j = 0; j < playerNum; j++)
                {
                    if (i == j)
                    {
                        resultHandlingMatrix[i][j] = 1;
                        continue;
                    }
                    // find all that i lost to
                    if (edges[i][j] == 0)
                    {
                        for (int k = 0; k < playerNum; k++)
                        {
                            // if j lost to k, check if k lost to i
                            if (edges[j][k] == 0 && edges[i][k] == 1)
                            {
                                resultHandlingMatrix[i][j] = 1;
                                break;
                            }
                        }
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