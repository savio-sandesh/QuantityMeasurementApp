using RepositoryLayer;

namespace BusinessLayer
{
    public static class QuantityServiceFactory
    {
        public static IQuantityMeasurementService CreateService()
        {
            var repository = QuantityMeasurementCacheRepository.Instance;

            return new QuantityMeasurementService(repository);
        }
    }
}