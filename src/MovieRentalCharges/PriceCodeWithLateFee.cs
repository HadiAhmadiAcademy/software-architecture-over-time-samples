namespace MovieRentalCharges;

public abstract class PriceCodeWithLateFee : IPriceCode
{
    // Every day after the allowed days costs one and a half times the daily price
    private const double LateFeeRate = 1.5;

    protected abstract double DailyPrice { get; }
    protected abstract int AllowedDays { get; }

    public double Charge(int daysRented)
    {
        if (daysRented <= AllowedDays)
            return daysRented * DailyPrice;

        var lateDays = daysRented - AllowedDays;
        return AllowedDays * DailyPrice + lateDays * DailyPrice * LateFeeRate;
    }
}
