using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("compare", HelpText = "Perform a time comparison.")]
    public class CompareOptions
    {
        [Option('f', "file", Required = true, HelpText = "The file name with tournament results which will be used for time comparision.")]
        [MinLength(1, ErrorMessage = "File name must have at least one character.")]
        public string? InputFileName { get; set; }
    }
}
