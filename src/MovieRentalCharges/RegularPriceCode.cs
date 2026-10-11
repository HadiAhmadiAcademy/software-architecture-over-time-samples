namespace MovieRentalCharges;

public class RegularPriceCode : PriceCodeWithLateFee
{
    protected override double DailyPrice => 2;
    protected override int AllowedDays => 2;
}
