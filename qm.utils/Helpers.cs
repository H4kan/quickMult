using System.Text;

namespace qm.utils
{
    public static class Helpers
    {
        public static byte[][] InitializeMatrix(int size)
        {
            return Enumerable.Range(0, size)
                .Select(i => new byte[size])
                .ToArray();
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
    }
}
