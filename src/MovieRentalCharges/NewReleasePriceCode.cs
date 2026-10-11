namespace MovieRentalCharges;

public class NewReleasePriceCode : IPriceCode
{
    public double Charge(int daysRented)
    {
        var rentalCostPerDay = 3;
        var totalRentalFee = rentalCostPerDay * daysRented;
        return totalRentalFee;
    }
}
