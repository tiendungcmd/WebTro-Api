using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotelApi.Common;
using MotelApi.Models;

namespace MotelApi.DBContext.SeedData
{
    public class RoleSeedData : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new Role
            {
                Id = new Guid("47d19a48-cc09-4725-82b0-0e5b74c84634"),
                RoleName = RoleNameContants.RoleName.Admin.ToString()
            },
            new Role
            {
                Id = new Guid("55d19a48-cc09-4725-82b0-0e5b74c84634"),
                RoleName = RoleNameContants.RoleName.User.ToString()
            }
            );
        }
    }
}
