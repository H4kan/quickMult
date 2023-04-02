using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("run", HelpText = "Solve the problem using the specified algorithm.")]
    public class RunOptions
    {
        [Option('f', "file", Required = true, HelpText = "The file name with tournament results that will be solved.")]
        [MinLength(1, ErrorMessage = "File name must have at least one character.")]
        public string? FileName { get; set; }

        [Option('a', "algorithm", Required = false, Default = Algorithm.Strassen, HelpText = "Algorithm used when solving. Options: Naive, Strassen, Hybrid.")]
        public Algorithm Algorithm { get; set; }
    }

    public enum Algorithm
    {
        Naive = 0,
        Strassen = 1,
        Hybrid = 2,
    }
}
