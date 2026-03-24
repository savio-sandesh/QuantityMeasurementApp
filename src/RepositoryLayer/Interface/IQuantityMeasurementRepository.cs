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

        int GetCount();

        void DeleteAll();
    }
}