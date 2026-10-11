namespace MovieRentalCharges;

public class RegularPriceCode : PriceCodeWithLateFee
{
    private const int RentalCostPerDay = 2;

    protected override int AllowedDays => 2;

    protected override double NormalCharge(int daysRented)
    {
        return daysRented * RentalCostPerDay;
    }

    protected override double ChargeWithLateFee(int daysRented)
    {
        var lateRentalDays = daysRented - AllowedDays;
        var lateRentalPercentageIncrease = 1.5;
        var totalRentalFeeIncludingLateFee = AllowedDays * RentalCostPerDay +
                                             lateRentalDays * RentalCostPerDay *
                                             lateRentalPercentageIncrease;

        return totalRentalFeeIncludingLateFee;
    }
}
