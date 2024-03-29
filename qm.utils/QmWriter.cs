﻿using qm.utils.Interfaces;
using qm.utils.Models;
using System.Text;

namespace qm.reader
{
    public class QmWriter : IQmWriter
    {
        public void SaveSolutionToFile(IEnumerable<int> solution, string fileName)
        {
            var solutionAsString = string.Join(", ", solution);
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            File.WriteAllText(filePath, solutionAsString);
        }

        public void SaveTimeComparisionResultsToFile(IDictionary<MatrixAlgorithm, TimeSpan> results, string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var sb = new StringBuilder();

            foreach (var p in results)
            {
                sb.AppendLine($"{p.Key} {p.Value.TotalNanoseconds}");
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        public void SaveMatrixToFile(byte[][] gameResultMatrix, string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var sb = new StringBuilder(gameResultMatrix.Length.ToString() + '\n');

            Array.ForEach(gameResultMatrix, row => sb.Append(string.Join(' ', row) + '\n'));
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
