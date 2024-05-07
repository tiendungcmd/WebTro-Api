namespace MotelApi.Request
{
    public class CommentRequest
    {
        public string Descriptions { get; set; }
        public Guid MotelId { get; set; }
        public string UserName { get; set; }
    }
}
