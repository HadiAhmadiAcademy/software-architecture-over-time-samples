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

## This step: 2 — first approval

[tests/WorldCupStandings.Tests/GroupStandingsTests.cs](tests/WorldCupStandings.Tests/GroupStandingsTests.cs)
has one test. It runs the 2018 Group B results through the legacy code and hands the table to
[Verify](https://github.com/VerifyTests/Verify).

The test has no expected value written in it. The expected value is the file
`GroupStandingsTests.Group_B_2018.verified.txt` next to it.

To see how that file came to be, delete it and run the tests:

```
dotnet test
```

1. The test fails, because there is nothing approved yet.
2. Verify writes what the code produced to `GroupStandingsTests.Group_B_2018.received.txt` and,
   if a diff tool is installed, opens it.
3. You look at the table. It matches the real 2018 table, so you approve it: accept it in the diff
   tool, or rename `.received.txt` to `.verified.txt`.
4. Run again. Green.

This is a characterization test: it records what the code does today, not what it should do. We
looked at the table and agreed with it, but the test would have recorded it either way.

The `.verified.txt` file is committed. `.received.txt` files are in `.gitignore`.

One group exercises only a few paths through the ranking. Spain and Portugal are level on points,
head-to-head and goal difference, so this group reaches the goals-scored rule, but no group of
six real matches reaches every tie-breaker.
