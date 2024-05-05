namespace MotelApi.Models
{
    public class MotelDetail
    {
        public Guid Id { get; set; }
        public Guid MotelId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int NumberBedRoom { get; set; }
        public int NumberBathRoom { get; set; }
        public int Acreage { get; set;}
        public int Deposit { get; set; }
    }
}
