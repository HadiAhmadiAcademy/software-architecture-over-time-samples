namespace MovieRentalCharges;

public class ChildrenPriceCode : IPriceCode
{
    public double Charge(int daysRented)
    {
        var rentalCostPerDay = 1.5;
        var allowedRentalPeriod = 3;
        if (daysRented > allowedRentalPeriod)
        {
            var daysLate = daysRented - allowedRentalPeriod;
            return rentalCostPerDay * allowedRentalPeriod + daysLate * rentalCostPerDay * 1.5;
        }
        else
        {
            var totalRentalFee = rentalCostPerDay * daysRented;
            return totalRentalFee;
        }
    }
}
