
using System;
using Microsoft.Data.SqlClient;
using RepositoryLayer.Exceptions;

namespace UtilityLayer
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            try
            {
                string connectionString = DatabaseConfig.GetConnectionString();
                var builder = new SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrWhiteSpace(databaseName))
                {
                    throw new InvalidOperationException("Connection string must include a database name (Initial Catalog/Database).");
                }

                // Ensure the configured database exists before creating tables in it.
                var masterBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master"
                };

                using (var masterConnection = new SqlConnection(masterBuilder.ConnectionString))
                {
                    masterConnection.Open();

                    const string createDbQuery = @"
                    IF DB_ID(@dbName) IS NULL
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX) = N'CREATE DATABASE [' + REPLACE(@dbName, N']', N']]') + N']';
                        EXEC(@sql);
                    END";

                    using var createDbCmd = new SqlCommand(createDbQuery, masterConnection);
                    createDbCmd.Parameters.AddWithValue("@dbName", databaseName);
                    createDbCmd.ExecuteNonQuery();
                }

                using var connection = new SqlConnection(connectionString);
                connection.Open();

                const string query = @"
                IF OBJECT_ID('dbo.Measurements', 'U') IS NULL
                CREATE TABLE dbo.Measurements (
                    Id INT PRIMARY KEY IDENTITY,
                    Value1 FLOAT,
                    Unit1 VARCHAR(50),
                    Value2 FLOAT,
                    Unit2 VARCHAR(50),
                    Result VARCHAR(50),
                    OperationType VARCHAR(50),
                    MeasurementType VARCHAR(50),
                    CreatedAt DATETIME DEFAULT GETDATE()
                );";

                using var cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error initializing database", ex);
            }
        }
    }
}