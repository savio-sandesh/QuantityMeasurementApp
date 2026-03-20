using ModelLayer;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAllMeasurements();

        // UC16 NEW METHODS
        List<QuantityMeasurementEntity> GetByOperation(string operationType);

        List<QuantityMeasurementEntity> GetByType(string measurementType);

        int GetCount();

        void DeleteAll();
    }
}