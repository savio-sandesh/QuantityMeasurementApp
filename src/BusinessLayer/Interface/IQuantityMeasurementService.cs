using ModelLayer;

namespace BusinessLayer
{
    public interface IQuantityMeasurementService
    {
        QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2, int userId);

        QuantityMeasurementDTO Convert(QuantityDTO quantity, string targetUnit, int userId = 0);


        QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2, int userId = 0, string targetUnit = "");
        QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit, int userId);
        QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit, int userId = 0);

        List<QuantityMeasurementDTO> GetOperationHistory(string operation);

        List<QuantityMeasurementDTO> GetMeasurementsByType(string type);

        List<QuantityMeasurementDTO> GetHistoryByUserId(int userId);

        int GetOperationCount(string operation);
        int GetOperationCountByUserId(int userId);

        List<QuantityMeasurementDTO> GetErroredOperations();
    }
}