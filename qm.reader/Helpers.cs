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
            for (int i = 0; i < source.Length; i++)
            {
                for (int j = 0; j < source[i].Length; j++)
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
    }
}
