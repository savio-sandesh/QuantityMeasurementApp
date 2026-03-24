using BusinessLayer;
using ModelLayer;

namespace QuantityMeasurementApp.Controllers
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            this.service = service;
        }

        public QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2)
        {
            return service.Compare(q1, q2);
        }

        public QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            return service.Add(q1, q2, targetUnit);
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            return service.Subtract(q1, q2, targetUnit);
        }
        public QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2)
        {
            return service.Divide(q1, q2);
        }

        public QuantityMeasurementDTO Convert(QuantityDTO q, string targetUnit)
        {
            return service.Convert(q, targetUnit);
        }
    }
}