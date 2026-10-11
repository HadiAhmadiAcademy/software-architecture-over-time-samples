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

## This step: 8 — rule properties

`LegacyMovieRental` is gone. Comparing with the old code tells us that nothing changed. It cannot
tell us that the code is right, and it is a copy of the code we wanted to get rid of.

[MovieRentalProperties.cs](tests/MovieRentalCharges.Tests/MovieRentalProperties.cs) now has four
properties, taken from [RULES.md](RULES.md) instead of from the old code. The rules table is at
the top of the file, as data:

```csharp
MovieRentalPriceCode.Regular => (2, 2),
MovieRentalPriceCode.NewRelease => (3, int.MaxValue),
MovieRentalPriceCode.Children => (1.5, 3),
```

| Property | Says |
|---|---|
| `One_day_costs_the_daily_price` | The charge for one day is the daily price |
| `One_more_day_costs_the_daily_price_or_the_late_fee` | Going from `n` to `n + 1` days adds the daily price, or 1.5 × the daily price once day `n + 1` is past the allowed days |
| `Keeping_a_movie_longer_never_costs_less` | `n + 1` days costs more than `n` days |
| `A_rental_is_charged_once` | The customer sees one charge |

None of them works out the total the way the code does. The first two together still fix every
charge: one day is right, and every next day adds the right amount, so every number of days is
right.

```
dotnet test
```

Green. Now bring the slip from step 6 back, run the tests, and put the code back:

```
git checkout movie-rental-charges/step-6-slip -- src
dotnet test
git checkout HEAD -- src
```

```
Failed MovieRentalCharges.Tests.MovieRentalProperties.One_more_day_costs_the_daily_price_or_the_late_fee
Label of failing property: day 3 of Regular cost 5, expected 3
```

Or `day 4 of Children cost 3.75, expected 2.25`, depending on the seed. The message talks about
the rule now: the first late day costs too much. The property that compared with the old code
could only say "7 before, 9 now".

`Keeping_a_movie_longer_never_costs_less` stays green with the slip: the wrong charges still go
up. A property can be true and still too weak to catch a mistake.

The days are `PositiveInt` now. `RULES.md` says nothing about 0 or negative days, and with the
oracle gone, nothing checks them any more. That is a decision to make on purpose: add a rule, or
write down that it is not covered.
