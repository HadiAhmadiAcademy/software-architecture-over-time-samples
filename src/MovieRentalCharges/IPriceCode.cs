namespace MovieRentalCharges;

public interface IPriceCode
{
    double Charge(int daysRented);
}
