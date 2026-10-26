
namespace TrainingManagementSystem.Services;

public class FeeCalculator
{
    public decimal CalculateFee(decimal baseFee)
    {
        if (baseFee < 0)
            throw new ArgumentException("Fee cannot be negative.");

        return baseFee;
    }

    public (decimal BaseFee, decimal Discount, decimal FinalFee)
        CalculateFee(decimal baseFee, decimal discount)
    {
        if (baseFee < 0 || discount < 0 || discount > baseFee)
            throw new ArgumentException("Invalid fee or discount.");

        return (
            BaseFee: baseFee,
            Discount: discount,
            FinalFee: baseFee - discount
        );
    }

    public (decimal BaseFee, decimal Discount, decimal Surcharge,
        decimal FinalFee) CalculateSessionFee(
            decimal baseFee,
            bool earlyBird,
            bool groupDiscount,
            bool online)
    {
        if (baseFee < 0)
            throw new ArgumentException("Fee cannot be negative.");

        decimal discount = 0m;

        if (earlyBird)
            discount += baseFee * 0.10m;

        if (groupDiscount)
            discount += baseFee * 0.05m;

        decimal surcharge = online ? 200m : 800m;

        return (
            BaseFee: baseFee,
            Discount: discount,
            Surcharge: surcharge,
            FinalFee: baseFee - discount + surcharge
        );
    }
}
