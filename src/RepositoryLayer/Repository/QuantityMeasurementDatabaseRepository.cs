using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementWebApi.Data;
using RepositoryLayer.Exceptions;

namespace RepositoryLayer
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityDbContext _context;

        public QuantityMeasurementDatabaseRepository(QuantityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Backward-compatible path used by non-DI service initialization.
        public QuantityMeasurementDatabaseRepository(string connectionString)
            : this(CreateDbContext(connectionString))
        {
        }

        private static QuantityDbContext CreateDbContext(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }

            var options = new DbContextOptionsBuilder<QuantityDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new QuantityDbContext(options);
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DatabaseOperationExecutor.Execute(() =>
            {
                _context.Measurements.Add(entity);
                _context.SaveChanges();
            }, "Error saving measurement");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements
                    .ToList()
                    .AsReadOnly();
            }, "Error fetching all measurements");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operationType)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements
                    .Where(x => x.OperationType == operationType)
                    .ToList()
                    .AsReadOnly();
            }, "Error fetching by operation");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByType(string measurementType)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements
                    .Where(x => x.MeasurementType == measurementType)
                    .ToList()
                    .AsReadOnly();
            }, "Error fetching by type");
        }

        public int GetCount()
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements.Count();
            }, "Error getting count");
        }

        public void DeleteAll()
        {
            DatabaseOperationExecutor.Execute(() =>
            {
                var measurements = _context.Measurements.ToList();
                _context.Measurements.RemoveRange(measurements);
                _context.SaveChanges();
            }, "Error deleting all data");
        }
    }
}