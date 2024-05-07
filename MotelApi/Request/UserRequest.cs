using static MotelApi.Common.UserContants;

namespace MotelApi.Request
{
    public class UserRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Phone { get; set; }
        public Sex Sex { get; set; }
        public int CCCD { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
