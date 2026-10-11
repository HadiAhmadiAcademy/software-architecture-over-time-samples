namespace MovieRentalCharges;

public class ChildrenPriceCode : PriceCodeWithLateFee
{
    protected override double DailyPrice => 1.5;
    protected override int AllowedDays => 3;
}
