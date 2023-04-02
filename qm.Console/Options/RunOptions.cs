using CommandLine;
using qm.utils.Models;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("run", HelpText = "Solve the problem using the specified algorithm.")]
    public class RunOptions
    {
        [Option('f', "file", Required = true, HelpText = "The file name with tournament results that will be solved.")]
        [MinLength(1, ErrorMessage = "File name must have at least one character.")]
        public string? InputFileName { get; set; }

        [Option('a', "algorithm", Required = false, Default = MatrixAlgorithm.Strassen, HelpText = "Algorithm used when solving. Options: Naive, Strassen, Hybrid.")]
        public MatrixAlgorithm Algorithm { get; set; }
    }
}
