namespace MovieRentalCharges.Tests;

// A copy of MovieRental as it was in step 1. Do not change it: it is the oracle that the
// refactored code is compared with.
public class LegacyMovieRental
{
    private readonly int DaysRented;
    private readonly MovieRentalPriceCode PriceCode;

    public LegacyMovieRental(int daysRented, MovieRentalPriceCode movieRentalPriceCode)
    {
        DaysRented = daysRented;
        PriceCode = movieRentalPriceCode;
    }

    public void Charge(IPayingCustomer customer)
    {
        switch (PriceCode)
        {
            case MovieRentalPriceCode.Regular:
                {
                    var rentalCostPerDay = 2;
                    var allowedRentalPeriod = 2;

                    if (DaysRented > allowedRentalPeriod)
                    {
                        var lateRentalDays = DaysRented - allowedRentalPeriod;
                        var lateRentalPercentageIncrease = 1.5;
                        var totalRentalFeeIncludingLateFee = allowedRentalPeriod * rentalCostPerDay +
                                                             lateRentalDays * rentalCostPerDay *
                                                             lateRentalPercentageIncrease;

                        customer.Charge(totalRentalFeeIncludingLateFee);
                    }
                    else
                    {
                        var totalRentalFee = DaysRented * rentalCostPerDay;
                        customer.Charge(totalRentalFee);
                    }
                    break;
                }
            case MovieRentalPriceCode.NewRelease:
                {
                    var rentalCostPerDay = 3;
                    var totalRentalFee = rentalCostPerDay * DaysRented;
                    customer.Charge(totalRentalFee);
                    break;
                }
            case MovieRentalPriceCode.Children:
                {
                    var rentalCostPerDay = 1.5;
                    var allowedRentalPeriod = 3;
                    if (DaysRented > allowedRentalPeriod)
                    {
                        var daysLate = DaysRented - allowedRentalPeriod;
                        customer.Charge(rentalCostPerDay * allowedRentalPeriod + daysLate * rentalCostPerDay * 1.5);
                    }
                    else
                    {
                        var totalRentalFee = rentalCostPerDay * DaysRented;
                        customer.Charge(totalRentalFee);
                    }

                    break;
                }
        }
    }
}
