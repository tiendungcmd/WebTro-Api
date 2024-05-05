using static MotelApi.Common.UserContants;

namespace MotelApi.Models
{
    public class UserDetail
    {
        public Guid Id { get; set; }
        public Sex Sex { get; set; }
        public int? Phone { get; set; }
        public int? CCCD { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
