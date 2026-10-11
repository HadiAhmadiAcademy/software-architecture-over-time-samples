# Walkthrough: World Cup group standings

Teaching notes for session 07. One block per step: what to run, what to show, the point of the
step, and what to watch for.

This file is in every step, so it stays open while you move between them.

## Before class

Run the last step once so the packages are restored and the first build in class is fast, then go
to step 1:

```
cd world-cup-standings
git checkout world-cup-standings/step-7-fixed
dotnet test
git checkout world-cup-standings/step-1-legacy
```

Verify opens a diff tool when a test fails, if one is installed. Leave it on for steps 2 and 6,
where the diff is the point. To turn it off for a terminal:

```
$env:DiffEngine_Disabled = "true"     # PowerShell
```

Files to have open: `GroupStandings.cs`, `RULES.md`.

## Step 1 — legacy

```
git checkout world-cup-standings/step-1-legacy
dotnet run --project src/WorldCupStandings.Cli
```

**Show:** the table for 2018 Group B, then `GroupStandings.cs` from top to bottom: `int[8]` with
the index comment, the bubble sort, the two copy-pasted head-to-head loops, the nested `if`s, the
string built at the end.

**Point:** it works. The table is right. Nobody wants to change it, and there are no tests to
say whether a change broke it.

**Watch for:** Spain and Portugal are level on points, head-to-head and goal difference; Spain is
first on goals scored. If the class asks, `RULES.md` has the order.

## Step 2 — first approval

```
git checkout world-cup-standings/step-2-first-approval
```

**Show:** `GroupStandingsTests.cs`. One test, no expected value in the code, `Verify(...)` at the
end. Then the `.verified.txt` file next to it.

**Live:** delete the approved file and run:

```
del tests\WorldCupStandings.Tests\GroupStandingsTests.Group_B_2018.verified.txt
dotnet test
```

Red. A `.received.txt` file appears (and the diff tool opens, if on). Read the table with the
class, agree it is right, approve it: accept in the diff tool, or rename `.received.txt` to
`.verified.txt`. Run again: green.

**Point:** the test records what the code does today. We agreed with this table, but the test
would have recorded a wrong one just the same.

**Watch for:** before moving on, put the file back the way git has it:

```
git checkout -- .
```

## Step 3 — golden master

```
git checkout world-cup-standings/step-3-golden-master
dotnet test
```

**Show:** `GoldenMasterTests.cs`: 200 groups, fixed seed `2026`, some groups with fewer rounds,
random cards. Then scroll the `.verified.txt` file: each group's matches above its table.

**Point:** one real group reaches only a few tie-breakers. Many generated inputs reach all of
them. Nobody checked these 200 tables by hand; the old code is the oracle.

**Watch for:** the question "how do you know the 200 tables are correct?" The answer is that we
don't, and for this test we don't need to. We only need to know they don't change.

## Step 4 — refactor the model

```
git checkout world-cup-standings/step-4-refactor-model
dotnet test
```

**Show:** `Match.cs`, `TeamRecord.cs`, then `GroupStandings.cs` side by side with step 1
(`git diff world-cup-standings/step-3-golden-master -- src`). `d[a][6]` became `a.Points`;
head-to-head became `TeamRecord.Tally` over the matches between the tied teams.

**Point:** green, both tests. The bubble sort is still there; small steps.

## Step 5 — refactor the ranking

```
git checkout world-cup-standings/step-5-refactor-ranking
dotnet test
```

**Show:** `Ranking.cs`. Put the `OrderByDescending ... ThenBy` chain next to `RULES.md`: one line
per rule, same order.

**Point:** the sorting algorithm and the shape of the logic both changed. Green. The table in the
README shows how many pairs each rule decided in the 200 groups, which is why green means
something here.

## Step 6 — the slip

```
git checkout world-cup-standings/step-6-slip
dotnet test
```

**Show:** first, without running anything, the diff:

```
git diff world-cup-standings/step-5-refactor-ranking -- src
```

`&&` became `||`. Ask the class whether it matters. Then run the tests.

- `Group_B_2018`: green.
- `Generated_groups`: red. The first changed table is Group 1: Switzerland and Portugal swap.
  Switzerland beat Portugal 2–1, so head-to-head should put them above.

**Point:** the one real example did not catch it, because Spain and Portugal drew. The golden
master did, and the diff shows the input that produced the change.

**Watch for:** do not approve the received file. If someone suggests it, that is the moment to ask
what approving would mean.

## Step 7 — fixed

```
git checkout world-cup-standings/step-7-fixed
dotnet test
```

**Show:** `GroupStandings.cs`, now one line; `StandingsTable.cs`. Then:

```
git diff world-cup-standings/step-3-golden-master -- "*.verified.txt"
```

Empty.

**Point:** every line of the code changed since step 1; none of the behaviour did.

## After class

```
git checkout world-cup-standings
```
