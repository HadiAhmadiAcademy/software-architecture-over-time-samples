namespace MovieRentalCharges;

public class ChildrenPriceCode : PriceCodeWithLateFee
{
    private const double RentalCostPerDay = 1.5;

    protected override int AllowedDays => 3;

    protected override double NormalCharge(int daysRented)
    {
        var totalRentalFee = RentalCostPerDay * daysRented;
        return totalRentalFee;
    }

    protected override double ChargeWithLateFee(int daysRented)
    {
        var daysLate = daysRented - AllowedDays;
        return RentalCostPerDay * AllowedDays + daysLate * RentalCostPerDay * 1.5;
    }
}
