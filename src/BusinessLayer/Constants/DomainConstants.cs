namespace BusinessLayer
{
    public static class MeasurementTypeConstants
    {
        public const string Length = "length";
        public const string Weight = "weight";
        public const string Volume = "volume";
        public const string Temperature = "temperature";
    }

    public static class OperationTypeConstants
    {
        public const string Compare = "COMPARE";
        public const string Add = "ADD";
        public const string Subtract = "SUBTRACT";
        public const string Divide = "DIVIDE";
        public const string Convert = "CONVERT";
    }

    public static class RepositoryTypeConstants
    {
        public const string Database = "database";
        public const string Cache = "cache";
    }
}