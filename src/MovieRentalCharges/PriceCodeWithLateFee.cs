namespace MovieRentalCharges;

public abstract class PriceCodeWithLateFee : IPriceCode
{
    protected abstract int AllowedDays { get; }

    public double Charge(int daysRented)
    {
        if (IsLate(daysRented))
            return ChargeWithLateFee(daysRented);
        else
            return NormalCharge(daysRented);
    }

    protected abstract double NormalCharge(int daysRented);
    protected abstract double ChargeWithLateFee(int daysRented);

    private bool IsLate(int daysRented) => daysRented > AllowedDays;
}
