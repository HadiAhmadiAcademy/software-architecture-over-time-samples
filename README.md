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

## This step: 6 — the slip

One character changed in [Ranking.cs](src/WorldCupStandings/Ranking.cs), in `HeadToHead`:

```diff
- matches.Where(m => teams.Contains(m.Home) && teams.Contains(m.Away))
+ matches.Where(m => teams.Contains(m.Home) || teams.Contains(m.Away))
```

It is the kind of change that looks harmless in review. Now "head-to-head" counts every match a
tied team played, not only the matches between the tied teams, so it is just the overall record
again.

```
dotnet test
```

- `Group_B_2018` is **green**. Spain and Portugal drew their match, so head-to-head did not decide
  anything there, and the slip does not show.
- `Generated_groups` is **red**. The diff starts at the very first group:

```diff
 Pos Team  P  W  D  L GF GA  GD Pts  FP
   1 CRO   3  2  1  0  9  5  +4   7 -12
   2 MAR   3  1  1  1  5  6  -1   4  -6
-  3 SUI   3  1  0  2  4  6  -2   3  -3
-  4 POR   3  1  0  2  4  5  -1   3  -8
+  3 POR   3  1  0  2  4  5  -1   3  -8
+  4 SUI   3  1  0  2  4  6  -2   3  -3
```

Switzerland beat Portugal 2–1, so Switzerland should be above them even with a worse goal
difference. With the slip, goal difference decides instead, and the two teams swap.

The test does not say what rule was broken. It shows which tables changed and the matches that
produced them, and that is enough to find the line.

Do not approve this output. The `.received.txt` file is the evidence; the code is what needs fixing.
