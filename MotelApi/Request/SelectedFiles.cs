namespace MotelApi.Request
{
    public class SelectedFiles
    {
        public string Name { get; set; }

        public IFormFile File { get; set; }

        public string Type { get; set; }
    }
}
