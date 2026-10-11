namespace WorldCupStandings;

public static class GroupStandings
{
    // matches look like "ESP 3-3 POR" or "ESP 3-3 POR 2/0/0 1/0/1"
    // (cards are yellow/second-yellow red/straight red, home team first)
    public static string Calculate(string[] lines) =>
        StandingsTable.Render(Ranking.Rank(lines.Select(Match.Parse).ToList()));
}
