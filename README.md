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

## This step: 1 — legacy

`MovieRental.Charge` in [src/MovieRentalCharges/MovieRental.cs](src/MovieRentalCharges/MovieRental.cs)
does everything in one `switch` over the price code. Each case has its own local variables for the
daily price and the allowed days. Regular and Children work out the late fee with the same formula,
written twice. The result goes straight to `customer.Charge(...)`: the method returns nothing, so the
only way to see what it charged is through the customer.

It gives the right answer. The console app prints a price list for one to six days:

```
dotnet run --project src/MovieRentalCharges.Cli
```

```
Days     Regular  NewRelease    Children
   1        2.00        3.00        1.50
   2        4.00        6.00        3.00
   3        7.00        9.00        4.50
   4       10.00       12.00        6.75
   5       13.00       15.00        9.00
   6       16.00       18.00       11.25
```

There are no tests. Before changing anything, we need a way to know that we haven't changed what
it does.
