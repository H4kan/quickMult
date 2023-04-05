using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace qm.console.Options
{
    [Verb("generate-random", HelpText = "Generate a new test file containing the results of a completely random tournament.")]
    public class GenerateRandomOptions : BaseGenerateOptions
    {
    }

    [Verb("generate-power", HelpText = "Generate a test file using ELO ranking. Tournament results are based on the randomly allocated ELO points of each player (the player with the higher ELO is more likely to win).")]
    public class GeneratePowerOptions : BaseGenerateOptions
    {
        [Option('r', "range", Default = 1000, Required = false, HelpText = "A range of power (ELO) points used. In a tournament, each player is assigned a random ELO value from this range (higher ELO is more likely to win).")]
        [Range(0, int.MaxValue, ErrorMessage = "Player range value must be greater than or equal to 0")]
        public int Range { get; set; }
    }

    [Verb("generate-auto-power", HelpText = "Generate a test file using the ELO ranking (analogous to generate-power), using a player range that increases dynaminclly with number of players in tournament.")]
    public class GenerateAutoPowerOptions : BaseGenerateOptions
    {
    }

    [Verb("generate-domination", HelpText = "Generate a new test file containing the results of a random tournament with one random player who dominates and beats all other players.")]
    public class GenerateDominationOptions : BaseGenerateOptions
    {
    }
}
