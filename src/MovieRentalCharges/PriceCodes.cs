namespace MovieRentalCharges;

public static class PriceCodes
{
    public static IPriceCode For(MovieRentalPriceCode priceCode) => priceCode switch
    {
        MovieRentalPriceCode.Regular => new RegularPriceCode(),
        MovieRentalPriceCode.NewRelease => new NewReleasePriceCode(),
        MovieRentalPriceCode.Children => new ChildrenPriceCode(),
        _ => throw new ArgumentOutOfRangeException(nameof(priceCode), priceCode, null),
    };
}
