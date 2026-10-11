using System.Text;

namespace WorldCupStandings;

public static class StandingsTable
{
    public static string Render(IReadOnlyList<TeamRecord> ranked)
    {
        var table = new StringBuilder();
        table.Append("Pos Team  P  W  D  L GF GA  GD Pts  FP\n");
        for (int i = 0; i < ranked.Count; i++)
        {
            var r = ranked[i];
            table.Append($"{i + 1,3} {r.Team,-4} {r.Played,2} {r.Won,2} {r.Drawn,2} {r.Lost,2} " +
                         $"{r.GoalsFor,2} {r.GoalsAgainst,2} {SignedGoalDifference(r),3} {r.Points,3} {r.FairPlay,3}\n");
        }
        return table.ToString();
    }

    static string SignedGoalDifference(TeamRecord r) =>
        r.GoalDifference > 0 ? $"+{r.GoalDifference}" : r.GoalDifference.ToString();
}
