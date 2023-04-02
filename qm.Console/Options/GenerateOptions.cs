using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("generate", HelpText = "Generate a new test file containing the results of a random tournament.")]
    public class GenerateOptions
    {
        [Option('s', "size", Required = true, HelpText = "The size of the test file (number of players participating in the tournament).")]
        [Range(2, int.MaxValue, ErrorMessage = "Size must be greater than or equal to 2.")]
        public int? Size { get; set; }

        [Option('f', "file", Required = true, HelpText = "The name of the file generated.")]
        [MinLength(1, ErrorMessage = "File name must have at least one character.")]
        public string? OutputFileName { get; set; }
    }
}
