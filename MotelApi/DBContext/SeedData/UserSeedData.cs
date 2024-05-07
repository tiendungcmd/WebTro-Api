using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotelApi.Models;

namespace MotelApi.DBContext.SeedData
{
    public class UserSeedData : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = new Guid("47d19a48-cc09-4725-82b0-0e5b74c84634"),
                    UserName = "admin",
                    PasswordHash = "123",
                    CreateTime = DateTime.UtcNow,
                });
        }
    }
}
