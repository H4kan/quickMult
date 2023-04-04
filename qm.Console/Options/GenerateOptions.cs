using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("generate-random", HelpText = "Generate a new test file containing the results of a completely random tournament.")]
    public class GenerateRandomOptions : BaseGenerateOptions
    {
    }

    [Verb("generate-power", HelpText = "Generate a test file of a tournament in which the results of matches are based on randomly assigned player powers (the player with the higher power is more likely to win).")]
    public class GeneratePowerOptions : BaseGenerateOptions
    {
        [Option('r', "range", Default = 1000, Required = false, HelpText = "The range of power values assigned to players. " +
            "In the tournament, each player is assigned a random power value within the range, and players with higher power have a greater chance of defeating those with lower power.")]
        [Range(0, 1000, ErrorMessage = "Player range value must be greater than or equal to 0 and less or equal than 1000")]
        public int Range { get; set; }
    }

    [Verb("generate-loser", HelpText = "Generate a new test file containing the results of a tournament in which there is a percentage of specific players who lose much more than they win.")]
    public class GenerateLoserOptions : BaseGenerateOptions
    {
        [Option('l', "losers", Default = 30, Required = false, HelpText = "The percentage of losers in the tournament.")]
        [Range(0, 100, ErrorMessage = "Percentage of losers in the tournament must be greater than or equal to 0 and less or equal than 100.")]
        public int PercentageOfLosers { get; set; }
    }
}
