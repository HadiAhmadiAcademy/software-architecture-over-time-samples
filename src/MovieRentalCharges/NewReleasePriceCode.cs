namespace MovieRentalCharges;

public class NewReleasePriceCode : IPriceCode
{
    private const double DailyPrice = 3;

    public double Charge(int daysRented) => daysRented * DailyPrice;
}
