namespace MotelApi.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Descriptions { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid MotelId { get; set; }
        public string UserName { get; set; }
    }
}
