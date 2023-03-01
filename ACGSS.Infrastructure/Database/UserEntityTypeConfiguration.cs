using ACGSS.Domain.Entities;
using ACGSS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACGSS.Infrastructure.Database
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.ModifiedDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.IsActive).HasDefaultValue(UserStatus.Active);
            builder.Property(x => x.IsActive).HasConversion(e => e == UserStatus.Active,
                                                            e => e ? UserStatus.Active : UserStatus.Inactive);
        }
    }
}
