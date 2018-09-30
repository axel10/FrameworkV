using System;

namespace Root
{
    public class JsonException
    {
        public string ErrorMsg { get; set; }

        public JsonException( Exception e)
        {
            ErrorMsg = e.Message;
        }
        public JsonException(string msg)
        {
            ErrorMsg = msg;
        }
        public JsonException() { }
    }

    public class DevJsonException
    {
        public string ErrorMsg { get; set; }
        public string StackTrace { get; set; }

        public DevJsonException(Exception e)
        {
            ErrorMsg = e.Message;
            StackTrace = e.StackTrace;
        }
        public DevJsonException() { }
    }
}