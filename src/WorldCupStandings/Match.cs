namespace WorldCupStandings;

public record Cards(int Yellow, int SecondYellowRed, int StraightRed)
{
    public static readonly Cards None = new(0, 0, 0);

    public int FairPlay => -(Yellow * 1 + SecondYellowRed * 3 + StraightRed * 4);

    // "2/0/1"
    public static Cards Parse(string text)
    {
        var parts = text.Split('/');
        return new Cards(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
    }
}

public record Match(string Home, int HomeGoals, int AwayGoals, string Away, Cards HomeCards, Cards AwayCards)
{
    // "ESP 3-3 POR" or "ESP 3-3 POR 2/0/0 1/0/1"
    public static Match Parse(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var score = parts[1].Split('-');
        return new Match(
            parts[0],
            int.Parse(score[0]),
            int.Parse(score[1]),
            parts[2],
            parts.Length > 3 ? Cards.Parse(parts[3]) : Cards.None,
            parts.Length > 3 ? Cards.Parse(parts[4]) : Cards.None);
    }
}
