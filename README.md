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

## This step: 4 — refactor the model

First refactoring: give the data names.

- [Match.cs](src/WorldCupStandings/Match.cs) parses a line like `ESP 3-3 POR 2/0/0 1/0/1` into
  a `Match` with its `Cards`. Each line is parsed once, not once per comparison.
- [TeamRecord.cs](src/WorldCupStandings/TeamRecord.cs) replaces the `int[]` with magic indices.
  `TeamRecord.Tally` counts any list of matches into one record per team.

Because `Tally` works on any list of matches, head-to-head becomes "tally only the matches between
the tied teams". The two copy-pasted loops that counted head-to-head for team `a` and team `b` are
gone.

The bubble sort and its nested `if`s are still there, now reading `a.Points` instead of `d[a][6]`.
The table is still built inside `Calculate`.

```
dotnet test
```

Both approval tests are green. The 2018 table and all 200 generated tables are exactly what the
legacy code produced.
