namespace Common
{
    public interface ICustomerErrorType
    {
        string Message { get;  }
        int StatusCode { get; }
    }
}