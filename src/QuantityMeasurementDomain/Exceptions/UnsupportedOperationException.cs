namespace QuantityMeasurementDomain.Exceptions
{
    /// <summary>
    /// Java-style unsupported operation exception used by UC14 operation constraints.
    /// </summary>
    public class UnsupportedOperationException : InvalidOperationException
    {
        public UnsupportedOperationException(string message)
            : base(message)
        {
        }
    }
}
