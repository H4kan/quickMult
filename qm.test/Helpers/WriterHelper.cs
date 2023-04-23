namespace qm.test.Helpers
{
    public class WriterHelper
    {
        public static void UpdateFileContent<T>(Dictionary<T, List<string>> content, string fileName) where T : Enum
        {
            var fullPath = Directory.GetCurrentDirectory();
            var projectPath = fullPath.Remove(fullPath.IndexOf("qm.test"));
            var directoryFilePath = $"{projectPath}test-results";

            if (!Directory.Exists(directoryFilePath))
            {
                Directory.CreateDirectory(directoryFilePath);
            }

            string filePath = Path.Combine(directoryFilePath, fileName);
            File.WriteAllLines(filePath, content.Values.SelectMany(v => v).ToList());
        }
    }
}
