using ModelLayer;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAllMeasurements();
    }
}