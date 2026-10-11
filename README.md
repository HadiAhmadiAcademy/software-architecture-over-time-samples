# Movie rental charges

A property-based testing example. The code charges a customer for renting a movie: a daily price
that depends on the movie's price code, and a late fee for some price codes. The first version is
one method with a `switch`, and nobody wants to touch it. We pin down what it does with a property
that compares it with a frozen copy of itself, refactor it while
[FsCheck](https://fscheck.github.io/FsCheck/) generates the inputs, and at the end replace the copy
with properties taken from the rules.

It follows the World Cup example (branch `world-cup-standings`). There, what the old code printed
for 200 generated groups was recorded in a file. Here nothing is recorded: FsCheck generates new
inputs on every run, the old code works out the expected answer on the spot, and when something
fails, FsCheck shrinks the input to the smallest one that still fails.

The pricing rules are in [RULES.md](RULES.md). Teaching notes for class are in
[session-07-movie-rental-charges-walkthrough.md](session-07-movie-rental-charges-walkthrough.md).

## Steps

Each step is a tag on this branch.

| Tag | What happens |
|---|---|
| `movie-rental-charges/step-1-legacy` | The legacy code and a console app that prints a price list |
| `movie-rental-charges/step-2-same-as-legacy` | A frozen copy of the legacy code, and one property: charge the same as the copy |
| `movie-rental-charges/step-3-price-codes` | Each case of the `switch` becomes its own price code class |
| `movie-rental-charges/step-4-late-fee-template` | Regular and Children share one late-or-not template |
| `movie-rental-charges/step-5-one-late-fee-rule` | The late fee written once; a price code is two numbers |
| `movie-rental-charges/step-6-slip` | A small mistake in the late fee; FsCheck shrinks it to the first late day |
| `movie-rental-charges/step-7-fixed` | The mistake fixed |
| `movie-rental-charges/step-8-rule-properties` | The copy is gone; properties taken from `RULES.md` instead |

Move between steps:

```
git checkout movie-rental-charges/step-3-price-codes
```

Go back to the latest:

```
git checkout movie-rental-charges
```

## Running

Needs the .NET 10 SDK.

```
dotnet run --project src/MovieRentalCharges.Cli
dotnet test
```

To see how many inputs FsCheck tried, run the tests at detailed verbosity:

```
dotnet test --logger "console;verbosity=detailed"
```

## This step: 6 — the slip

One word changed in [PriceCodeWithLateFee.cs](src/MovieRentalCharges/PriceCodeWithLateFee.cs):

```diff
- return AllowedDays * DailyPrice + lateDays * DailyPrice * LateFeeRate;
+ return daysRented * DailyPrice + lateDays * DailyPrice * LateFeeRate;
```

It reads well: "the days times the daily price, plus the late fee". But now every late day is
charged twice, once at the daily price and once at the late rate.

```
dotnet test
```

Red:

```
Label of failing property: legacy charged [7], now charged [9]
Original:
(6, Regular)
Shrunk:
(3, Regular)
```

Three days of Regular should be 2 × 2.00 + 1 × 3.00 = 7.00. The slip charges 3 × 2.00 + 3.00 =
9.00.

Run it again and the numbers change: FsCheck uses a new seed every run. Sometimes it shrinks to
`(3, Regular)`, sometimes to `(4, Children)`, which charges 8.25 instead of 6.75. It is always the
first late day of a price code that has a late fee: the smallest input that shows the mistake.
Nobody wrote a test for "three days of Regular". FsCheck found it.

To run the same inputs again, copy the pair of numbers after `Falsifiable, after ... tests` into
the attribute:

```csharp
[Property(Replay = "(11707880873758951520,1185712109210101887)")]
```

Remove it again when the bug is fixed.

Do not change `LegacyMovieRental` to make the test pass. The copy is the oracle; the code is what
needs fixing.
