namespace Huayu.Wms.Identity.Infrastructure.Data.EntityConfigurations;

public class UserTokenEntityTypeConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.ToTable("UserTokens", "Identity");
    }
}
