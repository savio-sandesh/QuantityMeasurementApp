using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementWebApi.Data;
using RepositoryLayer.Exceptions;
using UtilityLayer;

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

            if (entity.UserId <= 0)
            {
                Logger.Error($"Invalid UserId while saving measurement history. UserId={entity.UserId}, Operation={entity.OperationType}");
                throw new DatabaseException("Cannot save measurement history because UserId is missing or invalid.");
            }

            DatabaseOperationExecutor.Execute(() =>
            {
                try
                {
                    _context.Measurements.Add(entity);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    string detail = ex.InnerException?.Message ?? ex.Message;
                    Logger.Error($"History save failed for UserId={entity.UserId}. Possible FK/constraint issue. Detail: {detail}");
                    throw;
                }
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

        public IReadOnlyList<QuantityMeasurementEntity> GetHistoryByUserId(int userId)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .AsReadOnly();
            }, "Error fetching history by user id");
        }

        public int GetCount()
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements.Count();
            }, "Error getting count");
        }

        public int GetCountByUserId(int userId)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                return _context.Measurements.Count(x => x.UserId == userId);
            }, "Error getting user operation count");
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