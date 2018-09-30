namespace Root.Entity
{
    public class Response
    {
        public object Data { get; set; }
        public string Errmsg { get; set; }
        public int Errno { get; set; }

        public Response(object data, string errmsg, int errno)
        {
            Data = data;
            Errmsg = errmsg;
            Errno = errno;
        }

        public Response()
        {
            Errmsg = "";
            Errno = (int)ResponseType.Success;
            Data = "";
        }

        public Response(object data)
        {
            Data = data;
            Errmsg = "";
            Errno = (int)ResponseType.Success;
        }
    }
}