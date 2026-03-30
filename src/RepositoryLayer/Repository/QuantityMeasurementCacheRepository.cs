using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return new List<QuantityMeasurementEntity>(cache); // prevent external modification
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operationType)
        {
            return cache
                .Where(x => string.Equals(x.Operation, operationType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByType(string measurementType)
        {
            return cache
                .Where(x => string.Equals(x.FirstMeasurementType, measurementType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetHistoryByUserId(int userId)
        {
            return cache
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public int GetCount()
        {
            return cache.Count;
        }

        public void DeleteAll()
        {
            cache.Clear();
        }
    }
}