namespace MovieRentalCharges.Tests;

// Remembers every charge, so two charges of 2 and 5 are not the same as one charge of 7
public class RecordingCustomer : IPayingCustomer
{
    public List<double> Charges { get; } = [];

    public void Charge(double amountCharged) => Charges.Add(amountCharged);

    public override string ToString() => $"[{string.Join(", ", Charges)}]";
}
