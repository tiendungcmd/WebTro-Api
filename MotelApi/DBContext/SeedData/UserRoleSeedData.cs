using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotelApi.Models;

namespace MotelApi.DBContext.SeedData
{
    public class UserRoleSeedData : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData(new UserRole
            {
                RoleId = new Guid("47d19a48-cc09-4725-82b0-0e5b74c84634"),
                UserId = new Guid("47d19a48-cc09-4725-82b0-0e5b74c84634")
            });
        }
    }
}
