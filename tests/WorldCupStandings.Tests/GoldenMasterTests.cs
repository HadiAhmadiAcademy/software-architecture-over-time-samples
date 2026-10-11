using System.Text;

namespace WorldCupStandings.Tests;

public class GoldenMasterTests
{
    static readonly string[] Teams =
    [
        "ARG", "AUS", "BRA", "CAN", "CMR", "CRO", "ENG", "ESP",
        "FRA", "GER", "GHA", "IRN", "JPN", "KOR", "MAR", "MEX",
        "NED", "POR", "QAT", "SEN", "SUI", "URU", "USA", "WAL",
    ];

    // Who plays whom in each round of a four-team group
    static readonly (int Home, int Away)[][] Rounds =
    [
        [(0, 1), (2, 3)],
        [(0, 2), (1, 3)],
        [(0, 3), (1, 2)],
    ];

    [Fact]
    public Task Generated_groups()
    {
        var random = new Random(2026);
        var output = new StringBuilder();

        for (int group = 1; group <= 200; group++)
        {
            var teams = Teams.OrderBy(_ => random.Next()).Take(4).ToArray();
            var roundsPlayed = random.Next(4) == 0 ? random.Next(1, 3) : 3;

            var matches = new List<string>();
            foreach (var round in Rounds.Take(roundsPlayed))
            {
                foreach (var (home, away) in round)
                {
                    matches.Add($"{teams[home]} {random.Next(4)}-{random.Next(4)} {teams[away]} {Cards(random)} {Cards(random)}");
                }
            }

            output.AppendLine($"Group {group}");
            foreach (var match in matches)
            {
                output.AppendLine($"  {match}");
            }
            output.AppendLine();
            output.AppendLine(GroupStandings.Calculate(matches.ToArray()));
        }

        return Verify(output.ToString());
    }

    static string Cards(Random random)
    {
        var yellow = random.Next(4);
        var secondYellowRed = random.Next(12) == 0 ? 1 : 0;
        var straightRed = random.Next(12) == 0 ? 1 : 0;
        return $"{yellow}/{secondYellowRed}/{straightRed}";
    }
}
