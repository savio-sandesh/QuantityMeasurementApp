using ModelLayer;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements();

        // UC16 NEW METHODS
        IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operationType);

        IReadOnlyList<QuantityMeasurementEntity> GetByType(string measurementType);

        IReadOnlyList<QuantityMeasurementEntity> GetHistoryByUserId(int userId);

        int GetCount();
        int GetCountByUserId(int userId);

        void DeleteAll();
    }
}