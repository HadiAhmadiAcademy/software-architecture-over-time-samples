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

## This step: 1 — legacy

`GroupStandings.Calculate` in
[src/WorldCupStandings/GroupStandings.cs](src/WorldCupStandings/GroupStandings.cs) does
everything in one method: it parses the match strings, counts the stats into an `int[]` per team
indexed by magic numbers, ranks the teams with a bubble sort whose comparison re-parses every
match for head-to-head, and builds the table as a string.

It gives the right answer. Run the console app on the 2018 Group B results:

```
Pos Team  P  W  D  L GF GA  GD Pts  FP
  1 ESP   3  1  2  0  6  5  +1   5   0
  2 POR   3  1  2  0  5  4  +1   5   0
  3 IRN   3  1  1  1  2  2   0   4   0
  4 MAR   3  0  1  2  2  4  -2   1   0
```

There are no tests. Before changing anything, we need a way to know that we haven't changed what
it does.
