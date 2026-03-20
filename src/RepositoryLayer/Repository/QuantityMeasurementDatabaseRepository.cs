using ModelLayer;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using UtilityLayer;
using RepositoryLayer.Exceptions;

namespace RepositoryLayer
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string connectionString;

        public QuantityMeasurementDatabaseRepository()
        {
            connectionString = DatabaseConfig.GetConnectionString();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Measurements
                                (Value1, Unit1, Value2, Unit2, Result, OperationType, MeasurementType)
                                VALUES (@v1, @u1, @v2, @u2, @res, @op, @type)";

                using var cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@v1", entity.FirstValue);
                cmd.Parameters.AddWithValue("@u1", entity.FirstUnit);
                cmd.Parameters.AddWithValue("@v2", entity.SecondValue);
                cmd.Parameters.AddWithValue("@u2", entity.SecondUnit);
                cmd.Parameters.AddWithValue("@res", entity.ResultValue);
                cmd.Parameters.AddWithValue("@op", entity.Operation);
                cmd.Parameters.AddWithValue("@type", entity.FirstMeasurementType);

                Logger.Info("Executing INSERT query");
                cmd.ExecuteNonQuery();
                Logger.Info("Measurement saved successfully");
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving measurement: " + ex.Message);
                throw new DatabaseException("Error saving measurement", ex);
            }
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            var list = new List<QuantityMeasurementEntity>();

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Measurements";

                using var cmd = new SqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error fetching all measurements", ex);
            }

            return list;
        }

        public List<QuantityMeasurementEntity> GetByOperation(string operationType)
        {
            var list = new List<QuantityMeasurementEntity>();

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Measurements WHERE OperationType=@op";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@op", operationType);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error fetching by operation", ex);
            }

            return list;
        }

        public List<QuantityMeasurementEntity> GetByType(string measurementType)
        {
            var list = new List<QuantityMeasurementEntity>();

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Measurements WHERE MeasurementType=@type";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@type", measurementType);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error fetching by type", ex);
            }

            return list;
        }

        public int GetCount()
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Measurements";

                using var cmd = new SqlCommand(query, connection);

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error getting count", ex);
            }
        }

        public void DeleteAll()
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Measurements";

                using var cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error deleting all data", ex);
            }
        }

        private QuantityMeasurementEntity Map(SqlDataReader reader)
        {
            return new QuantityMeasurementEntity
            {
                FirstValue = Convert.ToDouble(reader["Value1"]),
                FirstUnit = reader["Unit1"]?.ToString() ?? string.Empty,
                FirstMeasurementType = reader["MeasurementType"]?.ToString() ?? string.Empty,
                SecondValue = Convert.ToDouble(reader["Value2"]),
                SecondUnit = reader["Unit2"]?.ToString() ?? string.Empty,
                SecondMeasurementType = reader["MeasurementType"]?.ToString() ?? string.Empty,
                ResultValue = reader["Result"]?.ToString() ?? string.Empty,
                ResultUnit = string.Empty,
                ResultMeasurementType = reader["MeasurementType"]?.ToString() ?? string.Empty,
                Operation = reader["OperationType"]?.ToString() ?? string.Empty,
                IsError = false,
                ErrorMessage = string.Empty
            };
        }
    }
}