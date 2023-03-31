using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qm.utils
{
    public static class Helpers
    {
        public static byte[][] InitializeMatrix(int size)
        {
            var matrix = new byte[size][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new byte[size];
            }
            return matrix;
        }

        public static void CopyMatrix(byte[][] target, byte[][] source)
        {
            for (int i = 0; i < Math.Min(source.Length, target.Length); i++)
            {
                for (int j = 0; j < Math.Min(source[i].Length, target[i].Length); j++)
                {
                    target[i][j] = source[i][j];
                }
            }
        }

        public static string FormatResult(List<int> result)
        {
            if (result.Count == 0)
            {
                return "Result []";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Result: [");
            for (int i = 0; i < result.Count - 1; i++)
            {
                sb.Append(result[i]);
                sb.Append(" ");
            }
            sb.Append(result.Last());
            sb.Append("]");
            return sb.ToString();
        }

        public static int CeilPower2(int num)
        {
            num--;
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            num++;
            return num;
        }
    }
}
