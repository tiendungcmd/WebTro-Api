using MotelApi.Common;

namespace MotelApi.Request
{
    public class MotelModelRequest
    {
        public string? Name { get; set; }
        public string? Descriptions { get; set; }
        public int? Price { get; set; }
        public int? Rate { get; set; }
        public Status? Status { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public int? NumberBedRoom { get; set; }
        public int? NumberBathRoom { get; set; }
        public int? Acreage { get; set; }
        public int? Deposit { get; set; }
        public IFormFile File { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? Reason { get; set; }
    }
}
