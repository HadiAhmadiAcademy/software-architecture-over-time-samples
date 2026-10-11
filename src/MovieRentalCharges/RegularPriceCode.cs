namespace MovieRentalCharges;

public class RegularPriceCode : IPriceCode
{
    public double Charge(int daysRented)
    {
        var rentalCostPerDay = 2;
        var allowedRentalPeriod = 2;

        if (daysRented > allowedRentalPeriod)
        {
            var lateRentalDays = daysRented - allowedRentalPeriod;
            var lateRentalPercentageIncrease = 1.5;
            var totalRentalFeeIncludingLateFee = allowedRentalPeriod * rentalCostPerDay +
                                                 lateRentalDays * rentalCostPerDay *
                                                 lateRentalPercentageIncrease;

            return totalRentalFeeIncludingLateFee;
        }
        else
        {
            var totalRentalFee = daysRented * rentalCostPerDay;
            return totalRentalFee;
        }
    }
}
