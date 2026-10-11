namespace WorldCupStandings;

public static class GroupStandings
{
    // matches look like "ESP 3-3 POR" or "ESP 3-3 POR 2/0/0 1/0/1"
    // (cards are yellow/second-yellow red/straight red, home team first)
    public static string Calculate(string[] lines)
    {
        var matches = lines.Select(Match.Parse).ToList();
        var records = TeamRecord.Tally(matches);

        // sort
        for (int i = 0; i < records.Count; i++)
        {
            for (int j = 0; j < records.Count - 1 - i; j++)
            {
                var a = records[j];
                var b = records[j + 1];
                bool swap = false;

                if (a.Points < b.Points)
                {
                    swap = true;
                }
                else if (a.Points == b.Points)
                {
                    // head to head between all the teams on these points
                    var tied = records.Where(r => r.Points == a.Points).Select(r => r.Team).ToList();
                    var headToHead = TeamRecord.Tally(
                        matches.Where(m => tied.Contains(m.Home) && tied.Contains(m.Away)));
                    var ha = headToHead.FirstOrDefault(r => r.Team == a.Team) ?? new TeamRecord(a.Team);
                    var hb = headToHead.FirstOrDefault(r => r.Team == b.Team) ?? new TeamRecord(b.Team);

                    if (ha.Points < hb.Points) swap = true;
                    else if (ha.Points == hb.Points)
                    {
                        if (ha.GoalDifference < hb.GoalDifference) swap = true;
                        else if (ha.GoalDifference == hb.GoalDifference)
                        {
                            if (ha.GoalsFor < hb.GoalsFor) swap = true;
                            else if (ha.GoalsFor == hb.GoalsFor)
                            {
                                if (a.GoalDifference < b.GoalDifference) swap = true;
                                else if (a.GoalDifference == b.GoalDifference)
                                {
                                    if (a.GoalsFor < b.GoalsFor) swap = true;
                                    else if (a.GoalsFor == b.GoalsFor)
                                    {
                                        if (a.FairPlay < b.FairPlay) swap = true;
                                        else if (a.FairPlay == b.FairPlay)
                                        {
                                            if (string.CompareOrdinal(a.Team, b.Team) > 0) swap = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (swap)
                {
                    records[j] = b;
                    records[j + 1] = a;
                }
            }
        }

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
