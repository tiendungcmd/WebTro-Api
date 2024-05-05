namespace MotelApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string? KeyHash { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public Guid? HistoryId { get; set; }

        public Guid? UserDetailId { get; set; }

    }
}
