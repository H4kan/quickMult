using qm.utils;

namespace qm.reader
{
    public static class QmReader
    {
        public static byte[][] LoadFromFile(string filename)
        {
            var filePath = Helpers.GetPathForFile(filename);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filename);

            using var reader = new StreamReader(filename);
            try
            {
                int n = int.Parse(reader.ReadLine()!);
                var matrix = Helpers.InitializeMatrix(n);

                for (int i = 0; i < n; i++)
                {
                    var parts = reader.ReadLine()!.Split(' ');
                    for (int j = 0; j < parts.Length; j++)
                    {
                        matrix[i][j] = byte.Parse(parts[j]);
                    }
                }

                return matrix;
            }
            catch (Exception)
            {
                throw new FormatException($"The input data from the {filename} has an invalid structure.");
            }
        }
    }
}