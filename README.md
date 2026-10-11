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

## This step: 2 — same as legacy

Two new files in `tests/MovieRentalCharges.Tests`:

- [LegacyMovieRental.cs](tests/MovieRentalCharges.Tests/LegacyMovieRental.cs) is a copy of
  `MovieRental` as it is in step 1, renamed. It will not change. It is the oracle: whatever it
  charges is the expected answer.
- [MovieRentalProperties.cs](tests/MovieRentalCharges.Tests/MovieRentalProperties.cs) has one
  property:

```csharp
[Property]
public Property Charges_the_same_as_the_legacy_code(int daysRented, MovieRentalPriceCode priceCode)
```

FsCheck calls it 100 times, each time with a generated `daysRented` and `priceCode`. Both classes
charge a [RecordingCustomer](tests/MovieRentalCharges.Tests/RecordingCustomer.cs), and the charges
must be the same.

The property has no expected value written in it. As in the World Cup golden master, the old code
is the oracle, but nothing is recorded: the old code runs next to the new one, on every input,
every run.

Two choices worth noticing:

- **`int`, not a positive number.** The generated days include 0 and negative numbers.
  [RULES.md](RULES.md) says nothing about them, but the code accepts them and charges something,
  so we pin that down too. A characterization test records what the code does, not what we think
  it should do.
- **A list of charges, not a total.** Charging 4 and then 3 is not the same as charging 7 once:
  the customer sees two charges. If a refactoring splits one charge into two, the property fails.

Right now `MovieRental` is the same code as the copy, so the property cannot fail. To see that it
can, change the `1.5` in the Children case of `MovieRental.cs` to `1.6` and run the tests:

```
dotnet test
```

```
Label of failing property: legacy charged [6.75], now charged [6.9]
Original:
(5, Children)
Shrunk:
(4, Children)
```

FsCheck found an input that fails, then shrank it: it tried smaller inputs until it found the
smallest one that still fails. Four days is the first late day for Children. The original input
is different on every run; the shrunk one is always `(4, Children)`. If the first failing input
is already `(4, Children)`, there is nothing to shrink and only `Original` is printed.

Put the `1.5` back before moving on:

```
git checkout -- .
```
