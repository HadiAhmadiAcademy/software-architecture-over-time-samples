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

## This step: 3 — price codes

[MovieRental.cs](src/MovieRentalCharges/MovieRental.cs) is now:

```csharp
public class MovieRental(int daysRented, MovieRentalPriceCode priceCode)
{
    public void Charge(IPayingCustomer customer) =>
        customer.Charge(PriceCodes.For(priceCode).Charge(daysRented));
}
```

Each case of the `switch` moved into its own class behind
[IPriceCode](src/MovieRentalCharges/IPriceCode.cs):
[RegularPriceCode](src/MovieRentalCharges/RegularPriceCode.cs),
[NewReleasePriceCode](src/MovieRentalCharges/NewReleasePriceCode.cs) and
[ChildrenPriceCode](src/MovieRentalCharges/ChildrenPriceCode.cs).
[PriceCodes.For](src/MovieRentalCharges/PriceCodes.cs) picks one. The bodies moved as they were;
only `customer.Charge(x)` became `return x`.

```
dotnet test
```

Green. The test project is the same as in step 2.

One difference the property cannot see: for a value that is not in the enum, such as
`(MovieRentalPriceCode)7`, the old `switch` charged nothing, and `PriceCodes.For` throws.
FsCheck generates only the three values the enum defines, so this change passes. A property only
checks the inputs its generators make. Throwing is the behaviour we want here, but it is our
decision, not something the test approved.
