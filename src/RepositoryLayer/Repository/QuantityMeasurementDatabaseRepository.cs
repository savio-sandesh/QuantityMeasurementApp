using ModelLayer;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RepositoryLayer.Exceptions;
using UtilityLayer;

namespace RepositoryLayer
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string connectionString;

        public QuantityMeasurementDatabaseRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));

            this.connectionString = connectionString;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            DatabaseOperationExecutor.Execute(() =>
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    string query = SqlQueryConstants.InsertMeasurement;

                    using var cmd = new SqlCommand(query, connection, transaction);

                    cmd.Parameters.AddWithValue("@v1", entity.FirstValue);
                    cmd.Parameters.AddWithValue("@u1", entity.FirstUnit);
                    cmd.Parameters.AddWithValue("@v2", entity.SecondValue);
                    cmd.Parameters.AddWithValue("@u2", entity.SecondUnit);
                    cmd.Parameters.AddWithValue("@res", entity.ResultValue);
                    cmd.Parameters.AddWithValue("@op", entity.Operation);
                    cmd.Parameters.AddWithValue("@type", entity.FirstMeasurementType);

                    Logger.Info("Executing INSERT query");
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    Logger.Info("Measurement saved successfully");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        Logger.Info("Transaction rolled back due to save failure");
                    }
                    catch (Exception rollbackEx)
                    {
                        Logger.Error("Rollback failed: " + rollbackEx.Message);
                    }

                    Logger.Error("Error saving measurement: " + ex.Message);
                    throw;
                }
            }, "Error saving measurement");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                var list = new List<QuantityMeasurementEntity>();

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = SqlQueryConstants.SelectAllMeasurements;

                using var cmd = new SqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, "Error fetching all measurements");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operationType)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                var list = new List<QuantityMeasurementEntity>();

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = SqlQueryConstants.SelectByOperation;

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@op", operationType);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, "Error fetching by operation");
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetByType(string measurementType)
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                var list = new List<QuantityMeasurementEntity>();

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = SqlQueryConstants.SelectByType;

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@type", measurementType);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
                return list;
            }, "Error fetching by type");
        }

        public int GetCount()
        {
            return DatabaseOperationExecutor.Execute(() =>
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = SqlQueryConstants.SelectCount;

                using var cmd = new SqlCommand(query, connection);

                return (int)cmd.ExecuteScalar();
            }, "Error getting count");
        }

        public void DeleteAll()
        {
            DatabaseOperationExecutor.Execute(() =>
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = SqlQueryConstants.DeleteAllMeasurements;

                using var cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }, "Error deleting all data");
        }

        private QuantityMeasurementEntity Map(SqlDataReader reader)
        {
            return new QuantityMeasurementEntity
            {
                FirstValue = Convert.ToDouble(reader[SqlColumnConstants.Value1]),
                FirstUnit = reader[SqlColumnConstants.Unit1]?.ToString() ?? string.Empty,
                FirstMeasurementType = reader[SqlColumnConstants.MeasurementType]?.ToString() ?? string.Empty,
                SecondValue = Convert.ToDouble(reader[SqlColumnConstants.Value2]),
                SecondUnit = reader[SqlColumnConstants.Unit2]?.ToString() ?? string.Empty,
                SecondMeasurementType = reader[SqlColumnConstants.MeasurementType]?.ToString() ?? string.Empty,
                ResultValue = reader[SqlColumnConstants.Result]?.ToString() ?? string.Empty,
                ResultUnit = string.Empty,
                ResultMeasurementType = reader[SqlColumnConstants.MeasurementType]?.ToString() ?? string.Empty,
                Operation = reader[SqlColumnConstants.OperationType]?.ToString() ?? string.Empty,
                IsError = false,
                ErrorMessage = string.Empty
            };
        }
    }
}