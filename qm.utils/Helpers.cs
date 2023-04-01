using System.Text;

namespace qm.utils
{
    public static class Helpers
    {
        public static T[][] InitializeMatrix<T>(int size)
        {
            return Enumerable.Range(0, size)
                .Select(i => new T[size])
                .ToArray();
        }

        public static void CopyMatrix<T>(T[][] target, T[][] source)
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

            StringBuilder sb = new("Result: [");
            for (int i = 0; i < result.Count - 1; i++)
            {
                sb.Append(result[i]);
                sb.Append(' ');
            }
            sb.Append(result.Last());
            sb.Append(']');

            return sb.ToString();
        }

        public static string GetPathForFile(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static string GetResultFileName(string problemFileName)
        {
            return $"{Path.GetFileNameWithoutExtension(problemFileName)}-result.txt";
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

        public static int CeilLog2(int value)
        {
            int i;
            for (i = -1; value != 0; i++)
                value >>= 1;

            return (i == -1) ? 1 : i + 1;
        }
    }
}
