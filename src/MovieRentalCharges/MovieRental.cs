namespace MovieRentalCharges;

public class MovieRental(int daysRented, MovieRentalPriceCode priceCode)
{
    public void Charge(IPayingCustomer customer) =>
        customer.Charge(PriceCodes.For(priceCode).Charge(daysRented));
}
