using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace MovieRentalCharges.Tests;

public class MovieRentalProperties
{
    // RULES.md, as data
    private const double LateFeeRate = 1.5;

    private static (double DailyPrice, int AllowedDays) RulesFor(MovieRentalPriceCode priceCode) => priceCode switch
    {
        MovieRentalPriceCode.Regular => (2, 2),
        MovieRentalPriceCode.NewRelease => (3, int.MaxValue),
        MovieRentalPriceCode.Children => (1.5, 3),
        _ => throw new ArgumentOutOfRangeException(nameof(priceCode), priceCode, null),
    };

    [Property]
    public Property One_day_costs_the_daily_price(MovieRentalPriceCode priceCode)
    {
        var (dailyPrice, _) = RulesFor(priceCode);
        var charge = ChargeFor(1, priceCode);

        return (charge == dailyPrice)
            .Label($"1 day of {priceCode} charged {charge}, the daily price is {dailyPrice}");
    }

    [Property]
    public Property One_more_day_costs_the_daily_price_or_the_late_fee(PositiveInt days, MovieRentalPriceCode priceCode)
    {
        var (dailyPrice, allowedDays) = RulesFor(priceCode);
        var nextDay = days.Get + 1;
        var expected = nextDay > allowedDays ? dailyPrice * LateFeeRate : dailyPrice;
        var actual = ChargeFor(nextDay, priceCode) - ChargeFor(days.Get, priceCode);

        return (actual == expected)
            .Label($"day {nextDay} of {priceCode} cost {actual}, expected {expected}");
    }

    [Property]
    public bool Keeping_a_movie_longer_never_costs_less(PositiveInt days, MovieRentalPriceCode priceCode) =>
        ChargeFor(days.Get + 1, priceCode) > ChargeFor(days.Get, priceCode);

    [Property]
    public bool A_rental_is_charged_once(PositiveInt days, MovieRentalPriceCode priceCode)
    {
        var customer = new RecordingCustomer();
        new MovieRental(days.Get, priceCode).Charge(customer);
        return customer.Charges.Count == 1;
    }

    private static double ChargeFor(int days, MovieRentalPriceCode priceCode)
    {
        var customer = new RecordingCustomer();
        new MovieRental(days, priceCode).Charge(customer);
        return customer.Charges.Sum();
    }
}
