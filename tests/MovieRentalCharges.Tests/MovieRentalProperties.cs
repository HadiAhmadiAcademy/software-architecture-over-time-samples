using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace MovieRentalCharges.Tests;

public class MovieRentalProperties
{
    [Property]
    public Property Charges_the_same_as_the_legacy_code(int daysRented, MovieRentalPriceCode priceCode)
    {
        var legacy = new RecordingCustomer();
        new LegacyMovieRental(daysRented, priceCode).Charge(legacy);

        var current = new RecordingCustomer();
        new MovieRental(daysRented, priceCode).Charge(current);

        return current.Charges.SequenceEqual(legacy.Charges)
            .Label($"legacy charged {legacy}, now charged {current}");
    }
}
