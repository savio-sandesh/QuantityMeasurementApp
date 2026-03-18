using ModelLayer;
using System.Collections.Generic;

namespace RepositoryLayer
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly QuantityMeasurementCacheRepository instance =
            new QuantityMeasurementCacheRepository();

        private readonly List<QuantityMeasurementEntity> cache =
            new List<QuantityMeasurementEntity>();

        private QuantityMeasurementCacheRepository() { }

        public static QuantityMeasurementCacheRepository Instance => instance;

        public void Save(QuantityMeasurementEntity entity)
        {
            cache.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return cache;
        }
    }
}