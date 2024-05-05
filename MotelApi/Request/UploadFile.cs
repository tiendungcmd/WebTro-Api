namespace MotelApi.Request
{
    public class UploadFile
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
