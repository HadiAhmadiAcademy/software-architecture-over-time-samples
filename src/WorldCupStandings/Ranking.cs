namespace WorldCupStandings;

public static class Ranking
{
    public static List<TeamRecord> Rank(IReadOnlyList<Match> matches) =>
        TeamRecord.Tally(matches)
            .GroupBy(r => r.Points)
            .OrderByDescending(tied => tied.Key)
            .SelectMany(tied => BreakTie(tied.ToList(), matches))
            .ToList();

    // The tie-breakers in RULES.md, in order
    static IEnumerable<TeamRecord> BreakTie(List<TeamRecord> tied, IReadOnlyList<Match> matches)
    {
        var headToHead = HeadToHead(tied, matches);

        return tied
            .OrderByDescending(r => headToHead(r).Points)
            .ThenByDescending(r => headToHead(r).GoalDifference)
            .ThenByDescending(r => headToHead(r).GoalsFor)
            .ThenByDescending(r => r.GoalDifference)
            .ThenByDescending(r => r.GoalsFor)
            .ThenByDescending(r => r.FairPlay)
            .ThenBy(r => r.Team, StringComparer.Ordinal);
    }

    // Each tied team's record counting only the matches between the tied teams
    static Func<TeamRecord, TeamRecord> HeadToHead(List<TeamRecord> tied, IReadOnlyList<Match> matches)
    {
        var teams = tied.Select(r => r.Team).ToHashSet();
        var records = TeamRecord.Tally(matches.Where(m => teams.Contains(m.Home) || teams.Contains(m.Away)));
        return r => records.FirstOrDefault(h => h.Team == r.Team) ?? new TeamRecord(r.Team);
    }
}
