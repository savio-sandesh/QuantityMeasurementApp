using ModelLayer;

namespace BusinessLayer
{
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDTO q1, QuantityDTO q2);

        QuantityDTO Convert(QuantityDTO quantity, string targetUnit);


        double Divide(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit);
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit);
    }
}