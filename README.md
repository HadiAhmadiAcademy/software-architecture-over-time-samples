# World Cup group standings

An approval testing example. The code takes the results of a World Cup group and prints the
standings table. The first version works but nobody wants to touch it. We pin down what it does
with approval tests, then refactor it with the tests watching.

The ranking rules are in [RULES.md](RULES.md). Teaching notes for class are in
[session-07-world-cup-standings-walkthrough.md](session-07-world-cup-standings-walkthrough.md).

## Steps

Each step is a tag on this branch.

| Tag | What happens |
|---|---|
| `world-cup-standings/step-1-legacy` | The legacy code and a console app that prints one group |
| `world-cup-standings/step-2-first-approval` | One approval test on one real group |
| `world-cup-standings/step-3-golden-master` | A golden master over 200 generated groups |
| `world-cup-standings/step-4-refactor-model` | Parse once into `Match` and `TeamRecord` |
| `world-cup-standings/step-5-refactor-ranking` | The tie-breakers as one ordered list of rules |
| `world-cup-standings/step-6-slip` | A small mistake in head-to-head; the golden master catches it |
| `world-cup-standings/step-7-fixed` | The mistake fixed and rendering split from ranking |

Move between steps:

```
git checkout world-cup-standings/step-3-golden-master
```

Go back to the latest:

```
git checkout world-cup-standings
```

## Running

Needs the .NET 10 SDK.

```
dotnet run --project src/WorldCupStandings.Cli
dotnet test
```

## This step: 7 — fixed

The `||` in `HeadToHead` is back to `&&`, and the last piece of the old method is moved out:
[StandingsTable.cs](src/WorldCupStandings/StandingsTable.cs) renders the table from the ranked
records.

[GroupStandings.cs](src/WorldCupStandings/GroupStandings.cs) is now one line:

```csharp
public static string Calculate(string[] lines) =>
    StandingsTable.Render(Ranking.Rank(lines.Select(Match.Parse).ToList()));
```

Parsing, counting, ranking and rendering each live in their own file. The public method and its
output are the same as in step 1.

```
dotnet test
```

Green. The `.verified.txt` files have not changed since steps 2 and 3. Compare:

```
git diff world-cup-standings/step-3-golden-master -- "*.verified.txt"
```

The diff is empty: every line of the code changed, and none of the behaviour did.
