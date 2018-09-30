using System;
using Common;

namespace Root
{
    [Serializable]
    public class InputException:ApplicationException
    {
        public int? StatusCode { get; set; }

        public InputException(string message) :base(message){ }

        public InputException(string message, int errorNo) : base(message)
        {
            StatusCode = errorNo;
        }

        public InputException(ICustomerErrorType errorType):base(errorType.Message)
        {
            StatusCode = errorType.StatusCode;
        }
    }
}