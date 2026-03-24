namespace RepositoryLayer
{
    public static class SqlColumnConstants
    {
        public const string Value1 = "Value1";
        public const string Unit1 = "Unit1";
        public const string Value2 = "Value2";
        public const string Unit2 = "Unit2";
        public const string Result = "Result";
        public const string OperationType = "OperationType";
        public const string MeasurementType = "MeasurementType";
    }

    public static class SqlQueryConstants
    {
        public const string InsertMeasurement = @"INSERT INTO Measurements
                                (Value1, Unit1, Value2, Unit2, Result, OperationType, MeasurementType)
                                VALUES (@v1, @u1, @v2, @u2, @res, @op, @type)";

        public const string SelectAllMeasurements = "SELECT * FROM Measurements";
        public const string SelectByOperation = "SELECT * FROM Measurements WHERE OperationType=@op";
        public const string SelectByType = "SELECT * FROM Measurements WHERE MeasurementType=@type";
        public const string SelectCount = "SELECT COUNT(*) FROM Measurements";
        public const string DeleteAllMeasurements = "DELETE FROM Measurements";
    }
}