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

## This step: 5 — refactor the ranking

The bubble sort and its nested `if`s are replaced by
[Ranking.cs](src/WorldCupStandings/Ranking.cs):

```csharp
return tied
    .OrderByDescending(r => headToHead(r).Points)
    .ThenByDescending(r => headToHead(r).GoalDifference)
    .ThenByDescending(r => headToHead(r).GoalsFor)
    .ThenByDescending(r => r.GoalDifference)
    .ThenByDescending(r => r.GoalsFor)
    .ThenByDescending(r => r.FairPlay)
    .ThenBy(r => r.Team, StringComparer.Ordinal);
```

Teams are grouped by points. Within each group of tied teams, the tie-breakers are applied in
order, one line per rule, the same order as [RULES.md](RULES.md). `HeadToHead` gives each tied
team's record counting only the matches between the tied teams.

This is the riskiest change so far: a different sorting algorithm and a different shape for the
tie-break logic. The golden master is what makes it safe. Across its 200 groups, each tie-breaker
decides at least one pair of teams:

| Rule | Pairs decided |
|---|---|
| Head-to-head points | 63 |
| Head-to-head goal difference | 19 |
| Head-to-head goals scored | 15 |
| Goal difference | 53 |
| Goals scored | 24 |
| Fair play | 33 |
| Team name | 4 |

```
dotnet test
```

Green. Every table is unchanged.
