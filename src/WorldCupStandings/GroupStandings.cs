namespace WorldCupStandings;

public static class GroupStandings
{
    // matches look like "ESP 3-3 POR" or "ESP 3-3 POR 2/0/0 1/0/1"
    // (cards are yellow/second-yellow red/straight red, home team first)
    public static string Calculate(string[] lines)
    {
        var matches = lines.Select(Match.Parse).ToList();
        var records = Ranking.Rank(matches);

        var s = "Pos Team  P  W  D  L GF GA  GD Pts  FP\n";
        for (int i = 0; i < records.Count; i++)
        {
            var t = records[i];
            var gd = t.GoalDifference;
            s += (i + 1).ToString().PadLeft(3) + " " + t.Team.PadRight(4)
                + " " + t.Played.ToString().PadLeft(2)
                + " " + t.Won.ToString().PadLeft(2)
                + " " + t.Drawn.ToString().PadLeft(2)
                + " " + t.Lost.ToString().PadLeft(2)
                + " " + t.GoalsFor.ToString().PadLeft(2)
                + " " + t.GoalsAgainst.ToString().PadLeft(2)
                + " " + (gd > 0 ? "+" + gd : gd.ToString()).PadLeft(3)
                + " " + t.Points.ToString().PadLeft(3)
                + " " + t.FairPlay.ToString().PadLeft(3)
                + "\n";
        }
        return s;
    }
}
