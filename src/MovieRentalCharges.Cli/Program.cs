using MovieRentalCharges;

MovieRentalPriceCode[] priceCodes =
[
    MovieRentalPriceCode.Regular,
    MovieRentalPriceCode.NewRelease,
    MovieRentalPriceCode.Children,
];

Console.WriteLine($"{"Days",4}{"Regular",12}{"NewRelease",12}{"Children",12}");
for (int days = 1; days <= 6; days++)
{
    Console.Write($"{days,4}");
    foreach (var priceCode in priceCodes)
    {
        var receipt = new Receipt();
        new MovieRental(days, priceCode).Charge(receipt);
        Console.Write($"{receipt.Total,12:0.00}");
    }
    Console.WriteLine();
}

class Receipt : IPayingCustomer
{
    public double Total { get; private set; }

    public void Charge(double amountCharged) => Total += amountCharged;
}
