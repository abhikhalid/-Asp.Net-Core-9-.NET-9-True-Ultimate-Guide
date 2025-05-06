namespace ContactsManager.Core.Exceptions
{
    public class InvalidPersonIDException : ArgumentException
    {
        public InvalidPersonIDException() { }

        public InvalidPersonIDException(string message) : base(message) { }

        public InvalidPersonIDException(string message, Exception innerException) : base(message, innerException) { }
    }
}
