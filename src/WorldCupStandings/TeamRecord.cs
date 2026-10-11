namespace WorldCupStandings;

public class TeamRecord(string team)
{
    public string Team { get; } = team;
    public int Played { get; private set; }
    public int Won { get; private set; }
    public int Drawn { get; private set; }
    public int Lost { get; private set; }
    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }
    public int FairPlay { get; private set; }

    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => Won * 3 + Drawn;

    public void Add(int scored, int conceded, Cards cards)
    {
        Played++;
        GoalsFor += scored;
        GoalsAgainst += conceded;
        FairPlay += cards.FairPlay;
        if (scored > conceded) Won++;
        else if (scored == conceded) Drawn++;
        else Lost++;
    }

    // One record per team, in the order the teams first appear
    public static List<TeamRecord> Tally(IEnumerable<Match> matches)
    {
        var records = new List<TeamRecord>();
        foreach (var match in matches)
        {
            RecordFor(records, match.Home).Add(match.HomeGoals, match.AwayGoals, match.HomeCards);
            RecordFor(records, match.Away).Add(match.AwayGoals, match.HomeGoals, match.AwayCards);
        }
        return records;
    }

    static TeamRecord RecordFor(List<TeamRecord> records, string team)
    {
        var record = records.FirstOrDefault(r => r.Team == team);
        if (record == null)
        {
            record = new TeamRecord(team);
            records.Add(record);
        }
        return record;
    }
}
