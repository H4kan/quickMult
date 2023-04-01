using qm.utils;

namespace qm.reader
{
    public static class QmReader
    {
        public static byte[][]? LoadFromFile(string filename)
        {
            var filePath = Helpers.GetPathForFile(filename);
            if (!File.Exists(filePath))
                return null;

            using var reader = new StreamReader(filename);
            try
            {
                int n = int.Parse(reader.ReadLine()!);
                var matrix = Helpers.InitializeMatrix<byte>(n);

                for (int i = 0; i < n; i++)
                {
                    var parts = reader.ReadLine()!.Split(' ');
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (!byte.TryParse(parts[j], out matrix[i][j]))
                            return null;
                    }
                }

                return matrix;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}