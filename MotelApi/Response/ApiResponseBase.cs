namespace MotelApi.Response
{
    public class ApiResponseBase
    {
        public int StatusCode { get; set; }

        public bool Success
        {
            get
            {
                return Messages is null ;
            }
        }

        public string Messages { get; set; }
    }

    public class ApiResponse<T> : ApiResponseBase
    {
        public T Data { get; set; }
    }
}
