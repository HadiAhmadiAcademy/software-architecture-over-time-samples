namespace WorldCupStandings.Tests;

public class GroupStandingsTests
{
    [Fact]
    public Task Group_B_2018()
    {
        string[] matches =
        [
            "MAR 0-1 IRN",
            "POR 3-3 ESP",
            "POR 1-0 MAR",
            "IRN 0-1 ESP",
            "IRN 1-1 POR",
            "ESP 2-2 MAR",
        ];

        return Verify(GroupStandings.Calculate(matches));
    }
}
