public class CustomError : Exception
{
    public int ErrorCode { get; }

    public CustomError(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}