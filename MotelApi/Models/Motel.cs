using MotelApi.Common;

namespace MotelApi.Models
{
    public class Motel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Descriptions { get; set; }
        public int? Price { get; set; }
        public int? Rate { get; set; }
        public Status? Status { get; set; }
        public string? Title { get; set; }
        public string Reason { get; set; }

        public DateTime? CreatedTime { get; set; }
      
    }
}
