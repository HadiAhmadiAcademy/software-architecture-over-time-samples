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

## This step: 3 — golden master

One real group reaches only a few of the tie-breakers. To pin down the ranking before refactoring,
we need inputs that reach all of them.

[tests/WorldCupStandings.Tests/GoldenMasterTests.cs](tests/WorldCupStandings.Tests/GoldenMasterTests.cs)
generates 200 groups: four teams drawn at random, some groups with all three rounds played and some
with only one or two, random scores from 0 to 3, and random cards. Low scores make ties on points
common, so the head-to-head, goal difference, goals scored and fair play rules all get used.

The random generator has a fixed seed (`2026`), so every run produces the same 200 groups. All of
them, each with its matches and its table, go into one file:
`GoldenMasterTests.Generated_groups.verified.txt`.

```
Group 1
  SUI 2-3 CRO 0/0/0 0/0/1
  POR 3-0 MAR 2/0/0 1/0/0
  SUI 2-1 POR 2/0/0 2/0/1
  CRO 3-3 MAR 3/1/0 3/0/0
  SUI 0-2 MAR 1/0/0 2/0/0
  CRO 3-0 POR 2/0/0 0/0/0

Pos Team  P  W  D  L GF GA  GD Pts  FP
  1 CRO   3  2  1  0  9  5  +4   7 -12
  2 MAR   3  1  1  1  5  6  -1   4  -6
  3 SUI   3  1  0  2  4  6  -2   3  -3
  4 POR   3  1  0  2  4  5  -1   3  -8
```

Nobody checked these 200 tables by hand, and nobody needs to. The old code is the oracle: whatever
it produced today is what the refactored code must produce tomorrow. That is the difference from
step 2, where we looked at the one table and agreed with it.

The matches are written above each table so that, when a table changes, the diff shows the input
that changed it.
