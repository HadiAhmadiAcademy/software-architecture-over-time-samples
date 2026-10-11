# Walkthrough: movie rental charges

Teaching notes for session 07, after the World Cup example. One block per step: what to run, what
to show, the point of the step, and what to watch for.

This file is in every step, so it stays open while you move between them.

## Before class

Run the last step once so the packages are restored and the first build in class is fast, then go
to step 1:

```
cd movie-rental-charges
git checkout movie-rental-charges/step-8-rule-properties
dotnet test
git checkout movie-rental-charges/step-1-legacy
```

FsCheck picks new inputs on every run, so the numbers in a failure message are different every
time. The input it shrinks to is not: the notes below say what it will be.

Files to have open: `MovieRental.cs`, `RULES.md`.

## Step 1 — legacy

```
git checkout movie-rental-charges/step-1-legacy
dotnet run --project src/MovieRentalCharges.Cli
```

**Show:** the price list, then `MovieRental.cs` from top to bottom: the `switch`, the local
variables with different names in each case, the late-fee formula written twice, `customer.Charge`
in four places and no return value.

**Point:** it works; check one row against `RULES.md` (Regular, 5 days: 13.00). Nobody wants to
change it, and there are no tests to say whether a change broke it.

**Watch for:** "why not write example tests?" Fair question. Ask how many examples it would take,
and which ones. Step 6 answers it.

## Step 2 — same as legacy

```
git checkout movie-rental-charges/step-2-same-as-legacy
dotnet test --logger "console;verbosity=detailed"
```

**Show:** `LegacyMovieRental.cs` (a renamed copy of step 1), then `MovieRentalProperties.cs`: one
method, two parameters, no expected value. In the test output: `Ok, passed 100 tests.`

**Live:** in `MovieRental.cs`, Children case, change `* 1.5` to `* 1.6`. Run `dotnet test`. Red:
`legacy charged [6.75], now charged [6.9]`, shrunk to `(4, Children)`. Read the shrunk input with
the class: four days is the first late day. Then put it back:

```
git checkout -- .
```

**Point:** the old code is the oracle, as in the golden master, but nothing is recorded. FsCheck
makes new inputs every run and shrinks a failure to the smallest input that still fails.

**Watch for:** the parameter is `int`, so 0 and negative days are tested too. If someone asks why,
the answer is that the code accepts them, and a characterization test records what the code does.
Also `RecordingCustomer` keeps a list: one charge of 7 is not two charges of 4 and 3.

## Step 3 — price codes

```
git checkout movie-rental-charges/step-3-price-codes
dotnet test
```

**Show:** `MovieRental.cs`, now one line, then `PriceCodes.cs` and the three price code classes.
The bodies are the old `case` blocks with `return` instead of `customer.Charge`.

**Point:** green, and the test did not change.

**Watch for:** an undefined enum value. The old `switch` charged nothing; `PriceCodes.For` throws.
FsCheck only generates the three defined values, so the property cannot see it. A property is only
as good as its generators.

## Step 4 — late-fee template

```
git checkout movie-rental-charges/step-4-late-fee-template
dotnet test
```

**Show:** `PriceCodeWithLateFee.cs`: late or not, decided once. `RegularPriceCode.cs` and
`ChildrenPriceCode.cs` still each have their own late-fee formula.

**Point:** green. Small steps: the shape first, the duplicated formula next.

## Step 5 — one late-fee rule

```
git checkout movie-rental-charges/step-5-one-late-fee-rule
dotnet test
```

**Show:** `PriceCodeWithLateFee.cs` with the formula and `LateFeeRate`. Then `RegularPriceCode.cs`
and `ChildrenPriceCode.cs`, two numbers each, next to the table in `RULES.md`.

**Point:** green. The code now reads like the rules.

**Watch for:** "`==` on doubles?" Safe here: every price is a multiple of 0.25, which a `double`
holds exactly. With prices like 0.10 the property would need a tolerance, or the code `decimal`.

## Step 6 — the slip

```
git checkout movie-rental-charges/step-6-slip
```

**Show:** first, without running anything, the diff:

```
git diff movie-rental-charges/step-5-one-late-fee-rule -- src
```

`AllowedDays` became `daysRented`. It reads fine: "days times the daily price, plus the late fee".
Ask the class whether it is right. Then run the tests:

```
dotnet test
```

Red, shrunk to `(3, Regular)` (7 expected, 9 charged) or `(4, Children)` (6.75 expected, 8.25
charged). Run it two or three times: the original input changes every time, the shrunk one is
always one of these two.

**Point:** nobody wrote "three days of Regular" anywhere. FsCheck found the first late day by
itself, and shrinking turned a random failing input into the one that explains the bug: the late
day is charged twice.

**Watch for:** an example test for 1 or 2 days, or for New release, stays green with this slip.
That is the answer to the question from step 1. And if someone suggests changing
`LegacyMovieRental` to match: that is the moment to ask what the oracle is for.

## Step 7 — fixed

```
git checkout movie-rental-charges/step-7-fixed
dotnet test
```

**Show:**

```
git diff movie-rental-charges/step-2-same-as-legacy -- tests
```

Empty. Then:

```
git diff --stat movie-rental-charges/step-2-same-as-legacy -- src
```

**Point:** every line of `MovieRental.Charge` changed; the test did not.

## Step 8 — rule properties

```
git checkout movie-rental-charges/step-8-rule-properties
dotnet test
```

**Show:** `MovieRentalProperties.cs`. The rules table at the top, then the four properties, each
next to the line of `RULES.md` it comes from. `LegacyMovieRental.cs` is gone.

**Live:** bring the slip back and run:

```
git checkout movie-rental-charges/step-6-slip -- src
dotnet test
git checkout HEAD -- src
```

`One_more_day_costs_the_daily_price_or_the_late_fee` fails: `day 3 of Regular cost 5, expected 3`
(or `day 4 of Children cost 3.75, expected 2.25`). `Keeping_a_movie_longer_never_costs_less` stays
green.

**Point:** comparing with the old code says "nothing changed". A rule property says "this is
right", and when it fails it names the rule. The first two properties together fix every charge,
like induction: day 1 is right, and every next day adds the right amount.

**Watch for:** the green property under the slip. A property can be true and too weak to catch a
mistake. And the days are `PositiveInt` now: with the oracle gone, 0 and negative days are not
checked. `RULES.md` lists it under "Not covered".

## After class

```
git checkout movie-rental-charges
```
