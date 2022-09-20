namespace Huayu.Wms.Identity.Infrastructure.Data.EntityConfigurations;

public class UserLoginEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable("UserLogins", "Identity");
    }
}
